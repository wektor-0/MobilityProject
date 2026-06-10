using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using program; 

namespace program.Tests
{
    [TestClass] 
    public class EScooterTests
    {
        [TestMethod] 
        public void Truewennbereitundakkuüber15()
        {
            // Arrange
            var escooter = new EScooter(1, 1, 0.0m, 0.0m, 1, "Xiaomi", 45, 0.20m, "Pro 2", 20);
            escooter.Status = "bereit";
            escooter.Akkustand = 50;

            // Act
            bool ergebnis = escooter.IstVerfuegbar();

            // Assert
            Assert.IsTrue(ergebnis);
        }

        [TestMethod]
        public void flasewennakkuunter15()
        {
            // Arrange
            var escooter = new EScooter(1, 1, 0.0m, 0.0m, 1, "Xiaomi", 45, 0.20m, "Pro 2", 20);
            escooter.Status = "bereit";
            escooter.Akkustand = 10; 

            // Act
            bool ergebnis = escooter.IstVerfuegbar();

            // Assert
            Assert.IsFalse(ergebnis);
        }
        [TestMethod]
        public void truebeiexakt15()
        {
            // Arrange
            var escooter = new EScooter(1, 1, 0.0m, 0.0m, 1, "Xiaomi", 45, 0.20m, "Pro 2", 20);
            escooter.Status = "bereit";
            escooter.Akkustand = 15;

            // Act
            bool ergebnis = escooter.IstVerfuegbar();

            // Assert
            Assert.IsTrue(ergebnis);
        }

    

        [TestMethod]
        public void Getakkstandrichtig()
        {
            // Arrange
            var escooter = new EScooter(1, 1, 0.0m, 0.0m, 1, "Xiaomi", 45, 0.20m, "Pro 2", 20);
            escooter.Akkustand = 85;

            // Act
            int aktuellerAkku = escooter.GetAkkustand();

            // Assert
            Assert.AreEqual(85, aktuellerAkku);
        }
    }
}