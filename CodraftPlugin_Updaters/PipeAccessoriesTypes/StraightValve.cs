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
        public List<object> Parameters { get; set; }
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

        public bool GetParams()
        {
            List<object> parametersList;

            if (FileOperationsPipeAccessories.LookupStraightValve(Query, QueryCount, ConnectionString, out parametersList))
            {
                //TODO meerdere keuzes van de database afhandelen.
            }

            this.Parameters = parametersList;
            return true;
        }

        public void CreateAccessory()
        {
            if (this.Parameters.Count == 0)
                throw new Exception("Geen parameters voor straightvalve!");

            this.PipeAccessory.LookupParameter("Lengte").Set((double)this.Parameters[0]);
            this.PipeAccessory.LookupParameter("Hoogte_operator").Set((double)this.Parameters[1]);
            this.PipeAccessory.LookupParameter("Hendel_lengte").Set((double)this.Parameters[2]);
            this.PipeAccessory.LookupParameter("Buitendiameter").Set((double)this.Parameters[3]);
            this.PipeAccessory.LookupParameter("Uiteinde_1_type").Set((double)this.Parameters[4]);
            this.PipeAccessory.LookupParameter("Uiteinde_2_type").Set((double)this.Parameters[5]);
            this.PipeAccessory.LookupParameter("Uiteinde_1_maat").Set((double)this.Parameters[6]);
            this.PipeAccessory.LookupParameter("Uiteinde_2_maat").Set((double)this.Parameters[7]);
            this.PipeAccessory.LookupParameter("Uiteinde_1_lengte").Set((double)this.Parameters[8]);
            this.PipeAccessory.LookupParameter("Uiteinde_2_lengte").Set((double)this.Parameters[9]);
            this.PipeAccessory.LookupParameter("Motor_hoogte").Set((double)this.Parameters[10]);
            this.PipeAccessory.LookupParameter("Motor_breedte").Set((double)this.Parameters[11]);
            this.PipeAccessory.LookupParameter("COD_Fabrikant").Set((string)this.Parameters[12]);
            this.PipeAccessory.LookupParameter("COD_Type").Set((string)this.Parameters[13]);
            this.PipeAccessory.LookupParameter("COD_Materiaal").Set((string)this.Parameters[14]);
            this.PipeAccessory.LookupParameter("COD_Productcode").Set((string)this.Parameters[15]);
            this.PipeAccessory.LookupParameter("COD_Omschrijving").Set((string)this.Parameters[16]);
            this.PipeAccessory.LookupParameter("COD_Beschikbaar").Set((string)this.Parameters[17]);
        }
    }
}
