using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace program
{
    internal class Datenbank
    {
        private string _dbPath = "datenbank.db";
        private string _connectionString;

        public Datenbank()
        {
            _connectionString = $"Data Source={_dbPath};";
            SetupDatabase();
        }

        private void SetupDatabase()
        {
            if (!File.Exists(_dbPath))
            {
                SQLiteConnection.CreateFile(_dbPath);
                ExecuteSqlScript("db_struktur.sql");
            }

            int v = GetCurrentDatabaseVersion() + 1; 

            while (File.Exists($"updates/update_v{v}.sql"))
            {
                string updateFile = $"updates/update_v{v}.sql";

                Console.WriteLine($"Neues Update gefunden: Version {v}. Führe aus...");

                try
                {
                    ExecuteSqlScript(updateFile);
                    UpdateVersionInDatabase(v);
                    Console.WriteLine($"Update auf Version {v} erfolgreich abgeschlossen.");

                    v++; // Zähler erhöhen, um nach der nächsten Datei zu suchen
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"FEHLER bei Update {v}: {ex.Message}");
                    break; // Stop, damit die DB nicht in einem inkonsistenten Zustand landet
                }
            }
        }

        // Hilfsmethode zum Ausführen von Dateien
        private void ExecuteSqlScript(string filePath)
        {
            string sql = File.ReadAllText(filePath);
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
        private void UpdateVersionInDatabase(int neueVersion)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "UPDATE DbInfo SET version = @v;";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@v", neueVersion);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        private int GetCurrentDatabaseVersion()
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT version FROM DbInfo LIMIT 1;";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }


        public List<EAuto> GetAllAutos()
        {
            List<EAuto> liste = new List<EAuto>();

            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM e_fahrzeuge f JOIN e_autos a ON f.efz_id = a.fk_efz_id";

                using (var cmd = new SQLiteCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        EAuto auto = new EAuto(
                            reader.GetInt32(reader.GetOrdinal("efz_id")),
                            Convert.ToDecimal(reader["standort_lat"]),
                            Convert.ToDecimal(reader["standort_lon"]),
                            reader.GetInt32(reader.GetOrdinal("akkustand")),
                            reader.GetString(reader.GetOrdinal("status")),
                            reader.GetInt32(reader.GetOrdinal("kilometerstand")),
                            Convert.ToDecimal(reader["tarif"]),
                            reader.GetString(reader.GetOrdinal("model")),
                            reader.GetInt32(reader.GetOrdinal("sitzplaetze")),
                            reader.GetString(reader.GetOrdinal("kennzeichen"))
                        );
                        liste.Add(auto);
                    }
                }
                return liste;
            }
        }

        public List<EBike> GetAllBikes()
        {
            List<EBike> liste = new List<EBike>();

            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                // Wir brauchen einen Join, um die Basisdaten + Auto-Daten zu kriegen
                string sql = "SELECT * FROM e_fahrzeuge f JOIN e_bikes b ON f.efz_id = b.fk_efz_id";

                using (var cmd = new SQLiteCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        EBike bike = new EBike(
                            reader.GetInt32(reader.GetOrdinal("efz_id")),
                            Convert.ToDecimal(reader["standort_lat"]),
                            Convert.ToDecimal(reader["standort_lon"]),
                            reader.GetInt32(reader.GetOrdinal("akkustand")),
                            reader.GetString(reader.GetOrdinal("status")),
                            reader.GetInt32(reader.GetOrdinal("kilometerstand")),
                            Convert.ToDecimal(reader["tarif"]),
                            reader.GetString(reader.GetOrdinal("model")),
                            reader.GetInt32(reader.GetOrdinal("hat_korb")) == 1
                                );
                        liste.Add(bike);
                    }
                }
            }
            return liste;
        }

        public List<EScooter> GetAllScooters()
        {
            List<EScooter> liste = new List<EScooter>();

            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM e_fahrzeuge f JOIN e_scooter s ON f.efz_id = s.fk_efz_id";

                using (var cmd = new SQLiteCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        EScooter scooter = new EScooter(
                            reader.GetInt32(reader.GetOrdinal("efz_id")),
                            Convert.ToDecimal(reader["standort_lat"]),
                            Convert.ToDecimal(reader["standort_lon"]),
                            reader.GetInt32(reader.GetOrdinal("akkustand")),
                            reader.GetString(reader.GetOrdinal("status")),
                            reader.GetInt32(reader.GetOrdinal("kilometerstand")),
                            Convert.ToDecimal(reader["tarif"]),
                            reader.GetString(reader.GetOrdinal("model")),
                            reader.GetInt32(reader.GetOrdinal("hoechstgeschwindigkeit"))
                        );
                        liste.Add(scooter);
                    }
                }
            }
            return liste;
        }
    }
}
