using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CodraftPlugin_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using CodraftPlugin_Updaters;
using System.Windows.Controls;

namespace CodraftPlugin_Loading
{
    [Transaction(TransactionMode.Manual)]
    public class OpstartWerkomgeving : IExternalCommand
    {
        private const float feetToMm = 304.8f;

        private string materialQuery = "SELECT * FROM Materiaal";
        private string scheduleQuery = "SELECT * FROM Schedule";
        private string SystemtypeQuery = "SELECT * FROM SystemTypes";
        private string pipeTypeQuery = "SELECT * FROM PipeTypes";
        private string joinSegmentAndSizesQuery = "SELECT * FROM SegmentSize";
        private string insulMaterialQuery = "SELECT * FROM IsolatieMateriaal";
        private string connection = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"";
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            string pathProject = doc.PathName.Substring(0, doc.PathName.LastIndexOf("\\") + 1);
            string pathDatabase = pathProject + @"RevitDatabases\OpstartWerkomgeving_Revit.accdb" + "\"";
            connection += pathDatabase;


            // Add all materials
            Transaction matTrans = new Transaction(doc, "AddMaterials");
            matTrans.Start();

            foreach (string i in FileOperations.GetMaterials(materialQuery, connection))
            {
                try
                {
                    Material.Create(doc, i);
                }
                catch (Exception)
                {
                    continue;
                }
                
            }

            foreach (string i in FileOperations.GetInsulationMaterials(insulMaterialQuery, connection))
            {
                try
                {
                    Material.Create(doc, i);
                }
                catch (Exception)
                {
                    continue;
                }
            }

            matTrans.Commit();



            // Add all schedules
            Transaction scheduleTrans = new Transaction(doc, "AddPipeSchedules");
            scheduleTrans.Start();

            foreach (string i in FileOperations.GetSchedules(scheduleQuery, connection))
            {
                try
                {
                    PipeScheduleType.Create(doc, i);
                }
                catch (Exception)
                {
                    continue;
                }
            }

            scheduleTrans.Commit();



            // Get a table of al the segments with its sizes
            List<List<object>> SegmentAndSizelist = FileOperations.GetSegmentsAndSizeList(joinSegmentAndSizesQuery, connection);

            string scheduleType = "";
            List<MEPSize> mepSizes = new List<MEPSize>();
            string mat = "";

            // Loop through the table and get for every id a list of mepsizes.
            int indexSegmentAndSizelist = 1;
            foreach (List<object> row in SegmentAndSizelist)
            {
                string newSchedulType = (string)row[0];
                string newMat = (string)row[1];
                if (scheduleType != newSchedulType || mat != newMat || indexSegmentAndSizelist == SegmentAndSizelist.Count)
                {
                    if (indexSegmentAndSizelist == SegmentAndSizelist.Count)
                    {
                        MEPSize lastMs = new MEPSize((double)row[2] / feetToMm, (double)row[3] / feetToMm, (double)row[4] / feetToMm, true, true);
                        mepSizes.Add(lastMs);
                    }
                    while (mepSizes.Count != 0)
                    {
                        PipeScheduleType pst = new FilteredElementCollector(doc)
                            .OfClass(typeof(PipeScheduleType))
                            .Cast<PipeScheduleType>()
                            .Single(x => x.Name == scheduleType);

                        Material material = new FilteredElementCollector(doc)
                            .OfClass(typeof(Material))
                            .Cast<Material>()
                            .Single(x => x.Name == mat);

                        // Add all the segments
                        Transaction t = new Transaction(doc, "AddPipeSegment");

                        try
                        {
                            t.Start();
                            PipeSegment.Create(doc, material.Id, pst.Id, mepSizes);
                            t.Commit();
                        }
                        catch (Exception)
                        {
                            t.Commit();
                            mepSizes.Clear();
                            continue;
                        }
                    }

                    scheduleType = (string)row[0];
                    mat = (string)row[1];
                }

                double insideDiameterDatabase = (double)row[4];
                double insideDiameter = insideDiameterDatabase <= 0 ? 0.5 : insideDiameterDatabase;

                MEPSize ms = new MEPSize((double)row[2] / feetToMm, insideDiameter / feetToMm, (double)row[4] / feetToMm, true, true);
                mepSizes.Add(ms);
                indexSegmentAndSizelist++;
            }



            // Get all insulation materials
            List<string> insulMatList = FileOperations.GetInsulationMaterials(insulMaterialQuery, connection);

            // Get a PipeInsulationType to duplicate
            PipeInsulationType pit = new FilteredElementCollector(doc)
                .OfClass(typeof(PipeInsulationType))
                .Cast<PipeInsulationType>()
                .First();

            foreach (string i in insulMatList)
            {
                // Get the material from the document
                Material insulMat = new FilteredElementCollector(doc)
                    .OfClass(typeof(Material))
                    .Cast<Material>()
                    .Single(x => x.Name == i);

                // Add the insulationType to the document.
                Transaction insulTypeTrans = new Transaction(doc, "AddInsulationType");
                

                try
                {
                    insulTypeTrans.Start();
                    ElementId elemId = pit.Duplicate(i).Id;
                    PipeInsulationType newType = (PipeInsulationType)doc.GetElement(elemId);
                    newType.get_Parameter(BuiltInParameter.MATERIAL_ID_PARAM).Set(insulMat.Id);
                    insulTypeTrans.Commit();
                }
                catch (Exception)
                {
                    insulTypeTrans.Commit();
                    continue;
                }

            }

            Transaction deletePit = new Transaction(doc, "Delete orginal insulaitonType");
            deletePit.Start();
            // Delete orginal insulationType.
            doc.Delete(pit.Id);

            deletePit.Commit();



            // Get the table of the all the systemTypes
            List<List<string>> systemTypes = FileOperations.GetSystemTypes(SystemtypeQuery, connection);

            // Loop over all the systemTypes
            foreach (List<string> i in systemTypes)
            {
                List<byte> col = i[2].Split(',').Select(x => byte.Parse(x)).ToList();
                Color revitColor = new Color(col[0], col[1], col[2]);
                string name = i[0];
                string typeTemplate = i[1];
                string abr = i[3];
                MEPSystemClassification msc;

                PipingSystemType psType = new FilteredElementCollector(doc)
                    .OfClass(typeof(PipingSystemType))
                    .Cast<PipingSystemType>()
                    .FirstOrDefault(x => x.Name == name);

                if (psType != null)
                {
                    continue;
                }

                // Set the MEPSystemClassification
                switch (typeTemplate)
                {
                    case "Domestic Cold Water":
                        msc = MEPSystemClassification.DomesticColdWater;
                        break;

                    case "Domestic Hot Water":
                        msc = MEPSystemClassification.DomesticHotWater;
                        break;

                    case "Fire Protection Dry":
                        msc = MEPSystemClassification.FireProtectDry;
                        break;

                    case "Fire Protection Other":
                        msc = MEPSystemClassification.FireProtectOther;
                        break;

                    case "Fire Protection Pre-Action":
                        msc = MEPSystemClassification.FireProtectPreaction;
                        break;

                    case "Fire Protection Wet":
                        msc = MEPSystemClassification.FireProtectWet;
                        break;

                    case "Hydronic Return":
                        msc = MEPSystemClassification.ReturnHydronic;
                        break;

                    case "Hydronic Supply":
                        msc = MEPSystemClassification.SupplyHydronic;
                        break;

                    case "Other":
                        msc = MEPSystemClassification.OtherPipe;
                        break;

                    case "Sanitary":
                        msc = MEPSystemClassification.Sanitary;
                        break;

                    case "Vent":
                        msc = MEPSystemClassification.Vent;
                        break;

                    default:
                        msc = MEPSystemClassification.OtherPipe;
                        name = "FoutTijdensUitoerenCode";
                        break;
                }

                // Add SystemType
                Transaction systemTypeTrans = new Transaction(doc, "AddSystemType");

                try
                {
                    systemTypeTrans.Start();
                    PipingSystemType pst = PipingSystemType.Create(doc, msc, name);
                    pst.LineColor = revitColor;
                    pst.get_Parameter(BuiltInParameter.ALL_MODEL_URL).Set("www.codraft.be");
                    pst.TwoLineDropType = RiseDropSymbol.YinYangFilled;
                    pst.TwoLineRiseType = RiseDropSymbol.YinYang;
                    pst.Abbreviation = abr;
                    systemTypeTrans.Commit();
                }
                catch (Exception)
                {
                    systemTypeTrans.Commit();
                    continue;
                }

            }



            // Get first pipeType to duplicate from
            PipeType pt = new FilteredElementCollector(doc)
                .OfClass(typeof(PipeType))
                .Cast<PipeType>()
                .First();

            // get CODFamilies familySymbols
            IEnumerable<FamilySymbol> fsFittings = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilySymbol))
                .Cast<FamilySymbol>()
                .Where(x => x.Name == "Elbow" || x.Name == "Transition_Concentrisch" || x.Name == "Transition_Excentrisch" || x.Name == "Tee" || x.Name == "Tap" || x.Name == "Cap");


            List<List<object>> pipeTypeList = FileOperations.GetPipeTypes(pipeTypeQuery, connection);
            List<string> segmentList = new List<string>();
            List<(double minDn, double maxDn)> minMaxDnList = new List<(double, double)> ();
            string ptName = "";
            string teeOrTap = "";
            string excenOrConcen = "";

            // Loop through all the pipeTypes
            for (int i = 0; i < pipeTypeList.Count; i++)
            {
                string newName = (string)pipeTypeList[i][1];
                if (newName != ptName)
                {
                    PipeTypesToevoegen(segmentList, ptName, pt, fsFittings, minMaxDnList, teeOrTap, excenOrConcen, doc);

                    segmentList.Clear();
                    minMaxDnList.Clear();
                    ptName = newName;
                    teeOrTap = ((string)pipeTypeList[i][5]).ToLower();
                    excenOrConcen = ((string)pipeTypeList[i][6]).ToLower();
                }

                segmentList.Add((string)pipeTypeList[i][2]);
                minMaxDnList.Add(((double)pipeTypeList[i][3], (double)pipeTypeList[i][4]));
            }

            PipeTypesToevoegen(segmentList, ptName, pt, fsFittings, minMaxDnList, teeOrTap, excenOrConcen, doc);

            return Result.Succeeded;
        }



        private void PipeTypesToevoegen(List<string> segmentList, string ptName, PipeType pt, IEnumerable<FamilySymbol> fsFittings, List<(double minDn, double maxDn)> minMaxDn, string teeOrTap, string excenOrConcen, Document doc)
        {
            for (int j = 0; j < segmentList.Count; j++)
            {
                // Get pipeSegment
                PipeSegment ps = new FilteredElementCollector(doc)
                    .OfClass(typeof(PipeSegment))
                    .Cast<PipeSegment>()
                    .Single(x => x.Name == segmentList[j]);

                // Create segment rule.
                RoutingPreferenceRule rprSegment = new RoutingPreferenceRule(ps.Id, "segment");
                rprSegment.AddCriterion(new PrimarySizeCriterion(minMaxDn[j].minDn / feetToMm, minMaxDn[j].maxDn / feetToMm));

                // Create fitting rules.
                RoutingPreferenceRule rprElbow = new RoutingPreferenceRule(fsFittings.Single(x => x.Name == "Elbow").Id, "elbow");
                rprElbow.AddCriterion(PrimarySizeCriterion.All());
                RoutingPreferenceRule rprCap = new RoutingPreferenceRule(fsFittings.Single(x => x.Name == "Cap").Id, "cap");
                rprCap.AddCriterion(PrimarySizeCriterion.All());

                RoutingPreferenceRule rprTee;
                RoutingPreferenceRule rprTran;

                if (teeOrTap == "tee")
                    rprTee = new RoutingPreferenceRule(fsFittings.Single(x => x.Name == "Tee").Id, "tee");
                else
                    rprTee = new RoutingPreferenceRule(fsFittings.Single(x => x.Name == "Tap").Id, "tap");

                if (excenOrConcen == "excentrisch")
                    rprTran = new RoutingPreferenceRule(fsFittings.Single(x => x.Name == "Transition_Excentrisch").Id, "excentrisch");
                else
                    rprTran = new RoutingPreferenceRule(fsFittings.Single(x => x.Name == "Transition_Concentrisch").Id, "concentrisch");

                rprTee.AddCriterion(PrimarySizeCriterion.All());
                rprTran.AddCriterion(PrimarySizeCriterion.All());

                // Start transation.
                Transaction ptTrans = new Transaction(doc, "AddPipeType");
                try
                {
                    ptTrans.Start();

                    // Create new pipetype.
                    PipeType nieuwPt;
                    if (j == 0)
                    {
                        nieuwPt = (PipeType)pt.Duplicate(ptName);
                    }
                    else
                    {
                        nieuwPt = new FilteredElementCollector(doc)
                            .OfClass(typeof(PipeType))
                            .Cast<PipeType>()
                            .Single(x => x.Name == ptName);
                    }

                    RoutingPreferenceManager rpm = nieuwPt.RoutingPreferenceManager;

                    // Add segment rules.
                    rpm.AddRule(RoutingPreferenceRuleGroupType.Segments, rprSegment);

                    if (j == 0)
                    {
                        // Set junctionType.
                        if (teeOrTap == "tee")
                            rpm.PreferredJunctionType = PreferredJunctionType.Tee;
                        else
                            rpm.PreferredJunctionType = PreferredJunctionType.Tap;

                        // Add fitting rules.
                        rpm.AddRule(RoutingPreferenceRuleGroupType.Elbows, rprElbow);
                        rpm.AddRule(RoutingPreferenceRuleGroupType.Caps, rprCap);
                        rpm.AddRule(RoutingPreferenceRuleGroupType.Junctions, rprTee);
                        rpm.AddRule(RoutingPreferenceRuleGroupType.Transitions, rprTran);

                        // Delete default rule.
                        rpm.RemoveRule(RoutingPreferenceRuleGroupType.Segments, 0);
                    }

                    ptTrans.Commit();
                }
                catch (Exception)
                {
                    ptTrans.Commit();
                    continue;
                }

            }
        }
    }
}
