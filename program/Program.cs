using program;
using System.IO;
using System;
using System.Diagnostics.Eventing.Reader;

bool programmLäuft = true;
Nutzer currentuser = null;
bool istEingeloggt = false;

while (programmLäuft)
{
    var db = Datenbank.GetInstance();
    db.SimuliereLadevorgang();

    if (!istEingeloggt)
    {
        Anmelden();
    }
    else
    {
        auswahl(); 
    }
}

void auswahl()
{
    Console.Clear();
    Console.WriteLine($"--- MENÜ (Eingeloggt als: {currentuser?.Vorname} {currentuser?.Nachname}) ---");
    Console.WriteLine($"Dein Guthaben: {currentuser?.Guthaben} CHF");
    Console.WriteLine("------------------------------------------");
    Console.WriteLine("1. Fahrzeug buchen");
    Console.WriteLine("2. Konto aufladen");
    Console.WriteLine("3. Meine Reservationen");
    Console.WriteLine("4. Ausloggen");
    Console.WriteLine("5. Konto löschen");
    Console.WriteLine("6. Beenden");
    Console.Write("\nDeine Wahl: ");

    string eingabe = Console.ReadLine();

    switch (eingabe)
    {
        case "1":
            FahrzeugBuchen();
            break;

        case "2":
            KontoAufladen();
            break;

        case "3":
            ZeigeReservationen();
            break;

        case "4":
            istEingeloggt = false;
            currentuser = null;
            Console.WriteLine("\nErfolgreich abgemeldet. Drücke eine Taste...");
            Console.ReadKey();
            break;

        case "5":
            KontoLoeschen();
            break;

        case "6":
            programmLäuft = false;
            break;

        default:
            Console.WriteLine("\nUngültige Eingabe! Drücke eine Taste...");
            Console.ReadKey();
            break;
    }
}


void Anmelden()
{
    Console.Clear();
    Console.WriteLine("--- LOGIN ---");
    Console.Write("Bitte gib deine E-Mail-Adresse ein: ");
    string emailEingegeben = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(emailEingegeben)) return; 

    var dbKonkret = Datenbank.GetInstance();
    Nutzer user = dbKonkret.GetNutzerByEmail(emailEingegeben);

    if (user != null)
    {
        Console.Write("Bitte gib deinen Vornamen zur Bestätigung ein: ");
        string vornameEingegeben = Console.ReadLine();

        if (user.Vorname.ToLower() == vornameEingegeben.ToLower())
        {
            currentuser = user;
            istEingeloggt = true;

            Console.WriteLine("Drücke eine Taste, um ins Menü zu gelangen...");
            Console.ReadKey();
        }
        else
        {
            Console.WriteLine("\nDer Vorname stimmt nicht mit der E-Mail überein!");
            Console.WriteLine("Drücke eine Taste zum Wiederholen...");
            Console.ReadKey();
        }
    }
    else
    {
        Console.WriteLine("\nDiese E-Mail-Adresse ist noch nicht registriert!");
        Console.Write("Möchtest du ein neues Konto erstellen? (ja/nein): ");
        string antwort = Console.ReadLine()?.ToLower();

        if (antwort == "ja" || antwort == "j")
        {
            Registrieren(emailEingegeben);
        }
    }
}

void Registrieren(string email)
{
    Console.Clear();
    Console.WriteLine("--- REGISTRIERUNG ---");
    Console.WriteLine($"Email: {email}\n");

    Console.Write("Vorname: ");
    string vorname = Console.ReadLine()?.Trim();

    Console.Write("Nachname: ");
    string nachname = Console.ReadLine()?.Trim();

    if (string.IsNullOrWhiteSpace(vorname) || string.IsNullOrWhiteSpace(nachname))
    {
        Console.WriteLine("\nNamen dürfen nicht leer sein!");
        Console.WriteLine("Registrierung abgebrochen. Drücke eine Taste...");
        Console.ReadKey();
        return;
    }

    Console.Write("Bitte gib deine Führerscheinnummer ein: ");
    int fsNummer;
    while (!int.TryParse(Console.ReadLine(), out fsNummer))
    {
        Console.Write("Ungültige Nummer! Bitte gib nur Zahlen ein: ");
    }

    INutzerRepository Nutzerrepo = Datenbank.GetInstance();
    Nutzerrepo.SaveNutzer(new Nutzer(0, vorname, nachname, email, 100, fsNummer));

    Console.WriteLine("\nKonto erstellt! Du kannst dich jetzt einloggen.");
    Console.WriteLine("Drücke eine Taste...");
    Console.ReadKey();
}
void KontoAufladen()
{
    Console.Clear();
    Console.WriteLine("--- KONTO AUFLADEN ---");
    Console.WriteLine($"Aktuelles Guthaben: {currentuser.Guthaben} CHF\n");

    IStammdatenRepository stammdatenRepo = Datenbank.GetInstance();
    List<Zahlungsmethode> methoden = stammdatenRepo.GetAllZahlungsmethoden();
    
    Console.WriteLine("Verfügbare Zahlungsmethoden:");
    for (int i = 0; i < methoden.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {methoden[i].Typ}");
    }
    Console.WriteLine($"{methoden.Count + 1}. Abbrechen");

    Console.Write("\nDeine Wahl: ");
    if (!int.TryParse(Console.ReadLine(), out int wahl) || wahl < 1 || wahl > methoden.Count + 1)
    {
        Console.WriteLine("Ungültige Wahl. Vorgang abgebrochen...");
        Console.ReadKey();
        return;
    }

    if (wahl == methoden.Count + 1) return; 

    Zahlungsmethode gewählteMethode = methoden[wahl - 1];

    Console.Write("Bitte gib den Aufladebetrag ein (CHF): ");
    decimal betrag;
    while (!decimal.TryParse(Console.ReadLine(), out betrag) || betrag <= 0)
    {
        Console.Write("Ungültiger Betrag! Bitte gib eine positive Zahl ein: ");
    }

    decimal neuesGuthaben = currentuser.Guthaben + betrag;

    INutzerRepository Nutzerrepo = Datenbank.GetInstance();
    Nutzerrepo.UpdateGuthaben(currentuser.NutzerId, neuesGuthaben);

    currentuser.Guthaben = neuesGuthaben;

    Console.WriteLine($"\nErfolgreich {betrag} CHF via {gewählteMethode.Typ} aufgeladen!");
    Console.WriteLine($"Neues Guthaben: {currentuser.Guthaben} CHF");
    Console.WriteLine("Drücke eine Taste zum Fortfahren...");
    Console.ReadKey();
}
void FahrzeugBuchen()
{
    if (currentuser == null || currentuser.Guthaben < 100.00m)
    {
        Console.Clear();
        Console.WriteLine("--- BUCHUNG GEBLOCKT ---");
        Console.WriteLine($"\nDu kannst kein Fahrzeug buchen, da dein Konto zu tiefes Salso hat.");
        Console.WriteLine("Bitte lade dein Konto im Hauptmenü auf mindestens 100 CHF auf, um wieder fahren zu können.");
        Console.WriteLine("\nDrücke eine Taste, um zum Menü zurückzukehren...");
        Console.ReadKey();
        return; 
    }

    Console.Clear();
    Console.WriteLine("--- FAHRZEUG BUCHEN ---");

    IFahrzeugRepository fahrzeugRepo = Datenbank.GetInstance();
    IBuchungsManager buchungMgr = Datenbank.GetInstance();
    IStammdatenRepository stammdatenRepo = Datenbank.GetInstance();

    List<EFahrzeug> verfügbareFahrzeuge = new List<EFahrzeug>();
    verfügbareFahrzeuge.AddRange(fahrzeugRepo.GetAllAutos().Where(f => f.IstVerfuegbar()));
    verfügbareFahrzeuge.AddRange(fahrzeugRepo.GetAllBikes().Where(f => f.IstVerfuegbar()));
    verfügbareFahrzeuge.AddRange(fahrzeugRepo.GetAllScooters().Where(f => f.IstVerfuegbar()));

    if (verfügbareFahrzeuge.Count == 0)
    {
        Console.WriteLine("Aktuell sind keine Fahrzeuge verfügbar.");
        Console.ReadKey();
        return;
    }

    Console.WriteLine("Verfügbare Fahrzeuge:");
    for (int i = 0; i < verfügbareFahrzeuge.Count; i++)
    {
        EFahrzeug fz = verfügbareFahrzeuge[i];
        Console.WriteLine($"{i + 1}. {fz.Model} (Akku: {fz.Akkustand}%, Tarif: {fz.Tarif} CHF/Km, Status: {fz.Status})");
    }
    Console.WriteLine($"{verfügbareFahrzeuge.Count + 1}. Zurück zum Menü");

    Console.Write("\nWähle ein Fahrzeug (Nummer): ");
    if (!int.TryParse(Console.ReadLine(), out int wahl) || wahl < 1 || wahl > verfügbareFahrzeuge.Count + 1)
    {
        Console.WriteLine("Ungültige Eingabe.");
        Console.ReadKey();
        return;
    }

    if (wahl == verfügbareFahrzeuge.Count + 1) return;

    EFahrzeug gewähltesFahrzeug = verfügbareFahrzeuge[wahl - 1];

    var zm = stammdatenRepo.GetAllZahlungsmethoden().First(zm => zm.Typ == "Guthaben");
    int zmId = zm.ZmId;

    buchungMgr.SaveBuchung(gewähltesFahrzeug.EfzId, currentuser.NutzerId, zmId, gewähltesFahrzeug.Akkustand);
    fahrzeugRepo.UpdateFahrzeugStatus(gewähltesFahrzeug.EfzId, "besetzt");

    Console.WriteLine($"\nErfolgreich gebucht! Gute Fahrt mit dem {gewähltesFahrzeug.Model}.");
    Console.WriteLine("Das Fahrzeug ist nun für dich aktiv geschaltet.");
    Console.WriteLine("Drücke eine Taste...");
    Console.ReadKey();
}

void ZeigeReservationen()
{
    Console.Clear();
    Console.WriteLine("--- DEINE BUCHUNGEN ---");

    IBuchungsManager buchungMgr = Datenbank.GetInstance();
    IStammdatenRepository stammdatenRepo = Datenbank.GetInstance();

    var alleBuchungen = buchungMgr.GetAllBuchungen();
    var meineMieten = alleBuchungen.Where(b => b.FK_Nutzer_Id == currentuser.NutzerId).ToList();

    if (meineMieten.Count == 0)
    {
        Console.WriteLine("Du hast bisher keine Buchungen im System.");
        Console.ReadKey();
        return;
    }

    Console.WriteLine("Aktive Fahrten:");
    var aktiveFahrten = meineMieten.Where(b => !b.Abgeschlossen).ToList();

    if (aktiveFahrten.Count == 0)
    {
        Console.WriteLine(" (Keine aktiven Fahrten im Moment)");
    }
    else
    {
        for (int i = 0; i < aktiveFahrten.Count; i++)
        {
            var b = aktiveFahrten[i];
            string modelName = buchungMgr.GetFahrzeugModelName(b.FK_Efahrzeuge_Id);
            Console.WriteLine($"[{i + 1}] {modelName} | Gestartet am {b.Startzeit}");
        }
    }

    Console.WriteLine("\nVergangene (abgeschlossene) Fahrten:");
    var alteFahrten = meineMieten.Where(b => b.Abgeschlossen).ToList();
    foreach (var b in alteFahrten)
    {
        string modelName = buchungMgr.GetFahrzeugModelName(b.FK_Efahrzeuge_Id);
        Console.WriteLine($"- {modelName} (Buchung #{b.BuchungId}): {b.Distanz} km, Kosten: {b.Betrag} CHF");
    }

    if (aktiveFahrten.Count > 0)
    {
        Console.Write("\nMöchtest du eine aktive Fahrt beenden? (Nummer eingeben oder Enter für Zurück): ");
        string eingabe = Console.ReadLine();

        if (int.TryParse(eingabe, out int index) && index > 0 && index <= aktiveFahrten.Count)
        {
            Buchung zuBeendendeBuchung = aktiveFahrten[index - 1];

            Console.Write("Gefahrene Kilometer eingeben: ");
            if (!int.TryParse(Console.ReadLine(), out int km) || km < 0) km = 0;

            var stationen = stammdatenRepo.GetAllStationen();
            var orte = stammdatenRepo.GetAllOrte();

            if (stationen.Count == 0)
            {
                Console.WriteLine("\nFehler: Keine Rückgabestationen im System registriert. Vorgang abgebrochen.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nWähle den Rückgabeort (Station):");
            for (int i = 0; i < stationen.Count; i++)
            {
                var st = stationen[i];
                var ort = orte.FirstOrDefault(o => o.OrteId == st.Fk_Orte_Id);
                string ortsName = ort != null ? $"{ort.Plz} {ort.Name}" : "Unbekannter Ort";

                Console.WriteLine($"{i + 1}. {st.Adresse} ({ortsName})");
            }

            Console.Write("Deine Wahl (Station-Nummer): ");
            if (!int.TryParse(Console.ReadLine(), out int stationsWahl) || stationsWahl < 1 || stationsWahl > stationen.Count)
            {
                Console.WriteLine("Ungültige Station. Vorgang abgebrochen.");
                Console.ReadKey();
                return;
            }

            int gewählteStationId = stationen[stationsWahl - 1].StationenId;

            try
            {
                buchungMgr.BeendeBuchung(zuBeendendeBuchung.BuchungId, currentuser.NutzerId, zuBeendendeBuchung.FK_Efahrzeuge_Id, km, gewählteStationId, 0m, 0m);
                var dbKonkret = Datenbank.GetInstance();
                Nutzer aktualisierterUser = dbKonkret.GetAllNutzer().First(n => n.NutzerId == currentuser.NutzerId);
                    currentuser.Guthaben = aktualisierterUser.Guthaben;

                Console.WriteLine("\nFahrt erfolgreich beendet! Die Abrechnung wurde durchgeführt.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nFehler beim Beenden der Miete: {ex.Message}");
            }
            Console.ReadKey();
        }
    }
    else
    {
        Console.WriteLine("\nDrücke eine Taste zum Zurückkehren...");
        Console.ReadKey();
    }
}
void TestDatabaseIntegration()
{
    Console.Clear();
    var db = Datenbank.GetInstance();
    IFahrzeugRepository fahrzeugRepo = db;
    IBuchungsManager buchungMgr = db;
    INutzerRepository nutzerRepo = db;

    Console.WriteLine("--- Test startet ---");

    var autos = fahrzeugRepo.GetAllAutos();
    Console.WriteLine($"Autos in DB: {autos.Count}");

    Nutzer testNutzer = new Nutzer(0, "Test", "User", "test@webapp.de", 100.00m, 9999);
    nutzerRepo.SaveNutzer(testNutzer);
    Console.WriteLine("Nutzer gespeichert.");

    if (autos.Count > 0)
    {
        int fzId = autos[0].EfzId;
        fahrzeugRepo.UpdateFahrzeugStatus(fzId, "besetzt");
        Console.WriteLine($"Fahrzeug {fzId} auf 'besetzt' gesetzt.");

        buchungMgr.SaveBuchung(fzId, 1, 1, 80);
        Console.WriteLine("Buchung erstellt.");
    }

    Console.WriteLine("--- Test beendet ---");
}

void KontoLoeschen()
{
    Console.Clear();
    Console.WriteLine("--- KONTO LÖSCHEN ---");
    Console.WriteLine("Möchtest du dein Konto wirklich unwiderruflich löschen?");
    Console.WriteLine("Alle deine Daten und Buchungen werden permanent entfernt.");
    Console.Write("\nBist du sicher? (ja/nein): ");


    string bestätigung = Console.ReadLine()?.ToLower();

    if (bestätigung == "ja" || bestätigung == "j")
    {
        try
        {
            var db = Datenbank.GetInstance();
            INutzerRepository nutzerRepo = db;

            nutzerRepo.DeleteNutzer(currentuser.NutzerId);

            Console.WriteLine("\nDein Konto wurde erfolgreich gelöscht. Auf Wiedersehen!");
            Console.ReadKey();

            currentuser = null;
            istEingeloggt = false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nFehler beim Löschen des Kontos: {ex.Message}");
            Console.ReadKey();
        }
    }
    else
    {
        Console.WriteLine("\nVorgang abgebrochen. Dein Konto ist sicher.");
        Console.ReadKey();
    }
}
