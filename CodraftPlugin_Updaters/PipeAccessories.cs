using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CodraftPlugin_DAL;
using CodraftPlugin_Exceptions;
using CodraftPlugin_Library;
using CodraftPlugin_Updaters.PipeAccessoriesTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodraftPlugin_Updaters
{
    public class PipeAccessories : IUpdater
    {
        private Guid _guid = new Guid("41494CDF-1377-434D-B8A5-C7D6A148D889");
        private List<ElementId> _familySubelementIds = new List<ElementId>();
        private List<ElementId> _updatedElementids = new List<ElementId>();
        private string _pipeAccessoryName;
        private StraightValve straightValve;
        private Guid failureGuidPipeAccessories = new Guid("4B81D4C5-185C-4830-8ECF-67370ADB06B0");

        public UpdaterId Id { get; set; }

        public PipeAccessories(AddInId addinId)
        {
            this.Id = new UpdaterId(addinId, _guid);
        }

        public void Execute(UpdaterData data)
        {
            Document doc = data.GetDocument();
            string projectMapPath = doc.PathName;
            string databasesMapPath = projectMapPath.Substring(0, projectMapPath.LastIndexOf('\\') + 1) + @"RevitDatabases\";
            string textFilesMapPath = projectMapPath.Substring(0, projectMapPath.LastIndexOf("\\") + 1) + @"RevitTextFiles\";
            FailureDefinitionId warning = new FailureDefinitionId(failureGuidPipeAccessories);
            FailureMessage fm = new FailureMessage(warning);

            foreach (ElementId id in data.GetAddedElementIds())
            {
                FamilyInstance pipeAccessory = (FamilyInstance)doc.GetElement(id);
                _pipeAccessoryName = pipeAccessory.Symbol.FamilyName;

                if (!_pipeAccessoryName.Contains("COD"))
                    continue;

                IEnumerable<ElementId> subElementTypeIds = pipeAccessory.GetSubComponentIds().Select(x => ((FamilyInstance)doc.GetElement(x)).GetTypeId());
                _familySubelementIds.AddRange(subElementTypeIds);

                if (_familySubelementIds.Contains(pipeAccessory.GetTypeId()))
                {
                    _familySubelementIds.Remove(pipeAccessory.GetTypeId());
                    continue;
                }

                _updatedElementids.Add(pipeAccessory.Id);

                try
                {
                    switch (_pipeAccessoryName)
                    {
                        case "COD_KOGELKRAAN":

                            straightValve = new StraightValve(pipeAccessory, doc, databasesMapPath);

                            if (!straightValve.GetParams())
                            {
                                doc.PostFailure(fm);
                                straightValve.SetWrongValues();
                                continue;
                            }

                            straightValve.CreateAccessory();

                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("test", ex.Message);
                }
            }

            foreach (ElementId id in data.GetModifiedElementIds())
            {
                FamilyInstance pipeAccessory = (FamilyInstance)doc.GetElement(id);
                _pipeAccessoryName = pipeAccessory.Symbol.FamilyName;
                IEnumerable<ElementId> subElementTypeIds = pipeAccessory.GetSubComponentIds().Select(x => ((FamilyInstance)doc.GetElement(x)).GetTypeId());
                _familySubelementIds.AddRange(subElementTypeIds);

                if (_familySubelementIds.Contains(pipeAccessory.GetTypeId()))
                {
                    _familySubelementIds.Remove(pipeAccessory.GetTypeId());
                    continue;
                }

                if (_updatedElementids.Contains(pipeAccessory.Id))
                {
                    _updatedElementids.Remove(pipeAccessory.Id);
                    continue;
                }

                try
                {
                    switch (_pipeAccessoryName)
                    {
                        case "COD_KOGELKRAAN":

                            straightValve = new StraightValve(pipeAccessory, doc, databasesMapPath);

                            if (!straightValve.GetParams())
                            {
                                doc.PostFailure(fm);
                                straightValve.SetWrongValues();
                                continue;
                            }

                            if (straightValve.ParametersAreTheSame())
                                continue;

                            straightValve.CreateAccessory();

                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("test", ex.Message);
                }

            }
        }

        public string GetAdditionalInformation()
        {
            return "PipeAccessoryUpdater";
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
            return "PipeAccessoryUpdater";
        }
    }
}
