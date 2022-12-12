﻿using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodraftPlugin_Library
{
    public static class ElementSettings
    {
        const float feetToMm = 304.8f;

        /// <summary>
        /// Set the parameters for a pipe element.
        /// </summary>
        /// <param name="pipe">A pipe element from the document.</param>
        public static void SetCodraftParametersPipe(Pipe pipe)
        {
            // Pipe parameters
            string startBoven = "COD_bovenkant_buis_start";
            string eindeBoven = "COD_bovenkant_buis_einde";
            string startCenter = "COD_center_buis_start";
            string eindeCenter = "COD_center_buis_einde";
            string startOnder = "COD_onderkant_buis_start";
            string eindeOnder = "COD_onderkant_buis_einde";
            double startOffset = pipe.get_Parameter(BuiltInParameter.RBS_START_OFFSET_PARAM).AsDouble();
            double eindeOffset = pipe.get_Parameter(BuiltInParameter.RBS_END_OFFSET_PARAM).AsDouble();
            double diameter = pipe.get_Parameter(BuiltInParameter.RBS_PIPE_OUTER_DIAMETER).AsDouble();

            try
            {
                // Set parameters
                pipe.LookupParameter(startBoven).Set(startOffset > eindeOffset ? startOffset + diameter / 2 : eindeOffset + diameter / 2);
                pipe.LookupParameter(eindeBoven).Set(eindeOffset > startOffset ? startOffset + diameter / 2 : eindeOffset + diameter / 2);
                pipe.LookupParameter(startCenter).Set(startOffset > eindeOffset ? startOffset : eindeOffset);
                pipe.LookupParameter(eindeCenter).Set(eindeOffset > startOffset ? startOffset : eindeOffset);
                pipe.LookupParameter(startOnder).Set(startOffset > eindeOffset ? startOffset - diameter / 2 : eindeOffset - diameter / 2);
                pipe.LookupParameter(eindeOnder).Set(eindeOffset > startOffset ? startOffset - diameter / 2 : eindeOffset - diameter / 2);
            }
            catch (NullReferenceException)
            {
                TaskDialog td = new TaskDialog("ProjectParameters Fout");
                td.MainInstruction = "Niet alle project parameters zijn toegevoegd voor de pipes.";
                td.ExpandedContent = "De volgende project paramters moeten toegevoegd zijn in het project voor alle pipes.\n" +
                    "COD_bovenkant_buis_start,\n" +
                    "COD_bovenkant_buis_einde,\n" +
                    "COD_center_buis_start,\n" +
                    "COD_center_buis_einde,\n" +
                    "COD_onderkant_buis_start,\n" +
                    "COD_onderkant_buis_einde";
                td.Show();
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Fout", ex.Message);
            }
        }

        /// <summary>
        /// Set the parameters for a tee fitting.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="fi"></param>
        public static void SetCodraftParametersTee(List<object> parameters, FamilyInstance fi)
        {
            fi.LookupParameter("COD_c1_Buitendiameter").Set((double)parameters[0]);
            fi.LookupParameter("COD_c2_Buitendiameter").Set((double)parameters[1]);
            fi.LookupParameter("COD_c3_Buitendiameter").Set((double)parameters[2]);
            fi.LookupParameter("Lengte_waarde").Set((double)parameters[3]);
            fi.LookupParameter("Center_uiteinde_3_waarde").Set((double)parameters[4]);
            fi.LookupParameter("Center_uiteinde_1_waarde").Set((double)parameters[5]);
            fi.LookupParameter("Uiteinde_1_type").Set((double)parameters[6]);
            fi.LookupParameter("Uiteinde_2_type").Set((double)parameters[7]);
            fi.LookupParameter("Uiteinde_3_type").Set((double)parameters[8]);
            fi.LookupParameter("Uiteinde_1_maat").Set((double)parameters[9]);
            fi.LookupParameter("Uiteinde_2_maat").Set((double)parameters[10]);
            fi.LookupParameter("Uiteinde_3_maat").Set((double)parameters[11]);
            fi.LookupParameter("Uiteinde_1_lengte").Set((double)parameters[12]);
            fi.LookupParameter("Uiteinde_2_lengte").Set((double)parameters[13]);
            fi.LookupParameter("Uiteinde_3_lengte").Set((double)parameters[14]);
            fi.LookupParameter("Flens_dikte_1").Set((double)parameters[15]);
            fi.LookupParameter("Flens_dikte_2").Set((double)parameters[16]);
            fi.LookupParameter("Flens_dikte_3").Set((double)parameters[17]);
            fi.LookupParameter("COD_Fabrikant").Set((string)parameters[18]);
            fi.LookupParameter("COD_Type").Set((string)parameters[19]);
            fi.LookupParameter("COD_Materiaal").Set((string)parameters[20]);
            fi.LookupParameter("COD_Productcode").Set((string)parameters[21]);
            fi.LookupParameter("COD_Omschrijving").Set((string)parameters[22]);
            fi.LookupParameter("COD_Beschikbaar").Set((string)parameters[23]);
        }

        /// <summary>
        /// Set the parameters for a transition fitting.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="fi"></param>
        /// <param name="switchNds"></param>
        /// <param name="excentrischOrConcentrisch">"1" is excentrisch "0" is concentrisch</param>
        public static void SetCodraftParametersTransition(List<object> parameters, FamilyInstance fi, bool switchNds, int excentrischOrConcentrisch)
        {
            if (!switchNds)
            {
                fi.LookupParameter("COD_c1_Buitendiameter").Set((double)parameters[0]);
                fi.LookupParameter("COD_c2_Buitendiameter").Set((double)parameters[1]);
                fi.LookupParameter("Uiteinde_1_type").Set((double)parameters[3]);
                fi.LookupParameter("Uiteinde_2_type").Set((double)parameters[4]);
                fi.LookupParameter("Uiteinde_1_maat").Set((double)parameters[5]);
                fi.LookupParameter("Uiteinde_2_maat").Set((double)parameters[6]);
                fi.LookupParameter("Uiteinde_1_lengte").Set((double)parameters[7]);
                fi.LookupParameter("Uiteinde_2_lengte").Set((double)parameters[8]);
                fi.LookupParameter("Flens_dikte_1").Set((double)parameters[9]);
                fi.LookupParameter("Flens_dikte_2").Set((double)parameters[10]);
            }
            else
            {
                fi.LookupParameter("COD_c1_Buitendiameter").Set((double)parameters[1]);
                fi.LookupParameter("COD_c2_Buitendiameter").Set((double)parameters[0]);
                fi.LookupParameter("Uiteinde_1_type").Set((double)parameters[4]);
                fi.LookupParameter("Uiteinde_2_type").Set((double)parameters[3]);
                fi.LookupParameter("Uiteinde_1_maat").Set((double)parameters[6]);
                fi.LookupParameter("Uiteinde_2_maat").Set((double)parameters[5]);
                fi.LookupParameter("Uiteinde_1_lengte").Set((double)parameters[8]);
                fi.LookupParameter("Uiteinde_2_lengte").Set((double)parameters[7]);
                fi.LookupParameter("Flens_dikte_1").Set((double)parameters[10]);
                fi.LookupParameter("Flens_dikte_2").Set((double)parameters[9]);
            }

            if (excentrischOrConcentrisch == 1)
                fi.LookupParameter("Do_not_modify_COD_Excentrisch").Set(excentrischOrConcentrisch);
            else
                fi.LookupParameter("Do_not_modify_COD_Excentrisch").Set(0);

            fi.LookupParameter("Lengte").Set((double)parameters[2]);
            fi.LookupParameter("COD_Fabrikant").Set((string)parameters[11]);
            fi.LookupParameter("COD_Type").Set((string)parameters[12]);
            fi.LookupParameter("COD_Materiaal").Set((string)parameters[13]);
            fi.LookupParameter("COD_Productcode").Set((string)parameters[14]);
            fi.LookupParameter("COD_Omschrijving").Set((string)parameters[15]);
            fi.LookupParameter("COD_Beschikbaar").Set((string)parameters[16]);
            fi.LookupParameter("COD_Isolatie").Set(1);
        }

        /// <summary>
        /// Set the systemtype of the pipeelement.
        /// </summary>
        /// <param name="pipe">A pipe element from the document.</param>
        /// <param name="doc">A revit document.</param>
        public static void SetPipeType(Pipe pipe, Document doc)
        {
            // Neem de huidige systemType van de pipe.
            string systemName = pipe.get_Parameter(BuiltInParameter.RBS_PIPING_SYSTEM_TYPE_PARAM).AsValueString();

            // Haal de juiste pipeType uit het document op basis van de systemName.
            PipeType pipeType = new FilteredElementCollector(doc)
                .OfClass(typeof(PipeType))
                .Where(x => x.Name.Split('%').First() == systemName)
                .Cast<PipeType>()
                .First();

            // Set de pipeType.
            pipe.PipeType = pipeType;
        }

        /// <summary>
        /// Set the paramters for an elbow element
        /// </summary>
        /// <param name="parameters">List with all parameters for an elbow.</param>
        /// <param name="fi">An elbow element from the document.</param>
        public static void SetCodraftParametersElbow(List<object> parameters, FamilyInstance fi)
        {
            fi.LookupParameter("COD_c1_Buitendiameter").Set((double)parameters[0]);
            fi.LookupParameter("COD_c2_Buitendiameter").Set((double)parameters[1]);
            fi.LookupParameter("Center_straal").Set((double)parameters[2]);
            fi.LookupParameter("Uiteinde_1_type").Set((double)parameters[3]);
            fi.LookupParameter("Uiteinde_2_type").Set((double)parameters[4]);
            fi.LookupParameter("Uiteinde_1_maat").Set((double)parameters[5]);
            fi.LookupParameter("Uiteinde_2_maat").Set((double)parameters[6]);
            fi.LookupParameter("Uiteinde_1_lengte").Set((double)parameters[7]);
            fi.LookupParameter("Uiteinde_2_lengte").Set((double)parameters[8]);
            fi.LookupParameter("Flens_dikte").Set((double)parameters[9]);
            fi.LookupParameter("Standaard_hoek").Set((double)parameters[10]);
            fi.LookupParameter("COD_Fabrikant").Set((string)parameters[11]);
            fi.LookupParameter("COD_Type").Set((string)parameters[12]);
            fi.LookupParameter("COD_Materiaal").Set((string)parameters[13]);
            fi.LookupParameter("COD_Productcode").Set((string)parameters[14]);
            fi.LookupParameter("COD_Omschrijving").Set((string)parameters[15]);
            fi.LookupParameter("COD_Beschikbaar").Set((string)parameters[16]);
            fi.LookupParameter("COD_Isolatie").Set(1);
        }

        /// <summary>
        /// Set the parameter for a tap element
        /// </summary>
        /// <param name="parameters">List with all parameters for a tap</param>
        /// <param name="fi">A tap element from the document.</param>
        public static void SetCodraftParametersTap(List<object> parameters, FamilyInstance fi)
        {
            fi.LookupParameter("COD_c1_Buitendiameter").Set((double)parameters[0]);
            fi.LookupParameter("Lengte").Set((double)parameters[1]);
            fi.LookupParameter("Lengte_waarde").Set((double)parameters[2]);
            fi.LookupParameter("COD_Fabrikant").Set((string)parameters[3]);
            fi.LookupParameter("COD_Type").Set((string)parameters[4]);
            fi.LookupParameter("COD_Materiaal").Set((string)parameters[5]);
            fi.LookupParameter("COD_Productcode").Set((string)parameters[6]);
            fi.LookupParameter("COD_Omschrijving").Set((string)parameters[7]);
            fi.LookupParameter("COD_Beschikbaar").Set((string)parameters[8]);
            fi.LookupParameter("COD_Isolatie").Set(1);
        }

        /// <summary>
        /// Set the parameters for an elbow fitting that does not exist in the database.
        /// </summary>
        /// <param name="fi">Elbow fitting from the document.</param>
        public static void ElbowDoesNotExist(FamilyInstance fi)
        {
            fi.LookupParameter("COD_Fabrikant").Set("BESTAAT NIET!");
            fi.LookupParameter("COD_Type").Set("BESTAAT NIET!");
            fi.LookupParameter("COD_Materiaal").Set("BESTAAT NIET!");
            fi.LookupParameter("COD_Productcode").Set("BESTAAT NIET!");
            fi.LookupParameter("COD_Omschrijving").Set("BESTAAT NIET!");
            fi.LookupParameter("COD_Beschikbaar").Set("nee");

            fi.LookupParameter("COD_c1_Buitendiameter").Set(15 / feetToMm);
            fi.LookupParameter("COD_c2_Buitendiameter").Set(15 / feetToMm);
            fi.LookupParameter("Center_straal").Set(30 / feetToMm);
            fi.LookupParameter("Uiteinde_1_type").Set(0);
            fi.LookupParameter("Uiteinde_2_type").Set(0);
            fi.LookupParameter("Uiteinde_1_maat").Set(0);
            fi.LookupParameter("Uiteinde_2_maat").Set(0);
            fi.LookupParameter("Uiteinde_1_lengte").Set(0);
            fi.LookupParameter("Uiteinde_2_lengte").Set(0);
        }

        /// <summary>
        /// Set the parameter for a tapfitting that does not exist in the database.
        /// </summary>
        /// <param name="fi">Tap fitting from the document</param>
        public static void TapDoesNotExist(FamilyInstance fi)
        {
            fi.LookupParameter("COD_Fabrikant").Set("BESTAAT NIET!");
            fi.LookupParameter("COD_Type").Set("BESTAAT NIET!");
            fi.LookupParameter("COD_Materiaal").Set("BESTAAT NIET!");
            fi.LookupParameter("COD_Productcode").Set("BESTAAT NIET!");
            fi.LookupParameter("COD_Omschrijving").Set("BESTAAT NIET!");
            fi.LookupParameter("COD_Beschikbaar").Set("nee");

            fi.LookupParameter("COD_c1_Buitendiameter").Set(15 / feetToMm);
            fi.LookupParameter("Lengte").Set(15 / feetToMm);
            fi.LookupParameter("Lengte_waarde").Set(15 / feetToMm);
        }

        /// <summary>
        /// Set the parameters for a tee fitting that does not exist in the database.
        /// </summary>
        /// <param name="fi">Tee fitting from the document</param>
        public static void TeeDoesNotExist(FamilyInstance fi)
        {
            fi.LookupParameter("COD_Fabrikant").Set("BESTAAT NIET!");
            fi.LookupParameter("COD_Type").Set("BESTAAT NIET!");
            fi.LookupParameter("COD_Materiaal").Set("BESTAAT NIET!");
            fi.LookupParameter("COD_Productcode").Set("BESTAAT NIET!");
            fi.LookupParameter("COD_Omschrijving").Set("BESTAAT NIET!");
            fi.LookupParameter("COD_Beschikbaar").Set("nee");

            fi.LookupParameter("COD_c1_Buitendiameter").Set(15 / feetToMm);
            fi.LookupParameter("COD_c2_Buitendiameter").Set(15 / feetToMm);
            fi.LookupParameter("COD_c3_Buitendiameter").Set(15 / feetToMm);
            fi.LookupParameter("Lengte_waarde").Set(15 / feetToMm);
            fi.LookupParameter("Center_uiteinde_3_waarde").Set(15 / feetToMm);
            fi.LookupParameter("Center_uiteinde_1_waarde").Set(15 / feetToMm);
            fi.LookupParameter("Uiteinde_1_type").Set(0);
            fi.LookupParameter("Uiteinde_2_type").Set(0);
            fi.LookupParameter("Uiteinde_3_type").Set(0);
            fi.LookupParameter("Uiteinde_1_maat").Set(0);
            fi.LookupParameter("Uiteinde_2_maat").Set(0);
            fi.LookupParameter("Uiteinde_3_maat").Set(0);
            fi.LookupParameter("Uiteinde_1_lengte").Set(0);
            fi.LookupParameter("Uiteinde_2_lengte").Set(0);
            fi.LookupParameter("Uiteinde_3_lengte").Set(0);
        }

        /// <summary>
        /// Set the parameters for a transition fitting that does not exist in the database.
        /// </summary>
        /// <param name="fi">Transition fitting from the document</param>
        public static void TransitionDoesNotExist(FamilyInstance fi)
        {
            fi.LookupParameter("COD_Fabrikant").Set("BESTAAT NIET!");
            fi.LookupParameter("COD_Type").Set("BESTAAT NIET!");
            fi.LookupParameter("COD_Materiaal").Set("BESTAAT NIET!");
            fi.LookupParameter("COD_Productcode").Set("BESTAAT NIET!");
            fi.LookupParameter("COD_Omschrijving").Set("BESTAAT NIET!");
            fi.LookupParameter("COD_Beschikbaar").Set("nee");

            fi.LookupParameter("COD_c1_Buitendiameter").Set(15 / feetToMm);
            fi.LookupParameter("COD_c2_Buitendiameter").Set(15 / feetToMm);
            fi.LookupParameter("Lengte").Set(15 / feetToMm);
            fi.LookupParameter("Uiteinde_1_type").Set(0);
            fi.LookupParameter("Uiteinde_2_type").Set(0);
            fi.LookupParameter("Uiteinde_1_maat").Set(0);
            fi.LookupParameter("Uiteinde_2_maat").Set(0);
            fi.LookupParameter("Uiteinde_1_lengte").Set(0);
            fi.LookupParameter("Uiteinde_2_lengte").Set(0);
        }

        /// <summary>
        /// Applies the insulation for the current pipe
        /// </summary>
        /// <param name="elemId">Id of the pipe element</param>
        /// <param name="lookupValues">The values from the access database</param>
        /// <param name="doc">the current revit document</param>
        public static void ApplyInsulation(ElementId elemId, List<object> lookupValues, Document doc)
        {
            string insulType = (string)lookupValues[0];
            double insulThickness = (double)lookupValues[1];
            List<ElementId> id = InsulationLiningBase.GetInsulationIds(doc, elemId).ToList();

            if (id.Count > 0)
                doc.Delete(id.First());

            try
            {
                PipeInsulationType insulationType = (PipeInsulationType)new FilteredElementCollector(doc)
                        .OfCategory(BuiltInCategory.OST_PipeInsulations)
                        .Where(x => x.Name == insulType)
                        .First();

                PipeInsulation.Create(doc, elemId, insulationType.Id, insulThickness);
            }
            catch (Exception)
            {
                TaskDialog.Show("Isolatie Error", $"De isolatietype van de database is niet in Revit gevonden. Het type {insulType} moet aangemaakt worden in de PipeInsulation family!");
            }

            
        }

        /// <summary>
        /// Checks if there's allready insulation applied.
        /// </summary>
        /// <param name="pipe">the current pipe element</param>
        /// <param name="lookupValues">the insulation values from the access database</param>
        /// <returns>true if insulation is allready applied, otherwise false.</returns>
        public static bool IsPipeInsulationApplied(Pipe pipe, List<object> lookupValues)
        {
            string insulName = pipe.get_Parameter(BuiltInParameter.RBS_REFERENCE_INSULATION_TYPE).AsString();
            double insulThickness = Math.Round(pipe.get_Parameter(BuiltInParameter.RBS_REFERENCE_INSULATION_THICKNESS).AsDouble() * feetToMm, 2);

            if ((string)lookupValues[0] == insulName && Math.Round((double)lookupValues[1] * feetToMm, 2) == insulThickness)
                return true;

            return false;
        }

        /// <summary>
        /// Checks if there's allready insulation applied.
        /// </summary>
        /// <param name="fi">the current fitting element</param>
        /// <param name="lookupValues">the insulation values from the access database</param>
        /// <returns>true if insulation is allready applied, otherwise false.</returns>
        public static bool IsFittingInsulationApplied(FamilyInstance fi, List<object> lookupValues)
        {
            string insulName = fi.get_Parameter(BuiltInParameter.RBS_REFERENCE_INSULATION_TYPE).AsString();
            double insulThickness = Math.Round(fi.get_Parameter(BuiltInParameter.RBS_REFERENCE_INSULATION_THICKNESS).AsDouble() * feetToMm, 2);

            if ((string)lookupValues[0] == insulName && Math.Round((double)lookupValues[1] * feetToMm, 2) == insulThickness)
                return true;

            return false;
        }
    }
}
