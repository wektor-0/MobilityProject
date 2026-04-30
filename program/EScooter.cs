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
        public EScooter(int id, decimal standortLat, decimal standortLong, int fahrzeugId, string hersteller, int reichweite, decimal preisProMinute, string modell, int Hoechstgeschwindigkeit)
              : base(id, standortLat, standortLong, fahrzeugId, hersteller, reichweite, preisProMinute, modell)
        {

            this.Hoechstgeschwindigkeit = Hoechstgeschwindigkeit;

        }
    }
}

