using program;
using System.IO;
using System;
using System.Diagnostics.Eventing.Reader;

INutzerRepository Nutzerdb = Datenbank.GetInstance();

List<Nutzer> AllNutzer = Nutzerdb.GetAllNutzer();

bool programmLäuft = true;
Nutzer currentuser = null;
bool istEingeloggt = false;

while (programmLäuft)
{
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
    Console.WriteLine($"--- MENÜ (Eingeloggt als: {currentuser?.Vorname}) ---");
    Console.WriteLine("1. Ausloggen"); 
    Console.WriteLine("2. Buchungen (Fahrzeug wählen)");
    Console.WriteLine("3. Meine Reservationen");
    Console.WriteLine("4. Beenden");
    Console.Write("\nDeine Wahl: ");

    string eingabe = Console.ReadLine();

    switch (eingabe)
    {
        case "1":
            istEingeloggt = false;
            currentuser = null;
            Console.WriteLine("Erfolgreich abgemeldet.");   
            break;

        case "2":
            //FahrzeugBuchen();
            break;

        case "3":
            //ZeigeReservationen();
            break;

        case "4":
            programmLäuft = false;
            break;

        default:
            Console.WriteLine("Ungültige Eingabe!");           
            break;
    }
}


void Anmelden()
{
    Console.Clear();
    Console.WriteLine("--- ANMELDUNG ---");
    Console.Write("Vorname: ");
    string VornamenEingegeben = Console.ReadLine();

    Console.Write("Email: ");
    string EmailEingegeben = Console.ReadLine();

    bool nutzerGefunden = false;
    bool emailExistiert = false;

    for (int i = 0; i < AllNutzer.Count; i++)
    {
        if (VornamenEingegeben == AllNutzer[i].Vorname && EmailEingegeben == AllNutzer[i].Email)
        {
            istEingeloggt = true;
            currentuser = AllNutzer[i];
            nutzerGefunden = true;

            Console.WriteLine($"\nSuper, Hallo {currentuser.Vorname}! Erfolgreich angemeldet.");
            Console.WriteLine("Drücke eine Taste, um ins Menü zu gelangen...");
            Console.ReadKey();
            return;
        }

        if (EmailEingegeben == AllNutzer[i].Email)
        {
            emailExistiert = true;
        }
    }

    if (!nutzerGefunden && emailExistiert)
    {
        Console.WriteLine("\nFalscher Vorname für diese E-Mail-Adresse! Bitte versuche es erneut.");
        Console.WriteLine("Drücke eine Taste zum Wiederholen...");
        Console.ReadKey();
    }
    else if (!nutzerGefunden && !emailExistiert)
    {
        Console.WriteLine("\nDiese E-Mail ist noch nicht registriert!");
        Console.Write("Möchtest du ein neues Konto erstellen? (ja/nein): ");
        string antwort = Console.ReadLine()?.ToLower();

        if (antwort == "ja" || antwort == "j")
        {
            Registrieren(VornamenEingegeben, EmailEingegeben);
        }
    }
}

void Registrieren(string vorname, string email)
{
    Console.Clear();
    Console.WriteLine("--- REGISTRIERUNG ---");
    Console.WriteLine($"Vorname: {vorname}");
    Console.WriteLine($"Email: {email}\n");

    Console.Write("Bitte gib deinen Nachnamen ein: ");
    string nachname = Console.ReadLine();

    Console.Write("Bitte gib deine Führerscheinnummer ein: ");
    int fsNummer;
    while (!int.TryParse(Console.ReadLine(), out fsNummer))
    {
        Console.Write("Ungültige Nummer! Bitte gib nur Zahlen ein: ");
    }

    Console.Write("Bitte zahle dein StartGuthaben ein: ");
    decimal StartGuthaben;
    while (!decimal.TryParse(Console.ReadLine(), out StartGuthaben))
    {
        Console.Write("Ungültiger Betrag! Bitte gib eine Zahl ein (z.B. 50 oder 50.00): ");
    }

    Nutzer neuerNutzer = new Nutzer(vorname, nachname, email, StartGuthaben, fsNummer);

    Nutzerdb.SaveNutzer(neuerNutzer);
    AllNutzer = Nutzerdb.GetAllNutzer();

    Console.WriteLine("\nKonto erfolgreich erstellt! Du kannst dich jetzt einloggen.");
    Console.WriteLine("Drücke eine Taste...");
    Console.ReadKey();
}
/*
static void FahrzeugBuchen()

{

    Console.Clear();

    if (!istEingeloggt)

    {

        Console.WriteLine("Fehler: Du musst dich zuerst anmelden!");

    }

    else

    {

        Console.WriteLine("--- VERFÜGBARE FAHRZEUGE ---");

        for (int i = 0; i < verfügbareFahrzeuge.Count; i++)

        {

            Console.WriteLine($"{i + 1}. {verfügbareFahrzeuge[i]}");

        }

        Console.Write("\nNummer wählen zum Buchen: ");

        if (int.TryParse(Console.ReadLine(), out int wahl) && wahl > 0 && wahl <= verfügbareFahrzeuge.Count)

        {

            // Logik: Aus der Verfügbarkeit entfernen -> Zu Reservationen hinzufügen

            string gewählt = verfügbareFahrzeuge[wahl - 1];

            meineReservationen.Add(gewählt);

            verfügbareFahrzeuge.RemoveAt(wahl - 1);

            Console.WriteLine($"\n{gewählt} wurde erfolgreich zu deinen Reservationen hinzugefügt!");

        }

        else

        {

            Console.WriteLine("Ungültige Wahl.");

        }

    }

    Console.ReadKey();

}

static void ZeigeReservationen()

{

    Console.Clear();

    Console.WriteLine("--- DEINE RESERVATIONEN ---");

    if (meineReservationen.Count == 0)

    {

        Console.WriteLine("Du hast noch keine Fahrzeuge reserviert.");

    }

    else

    {

        foreach (string item in meineReservationen)

        {

            Console.WriteLine($"- {item}");

        }

    }

    Console.WriteLine("\nDrücke eine Taste zum Zurückkehren...");

    Console.ReadKey();*/

//}
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
