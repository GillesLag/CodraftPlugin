using Autodesk.Revit.DB;
using CodraftPlugin_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodraftPlugin_Library;
using CodraftPlugin_PipeAccessoriesWPF;
using Newtonsoft.Json.Linq;

namespace CodraftPlugin_Updaters.PipeAccessoriesTypes
{
    public class StraightValve : BaseAccessory
    {
        public StraightValve(FamilyInstance accessory, Document doc, string databaseMapPath, JObject file) : base(accessory, doc, databaseMapPath, file)
        {
            this.Query = $"SELECT * " +
                $"FROM BMP_ValveStraightTbl " +
                $"WHERE Manufacturer = \"{this.Fabrikant}\" " +
                $"AND Type = \"{this.Type}\" " +
                $"AND D1 = {this.Dn}";

            this.QueryCount = $"SELECT COUNT(*) " +
                $"FROM BMP_ValveStraightTbl " +
                $"WHERE Manufacturer = \"{this.Fabrikant}\" " +
                $"AND Type = \"{this.Type}\" " +
                $"AND D1 = {this.Dn}";
        }

        public override bool? GetParams()
        {
            List<object> parametersList;

            if (FileOperationsPipeAccessories.LookupStraightValve(Query, QueryCount, ConnectionString, out parametersList, parameterConfiguration))
            {
                if (FileOperations.IsFound(CallingParams, RememberMeFilePath, out List<string> parameters))
                {
                    List<object> correctList = new List<object>();

                    correctList.AddRange(parameters.GetRange(0, 12).Select(x => (object)double.Parse(x)));
                    correctList.AddRange(parameters.GetRange(12, 2).Select(x => (object)int.Parse(x)));
                    correctList.AddRange(parameters.GetRange(14, 4).Select(x => (object)double.Parse(x)));
                    correctList.AddRange(parameters.GetRange(18, 6));

                    this.DatabaseParameters = correctList;
                    return true;
                }

                string typeName = this.ToString();
                string name = typeName.Substring(typeName.LastIndexOf('.') + 1);
                MainWindow accessoryWindow = new MainWindow(PipeAccessory, name, ConnectionString, Query, DatabaseFilePath, CallingParams, parameterConfiguration);
                accessoryWindow.ShowDialog();

                if (accessoryWindow.hasChosenAccessory)
                    return null;

                return false;
            }

            if (!parametersList.Any()) return false;

            this.DatabaseParameters = parametersList;
            return true;
        }

        public override bool ParametersAreTheSame()
        {
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_1"]["revit"]).AsDouble(), 4));
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_2"]["revit"]).AsDouble(), 4));
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_3"]["revit"]).AsDouble(), 4));
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_4"]["revit"]).AsDouble(), 4));
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_5"]["revit"]).AsDouble(), 4));
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_6"]["revit"]).AsDouble(), 4));
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_7"]["revit"]).AsDouble(), 4));
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_8"]["revit"]).AsDouble(), 4));
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_9"]["revit"]).AsDouble(), 4));
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_10"]["revit"]).AsDouble(), 4));
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_12"]["revit"]).AsDouble(), 4));
            this.RevitParameters.Add(this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_13"]["revit"]).AsInteger());
            this.RevitParameters.Add(this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_14"]["revit"]).AsInteger());
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_15"]["revit"]).AsDouble(), 4));
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_16"]["revit"]).AsDouble(), 4));
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_17"]["revit"]).AsDouble(), 4));
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_18"]["revit"]).AsDouble(), 4));
            this.RevitParameters.Add(this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_19"]["revit"]).AsString());
            this.RevitParameters.Add(this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_20"]["revit"]).AsString());
            this.RevitParameters.Add(this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_21"]["revit"]).AsString());
            this.RevitParameters.Add(this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_22"]["revit"]).AsString());
            this.RevitParameters.Add(this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_23"]["revit"]).AsString());
            this.RevitParameters.Add(this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_24"]["revit"]).AsString());

            return ElementSettings.CompareParameters(this.RevitParameters, this.DatabaseParameters);
        }

        public override void SetWrongValues()
        {
            this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_1"]["revit"]).Set(10 / feetToMm);
            this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_2"]["revit"]).Set(10 / feetToMm);
            this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_5"]["revit"]).Set(0);
            this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_6"]["revit"]).Set(0);
            this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_7"]["revit"]).Set(10 / feetToMm);
            this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_8"]["revit"]).Set(10 / feetToMm);
            this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_9"]["revit"]).Set(10 / feetToMm);
            this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_10"]["revit"]).Set(10 / feetToMm);
            this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_12"]["revit"]).Set(10 / feetToMm);
            this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_13"]["revit"]).Set(0);
            this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_14"]["revit"]).Set(0);
            this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_15"]["revit"]).Set(15 / feetToMm);
            this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_16"]["revit"]).Set(20 / feetToMm);
            this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_17"]["revit"]).Set(20 / feetToMm);
            this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_18"]["revit"]).Set(20 / feetToMm);
            this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_19"]["revit"]).Set("BESTAAT NIET!");
            this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_20"]["revit"]).Set("BESTAAT NIET!");
            this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_21"]["revit"]).Set("BESTAAT NIET!");
            this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_22"]["revit"]).Set("BESTAAT NIET!");
            this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_23"]["revit"]).Set("BESTAAT NIET!");
            this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["straightValve"]["property_24"]["revit"]).Set("BESTAAT NIET!");
        }
    }
}
