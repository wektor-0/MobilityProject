using program;

using System.Diagnostics.Eventing.Reader;

INutzerRepository Nutzerdb = Datenbank.GetInstance();

List<Nutzer> AllNutzer = Nutzerdb.GetAllNutzer();

bool programmLäuft = true;

bool istEingeloggt = false;

while (programmLäuft)

{

    Console.Clear();

    Console.WriteLine("--- MENÜ ---");

    Console.WriteLine("1. Anmelden / Registrieren");

    Console.WriteLine("2. Buchungen (Fahrzeug wählen)");

    Console.WriteLine("3. Meine Reservationen");

    Console.WriteLine("4. Beenden");

    Console.Write("\nDeine Wahl: ");

    string eingabe = Console.ReadLine();

    switch (eingabe)

    {

        case "1":

            Anmelden();

            break;

        case "2":

            FahrzeugBuchen();

            break;

        case "3":

            ZeigeReservationen();

            break;

        case "4":

            programmLäuft = false;

            break;

        default:

            Console.WriteLine("Ungültige Eingabe!");

            System.Threading.Thread.Sleep(1000);

            break;

    }

}


void Anmelden()

{

    Console.Clear();

    Console.WriteLine("--- ANMELDUNG ---");

    Console.Write("Vorname: ");

    string VornamenEingegeben = Console.ReadLine();

    for (int i = 0; i < AllNutzer.Count; i++)

    {

        if (VornamenEingegeben == AllNutzer[i].Vorname)

        {

            Console.WriteLine("Email:");

            string EmailEingegeben = Console.ReadLine();

            for (int j = 0; j < AllNutzer.Count; j++)

            {

                if (EmailEingegeben == AllNutzer[j].Email)

                {

                    istEingeloggt = true;

                    return;

                }

            }

        }

        else

        {

            Console.Clear();

            Console.WriteLine("Falsche Logindaten! Bitte versuche erneut oder drücke 1 um zu regristrieren.");//SaveNutzer

        }

    }



    Console.WriteLine("\nErfolgreich angemeldet! Drücke eine Taste...");

    Console.ReadKey();

}

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

    Console.ReadKey();

}
