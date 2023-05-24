using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using StartWerkomgevingWPF.Models;

namespace StartWerkomgevingWPF.DAL
{
    public static class FileOperations
    {
        public static List<Medium> MediumsOphalen(string connectionString)
        {
            string query1 = "SELECT * FROM PipeTypes";
            string query2 = "SELECT * FROM SystemTypes";

            List<Medium> mediumList = new List<Medium>();

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                OleDbCommand command1 = new OleDbCommand(query1, connection);
                OleDbCommand command2 = new OleDbCommand(query2, connection);

                connection.Open();

                try
                {
                    using (OleDbDataReader reader = command1.ExecuteReader())
                    {
                        Medium medium;
                        int id = 1;
                        while (reader.Read())
                        {
                            string[] typeEnFabrikant = ((string)reader["segmentNaam"]).Split('-');
                            string volledigeNaam = (string)reader["naam"];
                            medium = new Medium()
                            {
                                Id = id,
                                Naam = volledigeNaam.Substring(0, volledigeNaam.IndexOf('%')),
                                OrgineleNaam = volledigeNaam.Substring(0, volledigeNaam.IndexOf('%')),
                                NominaleDiameter1 = (double)reader["dn1"],
                                NominaleDiameter2 = (double)reader["dn2"],
                                Type = typeEnFabrikant[0].Trim(),
                                Fabrikant = typeEnFabrikant[1].Trim()
                            };

                            mediumList.Add(medium);
                            id++;
                        }
                    }

                    using (OleDbDataReader reader = command2.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string kleur = (string)reader["kleur"];
                            List<byte> rgbList = kleur.Split(',').Select(k => byte.Parse(k)).ToList();

                            IEnumerable<Medium> mediums = mediumList.Where(m => m.Naam == (string)reader["naam"]);

                            foreach (Medium m in mediums)
                            {
                                m.RGBKleur = rgbList;
                                m.Kleur = "#" + rgbList[0].ToString("X2") + rgbList[1].ToString("X2") + rgbList[2].ToString("X2");
                                m.Afkorting = (string)reader["afkorting"];
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    connection.Close();
                    return null;
                }

                connection.Close();
            }

            return mediumList;
        }
        public static List<string> FabrikantenOphalen(string connectionString)
        {
            string query = "SELECT DISTINCT Fabrikant FROM Revit";
            List<string> fabrikanten = new List<string>();

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                OleDbCommand command = new OleDbCommand(query, connection);

                connection.Open();

                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string fabrikant = reader[0] as string;

                        if (fabrikant == null || string.IsNullOrWhiteSpace(fabrikant))
                        {
                            continue;
                        }

                        fabrikanten.Add(fabrikant);
                    }
                }

                connection.Close();
            }

            return fabrikanten;
        }
        public static List<string> TypesOphalen(string connectionString, string fabrikant)
        {
            string query = $"SELECT DISTINCT Type FROM Revit WHERE Fabrikant = \"{fabrikant}\"";
            List<string> types = new List<string>();

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                OleDbCommand command = new OleDbCommand(query, connection);

                connection.Open();

                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string type = reader[0] as string;

                        if (type == null || string.IsNullOrWhiteSpace(type))
                        {
                            continue;
                        }

                        types.Add(type);
                    }
                }

                connection.Close();
            }

            return types;
        }
        public static List<double> Dn1Ophalen(string connectionString, string fabrikant, string type)
        {
            string query = $"SELECT DISTINCT Nominale_diameter FROM Revit WHERE Fabrikant = \"{fabrikant}\" AND Type = \"{type}\"";
            List<double> nominaleDiameters = new List<double>();

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                OleDbCommand command = new OleDbCommand(query, connection);

                connection.Open();

                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        double nd = (double)reader[0];
                        nominaleDiameters.Add(nd);
                    }
                }

                connection.Close();
            }

            return nominaleDiameters;
        }
        public static List<double> Dn2Ophalen(string connectionString, string fabrikant, string type, double dn1)
        {
            string query = $"SELECT DISTINCT Nominale_diameter FROM Revit WHERE Fabrikant = \"{fabrikant}\" AND Type = \"{type}\" AND Nominale_diameter >= {dn1}";
            List<double> nominaleDiameters = new List<double>();

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                OleDbCommand command = new OleDbCommand(query, connection);

                connection.Open();

                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        double nd = (double)reader[0];
                        nominaleDiameters.Add(nd);
                    }
                }

                connection.Close();
            }

            return nominaleDiameters;
        }
    }
}
