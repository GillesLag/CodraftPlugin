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

                    if ((int)reader[0] > 1) return true;
                }

                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    reader.Read();

                    parameters.Add((double)reader["Lengte"] / feetToMm);
                    parameters.Add((double)reader["a"] / feetToMm);
                    parameters.Add((double)reader["b"] / feetToMm);
                    parameters.Add((double)reader["PipeOd"] / feetToMm);
                    parameters.Add((double)reader["Uiteinde_1_type"]);
                    parameters.Add((double)reader["Uiteinde_2_type"]);
                    parameters.Add((double)reader["Uiteinde_1_maat"]);
                    parameters.Add((double)reader["Uiteinde_2_maat"]);
                    parameters.Add((double)reader["L1"]);
                    parameters.Add((double)reader["L2"]);
                }
            }

            return false;
        }
    }
}
