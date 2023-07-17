using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using Autodesk.Revit.DB;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;

namespace CodraftPlugin_DAL
{
    public static class FileOperationsPipeAccessories
    {
        private const double feetToMm = 304.8;

        public static bool LookupBalanceValve(string query, string queryCount, string connectionString, out List<object> parameters, JObject file)
        {
            parameters = new List<object>();
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                OleDbCommand command = new OleDbCommand(query, connection);
                OleDbCommand countCommand = new OleDbCommand(queryCount, connection);

                connection.Open();

                using (OleDbDataReader reader = countCommand.ExecuteReader())
                {
                    reader.Read();
                    int count = (int)reader[0];
                    if (count == 0) return false;
                    if ((int)reader[0] > 1) return true;
                }

                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    reader.Read();

                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["balanceValve"]["property_1"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["balanceValve"]["property_2"]["database"]] / feetToMm, 4));
                    parameters.Add((int)reader[(string)file["parameters"]["balanceValve"]["property_3"]["database"]]);
                    parameters.Add((int)reader[(string)file["parameters"]["balanceValve"]["property_4"]["database"]]);
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["balanceValve"]["property_5"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["balanceValve"]["property_6"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["balanceValve"]["property_7"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["balanceValve"]["property_8"]["database"]] / feetToMm, 4));
                    parameters.Add(reader[(string)file["parameters"]["balanceValve"]["property_9"]["database"]]);
                    parameters.Add(reader[(string)file["parameters"]["balanceValve"]["property_10"]["database"]]);
                    parameters.Add(reader[(string)file["parameters"]["balanceValve"]["property_11"]["database"]]);
                    parameters.Add(reader[(string)file["parameters"]["balanceValve"]["property_12"]["database"]]);
                    parameters.Add(reader[(string)file["parameters"]["balanceValve"]["property_13"]["database"]]);
                    parameters.Add(reader[(string)file["parameters"]["balanceValve"]["property_14"]["database"]]);
                }
            }

            return false;
        }

        public static bool LookupButterflyValve(string query, string queryCount, string connectionString, out List<object> parameters, JObject file)
        {
            parameters = new List<object>();
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                OleDbCommand command = new OleDbCommand(query, connection);
                OleDbCommand countCommand = new OleDbCommand(queryCount, connection);

                connection.Open();

                using (OleDbDataReader reader = countCommand.ExecuteReader())
                {
                    reader.Read();
                    int count = (int)reader[0];
                    if (count == 0) return false;
                    if ((int)reader[0] > 1) return true;
                }

                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    reader.Read();

                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["butterflyValve"]["property_1"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["butterflyValve"]["property_2"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((int)reader[(string)file["parameters"]["butterflyValve"]["property_3"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["butterflyValve"]["property_4"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["butterflyValve"]["property_5"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["butterflyValve"]["property_6"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["butterflyValve"]["property_7"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["butterflyValve"]["property_8"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["butterflyValve"]["property_9"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["butterflyValve"]["property_10"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["butterflyValve"]["property_11"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["butterflyValve"]["property_12"]["database"]] / feetToMm, 4));
                    parameters.Add(reader[(string)file["parameters"]["butterflyValve"]["property_13"]["database"]]);
                    parameters.Add(reader[(string)file["parameters"]["butterflyValve"]["property_14"]["database"]]);
                    parameters.Add(reader[(string)file["parameters"]["butterflyValve"]["property_15"]["database"]]);
                    parameters.Add(reader[(string)file["parameters"]["butterflyValve"]["property_16"]["database"]]);
                    parameters.Add(reader[(string)file["parameters"]["butterflyValve"]["property_17"]["database"]]);
                    parameters.Add(reader[(string)file["parameters"]["butterflyValve"]["property_18"]["database"]]);
                }
            }

            return false;
        }

        public static bool LookupStraightValve(string query, string queryCount, string connectionString, out List<object> parameters, JObject file)
        {
            parameters = new List<object>();
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                OleDbCommand command = new OleDbCommand(query, connection);
                OleDbCommand countCommand = new OleDbCommand(queryCount, connection);

                connection.Open();

                using (OleDbDataReader reader = countCommand.ExecuteReader())
                {
                    reader.Read();
                    int count = (int)reader[0];
                    if (count == 0) return false;
                    if ((int)reader[0] > 1) return true;
                }

                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    reader.Read();

                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["straightValve"]["property_1"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["straightValve"]["property_2"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["straightValve"]["property_3"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["straightValve"]["property_4"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["straightValve"]["property_5"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["straightValve"]["property_6"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["straightValve"]["property_7"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["straightValve"]["property_8"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["straightValve"]["property_9"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["straightValve"]["property_10"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["straightValve"]["property_11"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["straightValve"]["property_12"]["database"]] / feetToMm, 4));
                    parameters.Add((int)reader[(string)file["parameters"]["straightValve"]["property_13"]["database"]]);
                    parameters.Add((int)reader[(string)file["parameters"]["straightValve"]["property_14"]["database"]]);
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["straightValve"]["property_15"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["straightValve"]["property_16"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["straightValve"]["property_17"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["straightValve"]["property_18"]["database"]] / feetToMm, 4));
                    parameters.Add(reader[(string)file["parameters"]["straightValve"]["property_19"]["database"]]);
                    parameters.Add(reader[(string)file["parameters"]["straightValve"]["property_20"]["database"]]);
                    parameters.Add(reader[(string)file["parameters"]["straightValve"]["property_21"]["database"]]);
                    parameters.Add(reader[(string)file["parameters"]["straightValve"]["property_22"]["database"]]);
                    parameters.Add(reader[(string)file["parameters"]["straightValve"]["property_22"]["database"]]);
                    parameters.Add(reader[(string)file["parameters"]["straightValve"]["property_24"]["database"]]);
                }
            }

            return false;
        }

        public static bool LookupStrainer(string query, string queryCount, string connectionString, out List<object> parameters, JObject file)
        {
            parameters = new List<object>();
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                OleDbCommand command = new OleDbCommand(query, connection);
                OleDbCommand countCommand = new OleDbCommand(queryCount, connection);

                connection.Open();

                using (OleDbDataReader reader = countCommand.ExecuteReader())
                {
                    reader.Read();
                    int count = (int)reader[0];
                    if (count == 0) return false;
                    if ((int)reader[0] > 1) return true;
                }

                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    reader.Read();

                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["strainer"]["property_1"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["balanceValve"]["property_2"]["revit"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["balanceValve"]["property_3"]["revit"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["balanceValve"]["property_4"]["revit"]] / feetToMm, 4));
                    parameters.Add((int)reader[(string)file["parameters"]["balanceValve"]["property_5"]["revit"]]);
                    parameters.Add((int)reader[(string)file["parameters"]["balanceValve"]["property_6"]["revit"]]);
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["balanceValve"]["property_7"]["revit"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["balanceValve"]["property_8"]["revit"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["balanceValve"]["property_9"]["revit"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["balanceValve"]["property_10"]["revit"]] / feetToMm, 4));
                    parameters.Add(reader[(string)file["parameters"]["balanceValve"]["property_11"]["revit"]]);
                    parameters.Add(reader[(string)file["parameters"]["balanceValve"]["property_12"]["revit"]]);
                    parameters.Add(reader[(string)file["parameters"]["balanceValve"]["property_13"]["revit"]]);
                    parameters.Add(reader[(string)file["parameters"]["balanceValve"]["property_14"]["revit"]]);
                    parameters.Add(reader[(string)file["parameters"]["balanceValve"]["property_15"]["revit"]]);
                    parameters.Add(reader[(string)file["parameters"]["balanceValve"]["property_16"]["revit"]]);
                }
            }

            return false;
        }

        public static bool LookupThreeWayValve(string query, string queryCount, string connectionString, out List<object> parameters, JObject file)
        {
            parameters = new List<object>();
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                OleDbCommand command = new OleDbCommand(query, connection);
                OleDbCommand countCommand = new OleDbCommand(queryCount, connection);

                connection.Open();

                using (OleDbDataReader reader = countCommand.ExecuteReader())
                {
                    reader.Read();
                    int count = (int)reader[0];
                    if (count == 0) return false;
                    if ((int)reader[0] > 1) return true;
                }

                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    reader.Read();

                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["threewayGlobeValve"]["property_1"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["threewayGlobeValve"]["property_2"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["threewayGlobeValve"]["property_3"]["database"]] / feetToMm, 4));
                    parameters.Add((int)reader[(string)file["parameters"]["threewayGlobeValve"]["property_4"]["database"]]);
                    parameters.Add((int)reader[(string)file["parameters"]["threewayGlobeValve"]["property_5"]["database"]]);
                    parameters.Add((int)reader[(string)file["parameters"]["threewayGlobeValve"]["property_6"]["database"]]);
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["threewayGlobeValve"]["property_7"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["threewayGlobeValve"]["property_8"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["threewayGlobeValve"]["property_9"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["threewayGlobeValve"]["property_10"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["threewayGlobeValve"]["property_11"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["threewayGlobeValve"]["property_12"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["threewayGlobeValve"]["property_13"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["threewayGlobeValve"]["property_14"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["threewayGlobeValve"]["property_15"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["threewayGlobeValve"]["property_16"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["threewayGlobeValve"]["property_17"]["database"]] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader[(string)file["parameters"]["threewayGlobeValve"]["property_18"]["database"]] / feetToMm, 4));
                    parameters.Add(reader[(string)file["parameters"]["threewayGlobeValve"]["property_19"]["database"]]);
                    parameters.Add(reader[(string)file["parameters"]["threewayGlobeValve"]["property_20"]["database"]]);
                    parameters.Add(reader[(string)file["parameters"]["threewayGlobeValve"]["property_21"]["database"]]);
                    parameters.Add(reader[(string)file["parameters"]["threewayGlobeValve"]["property_22"]["database"]]);
                    parameters.Add(reader[(string)file["parameters"]["threewayGlobeValve"]["property_23"]["database"]]);
                    parameters.Add(reader[(string)file["parameters"]["threewayGlobeValve"]["property_24"]["database"]]);
                    //parameters.Add(reader["Maat_annotatie"]);
                }
            }

            return false;
        }
    }
}
