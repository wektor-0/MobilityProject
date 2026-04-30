using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace program
{
    internal class EScooter : EFahrzeug
    {
      
        private int _escooter_id;
        private int _hoechstgeschwindigkeit;
        private int _fk_efz_id;


        public int Fk_Efz_id
        {
            get { return _fk_efz_id; }
            set { _fk_efz_id = value; }
        }
        public int EScooterId
        {
            get { return _escooter_id; }
            set { _escooter_id = value; }
        }
        public int Hoechstgeschwindigkeit
        {
            get { return _hoechstgeschwindigkeit; }
            set { _hoechstgeschwindigkeit = value; }
        }


        public EScooter(decimal standortLat, decimal standortLong, int fahrzeugId, string hersteller, int reichweite, decimal preisProMinute, string modell, int Hoechstgeschwindigkeit)
              : base(standortLat, standortLong, fahrzeugId, hersteller, reichweite, preisProMinute, modell)
        {
          
            this.Hoechstgeschwindigkeit = Hoechstgeschwindigkeit;

        }
        public override decimal BerechnePreis(int minuten)
        {
            decimal grundgebuehr = 5.00m;

            decimal gesamtPreis = grundgebuehr + (minuten * this.Tarif);

            return gesamtPreis;
        }
    }
}

