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

namespace CodraftPlugin_Loading
{
    [Transaction(TransactionMode.Manual)]
    public class OpstartWerkomgeving : IExternalCommand
    {
        private const float feetToMm = 304.8f;

        private string materialQuery = "";
        private string scheduleQuery = "";
        private string SystemtypeQuery = "";
        private string pipeTypeQuery = "";
        private string joinSegmentAndSizesQuery = "";
        private string insulMaterialQuery = "";
        private string connection = "";

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            // Add all materials
            Transaction matTrans = new Transaction(doc, "AddMaterials");
            matTrans.Start();

            foreach (string i in FileOperations.GetMaterials(materialQuery, connection))
                Material.Create(doc, i);

            matTrans.Commit();



            // Add all schedules
            Transaction scheduleTrans = new Transaction(doc, "AddPipeSchedules");
            scheduleTrans.Start();

            foreach (string i in FileOperations.GetSchedules(scheduleQuery, connection))
                PipeScheduleType.Create(doc, i);

            scheduleTrans.Commit();



            // Get a table of al the segments with its sizes
            List<List<object>> SegmentAndSizelist = FileOperations.GetSegmentsAndSizeList(joinSegmentAndSizesQuery, connection);

            int id = -1;
            string scheduleType = "";
            List<MEPSize> mepSizes = new List<MEPSize>();
            string mat = "";

            // Loop through the table and get for every id a list of mepsizes.
            foreach (List<object> row in SegmentAndSizelist)
            {
                int newId = (int)row[0];
                if (id != newId)
                {
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
                        t.Start();

                        PipeSegment.Create(doc, material.Id, pst.Id, mepSizes);

                        t.Commit();

                        mepSizes.Clear();
                    }

                    id = newId;
                    scheduleType = (string)row[1];
                    mat = (string)row[5];
                }

                MEPSize ms = new MEPSize((double)row[2] / feetToMm, (double)row[3] / feetToMm, (double)row[4] / feetToMm, true, true);
                mepSizes.Add(ms);
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
                insulTypeTrans.Start();

                ElementId elemId = pit.Duplicate(i).Id;
                PipeInsulationType newType = (PipeInsulationType)doc.GetElement(elemId);
                newType.get_Parameter(BuiltInParameter.MATERIAL_ID_PARAM).Set(insulMat.Id);

                // Delete orginal insulationType.
                doc.Delete(pit.Id);

                insulTypeTrans.Commit();
            }



            // Get the table of the all the systemTypes
            List<List<string>> systemTypes = FileOperations.GetSystemTypes(SystemtypeQuery, connection);

            // Loop over all the systemTypes
            foreach (List<string> i in systemTypes)
            {
                System.Windows.Media.Color col = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(i[2]);
                Color revitColor = new Color(col.R, col.G, col.B);
                string name = i[0];
                string typeTemplate = i[3];
                string abr = i[1];
                MEPSystemClassification msc;

                // Set the MEPSystemClassification
                switch (i[3])
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
                systemTypeTrans.Start();

                PipingSystemType pst = PipingSystemType.Create(doc, msc, name);
                pst.LineColor = revitColor;
                pst.get_Parameter(BuiltInParameter.ALL_MODEL_URL).Set("www.codraft.be");
                pst.TwoLineDropType = RiseDropSymbol.YinYangFilled;
                pst.TwoLineRiseType = RiseDropSymbol.YinYang;

                systemTypeTrans.Commit();

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
                .Where(x => x.Name == "Elbow" || x.Name == "Transition_Concntrisch" || x.Name == "Transition_Excentrisch" || x.Name == "Tee" || x.Name == "Tap" || x.Name == "Cap");


            List<List<string>> pipeTypeList = FileOperations.GetPipeTypes(pipeTypeQuery, connection);
            List<string> segmentList = new List<string>();
            string ptName = "";
            string teeOrTap = "";
            string excenOrConcen = "";

            // Loop through all the pipeTypes
            foreach (List<string> i in pipeTypeList)
            {
                string newName = i[0];
                if (newName != ptName)
                {
                    for (int j = 0; j < segmentList.Count; j++)
                    {
                        // Get pipeSegment
                        PipeSegment ps = new FilteredElementCollector(doc)
                            .OfClass(typeof(PipeSegment))
                            .Cast<PipeSegment>()
                            .Single(x => x.Name == segmentList[j]);

                        // Get MEPSizeCollection
                        ICollection<MEPSize> ms = ps.GetSizes();
                        double minDN = 0;
                        double maxDN = 0;

                        foreach (MEPSize e in ms)
                        {
                            minDN = e.NominalDiameter < minDN ? e.NominalDiameter : minDN;
                            maxDN = e.NominalDiameter > maxDN ? e.NominalDiameter : maxDN;
                        }

                        // Create segment rule.
                        RoutingPreferenceRule rprSegment = new RoutingPreferenceRule(ps.Id, "segment");
                        rprSegment.AddCriterion(new PrimarySizeCriterion(minDN, maxDN));

                        // Create fitting rules.
                        RoutingPreferenceRule rprElbow = new RoutingPreferenceRule(fsFittings.Single(x => x.Name == "Elbow").Id, "elbow");
                        RoutingPreferenceRule rprCap = new RoutingPreferenceRule(fsFittings.Single(x => x.Name == "Cap").Id, "cap");
                        RoutingPreferenceRule rprTee;
                        RoutingPreferenceRule rprTran;

                        if (teeOrTap == "tee")
                            rprTee = new RoutingPreferenceRule(fsFittings.Single(x => x.Name == "Tee").Id, "tee");
                        else
                            rprTee = new RoutingPreferenceRule(fsFittings.Single(x => x.Name == "Tap").Id, "tap");

                        if (excenOrConcen == "excentrisch")
                            rprTran = new RoutingPreferenceRule(fsFittings.Single(x => x.Name == "Transition_Excentrisch").Id, "excentrisch");
                        else
                            rprTran = new RoutingPreferenceRule(fsFittings.Single(x => x.Name == "Transition_Concntrisch").Id, "concntrisch");

                        // Start transation.
                        Transaction ptTrans = new Transaction(doc, "AddPipeType");
                        ptTrans.Start();

                        // Create new pipetype.
                        PipeType nieuwPt = (PipeType)pt.Duplicate(ptName);
                        RoutingPreferenceManager rpm = nieuwPt.RoutingPreferenceManager;

                        // Set junctionType.
                        if (teeOrTap == "tee")
                            rpm.PreferredJunctionType = PreferredJunctionType.Tee;
                        else
                            rpm.PreferredJunctionType = PreferredJunctionType.Tap;

                        // Add rules.
                        rpm.AddRule(RoutingPreferenceRuleGroupType.Segments, rprSegment);
                        rpm.AddRule(RoutingPreferenceRuleGroupType.Elbows, rprElbow);
                        rpm.AddRule(RoutingPreferenceRuleGroupType.Caps, rprCap);
                        rpm.AddRule(RoutingPreferenceRuleGroupType.Junctions, rprTee);
                        rpm.AddRule(RoutingPreferenceRuleGroupType.Transitions, rprTran);

                        // Delete default rules.
                        rpm.RemoveRule(RoutingPreferenceRuleGroupType.Segments, 0);
                        rpm.RemoveRule(RoutingPreferenceRuleGroupType.Elbows, 0);
                        rpm.RemoveRule(RoutingPreferenceRuleGroupType.Caps, 0);
                        rpm.RemoveRule(RoutingPreferenceRuleGroupType.Junctions, 0);
                        rpm.RemoveRule(RoutingPreferenceRuleGroupType.Transitions, 0);

                        ptTrans.Commit();
                    }

                    segmentList.Clear();
                    ptName = newName;
                    teeOrTap = i[2].ToLower();
                    excenOrConcen = i[3].ToLower();
                }

                segmentList.Add(i[1]);
            }

            return Result.Succeeded;
        }
    }
}
