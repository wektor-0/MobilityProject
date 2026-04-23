using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace program
{
    internal class EAuto : EFahrzeug
    {

       
        private int _eauto_id;
        private int _sitzplaetze;
        private string _kennzeichen;

        public int EAutoId
        {
            get { return _eauto_id; }
            set { _eauto_id = value; }
        }
        public int Sitzplaetze
        {
            get { return _sitzplaetze; }
            set { _sitzplaetze = value; }
        }
        public string Kennzeichen
        {
            get { return _kennzeichen; }
            set { _kennzeichen = value; }
        }

        public EAuto(decimal standortLat, decimal standortLong, int fahrzeugId, string hersteller, int reichweite, decimal preisProMinute, string modell, int sitzplaetze, string kennzeichen)
             : base(standortLat, standortLong, fahrzeugId, hersteller, reichweite, preisProMinute, modell)
        {
            this.Sitzplaetze = sitzplaetze;
            this.Kennzeichen = kennzeichen;
   
        }
    }
}
