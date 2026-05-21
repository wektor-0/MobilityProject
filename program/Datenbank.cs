using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace program
{
    internal class Datenbank : IFahrzeugRepository, INutzerRepository, IBuchungsManager, IStammdatenRepository
    {
        private static Datenbank _instance;
        private string _dbPath = "datenbank.db";
        private string _connectionString;

        private Datenbank()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _dbPath = Path.Combine(baseDir, "datenbank.db");
            _connectionString = $"Data Source={_dbPath};Version=3;";

            // Wir rufen Setup direkt auf. Setup muss selbst merken, ob Arbeit nötig ist.
            SetupDatabase();
        }
        public static Datenbank GetInstance()
        {
            if (_instance == null) _instance = new Datenbank();
            return _instance;
        }

        private void SetupDatabase()
        {
            // Wir prüfen, ob die Tabelle DbInfo existiert. Wenn nicht, ist die DB neu/leer.
            bool dbNeu = false;
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT count(*) FROM sqlite_master WHERE type='table' AND name='DbInfo';";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    dbNeu = Convert.ToInt32(cmd.ExecuteScalar()) == 0;
                }
            }

            if (dbNeu)
            {
                Console.WriteLine("Datenbank leer. Initialisiere Schema...");
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

        public bool IsVehicleAvailable(int fahrzeugId)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT status FROM e_fahrzeuge WHERE efz_id = @id";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", fahrzeugId);
                    string status = cmd.ExecuteScalar()?.ToString();
                    return status == "verfügbar";
                }
            }
        }

        public void UpdateFahrzeugStatus(int id, string status)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "UPDATE e_fahrzeuge SET status = @status WHERE efz_id = @id";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteFahrzeug(int id)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "DELETE FROM e_fahrzeuge WHERE efz_id = @id";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SaveNutzer(Nutzer n)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = @"INSERT INTO nutzer (vorname, nachname, email, guthaben, fuehrerschein_nr) 
                       VALUES (@vn, @nn, @mail, @gut, @fs)";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@vn", n.Vorname);
                    cmd.Parameters.AddWithValue("@nn", n.Nachname);
                    cmd.Parameters.AddWithValue("@mail", n.Email);
                    cmd.Parameters.AddWithValue("@gut", n.Guthaben);
                    cmd.Parameters.AddWithValue("@fs", n.FuehrerscheinNr);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateGuthaben(int nutzerId, decimal neuerBetrag)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "UPDATE nutzer SET guthaben = @betrag WHERE nutzer_id = @id";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@betrag", neuerBetrag);
                    cmd.Parameters.AddWithValue("@id", nutzerId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SaveBuchung(int fahrzeugId, int nutzerId, int zmId, int startAkku)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = @"INSERT INTO buchungen 
            (fk_efz_id, fk_nutzer_id, fk_zahlungsmethoden, startzeit, start_akku, abgeschlossen, status) 
            VALUES (@efz, @nutzer, @zm, @start, @akku, 0, 'aktiv')";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@efz", fahrzeugId);
                    cmd.Parameters.AddWithValue("@nutzer", nutzerId);
                    cmd.Parameters.AddWithValue("@zm", zmId);
                    cmd.Parameters.AddWithValue("@start", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@akku", startAkku);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void BeendeBuchung(int buchungId, int endAkku, decimal betrag)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = @"UPDATE buchungen SET 
                       endzeit = @end, 
                       end_akku = @akku, 
                       betrag = @betrag, 
                       abgeschlossen = 1, 
                       status = 'beendet' 
                       WHERE buchung_id = @id";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@end", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@akku", endAkku);
                    cmd.Parameters.AddWithValue("@betrag", betrag);
                    cmd.Parameters.AddWithValue("@id", buchungId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
