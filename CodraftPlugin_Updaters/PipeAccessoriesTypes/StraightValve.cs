using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodraftPlugin_Updaters.PipeAccessoriesTypes
{
    public class StraightValve : BaseAccessory
    {
        public StraightValve(FamilyInstance accessory, Document doc, string databaseMapPath) : base(accessory, doc, databaseMapPath)
        {
        }
    }
}
