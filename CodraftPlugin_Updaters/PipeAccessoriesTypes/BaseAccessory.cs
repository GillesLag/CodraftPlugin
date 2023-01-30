using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodraftPlugin_Updaters.PipeAccessoriesTypes
{
    public class BaseAccessory
    {
        public string ConnectionString { get; set; }
        public string DatabaseFilePath { get; set; }
        public string DatabaseFile { get; set; }
        public FamilyInstance PipeAccessory { get; set; }
        public Document Doc { get; set; }

        public BaseAccessory(FamilyInstance accessory, Document doc, string databaseMapPath)
        {
            this.PipeAccessory = accessory;
            this.Doc = doc;
            this.DatabaseFile = accessory.Symbol.FamilyName + ".mdb";
            this.DatabaseFilePath = databaseMapPath + this.DatabaseFile;
            this.ConnectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={databaseMapPath}{this.DatabaseFile}";
        }
    }
}
