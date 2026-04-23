using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace program
{
    internal class EBike : EFahrzeug
    {
        private int _ebike_id;
        private bool _hat_korb;

        public int EBikeId
        {
            get { return _ebike_id; }
            set { _ebike_id = value; }
        }
        public bool HatKorb
        {
            get { return _hat_korb; }
            set { _hat_korb = value; }
        }




        public EBike(decimal standortLat, decimal standortLong, int fahrzeugId, string hersteller, int reichweite, decimal preisProMinute, string modell, bool hatKorb)
               : base(standortLat, standortLong, fahrzeugId, hersteller, reichweite, preisProMinute, modell)
        {
            
            this.HatKorb = hatKorb;
        }

        public override decimal BerechnePreis(int minuten)
        {
            decimal grundgebuehr = 5.00m;

            decimal gesamtPreis = grundgebuehr + (minuten * this.Tarif);

            return gesamtPreis;
        }


    }  
}       


