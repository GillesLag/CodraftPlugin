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
        public List<object> RevitParameters { get; set; } = new List<object>();
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

            if (!parametersList.Any()) return false;

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
            this.PipeAccessory.LookupParameter("Motor_lengte").Set((double)this.Parameters[3]);
            this.PipeAccessory.LookupParameter("Buitendiameter").Set((double)this.Parameters[4]);
            this.PipeAccessory.LookupParameter("Uiteinde_1_type").Set((int)this.Parameters[5]);
            this.PipeAccessory.LookupParameter("Uiteinde_2_type").Set((int)this.Parameters[6]);
            this.PipeAccessory.LookupParameter("Uiteinde_1_maat").Set((double)this.Parameters[7]);
            this.PipeAccessory.LookupParameter("Uiteinde_2_maat").Set((double)this.Parameters[8]);
            this.PipeAccessory.LookupParameter("Uiteinde_1_lengte").Set((double)this.Parameters[9]);
            this.PipeAccessory.LookupParameter("Uiteinde_2_lengte").Set((double)this.Parameters[10]);
            this.PipeAccessory.LookupParameter("Motor_hoogte").Set((double)this.Parameters[11]);
            this.PipeAccessory.LookupParameter("Motor_breedte").Set((double)this.Parameters[12]);
            this.PipeAccessory.LookupParameter("COD_Fabrikant").Set((string)this.Parameters[13]);
            this.PipeAccessory.LookupParameter("COD_Type").Set((string)this.Parameters[14]);
            this.PipeAccessory.LookupParameter("COD_Materiaal").Set((string)this.Parameters[15]);
            this.PipeAccessory.LookupParameter("COD_Productcode").Set((string)this.Parameters[16]);
            this.PipeAccessory.LookupParameter("COD_Omschrijving").Set((string)this.Parameters[17]);
            this.PipeAccessory.LookupParameter("COD_Beschikbaar").Set((string)this.Parameters[18]);
        }

        public bool ParametersAreTheSame()
        {
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter("Lengte").AsDouble(), 4));
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter("Hoogte_operator").AsDouble(), 4));
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter("Hendel_lengte").AsDouble(), 4));
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter("Motor_lengte").AsDouble(), 4));
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter("Buitendiameter").AsDouble(), 4));
            this.RevitParameters.Add(this.PipeAccessory.LookupParameter("Uiteinde_1_type").AsInteger());
            this.RevitParameters.Add(this.PipeAccessory.LookupParameter("Uiteinde_2_type").AsInteger());
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter("Uiteinde_1_maat").AsDouble(), 4));
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter("Uiteinde_2_maat").AsDouble(), 4));
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter("Uiteinde_1_lengte").AsDouble(), 4));
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter("Uiteinde_2_lengte").AsDouble(), 4));
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter("Motor_hoogte").AsDouble(), 4));
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter("Motor_breedte").AsDouble(), 4));
            this.RevitParameters.Add(this.PipeAccessory.LookupParameter("COD_Fabrikant").AsString());
            this.RevitParameters.Add(this.PipeAccessory.LookupParameter("COD_Type").AsString());
            this.RevitParameters.Add(this.PipeAccessory.LookupParameter("COD_Materiaal").AsString());
            this.RevitParameters.Add(this.PipeAccessory.LookupParameter("COD_Productcode").AsString());
            this.RevitParameters.Add(this.PipeAccessory.LookupParameter("COD_Omschrijving").AsString());
            this.RevitParameters.Add(this.PipeAccessory.LookupParameter("COD_Beschikbaar").AsString());

            for (int i = 0; i < 5; i++)
            {
                
                if ((double)RevitParameters[i] != (double)Parameters[i])
                    return false;
            }

            for (int i = 5; i < 7; i++)
            {

                if ((int)RevitParameters[i] != (int)Parameters[i])
                    return false;
            }

            for (int i = 7; i < 13; i++)
            {

                if ((double)RevitParameters[i] != (double)Parameters[i])
                    return false;
            }

            for (int i = 13; i < 18; i++)
            {
                if ((string)RevitParameters[i] != (string)Parameters[i])
                    return false;
            }

            return true;
        }

        public void SetWrongValues()
        {
            this.PipeAccessory.LookupParameter("Lengte").Set(10 / feetToMm);
            this.PipeAccessory.LookupParameter("Hoogte_operator").Set(10 / feetToMm);
            this.PipeAccessory.LookupParameter("Hendel_lengte").Set(10 / feetToMm);
            this.PipeAccessory.LookupParameter("Buitendiameter").Set(10 / feetToMm);
            this.PipeAccessory.LookupParameter("Uiteinde_1_type").Set(0);
            this.PipeAccessory.LookupParameter("Uiteinde_2_type").Set(0);
            this.PipeAccessory.LookupParameter("Uiteinde_1_maat").Set(10 / feetToMm);
            this.PipeAccessory.LookupParameter("Uiteinde_2_maat").Set(10 / feetToMm);
            this.PipeAccessory.LookupParameter("Uiteinde_1_lengte").Set(10 / feetToMm);
            this.PipeAccessory.LookupParameter("Uiteinde_2_lengte").Set(10 / feetToMm);
            this.PipeAccessory.LookupParameter("Motor_hoogte").Set(10 / feetToMm);
            this.PipeAccessory.LookupParameter("Motor_breedte").Set(10 / feetToMm);
            this.PipeAccessory.LookupParameter("COD_Fabrikant").Set("BESTAAT NIET!");
            this.PipeAccessory.LookupParameter("COD_Type").Set("BESTAAT NIET!");
            this.PipeAccessory.LookupParameter("COD_Materiaal").Set("BESTAAT NIET!");
            this.PipeAccessory.LookupParameter("COD_Productcode").Set("BESTAAT NIET!");
            this.PipeAccessory.LookupParameter("COD_Omschrijving").Set("BESTAAT NIET!");
            this.PipeAccessory.LookupParameter("COD_Beschikbaar").Set("BESTAAT NIET!");
        }
    }
}
