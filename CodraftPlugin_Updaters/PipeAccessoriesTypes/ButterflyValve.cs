using Autodesk.Revit.DB;
using CodraftPlugin_DAL;
using CodraftPlugin_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodraftPlugin_PipeAccessoriesWPF;
using Newtonsoft.Json.Linq;

namespace CodraftPlugin_Updaters.PipeAccessoriesTypes
{
    public class ButterflyValve : BaseAccessory
    {
        public ButterflyValve(FamilyInstance accessory, Document doc, string databaseMapPath, JObject file) : base(accessory, doc, databaseMapPath, file)
        {
            this.Query = $"SELECT * " +
                $"FROM {(string)parameterConfiguration["parameters"]["butterflyValve"]["property_19"]["database"]} " +
                $"WHERE {(string)parameterConfiguration["parameters"]["butterflyValve"]["property_13"]["database"]} = \"{this.Fabrikant}\" " +
                $"AND {(string)parameterConfiguration["parameters"]["butterflyValve"]["property_14"]["database"]} = \"{this.Type}\" " +
                $"AND {(string)parameterConfiguration["parameters"]["butterflyValve"]["property_3"]["database"]} = {this.Dn}";

            this.QueryCount = $"SELECT COUNT(*) " +
                $"FROM {(string)parameterConfiguration["parameters"]["butterflyValve"]["property_19"]["database"]} " +
                $"WHERE {(string)parameterConfiguration["parameters"]["butterflyValve"]["property_13"]["database"]} = \"{this.Fabrikant}\" " +
                $"AND {(string)parameterConfiguration["parameters"]["butterflyValve"]["property_14"]["database"]} = \"{this.Type}\" " +
                $"AND {(string)parameterConfiguration["parameters"]["butterflyValve"]["property_3"]["database"]} = {this.Dn}";
        }

        public override bool? GetParams()
        {
            List<object> parametersList;

            if (FileOperationsPipeAccessories.LookupButterflyValve(Query, QueryCount, ConnectionString, out parametersList, parameterConfiguration))
            {
                if (FileOperations.IsFound(CallingParams, RememberMeFilePath, out List<string> parameters))
                {
                    List<object> correctList = new List<object>();

                    correctList.AddRange(parameters.GetRange(0, 12).Select(x => (object)double.Parse(x)));
                    correctList.AddRange(parameters.GetRange(12, 6));

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
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["butterflyValve"]["property_1"]["revit"]).AsDouble(), 4));
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["butterflyValve"]["property_2"]["revit"]).AsDouble(), 4));
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["butterflyValve"]["property_3"]["revit"]).AsDouble(), 4));
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["butterflyValve"]["property_4"]["revit"]).AsDouble(), 4));
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["butterflyValve"]["property_5"]["revit"]).AsDouble(), 4));
            this.RevitParameters.Add(this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["butterflyValve"]["property_6"]["revit"]).AsDouble());
            this.RevitParameters.Add(this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["butterflyValve"]["property_7"]["revit"]).AsDouble());
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["butterflyValve"]["property_8"]["revit"]).AsDouble(), 4));
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["butterflyValve"]["property_9"]["revit"]).AsDouble(), 4));
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["butterflyValve"]["property_10"]["revit"]).AsDouble(), 4));
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["butterflyValve"]["property_11"]["revit"]).AsDouble(), 4));
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["butterflyValve"]["property_12"]["revit"]).AsDouble(), 4));
            this.RevitParameters.Add(this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["butterflyValve"]["property_13"]["revit"]).AsString());
            this.RevitParameters.Add(this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["butterflyValve"]["property_14"]["revit"]).AsString());
            this.RevitParameters.Add(this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["butterflyValve"]["property_15"]["revit"]).AsString());
            this.RevitParameters.Add(this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["butterflyValve"]["property_16"]["revit"]).AsString());
            this.RevitParameters.Add(this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["butterflyValve"]["property_17"]["revit"]).AsString());
            this.RevitParameters.Add(this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["butterflyValve"]["property_18"]["revit"]).AsString());

            return ElementSettings.CompareParameters(this.RevitParameters, this.DatabaseParameters);
        }

        public override void SetWrongValues()
        {
            this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["butterflyValve"]["property_1"]["revit"]).Set(10 / feetToMm);
            this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["butterflyValve"]["property_2"]["revit"]).Set(10 / feetToMm);
            this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["butterflyValve"]["property_3"]["revit"]).Set(10 / feetToMm);
            this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["butterflyValve"]["property_4"]["revit"]).Set(10 / feetToMm);
            this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["butterflyValve"]["property_5"]["revit"]).Set(0);
            this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["butterflyValve"]["property_6"]["revit"]).Set(0);
            this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["butterflyValve"]["property_7"]["revit"]).Set(10 / feetToMm);
            this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["butterflyValve"]["property_8"]["revit"]).Set(10 / feetToMm);
            this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["butterflyValve"]["property_9"]["revit"]).Set(10 / feetToMm);
            this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["butterflyValve"]["property_10"]["revit"]).Set(10 / feetToMm);
            this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["butterflyValve"]["property_11"]["revit"]).Set(15 / feetToMm);
            this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["butterflyValve"]["property_12"]["revit"]).Set(20 / feetToMm);
            this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["butterflyValve"]["property_13"]["revit"]).Set("BESTAAT NIET!");
            this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["butterflyValve"]["property_14"]["revit"]).Set("BESTAAT NIET!");
            this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["butterflyValve"]["property_15"]["revit"]).Set("BESTAAT NIET!");
            this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["butterflyValve"]["property_16"]["revit"]).Set("BESTAAT NIET!");
            this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["butterflyValve"]["property_17"]["revit"]).Set("BESTAAT NIET!");
            this.PipeAccessory.LookupParameter((string)parameterConfiguration["parameters"]["butterflyValve"]["property_18"]["revit"]).Set("BESTAAT NIET!");
        }
    }
}
