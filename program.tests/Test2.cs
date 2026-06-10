using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Data.SQLite;
using program;

namespace program.Tests
{
    [TestClass]
    
    [DoNotParallelize]
    public class DatenbankTests
    {
        private string _currentTestDbPath;
        private string _backupConnString;

        [TestInitialize]
        public void Setup()
        {
            _currentTestDbPath = $"test_db_{Guid.NewGuid()}.db";

            var db = Datenbank.GetInstance();

            var connectionStringField = typeof(Datenbank).GetField("_connectionString", BindingFlags.NonPublic | BindingFlags.Instance);
            if (_backupConnString == null)
            {
                _backupConnString = connectionStringField?.GetValue(db)?.ToString();
            }

            string testConnString = $"Data Source={_currentTestDbPath};Version=3;";
            connectionStringField?.SetValue(db, testConnString);

            var setupMethod = typeof(Datenbank).GetMethod("SetupDatabase", BindingFlags.NonPublic | BindingFlags.Instance);
            setupMethod?.Invoke(db, null);

            using (var conn = new SQLiteConnection(testConnString))
            {
                conn.Open();
                string fallBackSql = @"
                    CREATE TABLE IF NOT EXISTS DbInfo (Dbinfo_id INTEGER PRIMARY KEY AUTOINCREMENT, version INTEGER);
                    CREATE TABLE IF NOT EXISTS nutzer (nutzer_id INTEGER PRIMARY KEY AUTOINCREMENT, vorname TEXT, nachname TEXT, email TEXT UNIQUE, guthaben NUMERIC(9,2), fuehrerschein_nr INTEGER);
                    CREATE TABLE IF NOT EXISTS e_fahrzeuge (efz_id INTEGER PRIMARY KEY AUTOINCREMENT, fk_stationen_id INTEGER, standort_lat NUMERIC(7,4), standort_lon NUMERIC(7,4), akkustand INTEGER, status TEXT, kilometerstand INTEGER, tarif NUMERIC(4,2), model TEXT);
                    CREATE TABLE IF NOT EXISTS buchungen (buchung_id INTEGER PRIMARY KEY AUTOINCREMENT, fk_efz_id INTEGER, fk_zahlungsmethoden INTEGER, fk_nutzer_id INTEGER, startzeit TEXT, endzeit TEXT, start_akku INTEGER, end_akku INTEGER, betrag NUMERIC(9,2), distanz NUMERIC(6,2), abgeschlossen INTEGER, status TEXT);
                    INSERT OR IGNORE INTO DbInfo (Dbinfo_id, version) VALUES (1, 1);
                ";
                using (var cmd = new SQLiteCommand(fallBackSql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            var db = Datenbank.GetInstance();

            SQLiteConnection.ClearAllPools();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            if (!string.IsNullOrEmpty(_currentTestDbPath) && File.Exists(_currentTestDbPath))
            {
                try
                {
                    File.Delete(_currentTestDbPath);
                }
                catch (IOException)
                {
                }
            }

            var connectionStringField = typeof(Datenbank).GetField("_connectionString", BindingFlags.NonPublic | BindingFlags.Instance);
            connectionStringField?.SetValue(db, _backupConnString);
        }

        [TestMethod]
        public void Nutzerindbspeichern()
        {
            // Arrange
            var db = Datenbank.GetInstance();
            var neuerNutzer = new Nutzer(0, "Max", "Mustermann", "max@test.de", 50.00m, 12345);

            // Act
            db.SaveNutzer(neuerNutzer);

            // Assert
            var ausgelesenerNutzer = db.GetNutzerByEmail("max@test.de");

            Assert.IsNotNull(ausgelesenerNutzer, "Nutzer wurde nicht gefunden!");
            Assert.AreEqual("Max", ausgelesenerNutzer.Vorname);
            Assert.AreEqual("Mustermann", ausgelesenerNutzer.Nachname);
            Assert.AreEqual(50.00m, ausgelesenerNutzer.Guthaben);
        }

        [TestMethod]
        public void Truewennfahrzeugfrei()
        {
            // Arrange
            var db = Datenbank.GetInstance();
            int testFahrzeugId = 99;

            var connectionStringField = typeof(Datenbank).GetField("_connectionString", BindingFlags.NonPublic | BindingFlags.Instance);
            string connString = connectionStringField?.GetValue(db)?.ToString();

            using (var conn = new SQLiteConnection(connString))
            {
                conn.Open();
                string insertSql = "INSERT INTO e_fahrzeuge (efz_id, status, tarif, model) VALUES (@id, 'bereit', 2.50, 'TestModel')";
                using (var cmd = new SQLiteCommand(insertSql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", testFahrzeugId);
                    cmd.ExecuteNonQuery();
                }
            }

            // Act
            db.UpdateFahrzeugStatus(testFahrzeugId, "verfügbar");
            bool istVerfuegbar = db.IsVehicleAvailable(testFahrzeugId);

            // Assert
            Assert.IsTrue(istVerfuegbar, "Das Fahrzeug sollte als verfügbar markiert sein.");
        }

        [TestMethod]
        public void Guthabenindbändern()
        {
            // Arrange
            var db = Datenbank.GetInstance();
            var nutzer = new Nutzer(0, "Anna", "Schmidt", "anna@test.de", 20.00m, 67890);
            db.SaveNutzer(nutzer);

            var angelegterNutzer = db.GetNutzerByEmail("anna@test.de");
            Assert.IsNotNull(angelegterNutzer);

            // Act
            db.UpdateGuthaben(angelegterNutzer.NutzerId, 45.50m);

            // Assert
            var upgedateterNutzer = db.GetNutzerByEmail("anna@test.de");
            Assert.AreEqual(45.50m, upgedateterNutzer.Guthaben);
        }

        [TestMethod]

        public void Nutzerundbuchunglöschen()
        {
            // Arrange
            var db = Datenbank.GetInstance();

            var connectionStringField = typeof(Datenbank).GetField("_connectionString", BindingFlags.NonPublic | BindingFlags.Instance);
            string connString = connectionStringField?.GetValue(db)?.ToString();

            var nutzer = new Nutzer(0, "Sven", "Müller", "sven@test.de", 10.00m, 11111);
            db.SaveNutzer(nutzer);

            var angelegterNutzer = db.GetNutzerByEmail("sven@test.de");
            Assert.IsNotNull(angelegterNutzer, "Nutzer konnte für den Test nicht angelegt werden.");

            int fahrzeugId = 1;
            int zahlungsMethodeId = 1;

            using (var conn = new SQLiteConnection(connString))
            {
                conn.Open();
                using (var pragmaCmd = new SQLiteCommand("PRAGMA foreign_keys = ON;", conn))
                {
                    pragmaCmd.ExecuteNonQuery();
                }

                string insertFz = "INSERT OR REPLACE INTO e_fahrzeuge (efz_id, status, tarif, model) VALUES (@id, 'verfügbar', 1.50, 'Test')";
                using (var cmd = new SQLiteCommand(insertFz, conn))
                {
                    cmd.Parameters.AddWithValue("@id", fahrzeugId);
                    cmd.ExecuteNonQuery();
                }

                string insertZm = "INSERT OR REPLACE INTO zahlungsmethoden (zm_id, typ) VALUES (@id, 'Kreditkarte')";
                using (var cmd = new SQLiteCommand(insertZm, conn))
                {
                    cmd.Parameters.AddWithValue("@id", zahlungsMethodeId);
                    cmd.ExecuteNonQuery();
                }
            }

            db.SaveBuchung(fahrzeugId, angelegterNutzer.NutzerId, zahlungsMethodeId, 100);

            // Act
            db.DeleteNutzer(angelegterNutzer.NutzerId);

            // Assert
            var geloeschterNutzer = db.GetNutzerByEmail("sven@test.de");
            Assert.IsNull(geloeschterNutzer, "Der Nutzer selbst wurde nicht aus der Datenbank gelöscht.");

            int verbliebeneBuchungen = 0;
            using (var conn = new SQLiteConnection(connString))
            {
                conn.Open();
                string sql = "SELECT COUNT(*) FROM buchungen WHERE fk_nutzer_id = @id";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", angelegterNutzer.NutzerId);
                    verbliebeneBuchungen = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            Assert.AreEqual(0, verbliebeneBuchungen, "Die verknüpften Buchungen des Nutzers wurden nicht gelöscht! Prüfe, ob in der DeleteNutzer-Methode das Löschen der Buchungen implementiert ist.");
        }
    }
}