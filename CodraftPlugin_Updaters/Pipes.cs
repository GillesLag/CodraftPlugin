using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using CodraftPlugin_Library;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Windows.Forms;

namespace CodraftPlugin_Updaters
{
    public class Pipes : IUpdater
    {
        private Guid _guid = new Guid("119D4855-D967-4DD0-AE69-0DB8B0C06296");
        private JObject parameterConfiguration;
<<<<<<< HEAD:CodraftPlugin/CodraftPlugin_Updaters/Pipes.cs
        private string globalParameterName = "RevitProjectMap";
=======
        private string globalParameterName;
>>>>>>> d6e0d3104505cbbcc016557f6e71b292ef6d28c5:CodraftPlugin_Updaters/Pipes.cs
        public UpdaterId Id { get; set; }

        public Pipes(AddInId addinId)
        {
            this.Id = new UpdaterId(addinId, _guid);
        }

        public void Execute(UpdaterData data)
        {
            Document doc = data.GetDocument();

            string projectMapPath;
            string databasesMapPath;
            string insulationDatabasePath;
            string textFilesMapPath;

            // Check globalparameter for projectfoldermap
<<<<<<< HEAD:CodraftPlugin/CodraftPlugin_Updaters/Pipes.cs
            var globalParameter = GlobalParametersManager.FindByName(doc, globalParameterName);
=======
            ElementId globalParameter = GlobalParametersManager.FindByName(doc, globalParameterName);
>>>>>>> d6e0d3104505cbbcc016557f6e71b292ef6d28c5:CodraftPlugin_Updaters/Pipes.cs

            if (globalParameter == ElementId.InvalidElementId)
            {
                projectMapPath = GlobalParameters.SetGlobalParameter(doc, globalParameterName);
            }
            else
            {
                GlobalParameter revitProjectMapParameter = (GlobalParameter)doc.GetElement(globalParameter);
                projectMapPath = ((StringParameterValue)revitProjectMapParameter.GetValue()).Value;
            }

            if (projectMapPath.Contains("(user)"))
            {
                string username = WindowsIdentity.GetCurrent().Name.Split('\\')[1];
                projectMapPath = projectMapPath.Replace("(user)", username);
            }

            databasesMapPath = projectMapPath + @"\RevitDatabases\";
            textFilesMapPath = projectMapPath + @"\RevitTextFiles\";
            insulationDatabasePath = databasesMapPath + @"Isolatie.accdb";

            using (StreamReader reader = File.OpenText(textFilesMapPath + "configuration.json"))
            {
                parameterConfiguration = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
            }

            // List of all pipesystems.
            List<PipingSystemType> pipingSystems = new FilteredElementCollector(doc)
                .OfClass(typeof(PipingSystemType))
                .Cast<PipingSystemType>()
                .ToList();

            List<ElementId> systemIds = pipingSystems.Select(x => x.Id).ToList();
            List<string> systemNames = pipingSystems.Select(x => x.Name).ToList();

            //
            // Update all added pipes.
            //
            foreach (ElementId pipeId in data.GetAddedElementIds())
            {
                try
                {
                    // Get pipe element.
                    Pipe pipe = (Pipe)doc.GetElement(pipeId);

                    // Get systemname from pipe
                    string systemName = pipe.Name.Split('%').First();

                    if (systemName.Contains("PI"))
                    {
                        systemName = systemName.Replace("PI", "PIS");
                    }

                    // Delete pipe if systemname does  not exist.
                    if (!systemNames.Contains(systemName))
                    {
                        // Delete pipe
                        doc.Delete(pipeId);
                        SendKeys.SendWait("{ESC}");

                        // Message for user.
                        TaskDialog td = new TaskDialog("PipeType of PipingSystem Fout")
                        {
                            MainInstruction = "De PipeType naam komt niet overeen met een PipingSystem naam",
                            ExpandedContent = "De PipeType naam moet overeenkomen met de PipingSystem naam\n" +
                            "voorbeeld PipeType naam: \'KW_Retour\'.\n" +
                            "Als je extra info in de PipeType naam wilt doe je dit door een \'%\' symbool\n toe te voegen." +
                            "Voorbeeld: \'KW_Retour%extra info...\'"
                        };
                        td.Show();

                        continue;
                    }

                    // Set all parameters.
                    pipe.SetSystemType(systemIds[systemNames.IndexOf(systemName)]);
                    ElementSettings.SetCodraftParametersPipe(pipe, parameterConfiguration);
                    pipe.LookupParameter("COD_Isolatie").Set(1);
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Pipe added element error", ex.Message);
                    continue;
                }
            }

            //
            // Update all modified pipes.
            // 
            foreach (ElementId pipeId in data.GetModifiedElementIds())
            {
                // Get pipe element
                Pipe pipe = (Pipe)doc.GetElement(pipeId);
                // Get the systemname from the pipe
                string systemName = pipe.Name.Split('%').First();

                // check if pipetype is changed.
                if (data.IsChangeTriggered(pipeId, Element.GetChangeTypeParameter(new ElementId(BuiltInParameter.ELEM_TYPE_PARAM))))
                {
                    try
                    {
                        // Set systemType to pipeType name
                        pipe.SetSystemType(systemIds[systemNames.IndexOf(systemName)]);
                    }
                    catch (Exception ex)
                    {
                        // If error occurs, set previous pipetype.
                        TaskDialog.Show("Pipe modified element error", ex.Message);
                        ElementSettings.SetPipeType(pipe, doc); ;
                    }
                }

                // check if systemtype is changed.
                if (data.IsChangeTriggered(pipeId, Element.GetChangeTypeParameter(new ElementId(BuiltInParameter.RBS_PIPING_SYSTEM_TYPE_PARAM))))
                {
                    try
                    {
                        // Set pipeType name to systemType name.
                        ElementSettings.SetPipeType(pipe, doc);
                    }
                    catch (Exception ex)
                    {
                        // If error occurs set previous systemType.
                        TaskDialog.Show("Pipe modified element error", ex.Message);
                        pipe.SetSystemType(systemIds[systemNames.IndexOf(systemName)]);
                    }
                }

                // check if height of pipe is changed.
                if (data.IsChangeTriggered(pipeId, Element.GetChangeTypeParameter(new ElementId(BuiltInParameter.RBS_START_OFFSET_PARAM)))
                    || data.IsChangeTriggered(pipeId, Element.GetChangeTypeParameter(new ElementId(BuiltInParameter.RBS_END_OFFSET_PARAM)))
                    || data.IsChangeTriggered(pipeId, Element.GetChangeTypeParameter(new ElementId(BuiltInParameter.RBS_PIPE_OUTER_DIAMETER))))
                {
                    // Set all parameters.
                    ElementSettings.SetCodraftParametersPipe(pipe, parameterConfiguration);
                }
            }
        }

        public string GetAdditionalInformation()
        {
            return "PipesUpdater";
        }

        public ChangePriority GetChangePriority()
        {
            return ChangePriority.MEPAccessoriesFittingsSegmentsWires;
        }

        public UpdaterId GetUpdaterId()
        {
            return this.Id;
        }

        public string GetUpdaterName()
        {
            return "PipeUpdater";
        }
    }
}
