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
                ExecuteSqlScript("Createscript.sql");
                if (File.Exists("MobilityInsert.sql"))
                {
                    ExecuteSqlScript("MobilityInsert.sql");
                }
            }

            int v = GetCurrentDatabaseVersion() + 1; 

            while (File.Exists($"updates/update_v{v}.sql"))
            {
                string updateFile = $"updates/update_v{v}.sql";

                try
                {
                    ExecuteSqlScript(updateFile);
                    UpdateVersionInDatabase(v);
                    Console.WriteLine($"Update auf Version {v} erfolgreich abgeschlossen.");
                    v++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"FEHLER bei Update {v}: {ex.Message}");
                    break; 
                }
            }
        }

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

        public List<Station> GetAllStationen()
        {
            List<Station> liste = new List<Station>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM stationen";
                using (var cmd = new SQLiteCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        liste.Add(new Station(
                            reader.GetInt32(reader.GetOrdinal("stationen_id")),
                            reader.GetInt32(reader.GetOrdinal("fk_ort_id")),
                            reader.GetString(reader.GetOrdinal("adresse")),
                            reader.GetInt32(reader.GetOrdinal("kapazitaet"))
                        ));
                    }
                }
            }
            return liste;
        }



        public List<Nutzer> GetAllNutzer()
        {
            List<Nutzer> liste = new List<Nutzer>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM nutzer";
                using (var cmd = new SQLiteCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        liste.Add(new Nutzer(
                            reader.GetInt32(reader.GetOrdinal("nutzer_id")),
                            reader.GetString(reader.GetOrdinal("vorname")),
                            reader.GetString(reader.GetOrdinal("nachname")),
                            reader.GetString(reader.GetOrdinal("email")),
                            Convert.ToDecimal(reader["guthaben"]),
                            reader.GetInt32(reader.GetOrdinal("fuehrerschein_nr"))
                        ));
                    }
                }
            }
            return liste;
        }

        public List<Ort> GetAllOrte()
        {
            List<Ort> liste = new List<Ort>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM orte";
                using (var cmd = new SQLiteCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        liste.Add(new Ort(
                            reader.GetInt32(reader.GetOrdinal("orte_id")),
                            reader.GetInt32(reader.GetOrdinal("plz")),
                            reader.GetString(reader.GetOrdinal("name"))
                        ));
                    }
                }
            }
            return liste;
        }

        public List<Zahlungsmethode> GetAllZahlungsmethoden()
        {
            List<Zahlungsmethode> liste = new List<Zahlungsmethode>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM zahlungsmethoden";
                using (var cmd = new SQLiteCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        liste.Add(new Zahlungsmethode(
                            reader.GetInt32(reader.GetOrdinal("zm_id")),
                            reader.GetString(reader.GetOrdinal("typ"))
                        ));
                    }
                }
            }
            return liste;
        }

        public List<Buchung> GetAllBuchungen()
        {
            List<Buchung> liste = new List<Buchung>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM buchungen";
                using (var cmd = new SQLiteCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        liste.Add(new Buchung(
                            reader.GetInt32(reader.GetOrdinal("buchung_id")),
                            reader.GetInt32(reader.GetOrdinal("fk_efz_id")),
                            reader.GetInt32(reader.GetOrdinal("fk_zahlungsmethoden")),
                            reader.GetInt32(reader.GetOrdinal("fk_nutzer_id")),
                            DateTime.Parse(reader.GetString(reader.GetOrdinal("startzeit"))),
                            reader.IsDBNull(reader.GetOrdinal("endzeit")) ? (DateTime?)null : DateTime.Parse(reader.GetString(reader.GetOrdinal("endzeit"))),
                            reader.GetInt32(reader.GetOrdinal("start_akku")),
                            reader.GetInt32(reader.GetOrdinal("end_akku")),
                            Convert.ToDecimal(reader["betrag"]),
                            Convert.ToDecimal(reader["distanz"]),
                            reader.GetInt32(reader.GetOrdinal("abgeschlossen")) == 1,
                            reader.GetString(reader.GetOrdinal("status"))
                        ));
                    }
                }
            }
            return liste;
        }
    }
}
