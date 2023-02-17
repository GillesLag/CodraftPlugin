using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using Autodesk.Revit.DB;

namespace CodraftPlugin_DAL
{
    public static class FileOperationsPipeAccessories
    {
        private const double feetToMm = 304.8;
        public static bool LookupStraightValve(string query, string queryCount, string connectionString, out List<object> parameters)
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

                    parameters.Add(Math.Round((double)reader["Lengte"] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader["a"] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader["b"] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader["b"] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader["PipeOd"] / feetToMm, 4));
                    parameters.Add((int)reader["Uiteinde_1_type"]);
                    parameters.Add((int)reader["Uiteinde_2_type"]);
                    parameters.Add(Math.Round((double)reader["Uiteinde_1_maat"] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader["Uiteinde_2_maat"] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader["L1"] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader["L2"] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader["c"] / feetToMm, 4));
                    parameters.Add(Math.Round((double)reader["d"] / feetToMm, 4));
                    parameters.Add(reader["Manufacturer"]);
                    parameters.Add(reader["Type"]);
                    parameters.Add(reader["Material"]);
                    parameters.Add(reader["Product Code"]);
                    parameters.Add(reader["Omschrijving"]);
                    parameters.Add(reader["Beschikbaar"]);
                }
            }

            return false;
        }
    }
}
