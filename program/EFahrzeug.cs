using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace program
{
    internal abstract class EFahrzeug : IFahrzeug
    {
        internal static int _iddistributor = 1;
        private int _efz_id;
        private decimal _standort_lat;
        private decimal _standort_lon;
        private int _akkustand;
        private string _status;
        private int _kilometerstand;
        private decimal _tarif;
        private string _model;

        public int EfzId
        {
            get { return _efz_id; }
            set { _efz_id = value; }
        }
        public decimal StandortLat
        {
            get { return _standort_lat; }
            set { _standort_lat = value; }
        }
        public decimal StandortLon
        {
            get { return _standort_lon; }
            set { _standort_lon = value; }
        }
        public int Akkustand
        {
            get { return _akkustand; }
            set { _akkustand = value; }
        }
        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }
        public int Kilometerstand
        {
            get { return _kilometerstand; }
            set { _kilometerstand = value; }
        }
        public decimal Tarif
        {
            get { return _tarif; }
            set { _tarif = value; }
        }
        public string Model
        {
            get { return _model; }
            set { _model = value; }
        }

        public EFahrzeug(decimal StandortLat, decimal StandortLon, int Akkustand, string Status, int Kilometerstand, decimal Tarif, string Model)
        {
            EfzId = _iddistributor++;
            this.StandortLat = StandortLat;
            this.StandortLon = StandortLon;
            this.Akkustand = Akkustand;
            this.Status = Status;
            this.Kilometerstand = Kilometerstand;
            this.Tarif = Tarif;
            this.Model = Model;
        }

        public EFahrzeug(int id, decimal StandortLat, decimal StandortLon, int Akkustand, string Status, int Kilometerstand, decimal Tarif, string Model)
        {
            EfzId = id;
            this.StandortLat = StandortLat;
            this.StandortLon = StandortLon;
            this.Akkustand = Akkustand;
            this.Status = Status;
            this.Kilometerstand = Kilometerstand;
            this.Tarif = Tarif;
            this.Model = Model;
        }

        public int GetAkkustand()
            { return _akkustand; }

        public bool IstVerfuegbar()
            { return this.Status.ToLower() == "bereit" && this.Akkustand > 10; 
        }

        public abstract decimal BerechnePreis(int minuten);
    }
}

