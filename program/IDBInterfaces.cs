using System;
using System.Collections.Generic;

namespace program
{
    internal interface IFahrzeugRepository
    {
        List<EAuto> GetAllAutos();
        List<EBike> GetAllBikes();
        List<EScooter> GetAllScooters();
        bool IsVehicleAvailable(int fahrzeugId);
        void UpdateFahrzeugStatus(int id, string status); 
    }

    internal interface INutzerRepository
    {
        List<Nutzer> GetAllNutzer();
        void SaveNutzer(Nutzer n);
        void DeleteNutzer(int id);
        Nutzer GetNutzerByEmail(string email);
        void UpdateGuthaben(int nutzerId, decimal neuerBetrag);
    }

    internal interface IBuchungsManager
    {
        List<Buchung> GetAllBuchungen();
        void SaveBuchung(int fzId, int nutzerId, int zmId, int startAkku);
        void BeendeBuchung(int buchungId, int nutzerId, int fahrzeugId, int kilometer, int zielStationId, decimal neueLat, decimal neueLon);
        string GetFahrzeugModelName(int fahrzeugId);
    }

    internal interface IStammdatenRepository
    {
        List<Ort> GetAllOrte();
        List<Station> GetAllStationen();
        List<Zahlungsmethode> GetAllZahlungsmethoden();
    }
}
