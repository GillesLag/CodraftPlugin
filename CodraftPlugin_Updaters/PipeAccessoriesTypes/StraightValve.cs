using Autodesk.Revit.DB;
using CodraftPlugin_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodraftPlugin_Updaters.PipeAccessoriesTypes
{
    public class StraightValve : BaseAccessory
    {
        private const double feetToMm = 304.8;
        public double Dn { get; private set; }
        public string Fabrikant { get; private set; }
        public string Type { get; private set; }
        public string Query { get; private set; }
        public string QueryCount { get; private set; }
        public StraightValve(FamilyInstance accessory, Document doc, string databaseMapPath) : base(accessory, doc, databaseMapPath)
        {
            ConnectorSetIterator iterator = accessory.MEPModel.ConnectorManager.Connectors.ForwardIterator();

            while (iterator.MoveNext())
            {
                Connector connector = (Connector)iterator.Current;
                this.Dn = Math.Round(connector.Radius * 2 * feetToMm, 2);
                break;
            }

            string fabrikantType = accessory.Name.Substring(0, accessory.Name.IndexOf('%'));

            this.Fabrikant = fabrikantType.Substring(0, fabrikantType.IndexOf('_'));
            this.Type = fabrikantType.Substring(fabrikantType.IndexOf('_') + 1);

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

        public bool GetParams(out List<object> parameters)
        {
            List<object> parametersList;

            if (FileOperationsPipeAccessories.LookupStraightValve(Query, QueryCount, ConnectionString, out parametersList))
            {
                //TODO meerdere keuzes van de database afhandelen.
            }

            parameters = parametersList;
            return true;
        }

        public void CreateAccessory(List<object> parameters)
        {
            throw new NotImplementedException();
        }
    }
}
