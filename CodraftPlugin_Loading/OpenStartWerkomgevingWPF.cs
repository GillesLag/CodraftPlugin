using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using StartWerkomgevingWPF;
using StartWerkomgevingWPF.Views;


namespace CodraftPlugin_Loading
{
    [Transaction(TransactionMode.Manual)]
    public class OpenStartWerkomgevingWPF : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            MainView window = new MainView(commandData.Application.ActiveUIDocument.Document);

            window.ShowDialog();

            return Result.Succeeded;
        }
    }
}
