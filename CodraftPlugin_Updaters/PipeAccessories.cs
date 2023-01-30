using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CodraftPlugin_DAL;
using CodraftPlugin_Exceptions;
using CodraftPlugin_Library;
using CodraftPlugin_Updaters.FittingTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodraftPlugin_Updaters
{
    public class PipeAccessories : IUpdater
    {
        private Guid _guid = new Guid("41494CDF-1377-434D-B8A5-C7D6A148D889");
        private List<ElementId> _familySubelementIds = new List<ElementId>();
        private List<ElementId> _addedElementids = new List<ElementId>();
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

            foreach (ElementId id in data.GetAddedElementIds())
            {
                FamilyInstance pipeAccessory = (FamilyInstance)doc.GetElement(id);

                IEnumerable<ElementId> subElementTypeIds = pipeAccessory.GetSubComponentIds().Select(x => ((FamilyInstance)doc.GetElement(x)).GetTypeId());
                _familySubelementIds.AddRange(subElementTypeIds);

                if (_familySubelementIds.Contains(pipeAccessory.GetTypeId()))
                    continue;

                _addedElementids.Add(pipeAccessory.Id);

                if (pipeAccessory)



                TaskDialog.Show("test", "hey");

            }

            foreach (ElementId id in data.GetModifiedElementIds())
            {
                FamilyInstance pipeAccessory = (FamilyInstance)doc.GetElement(id);
                IEnumerable<ElementId> subElementTypeIds = pipeAccessory.GetSubComponentIds().Select(x => ((FamilyInstance)doc.GetElement(x)).GetTypeId());
                _familySubelementIds.AddRange(subElementTypeIds);

                if (_familySubelementIds.Contains(pipeAccessory.GetTypeId()))
                    continue;

                if (_addedElementids.Contains(pipeAccessory.Id))
                    continue;

                TaskDialog.Show("test", "hey");
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
