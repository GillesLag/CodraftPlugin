using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using StartWerkomgevingWPF.Models;
using StartWerkomgevingWPF.DAL;
using System.Security.Principal;
using CodraftPlugin_Library;
using StartWerkomgevingWPF.Views;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.Attributes;

namespace StartWerkomgevingWPF.ViewModels
{
    public class MainViewModel : BasisViewModel
    {
        private string globalParameterName = "RevitProjectMap";
        private string projectMapPath;
        private string databasePath;
        private string connection = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"";

        private Document doc;
        private ObservableCollection<Medium> _mediums;
        private Medium _geselecteerdeMedium;

        public override string this[string columnName] { get { return ""; } }

        public override bool CanExecute(object parameter)
        {
            switch (parameter.ToString())
            {
                case "Aanpassen": return GeselecteerdeMedium != null;
                case "Update": return MediumsToUpdate.Any();
            }

            return true;
        }

        public override void Execute(object parameter)
        {
            switch (parameter.ToString())
            {
                case "Aanpassen": Aanpassen(); break;
                case "Update": Update(); break;
            }
        }

        public List<Medium> MediumsToUpdate { get; set; }

        public ObservableCollection<Medium> Mediums
        {
            get { return _mediums; }
            set { _mediums = value; NotifyPropertyChanged(nameof(Mediums));  }
        }

        public Medium GeselecteerdeMedium
        {
            get { return _geselecteerdeMedium; }
            set { _geselecteerdeMedium = value; NotifyPropertyChanged(nameof(GeselecteerdeMedium)); }
        }


        public MainViewModel(Document doc)
        {
            this.doc = doc;
            GetDatabasePath();

            connection += databasePath + "\"";
            Mediums = new ObservableCollection<Medium>(FileOperations.MediumsOphalen(connection).OrderBy(m => m.Id));
            MediumsToUpdate = new List<Medium>();
        }

        private void Update()
        {
            IEnumerable<PipingSystemType> pstList = GetElementTypesToUpdate<PipingSystemType>();

            Transaction t = new Transaction(doc, "Pipingsystem aanpassen");
            t.Start();

            foreach (PipingSystemType pst in pstList)
            {
                Medium medium = MediumsToUpdate.First(m => m.OrgineleNaam == pst.Name);

                pst.Name = medium.Naam;
                pst.Abbreviation = medium.Afkorting;
                pst.LineColor = new Color(medium.RGBKleur[0], medium.RGBKleur[1], medium.RGBKleur[2]);
            }

            t.Commit();

            IEnumerable<PipeType> ptList = GetElementTypesToUpdate<PipeType>();

            Transaction ptTrans = new Transaction(doc, "Pipetype aanpassen");
            ptTrans.Start();

            foreach (PipeType pt in ptList)
            {
                List<Medium> mediums = MediumsToUpdate.Where(m => m.OrgineleNaam == pt.Name.Substring(0, pt.Name.IndexOf('%'))).ToList();
                // TODO volledige pipetype naam meegeven. dit is de naam met de extra info.
                foreach (Medium m in mediums)
                {
                    pt.Name =  m.Naam + pt.Name.Substring(pt.Name.IndexOf('%'));

                    UpdateSizes(pt, m); 
                }
            }

            ptTrans.Commit();

            MediumsToUpdate.Clear();
        }

        private void Aanpassen()
        {
            AanpassenViewModel viewModel = new AanpassenViewModel(GeselecteerdeMedium, connection);
            AanpassenView view = new AanpassenView();
            view.DataContext = viewModel;

            if (viewModel.CloseActions == null)
                viewModel.CloseActions = new Action(view.Close);

            view.ShowDialog();

            MediumsToUpdate.Remove(GeselecteerdeMedium);
            Mediums.Remove(GeselecteerdeMedium);

            IEnumerable<Medium> mediumList = Mediums.Where(m => m.Naam == viewModel.Medium.OrgineleNaam);

            foreach (Medium medium in mediumList)
            {
                medium.Naam = viewModel.Medium.Naam;
                medium.Afkorting = viewModel.Medium.Afkorting;
                medium.Kleur = viewModel.Medium.Kleur;
                medium.RGBKleur = viewModel.Medium.RGBKleur;
            }

            Mediums.Add(viewModel.Medium);
            Mediums = new ObservableCollection<Medium>(Mediums.OrderBy(x => x.Id));

            MediumsToUpdate.Add(viewModel.Medium);
        }

        private void GetDatabasePath()
        {
            ElementId globalParameter = GlobalParametersManager.FindByName(doc, globalParameterName);

            if (globalParameter == ElementId.InvalidElementId)
            {
                projectMapPath = GlobalParameters.SetGlobalParameter(doc, globalParameterName);
            }
            else
            {
                GlobalParameter revitProjectMapParameter = (GlobalParameter)doc.GetElement(globalParameter);
                projectMapPath = ((StringParameterValue)revitProjectMapParameter.GetValue()).Value;
            }

            if (projectMapPath.Contains("(user)"))
            {
                string username = WindowsIdentity.GetCurrent().Name.Split('\\')[1];
                projectMapPath = projectMapPath.Replace("(user)", username);
            }

            databasePath = projectMapPath + @"\RevitDatabases\OpstartWerkomgeving_Revit.accdb";
        }

        private List<T> GetElementTypesToUpdate<T>() where T : ElementType
        {
            List<T> result = new List<T>();

            List<T> pstList = new FilteredElementCollector(doc)
                .OfClass(typeof(T))
                .Cast<T>().ToList(); ;

            foreach (Medium medium in MediumsToUpdate)
            {
                try
                {
                    List<T> pstList2 = new List<T>(pstList);
                    pstList2.RemoveAll(p => !(p.Name.Contains('%')));
                    result.Add(pstList2.First(p => p.Name.Substring(0, p.Name.IndexOf('%')) == medium.OrgineleNaam));
                }
                catch (Exception)
                {
                    result.Add(pstList.First(p => p.Name == medium.OrgineleNaam));
                }
            }

            return result;    
        }

        private void UpdateSizes(PipeType pt, Medium medium)
        {
            RoutingPreferenceRule segmentRule = pt.RoutingPreferenceManager.GetRule(RoutingPreferenceRuleGroupType.Segments, 0);
            string test = segmentRule.Description;
        }
    }
}
