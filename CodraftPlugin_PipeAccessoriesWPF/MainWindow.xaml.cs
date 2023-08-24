using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.OleDb;
using System.Windows;
using System.Windows.Controls;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CodraftPlugin_PipeAccessoriesWPF.Model;
using CodraftPlugin_Library;
using CodraftPlugin_DAL;
using Newtonsoft.Json.Linq;

namespace CodraftPlugin_PipeAccessoriesWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const double feetToMm = 304.8;

        private string _accessoryName;
        private string _connectionString;
        private string _sqlQuery;
        private string _databaseFilePath;
        private List<string> _callingParams;
        private FamilyInstance _pipeAccessory;
        private List<BaseAccessory> _accessories = new List<BaseAccessory>();
        private JObject parameterConfiguration;

        public bool hasChosenAccessory { get; private set; } = false;
        public MainWindow(FamilyInstance PipeAccessory, string name, string connectionString, string sqlQuery, string databaseFilePath, List<string> callingParams, JObject file)
        {
            InitializeComponent();

            _accessoryName = name;
            _connectionString = connectionString;
            _sqlQuery = sqlQuery;
            _pipeAccessory = PipeAccessory;
            _databaseFilePath = databaseFilePath;
            _callingParams = callingParams;
            parameterConfiguration = file;

            FillDataGrid();
        }

        private void fdgCataloog_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            PropertyDescriptor propertyDescriptor = (PropertyDescriptor)e.PropertyDescriptor;
            e.Column.Header = propertyDescriptor.DisplayName;
            if (propertyDescriptor.DisplayName == "Accessory")
            {
                e.Cancel = true;
            }
        }

        private void FillDataGrid()
        {
            using (OleDbConnection connection = new OleDbConnection(_connectionString))
            {
                OleDbCommand command = new OleDbCommand(_sqlQuery, connection);
                connection.Open();

                BaseAccessory accessory = null;

                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        switch (_accessoryName)
                        {
                            case "StraightValve":
                                accessory = new StraightValve(_pipeAccessory,
                                    (string)reader[(string)parameterConfiguration["parameters"]["straightValve"]["property_19"]["database"]],
                                    (string)reader[(string)parameterConfiguration["parameters"]["straightValve"]["property_20"]["database"]],
                                    (string)reader[(string)parameterConfiguration["parameters"]["straightValve"]["property_21"]["database"]],
                                    (string)reader[(string)parameterConfiguration["parameters"]["straightValve"]["property_22"]["database"]],
                                    (string)reader[(string)parameterConfiguration["parameters"]["straightValve"]["property_23"]["database"]],
                                    (string)reader[(string)parameterConfiguration["parameters"]["straightValve"]["property_24"]["database"]],
                                    (int)reader[(string)parameterConfiguration["parameters"]["straightValve"]["property_25"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["straightValve"]["property_1"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["straightValve"]["property_2"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["straightValve"]["property_3"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["straightValve"]["property_4"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["straightValve"]["property_5"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["straightValve"]["property_6"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["straightValve"]["property_7"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["straightValve"]["property_8"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["straightValve"]["property_9"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["straightValve"]["property_10"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["straightValve"]["property_11"]["database"]],
                                    (int)reader[(string)parameterConfiguration["parameters"]["straightValve"]["property_13"]["database"]],
                                    (int)reader[(string)parameterConfiguration["parameters"]["straightValve"]["property_14"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["straightValve"]["property_15"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["straightValve"]["property_16"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["straightValve"]["property_17"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["straightValve"]["property_18"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["straightValve"]["property_12"]["database"]]);
                                break;

                            case "BalanceValve":
                                accessory = new BalanceValve(_pipeAccessory,
                                    (string)reader[(string)parameterConfiguration["parameters"]["balanceValve"]["property_9"]["database"]],
                                    (string)reader[(string)parameterConfiguration["parameters"]["balanceValve"]["property_10"]["database"]],
                                    (string)reader[(string)parameterConfiguration["parameters"]["balanceValve"]["property_11"]["database"]],
                                    (string)reader[(string)parameterConfiguration["parameters"]["balanceValve"]["property_12"]["database"]],
                                    (string)reader[(string)parameterConfiguration["parameters"]["balanceValve"]["property_13"]["database"]],
                                    (string)reader[(string)parameterConfiguration["parameters"]["balanceValve"]["property_14"]["database"]],
                                    (int)reader[(string)parameterConfiguration["parameters"]["balanceValve"]["property_15"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["balanceValve"]["property_1"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["balanceValve"]["property_2"]["database"]],
                                    (int)reader[(string)parameterConfiguration["parameters"]["balanceValve"]["property_3"]["database"]],
                                    (int)reader[(string)parameterConfiguration["parameters"]["balanceValve"]["property_4"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["balanceValve"]["property_5"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["balanceValve"]["property_6"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["balanceValve"]["property_7"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["balanceValve"]["property_8"]["database"]]);
                                break;

                            case "Strainer":
                                accessory = new Strainer(_pipeAccessory,
                                    (string)reader[(string)parameterConfiguration["parameters"]["strainer"]["property_11"]["database"]],
                                    (string)reader[(string)parameterConfiguration["parameters"]["strainer"]["property_12"]["database"]],
                                    (string)reader[(string)parameterConfiguration["parameters"]["strainer"]["property_13"]["database"]],
                                    (string)reader[(string)parameterConfiguration["parameters"]["strainer"]["property_14"]["database"]],
                                    (string)reader[(string)parameterConfiguration["parameters"]["strainer"]["property_15"]["database"]],
                                    (string)reader[(string)parameterConfiguration["parameters"]["strainer"]["property_16"]["database"]],
                                    (int)reader[(string)parameterConfiguration["parameters"]["strainer"]["property_17"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["strainer"]["property_1"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["strainer"]["property_2"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["strainer"]["property_3"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["strainer"]["property_4"]["database"]],
                                    (int)reader[(string)parameterConfiguration["parameters"]["strainer"]["property_5"]["database"]],
                                    (int)reader[(string)parameterConfiguration["parameters"]["strainer"]["property_6"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["strainer"]["property_7"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["strainer"]["property_8"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["strainer"]["property_9"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["strainer"]["property_10"]["database"]]);
                                break;

                            case "ThreeWayGlobeValve":
                                accessory = new ThreeWayGlobeValve(_pipeAccessory,
                                    (string)reader[(string)parameterConfiguration["parameters"]["threewayGlobalValve"]["property_19"]["database"]],
                                    (string)reader[(string)parameterConfiguration["parameters"]["threewayGlobalValve"]["property_20"]["database"]],
                                    (string)reader[(string)parameterConfiguration["parameters"]["threewayGlobalValve"]["property_21"]["database"]],
                                    (string)reader[(string)parameterConfiguration["parameters"]["threewayGlobalValve"]["property_22"]["database"]],
                                    (string)reader[(string)parameterConfiguration["parameters"]["threewayGlobalValve"]["property_23"]["database"]],
                                    (string)reader[(string)parameterConfiguration["parameters"]["threewayGlobalValve"]["property_24"]["database"]],
                                    (int)reader[(string)parameterConfiguration["parameters"]["threewayGlobalValve"]["property_25"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["threewayGlobalValve"]["property_1"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["threewayGlobalValve"]["property_2"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["threewayGlobalValve"]["property_3"]["database"]],
                                    (int)reader[(string)parameterConfiguration["parameters"]["threewayGlobalValve"]["property_4"]["database"]],
                                    (int)reader[(string)parameterConfiguration["parameters"]["threewayGlobalValve"]["property_5"]["database"]],
                                    (int)reader[(string)parameterConfiguration["parameters"]["threewayGlobalValve"]["property_6"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["threewayGlobalValve"]["property_7"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["threewayGlobalValve"]["property_8"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["threewayGlobalValve"]["property_9"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["threewayGlobalValve"]["property_10"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["threewayGlobalValve"]["property_11"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["threewayGlobalValve"]["property_12"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["threewayGlobalValve"]["property_13"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["threewayGlobalValve"]["property_14"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["threewayGlobalValve"]["property_15"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["threewayGlobalValve"]["property_16"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["threewayGlobalValve"]["property_17"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["threewayGlobalValve"]["property_18"]["database"]]);
                                break;

                            case "ButterflyValve":
                                accessory = new ButterflyValve(_pipeAccessory,
                                    (string)reader[(string)parameterConfiguration["parameters"]["butterflyValve"]["property_13"]["database"]],
                                    (string)reader[(string)parameterConfiguration["parameters"]["butterflyValve"]["property_14"]["database"]],
                                    (string)reader[(string)parameterConfiguration["parameters"]["butterflyValve"]["property_15"]["database"]],
                                    (string)reader[(string)parameterConfiguration["parameters"]["butterflyValve"]["property_16"]["database"]],
                                    (string)reader[(string)parameterConfiguration["parameters"]["butterflyValve"]["property_17"]["database"]],
                                    (string)reader[(string)parameterConfiguration["parameters"]["butterflyValve"]["property_18"]["database"]],
                                    (int)reader[(string)parameterConfiguration["parameters"]["butterflyValve"]["property_3"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["butterflyValve"]["property_1"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["butterflyValve"]["property_2"]["database"]],
                                    (int)reader[(string)parameterConfiguration["parameters"]["butterflyValve"]["property_3"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["butterflyValve"]["property_4"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["butterflyValve"]["property_5"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["butterflyValve"]["property_6"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["butterflyValve"]["property_7"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["butterflyValve"]["property_8"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["butterflyValve"]["property_9"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["butterflyValve"]["property_10"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["butterflyValve"]["property_11"]["database"]],
                                    (double)reader[(string)parameterConfiguration["parameters"]["butterflyValve"]["property_12"]["database"]]);
                                break;

                            default:
                                break;
                        }

                        _accessories.Add(accessory);
                    }
                }

                fdgCataloog.ItemsSource = _accessories;
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnOke_Click(object sender, RoutedEventArgs e)
        {
            BaseAccessory ba = fdgCataloog.SelectedItem as BaseAccessory;

            if (ba == null)
            {
                TaskDialog.Show("Fout", "Selecteer een accessory.");
                return;
            }

            List<object> paramList = new List<object>();

            switch (_accessoryName)
            {
                case "StraightValve":
                    StraightValve straightValve = (StraightValve)ba;
                    paramList.Add(Math.Round(straightValve.Lengte / feetToMm, 4));
                    paramList.Add(Math.Round(straightValve.HendelLengte / feetToMm, 4));
                    paramList.Add(Math.Round(straightValve.HendelBreedte / feetToMm, 4));
                    paramList.Add(Math.Round(straightValve.HendelHoogte / feetToMm, 4));
                    paramList.Add(Math.Round(straightValve.MotorLengte / feetToMm, 4));
                    paramList.Add(Math.Round(straightValve.MotorBreedte / feetToMm, 4));
                    paramList.Add(Math.Round(straightValve.MotorHoogte / feetToMm, 4));
                    paramList.Add(Math.Round(straightValve.WormwielStraal / feetToMm, 4));
                    paramList.Add(Math.Round(straightValve.WormwielStaafStraal / feetToMm, 4));
                    paramList.Add(Math.Round(straightValve.OperatorHoogte / feetToMm, 4));
                    paramList.Add(Math.Round(straightValve.VlinderhendelDiameter / feetToMm, 4));
                    paramList.Add(Math.Round(straightValve.BuitenDiameter / feetToMm, 4));
                    paramList.Add(straightValve.UiteindeType1);
                    paramList.Add(straightValve.UiteindeType2);
                    paramList.Add(Math.Round(straightValve.UiteindeMaat1 / feetToMm, 4));
                    paramList.Add(Math.Round(straightValve.UiteindeMaat2 / feetToMm, 4));
                    paramList.Add(Math.Round(straightValve.L1 / feetToMm, 4));
                    paramList.Add(Math.Round(straightValve.L2 / feetToMm, 4));
                    paramList.Add(straightValve.Fabrikant);
                    paramList.Add(straightValve.Type);
                    paramList.Add(straightValve.Materiaal);
                    paramList.Add(straightValve.ProductCode);
                    paramList.Add(straightValve.Omschrijving);
                    paramList.Add(straightValve.Beschikbaar);

                    ElementSettings.SetCodraftParamtersStraightValve(paramList, ba.Accessory, parameterConfiguration);
                    break;

                case "BalanceValve":
                    BalanceValve balanceValve = (BalanceValve)ba;
                    paramList.Add(Math.Round(balanceValve.Lengte / feetToMm, 4));
                    paramList.Add(Math.Round(balanceValve.BuitenDiameter / feetToMm, 4));
                    paramList.Add(balanceValve.UiteindeType1);
                    paramList.Add(balanceValve.UiteindeType2);
                    paramList.Add(Math.Round(balanceValve.UiteindeMaat1 / feetToMm, 4));
                    paramList.Add(Math.Round(balanceValve.UiteindeMaat2 / feetToMm, 4));
                    paramList.Add(Math.Round(balanceValve.L1 / feetToMm, 4));
                    paramList.Add(Math.Round(balanceValve.L2 / feetToMm, 4));
                    paramList.Add(balanceValve.Fabrikant);
                    paramList.Add(balanceValve.Type);
                    paramList.Add(balanceValve.Materiaal);
                    paramList.Add(balanceValve.ProductCode);
                    paramList.Add(balanceValve.Omschrijving);
                    paramList.Add(balanceValve.Beschikbaar);

                    ElementSettings.SetCodraftParametersBalanceValve(paramList, ba.Accessory, parameterConfiguration);
                    break;

                case "Strainer":
                    Strainer strainer = (Strainer)ba;
                    paramList.Add(Math.Round(strainer.BuitenDiameter / feetToMm, 4));
                    paramList.Add(Math.Round(strainer.Hoogte / feetToMm, 4));
                    paramList.Add(Math.Round(strainer.Lengte / feetToMm, 4));
                    paramList.Add(Math.Round(strainer.Offset / feetToMm, 4));
                    paramList.Add(strainer.UiteindeType1);
                    paramList.Add(strainer.UiteindeType2);
                    paramList.Add(Math.Round(strainer.L1 / feetToMm, 4));
                    paramList.Add(Math.Round(strainer.L2 / feetToMm, 4));
                    paramList.Add(Math.Round(strainer.UiteindeMaat1 / feetToMm, 4));
                    paramList.Add(Math.Round(strainer.UiteindeMaat2 / feetToMm, 4));
                    paramList.Add(strainer.Fabrikant);
                    paramList.Add(strainer.Type);
                    paramList.Add(strainer.Materiaal);
                    paramList.Add(strainer.ProductCode);
                    paramList.Add(strainer.Omschrijving);
                    paramList.Add(strainer.Beschikbaar);

                    ElementSettings.SetCodraftParametersStrainer(paramList, ba.Accessory, parameterConfiguration);
                    break;

                case "ThreeWayGlobeValve":
                    ThreeWayGlobeValve threeWayGlobeValve = (ThreeWayGlobeValve)ba;
                    paramList.Add(Math.Round(threeWayGlobeValve.BuitenDiameter / feetToMm, 4));
                    paramList.Add(Math.Round(threeWayGlobeValve.Lengte / feetToMm, 4));
                    paramList.Add(Math.Round(threeWayGlobeValve.Lengte3 / feetToMm, 4));
                    paramList.Add(threeWayGlobeValve.UiteindeType1);
                    paramList.Add(threeWayGlobeValve.UiteindeType2);
                    paramList.Add(threeWayGlobeValve.UiteindeType3);
                    paramList.Add(Math.Round(threeWayGlobeValve.L1 / feetToMm, 4));
                    paramList.Add(Math.Round(threeWayGlobeValve.L2 / feetToMm, 4));
                    paramList.Add(Math.Round(threeWayGlobeValve.L3 / feetToMm, 4));
                    paramList.Add(Math.Round(threeWayGlobeValve.UiteindeMaat1 / feetToMm, 4));
                    paramList.Add(Math.Round(threeWayGlobeValve.UiteindeMaat2 / feetToMm, 4));
                    paramList.Add(Math.Round(threeWayGlobeValve.UiteindeMaat3 / feetToMm, 4));
                    paramList.Add(Math.Round(threeWayGlobeValve.MotorLengte / feetToMm, 4));
                    paramList.Add(Math.Round(threeWayGlobeValve.MotorBreedte / feetToMm, 4));
                    paramList.Add(Math.Round(threeWayGlobeValve.MotorHoogte / feetToMm, 4));
                    paramList.Add(Math.Round(threeWayGlobeValve.HoogteOperator / feetToMm, 4));
                    paramList.Add(Math.Round(threeWayGlobeValve.WormwielDiameter / feetToMm, 4));
                    paramList.Add(Math.Round(threeWayGlobeValve.WormwielLengte / feetToMm, 4));
                    paramList.Add(threeWayGlobeValve.Fabrikant);
                    paramList.Add(threeWayGlobeValve.Type);
                    paramList.Add(threeWayGlobeValve.Materiaal);
                    paramList.Add(threeWayGlobeValve.ProductCode);
                    paramList.Add(threeWayGlobeValve.Omschrijving);
                    paramList.Add(threeWayGlobeValve.Beschikbaar);

                    ElementSettings.SetCodraftParametersThreeWayGlobeValve(paramList, ba.Accessory, parameterConfiguration);
                    break;

                case "ButterflyValve":
                    ButterflyValve butterflyValve = (ButterflyValve)ba;
                    paramList.Add(Math.Round(butterflyValve.BuitenDiameterTotaal / feetToMm, 4));
                    paramList.Add(Math.Round(butterflyValve.Lengte / feetToMm, 4));
                    paramList.Add(Math.Round(butterflyValve.BuitenDiameter / feetToMm, 4));
                    paramList.Add(Math.Round(butterflyValve.StaafLengte / feetToMm, 4));
                    paramList.Add(Math.Round(butterflyValve.HendelLengte / feetToMm, 4));
                    paramList.Add(Math.Round(butterflyValve.MotorLengte / feetToMm, 4));
                    paramList.Add(Math.Round(butterflyValve.MotorHoogte / feetToMm, 4));
                    paramList.Add(Math.Round(butterflyValve.MotorBreedte / feetToMm, 4));
                    paramList.Add(Math.Round(butterflyValve.BladeDikte / feetToMm, 4));
                    paramList.Add(Math.Round(butterflyValve.BladeDiameter / feetToMm, 4));
                    paramList.Add(Math.Round(butterflyValve.WormwielDiameter / feetToMm, 4));
                    paramList.Add(Math.Round(butterflyValve.WormwielLengte / feetToMm, 4));
                    paramList.Add(butterflyValve.Fabrikant);
                    paramList.Add(butterflyValve.Type);
                    paramList.Add(butterflyValve.Materiaal);
                    paramList.Add(butterflyValve.ProductCode);
                    paramList.Add(butterflyValve.Omschrijving);
                    paramList.Add(butterflyValve.Beschikbaar);

                    ElementSettings.SetCodraftParametersButterflyValve(paramList, ba.Accessory, parameterConfiguration);
                    break;

                default:
                    break;
            }

            if (cbRememberChoice.IsChecked == true)
            {
                string revitFilePath = _pipeAccessory.Document.PathName;
                string rememberFilePath = revitFilePath.Substring(0, revitFilePath.LastIndexOf('\\') + 1) + "RevitTextFiles\\RememberMePipeAccessories.txt";
                FileOperations.RememberMe(paramList, rememberFilePath, _callingParams);
            }

            hasChosenAccessory = true;
            this.Close();
        }

        private void btnOpenDatabase_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"C:\Program Files\Microsoft Office\root\Office16\MSACCESS.EXE");

            System.Threading.Thread.Sleep(1500);

            System.Diagnostics.Process.Start(_databaseFilePath);
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            _accessories.Clear();
            fdgCataloog.ItemsSource = null;
            FillDataGrid();
            fdgCataloog.Items.Refresh();
        }
    }
}
