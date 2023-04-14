using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB.Plumbing;
using System.Windows.Markup.Localizer;
using Autodesk.Revit.UI.Events;
using System.Windows.Input;

namespace CodraftPlugin_Loading
{
    [Transaction(TransactionMode.Manual)]
    public class ChangeTagColor : IExternalCommand
    {
        private bool _isRunning = true;
        private UIApplication _app;
        private UIDocument _uidoc;
        private Document _doc;
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            _app = commandData.Application;
            _uidoc = _app.ActiveUIDocument;
            _doc = _uidoc.Document;

            _app.Idling += OnIdle;

            return Result.Succeeded;
        }

        private void OnIdle(object sender, IdlingEventArgs e)
        {
            if (!_isRunning)
            {
                _app.Idling -= OnIdle;
            }

            if (_isRunning)
            {
                try
                {
                    Reference refTag = _uidoc.Selection.PickObject(ObjectType.Element, new SelectionFilter(), "Selecteer een pipe of duct tag");
                    IndependentTag tagElement = (IndependentTag)_doc.GetElement(refTag);
                    BuiltInCategory bic = (BuiltInCategory)tagElement.Category.Id.IntegerValue;

                    IEnumerable<View> viewCollector = new FilteredElementCollector(_doc)
                        .OfClass(typeof(View))
                        .Cast<View>();

                    Transaction t = new Transaction(_doc, "change tag color");
                    t.Start();

                    foreach (View view in viewCollector)
                    {
                        if (view.IsTemplate)
                        {
                            continue;
                        }

                        IEnumerable<IndependentTag> allTagsInview = new FilteredElementCollector(_doc, view.Id)
                        .WhereElementIsNotElementType()
                        .OfCategory(bic)
                        .Cast<IndependentTag>();

                        if (!allTagsInview.Any())
                        {
                            continue;
                        }

                        foreach (IndependentTag tag in allTagsInview)
                        {
                            Element element = tag.GetTaggedLocalElement();
                            if (element == null)
                            {
                                continue;
                            }

                            ElementId pstId = element.GetParameters("System Type").First().AsElementId();
                            MEPSystemType mst = (MEPSystemType)_doc.GetElement(pstId);
                            Color mstColor = mst.LineColor;

                            OverrideGraphicSettings overrideSettings = new OverrideGraphicSettings();
                            overrideSettings.SetProjectionLineColor(mstColor);

                            view.SetElementOverrides(tag.Id, overrideSettings);
                        }
                    }

                    t.Commit();
                }
                catch (Exception)
                {
                    _isRunning = false;
                }
            }
        }
    }

    public class SelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            if (elem.Category == null)
            {
                return false;
            }

            return elem.Category.CategoryType == CategoryType.Annotation && elem.Category.Name.Contains("Tags")
                && (elem.Category.Name.Contains("Duct") || elem.Category.Name.Contains("Pipe"));
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return true;
        }
    }
}
