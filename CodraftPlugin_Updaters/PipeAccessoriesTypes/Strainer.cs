using Autodesk.Revit.DB;
using CodraftPlugin_DAL;
using CodraftPlugin_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodraftPlugin_Updaters.PipeAccessoriesTypes
{
    public class Strainer : BaseAccessory
    {
        public Strainer(FamilyInstance accessory, Document doc, string databaseMapPath) : base(accessory, doc, databaseMapPath)
        {
            this.Query = $"SELECT * " +
                $"FROM BMP_StrainerTbl " +
                $"WHERE Manufacturer = \"{this.Fabrikant}\" " +
                $"AND Type = \"{this.Type}\" " +
                $"AND D1 = {this.Dn}";

            this.QueryCount = $"SELECT COUNT(*) " +
                $"FROM BMP_StrainerTbl " +
                $"WHERE Manufacturer = \"{this.Fabrikant}\" " +
                $"AND Type = \"{this.Type}\" " +
                $"AND D1 = {this.Dn}";
        }

        public override void CreateAccessory()
        {
            this.PipeAccessory.LookupParameter("Nominale_diameter").Set((double)this.DatabaseParameters[0]);
            this.PipeAccessory.LookupParameter("Buitendiameter").Set((double)this.DatabaseParameters[1]);
            this.PipeAccessory.LookupParameter("Hoogte").Set((double)this.DatabaseParameters[2]);
            this.PipeAccessory.LookupParameter("Lengte").Set((double)this.DatabaseParameters[3]);
            this.PipeAccessory.LookupParameter("Offset").Set((double)this.DatabaseParameters[4]);
            this.PipeAccessory.LookupParameter("Uiteinde_1_type").Set((int)this.DatabaseParameters[5]);
            this.PipeAccessory.LookupParameter("Uiteinde_2_type").Set((int)this.DatabaseParameters[6]);
            this.PipeAccessory.LookupParameter("Uiteinde_1_lengte").Set((double)this.DatabaseParameters[7]);
            this.PipeAccessory.LookupParameter("Uiteinde_2_lengte").Set((double)this.DatabaseParameters[8]);
            this.PipeAccessory.LookupParameter("Uiteinde_1_maat").Set((double)this.DatabaseParameters[9]);
            this.PipeAccessory.LookupParameter("Uiteinde_2_maat").Set((double)this.DatabaseParameters[10]);
            this.PipeAccessory.LookupParameter("COD_Fabrikant").Set((string)this.DatabaseParameters[11]);
            this.PipeAccessory.LookupParameter("COD_Type").Set((string)this.DatabaseParameters[12]);
            this.PipeAccessory.LookupParameter("COD_Materiaal").Set((string)this.DatabaseParameters[13]);
            this.PipeAccessory.LookupParameter("COD_Productcode").Set((string)this.DatabaseParameters[14]);
            this.PipeAccessory.LookupParameter("COD_Omschrijving").Set((string)this.DatabaseParameters[15]);
            this.PipeAccessory.LookupParameter("COD_Beschikbaar").Set((string)this.DatabaseParameters[16]);
            this.PipeAccessory.LookupParameter("COD_Maat_annotatie").Set((string)this.DatabaseParameters[17]);
        }

        public override bool GetParams()
        {
            List<object> parametersList;

            if (FileOperationsPipeAccessories.LookupStrainer(Query, QueryCount, ConnectionString, out parametersList))
            {
                //TODO meerdere keuzes van de database afhandelen.
            }

            if (!parametersList.Any()) return false;

            this.DatabaseParameters = parametersList;
            return true;
        }

        public override bool ParametersAreTheSame()
        {
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter("Nominale_diameter").AsDouble(), 4));
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter("Buitendiameter").AsDouble(), 4));
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter("Hoogte").AsDouble(), 4));
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter("Lengte").AsDouble(), 4));
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter("Offset").AsDouble(), 4));
            this.RevitParameters.Add(this.PipeAccessory.LookupParameter("Uiteinde_1_type").AsInteger());
            this.RevitParameters.Add(this.PipeAccessory.LookupParameter("Uiteinde_2_type").AsInteger());
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter("Uiteinde_1_maat").AsDouble(), 4));
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter("Uiteinde_2_maat").AsDouble(), 4));
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter("Uiteinde_1_lengte").AsDouble(), 4));
            this.RevitParameters.Add(Math.Round(this.PipeAccessory.LookupParameter("Uiteinde_2_lengte").AsDouble(), 4));
            this.RevitParameters.Add(this.PipeAccessory.LookupParameter("COD_Fabrikant").AsString());
            this.RevitParameters.Add(this.PipeAccessory.LookupParameter("COD_Type").AsString());
            this.RevitParameters.Add(this.PipeAccessory.LookupParameter("COD_Materiaal").AsString());
            this.RevitParameters.Add(this.PipeAccessory.LookupParameter("COD_Productcode").AsString());
            this.RevitParameters.Add(this.PipeAccessory.LookupParameter("COD_Omschrijving").AsString());
            this.RevitParameters.Add(this.PipeAccessory.LookupParameter("COD_Beschikbaar").AsString());
            this.RevitParameters.Add(this.PipeAccessory.LookupParameter("COD_Maat_annotatie").AsString());

            return ElementSettings.CompareParameters(this.RevitParameters, this.DatabaseParameters);
        }

        public override void SetWrongValues()
        {
            this.PipeAccessory.LookupParameter("Nominale_diameter").Set(10 / feetToMm);
            this.PipeAccessory.LookupParameter("Buitendiameter").Set(10 / feetToMm);
            this.PipeAccessory.LookupParameter("Hoogte").Set(10 / feetToMm);
            this.PipeAccessory.LookupParameter("Lengte").Set(10 / feetToMm);
            this.PipeAccessory.LookupParameter("Offset").Set(0);
            this.PipeAccessory.LookupParameter("Uiteinde_1_type").Set(0);
            this.PipeAccessory.LookupParameter("Uiteinde_2_type").Set(0);
            this.PipeAccessory.LookupParameter("Uiteinde_1_maat").Set(10 / feetToMm);
            this.PipeAccessory.LookupParameter("Uiteinde_2_maat").Set(10 / feetToMm);
            this.PipeAccessory.LookupParameter("Uiteinde_1_lengte").Set(10 / feetToMm);
            this.PipeAccessory.LookupParameter("Uiteinde_2_lengte").Set(10 / feetToMm);
            this.PipeAccessory.LookupParameter("COD_Fabrikant").Set("BESTAAT NIET!");
            this.PipeAccessory.LookupParameter("COD_Type").Set("BESTAAT NIET!");
            this.PipeAccessory.LookupParameter("COD_Materiaal").Set("BESTAAT NIET!");
            this.PipeAccessory.LookupParameter("COD_Productcode").Set("BESTAAT NIET!");
            this.PipeAccessory.LookupParameter("COD_Omschrijving").Set("BESTAAT NIET!");
            this.PipeAccessory.LookupParameter("COD_Beschikbaar").Set("BESTAAT NIET!");
            this.PipeAccessory.LookupParameter("COD_Maat_annotatie").Set("BESTAAT NIET!");
        }
    }
}
