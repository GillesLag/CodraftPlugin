using Autodesk.Revit.DB;
using CodraftPlugin_DAL;
using CodraftPlugin_Library;
using CodraftPlugin_UIDatabaseWPF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.OleDb;
using System.Windows;
using System.Windows.Input;
using Autodesk.Revit.UI;
using Newtonsoft.Json.Linq;
using JObject = Newtonsoft.Json.Linq.JObject;

namespace CodraftPlugin_UIDatabaseWPF.ModelView
{
    public class FittingModelView : INotifyPropertyChanged
    {
        const float feetToMm = 304.8f;
        #region Private Fields

        private string connectionString;
        private string query;
        private string databaseFilePath;
        private FamilyInstance fittingModel;
        private bool _RememberChoice;
        private bool _switchNd;
        private int _excentrisch;
        private string rememberMeFile;
        private List<string> parameters;
        private double maxDiameter;
        private JObject paramterConfiguration;

        #endregion

        #region Properties

        public ObservableCollection<FittingModel> Fittings { get; set; } = new ObservableCollection<FittingModel>();
        public ICommand RefreshCommand => new RelayCommand(Refresh);
        public ICommand OpenDatabaseCommand => new RelayCommand(OpenDatabase);
        public Action CloseWindow { get; set; }
        public ICommand OkeCommand => new RelayCommand(Oke);
        public ICommand CloseCommand => new RelayCommand(CloseWindow);
        public bool RememberChoice
        {
            get { return _RememberChoice; }
            set
            {
                _RememberChoice = value;

                OnPropertyChanged(nameof(RememberChoice));
            }
        }

        #endregion

        #region Constructors

        public FittingModelView(string connectionString, string databaseFilePath, string rememberMeFilePath, string strSQL, FamilyInstance fitting,
            List<string> parameters, bool switchNd, int excentrisch, JObject file, double maxDiameter = 0)
        {
            this.connectionString = connectionString;
            this.databaseFilePath = databaseFilePath;
            this.query = strSQL;
            this.fittingModel = fitting;
            this.rememberMeFile = rememberMeFilePath;
            this.parameters = parameters;
            this._switchNd = switchNd;
            this._excentrisch = excentrisch;
            this.maxDiameter = maxDiameter;
            this.paramterConfiguration = file;

            FillList();
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        /// <summary>
        /// Fill the datatable with information from the database
        /// </summary>
        public void FillList()
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    OleDbCommand command = new OleDbCommand(query, connection);
                    connection.Open();

                    try
                    {
                        FittingModel fitting = null;
                        var test = fittingModel.Name;

                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                switch (fittingModel.Name)
                                {
                                    case "Elbow":
                                        fitting = new Elbow((string)reader[(string)paramterConfiguration["parameters"]["elbow"]["property_11"]["database"]],
                                            (string)reader[(string)paramterConfiguration["parameters"]["elbow"]["property_12"]["database"]],
                                            (string)reader[(string)paramterConfiguration["parameters"]["elbow"]["property_13"]["database"]], 
                                            (string)reader[(string)paramterConfiguration["parameters"]["elbow"]["property_14"]["database"]],
                                            (string)reader[(string)paramterConfiguration["parameters"]["elbow"]["property_16"]["database"]],
                                            (string)reader[(string)paramterConfiguration["parameters"]["elbow"]["property_15"]["database"]],
                                            fittingModel,
                                            (double)reader[(string)paramterConfiguration["parameters"]["elbow"]["property_20"]["database"]], 
                                            (double)reader[(string)paramterConfiguration["parameters"]["elbow"]["property_21"]["database"]], 
                                            (double)reader[(string)paramterConfiguration["parameters"]["elbow"]["property_1"]["database"]], 
                                            (double)reader[(string)paramterConfiguration["parameters"]["elbow"]["property_2"]["database"]],
                                            (double)reader[(string)paramterConfiguration["parameters"]["elbow"]["property_17"]["database"]], 
                                            (double)reader[(string)paramterConfiguration["parameters"]["elbow"]["property_22"]["database"]], 
                                            (double)reader[(string)paramterConfiguration["parameters"]["elbow"]["property_4"]["database"]],
                                            (double)reader[(string)paramterConfiguration["parameters"]["elbow"]["property_5"]["database"]],
                                            (double)reader[(string)paramterConfiguration["parameters"]["elbow"]["property_8"]["database"]], 
                                            (double)reader[(string)paramterConfiguration["parameters"]["elbow"]["property_9"]["database"]], 
                                            (double)reader[(string)paramterConfiguration["parameters"]["elbow"]["property_6"]["database"]], 
                                            (double)reader[(string)paramterConfiguration["parameters"]["elbow"]["property_7"]["database"]],
                                            (double)reader[(string)paramterConfiguration["parameters"]["elbow"]["property_10"]["database"]], 
                                            (double)reader[(string)paramterConfiguration["parameters"]["elbow"]["property_3"]["database"]]);
                                        break;

                                    case "Tee":
                                        fitting = new Tee((string)reader[(string)paramterConfiguration["parameters"]["tee"]["property_19"]["database"]],
                                           (string)reader[(string)paramterConfiguration["parameters"]["tee"]["property_20"]["database"]],
                                           (string)reader[(string)paramterConfiguration["parameters"]["tee"]["property_21"]["database"]],
                                           (string)reader[(string)paramterConfiguration["parameters"]["tee"]["property_22"]["database"]],
                                           (string)reader[(string)paramterConfiguration["parameters"]["tee"]["property_24"]["database"]],
                                           (string)reader[(string)paramterConfiguration["parameters"]["tee"]["property_23"]["database"]],
                                           fittingModel,
                                           (double)reader[(string)paramterConfiguration["parameters"]["tee"]["property_25"]["database"]],
                                           (double)reader[(string)paramterConfiguration["parameters"]["tee"]["property_26"]["database"]],
                                           (double)reader[(string)paramterConfiguration["parameters"]["tee"]["property_27"]["database"]],
                                           (double)reader[(string)paramterConfiguration["parameters"]["tee"]["property_1"]["database"]],
                                           (double)reader[(string)paramterConfiguration["parameters"]["tee"]["property_2"]["database"]],
                                           (double)reader[(string)paramterConfiguration["parameters"]["tee"]["property_3"]["database"]],
                                           (double)reader[(string)paramterConfiguration["parameters"]["tee"]["property_28"]["database"]],
                                           (double)reader[(string)paramterConfiguration["parameters"]["tee"]["property_7"]["database"]],
                                           (double)reader[(string)paramterConfiguration["parameters"]["tee"]["property_8"]["database"]],
                                           (double)reader[(string)paramterConfiguration["parameters"]["tee"]["property_9"]["database"]],
                                           (double)reader[(string)paramterConfiguration["parameters"]["tee"]["property_13"]["database"]],
                                           (double)reader[(string)paramterConfiguration["parameters"]["tee"]["property_14"]["database"]],
                                           (double)reader[(string)paramterConfiguration["parameters"]["tee"]["property_15"]["database"]],
                                           (double)reader[(string)paramterConfiguration["parameters"]["tee"]["property_10"]["database"]],
                                           (double)reader[(string)paramterConfiguration["parameters"]["tee"]["property_11"]["database"]],
                                           (double)reader[(string)paramterConfiguration["parameters"]["tee"]["property_12"]["database"]],
                                           (double)reader[(string)paramterConfiguration["parameters"]["tee"]["property_4"]["database"]],
                                           (double)reader[(string)paramterConfiguration["parameters"]["tee"]["property_6"]["database"]],
                                           (double)reader[(string)paramterConfiguration["parameters"]["tee"]["property_5"]["database"]],
                                           (double)reader[(string)paramterConfiguration["parameters"]["tee"]["property_16"]["database"]],
                                           (double)reader[(string)paramterConfiguration["parameters"]["tee"]["property_17"]["database"]],
                                           (double)reader[(string)paramterConfiguration["parameters"]["tee"]["property_18"]["database"]]);

                                        break;

                                    case "Transition_Concentrisch":
                                    case "Transition_Excentrisch":
                                        fitting = new Transition((string)reader[(string)paramterConfiguration["parameters"]["transistion"]["property_12"]["database"]],
                                           (string)reader[(string)paramterConfiguration["parameters"]["transistion"]["property_13"]["database"]],
                                           (string)reader[(string)paramterConfiguration["parameters"]["transistion"]["property_14"]["database"]],
                                           (string)reader[(string)paramterConfiguration["parameters"]["transistion"]["property_15"]["database"]],
                                           (string)reader[(string)paramterConfiguration["parameters"]["transistion"]["property_17"]["database"]],
                                           (string)reader[(string)paramterConfiguration["parameters"]["transistion"]["property_16"]["database"]],
                                           fittingModel,
                                           (double)reader[(string)paramterConfiguration["parameters"]["transistion"]["property_18"]["database"]],
                                           (double)reader[(string)paramterConfiguration["parameters"]["transistion"]["property_19"]["database"]],
                                           (double)reader[(string)paramterConfiguration["parameters"]["transistion"]["property_1"]["database"]],
                                           (double)reader[(string)paramterConfiguration["parameters"]["transistion"]["property_2"]["database"]],
                                           (double)reader[(string)paramterConfiguration["parameters"]["transistion"]["property_4"]["database"]],
                                           (double)reader[(string)paramterConfiguration["parameters"]["transistion"]["property_5"]["database"]],
                                           (double)reader[(string)paramterConfiguration["parameters"]["transistion"]["property_8"]["database"]],
                                           (double)reader[(string)paramterConfiguration["parameters"]["transistion"]["property_9"]["database"]],
                                           (double)reader[(string)paramterConfiguration["parameters"]["transistion"]["property_6"]["database"]],
                                           (double)reader[(string)paramterConfiguration["parameters"]["transistion"]["property_7"]["database"]],
                                           (double)reader[(string)paramterConfiguration["parameters"]["transistion"]["property_10"]["database"]],
                                           (double)reader[(string)paramterConfiguration["parameters"]["transistion"]["property_11"]["database"]],
                                           (double)reader[(string)paramterConfiguration["parameters"]["transistion"]["property_3"]["database"]]);

                                        break;

                                    case "Tap":
                                        fitting = new Tap((string)reader[(string)paramterConfiguration["parameters"]["tap"]["property_4"]["database"]],
                                           (string)reader[(string)paramterConfiguration["parameters"]["tap"]["property_5"]["database"]],
                                           (string)reader[(string)paramterConfiguration["parameters"]["tap"]["property_6"]["database"]],
                                           (string)reader[(string)paramterConfiguration["parameters"]["tap"]["property_7"]["database"]],
                                           (string)reader[(string)paramterConfiguration["parameters"]["tap"]["property_9"]["database"]],
                                           (string)reader[(string)paramterConfiguration["parameters"]["tap"]["property_8"]["database"]],
                                           fittingModel,
                                           (double)reader[(string)paramterConfiguration["parameters"]["tap"]["property_10"]["database"]],
                                           (double)reader[(string)paramterConfiguration["parameters"]["tap"]["property_1"]["database"]],
                                           (double)reader[(string)paramterConfiguration["parameters"]["tap"]["property_2"]["database"]],
                                           maxDiameter);

                                        break;
                                }

                                Fittings.Add(fitting);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        TaskDialog.Show("FittingModelView reader error", ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("FittingModelView connection error", ex.Message);
            }

            OnPropertyChanged(nameof(Fittings));
        }

        /// <summary>
        /// gets the chosen fitting and inject the parameters int the fitting from the document.
        /// </summary>
        /// <param name="fitting"></param>
        public void Oke(FittingModel fitting)
        {
            if (fitting == null)
            {
                MessageBox.Show("Selecteer een fitting", "Geen fitting geselecteerd!");
                return;
            }

            List<object> paramList = new List<object>();

            switch (fitting.Fitting.Name)
            {
                case "Elbow":
                    Elbow elbow = (Elbow)fitting;
                    paramList.Add(Math.Round(elbow.Buitendiameter_1 / feetToMm, 4));
                    paramList.Add(Math.Round(elbow.Buitendiameter_2 / feetToMm, 4));
                    paramList.Add(Math.Round(elbow.CenterStraal / feetToMm, 4));
                    paramList.Add(elbow.Uiteinde_1_Type);
                    paramList.Add(elbow.Uiteinde_2_Type);
                    paramList.Add(Math.Round(elbow.Uiteinde_1_Maat / feetToMm, 4));
                    paramList.Add(Math.Round(elbow.Uiteinde_2_Maat / feetToMm, 4));
                    paramList.Add(Math.Round(elbow.Uiteinde_1_Lengte / feetToMm, 4));
                    paramList.Add(Math.Round(elbow.Uiteinde_2_Lengte / feetToMm, 4));
                    paramList.Add(Math.Round(elbow.FlensDikte / feetToMm, 4));
                    paramList.Add(elbow.Hoek);
                    paramList.Add(elbow.Fabrikant);
                    paramList.Add(elbow.Type);
                    paramList.Add(elbow.Materiaal);
                    paramList.Add(elbow.ProductCode);
                    paramList.Add(elbow.Omschrijving);
                    paramList.Add(elbow.Beschikbaar);

                    ElementSettings.SetCodraftParametersElbow(paramList, this.fittingModel, paramterConfiguration);
                    break;

                case "Tee":
                    Tee tee = (Tee)fitting;
                    paramList.Add(Math.Round(tee.Buitendiameter_1 / feetToMm, 4));
                    paramList.Add(Math.Round(tee.Buitendiameter_2 / feetToMm, 4));
                    paramList.Add(Math.Round(tee.Buitendiameter_3 / feetToMm, 4));
                    paramList.Add(Math.Round(tee.Lengte / feetToMm, 4));
                    paramList.Add(Math.Round(tee.Center_Uiteinde_3 / feetToMm, 4));
                    paramList.Add(Math.Round(tee.Center_Uiteinde_1 / feetToMm, 4));
                    paramList.Add(tee.Uiteinde_1_Type);
                    paramList.Add(tee.Uiteinde_2_Type);
                    paramList.Add(tee.Uiteinde_3_Type);
                    paramList.Add(Math.Round(tee.Uiteinde_1_Maat / feetToMm, 4));
                    paramList.Add(Math.Round(tee.Uiteinde_2_Maat / feetToMm, 4));
                    paramList.Add(Math.Round(tee.Uiteinde_3_Maat / feetToMm, 4));
                    paramList.Add(Math.Round(tee.Uiteinde_1_Lengte / feetToMm, 4));
                    paramList.Add(Math.Round(tee.Uiteinde_2_Lengte / feetToMm, 4));
                    paramList.Add(Math.Round(tee.Uiteinde_3_Lengte / feetToMm, 4));
                    paramList.Add(Math.Round(tee.FlensDikte1 / feetToMm, 4));
                    paramList.Add(Math.Round(tee.FlensDikte2 / feetToMm, 4));
                    paramList.Add(Math.Round(tee.FlensDikte3 / feetToMm, 4));
                    paramList.Add(tee.Hoek);
                    paramList.Add(tee.Fabrikant);
                    paramList.Add(tee.Type);
                    paramList.Add(tee.Materiaal);
                    paramList.Add(tee.ProductCode);
                    paramList.Add(tee.Omschrijving);
                    paramList.Add(tee.Beschikbaar);

                    ElementSettings.SetCodraftParametersTee(paramList, this.fittingModel, paramterConfiguration);
                    break;

                case "Transition_Concentrisch":
                case "Transition_Excentrisch":
                    Transition transition = (Transition)fitting;
                    paramList.Add(Math.Round(transition.Buitendiameter_1 / feetToMm, 4));
                    paramList.Add(Math.Round(transition.Buitendiameter_2 / feetToMm, 4));
                    paramList.Add(Math.Round(transition.Lengte / feetToMm, 4));
                    paramList.Add(transition.Uiteinde_1_Type);
                    paramList.Add(transition.Uiteinde_2_Type);
                    paramList.Add(Math.Round(transition.Uiteinde_1_Maat / feetToMm, 4));
                    paramList.Add(Math.Round(transition.Uiteinde_2_Maat / feetToMm, 4));
                    paramList.Add(Math.Round(transition.Uiteinde_1_Lengte / feetToMm, 4));
                    paramList.Add(Math.Round(transition.Uiteinde_2_Lengte / feetToMm, 4));
                    paramList.Add(Math.Round(transition.FlensDikte1 / feetToMm, 4));
                    paramList.Add(Math.Round(transition.FlensDikte2 / feetToMm, 4));
                    paramList.Add(transition.Fabrikant);
                    paramList.Add(transition.Type);
                    paramList.Add(transition.Materiaal);
                    paramList.Add(transition.ProductCode);
                    paramList.Add(transition.Omschrijving);
                    paramList.Add(transition.Beschikbaar);

                    ElementSettings.SetCodraftParametersTransition(paramList, this.fittingModel, this._switchNd, this._excentrisch, paramterConfiguration);
                    break;

                case "Tap":
                    Tap tap = (Tap)fitting;
                    paramList.Add(Math.Round(tap.Buitendiameter / feetToMm, 4));
                    paramList.Add(Math.Round(tap.Lengte / feetToMm, 4));
                    paramList.Add(Math.Round(tap.Lengte_Waarde / feetToMm, 4));
                    paramList.Add(tap.Fabrikant);
                    paramList.Add(tap.Type);
                    paramList.Add(tap.Materiaal);
                    paramList.Add(tap.ProductCode);
                    paramList.Add(tap.Omschrijving);
                    paramList.Add(tap.Beschikbaar);

                    ElementSettings.SetCodraftParametersTap(paramList, this.fittingModel, paramterConfiguration);
                    break;

                default:
                    break;
            }

            if (RememberChoice)
            {
                FileOperations.RememberMe(paramList, rememberMeFile, parameters);
            }

            CloseWindow();
        }

        /// <summary>
        /// Open the current database in access
        /// </summary>
        public void OpenDatabase()
        {
            System.Diagnostics.Process.Start(@"C:\Program Files\Microsoft Office\root\Office16\MSACCESS.EXE");

            System.Threading.Thread.Sleep(1500);

            System.Diagnostics.Process.Start(databaseFilePath);
        }

        public void Refresh()
        {
            Fittings.Clear();
            FillList();
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
