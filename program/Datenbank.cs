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
            // 1. Erst-Erstellung
            if (!File.Exists(_dbPath))
            {
                SQLiteConnection.CreateFile(_dbPath);
                ExecuteSqlScript("db_struktur.sql");
            }

            // 2. Automatisches Update-System
            int v = GetCurrentDatabaseVersion() + 1; // Wir starten bei der nächsten Nummer

            // Die Schleife läuft so lange, wie sie die passende Datei im Ordner findet
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



        // Hilfsmethode zum Auslesen der Version
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
    }
}
