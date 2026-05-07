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
        void DeleteFahrzeug(int id);
    }

    internal interface INutzerRepository
    {
        List<Nutzer> GetAllNutzer();
        void SaveNutzer(Nutzer n); 
        void UpdateGuthaben(int nutzerId, decimal neuerBetrag);
    }

    internal interface IBuchungsManager
    {
        List<Buchung> GetAllBuchungen();
        void SaveBuchung(int fzId, int nutzerId, int zmId, int startAkku); 
        void BeendeBuchung(int buchungId, int endAkku, decimal betrag); 
    }

    internal interface IStammdatenRepository
    {
        List<Ort> GetAllOrte();
        List<Station> GetAllStationen();
        List<Zahlungsmethode> GetAllZahlungsmethoden();
    }
}
