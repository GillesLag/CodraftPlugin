using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using StartWerkomgevingWPF.DAL;
using StartWerkomgevingWPF.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Color = System.Windows.Media.Color;

namespace StartWerkomgevingWPF.ViewModels
{
    public class AanpassenViewModel : BasisViewModel
    {
        private string connectionString;

        private List<string> _fabrikanten;
        private string _geselecteerdeFabrikant;
        private List<string> _types;
        private string _geselecteerdeType;
        private List<double> _nominaleDiameters1;
        private List<double> _nominaleDiameters2;
        private double _geselecteerdeNominaleDiameter1;
        private double _geselecteerdeNominaleDiameter2;
        private string _kleur;
        private string _naam;
        private string _afkoring;

        public override string this[string columnName] { get { return ""; } }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            Oke();
        }

        public string Naam
        {
            get { return _naam; }
            set { _naam = value; NotifyPropertyChanged(nameof(Naam)); }
        }

        public string Afkorting
        {
            get { return _afkoring; }
            set { _afkoring = value; NotifyPropertyChanged(nameof(Afkorting)); }
        }

        public List<string> Fabrikanten
        {
            get { return _fabrikanten; }
            set { _fabrikanten = value; NotifyPropertyChanged(nameof(Fabrikanten)); }
        }

        public string GeselecteerdeFabrikant
        {
            get { return _geselecteerdeFabrikant; }
            set 
            {
                _geselecteerdeFabrikant = value;
                Types = FileOperations.TypesOphalen(connectionString, _geselecteerdeFabrikant);
                GeselecteerdeType = null;
                NotifyPropertyChanged(nameof(GeselecteerdeFabrikant)); 
            }
        }

        public List<string> Types
        {
            get { return _types; }
            set 
            {
                _types = value;
                GeselecteerdeNominaleDiameter1 = 0;
                NotifyPropertyChanged(nameof(Types));
            }
        }

        public string GeselecteerdeType
        {
            get { return _geselecteerdeType; }
            set 
            { 
                _geselecteerdeType = value;
                NominaleDiameters1 = FileOperations.Dn1Ophalen(connectionString, GeselecteerdeFabrikant, _geselecteerdeType);
                GeselecteerdeNominaleDiameter1 = 0;
                NotifyPropertyChanged(nameof(GeselecteerdeType));
            }
        }

        public List<double> NominaleDiameters1
        {
            get { return _nominaleDiameters1; }
            set 
            { 
                _nominaleDiameters1 = value;
                GeselecteerdeNominaleDiameter2 = 0;
                NotifyPropertyChanged(nameof(NominaleDiameters1));
            }
        }
        public List<double> NominaleDiameters2
        {
            get { return _nominaleDiameters2; }
            set { _nominaleDiameters2 = value; NotifyPropertyChanged(nameof(NominaleDiameters2)); }
        }

        public double GeselecteerdeNominaleDiameter1
        {
            get { return _geselecteerdeNominaleDiameter1; }
            set 
            {
                _geselecteerdeNominaleDiameter1 = value;
                NominaleDiameters2 = FileOperations.Dn2Ophalen(connectionString, GeselecteerdeFabrikant, GeselecteerdeType, _geselecteerdeNominaleDiameter1);
                NotifyPropertyChanged(nameof(GeselecteerdeNominaleDiameter1));
            }
        }

        public double GeselecteerdeNominaleDiameter2
        {
            get { return _geselecteerdeNominaleDiameter2; }
            set { _geselecteerdeNominaleDiameter2 = value; NotifyPropertyChanged(nameof(GeselecteerdeNominaleDiameter2)); }
        }

        public string Kleur
        {
            get { return _kleur; }
            set { _kleur = value; NotifyPropertyChanged(nameof(Kleur)); }
        }

        public Action CloseActions { get; set; }

        public Medium Medium { get; set; }

        public AanpassenViewModel(Medium  medium, string connectionString)
        {
            Medium = medium;
            this.connectionString = connectionString;

            Naam = medium.Naam;
            Afkorting = medium.Afkorting;
            GeselecteerdeFabrikant = medium.Fabrikant;
            GeselecteerdeType = medium.Type;
            GeselecteerdeNominaleDiameter1 = medium.NominaleDiameter1;
            GeselecteerdeNominaleDiameter2 = medium.NominaleDiameter2;
            Kleur = medium.Kleur;

            Fabrikanten = FileOperations.FabrikantenOphalen(connectionString);
        }

        private void Oke()
        {
            Medium.Naam = Naam;
            Medium.Afkorting = Afkorting;
            Medium.Fabrikant = GeselecteerdeFabrikant;
            Medium.Type = GeselecteerdeType;
            Medium.NominaleDiameter1 = GeselecteerdeNominaleDiameter1;
            Medium.NominaleDiameter2 = GeselecteerdeNominaleDiameter2;
            Medium.Kleur = Kleur;
            Medium.RGBKleur = RgbKleurConverter(Kleur);

            CloseActions();
        }

        private List<byte> RgbKleurConverter(string hexKleur)
        {
            hexKleur = hexKleur.Substring(1);

            if (hexKleur.Length > 7)
                hexKleur = hexKleur.Substring(2);

            byte red = byte.Parse(hexKleur.Substring(0, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            byte green = byte.Parse(hexKleur.Substring(2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            byte blue = byte.Parse(hexKleur.Substring(4, 2), System.Globalization.NumberStyles.AllowHexSpecifier);

            return new List<byte>()
            {
                red,
                green, 
                blue
            };
        }
    }
}
