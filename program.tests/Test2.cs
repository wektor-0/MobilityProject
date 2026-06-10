using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Reflection;
using program;

namespace program.Tests
{
    [TestClass]
    public class DatenbankTests
    {
        private static string _testDbPath = "test_datenbank.db";

        [TestInitialize]
        public void Setup()
        {
            
            if (File.Exists(_testDbPath))
            {
                File.Delete(_testDbPath);
            }

            var db = Datenbank.GetInstance();

            
            var connectionStringField = typeof(Datenbank).GetField("_connectionString", BindingFlags.NonPublic | BindingFlags.Instance);
            connectionStringField?.SetValue(db, $"Data Source={_testDbPath};Version=3;");

            var setupMethod = typeof(Datenbank).GetMethod("SetupDatabase", BindingFlags.NonPublic | BindingFlags.Instance);
            setupMethod?.Invoke(db, null);
        }

        [TestCleanup]
        public void Cleanup()
        {
           
            if (File.Exists(_testDbPath))
            {
                
                try { File.Delete(_testDbPath); } catch { }
            }
        }

        [TestMethod]
        public void SaveNutzer_SollteNutzerInDatenbankSpeichern()
        {
            // Arrange
            var db = Datenbank.GetInstance();
            var neuerNutzer = new Nutzer(0, "Max", "Mustermann", "max@test.de", 50.00m, 12345);

            // Act
            db.SaveNutzer(neuerNutzer);

            // Assert
            var ausgelesenerNutzer = db.GetNutzerByEmail("max@test.de");

            Assert.IsNotNull(ausgelesenerNutzer);
            Assert.AreEqual("Max", ausgelesenerNutzer.Vorname);
            Assert.AreEqual("Mustermann", ausgelesenerNutzer.Nachname);
            Assert.AreEqual(50.00m, ausgelesenerNutzer.Guthaben);
        }
    }
}