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
        private int _fk_efz_id;
        

        public int Fk_Efz_id 
        {
            get {  return _fk_efz_id; }
            set {  _fk_efz_id = value;}
        }

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

        public EAuto(int fk_Stationen_Id, int id, decimal standortLat, decimal standortLong, int fahrzeugId, string hersteller, int reichweite, decimal preisProMinute, string modell, int sitzplaetze, string kennzeichen)
             : base(fk_Stationen_Id, id, standortLat, standortLong, fahrzeugId, hersteller, reichweite, preisProMinute, modell)
        {
            this.Sitzplaetze = sitzplaetze;
            this.Kennzeichen = kennzeichen;
        }

        public override bool IstVerfuegbar()
        {
            return this.Status.ToLower() == "bereit" && this.Akkustand >= 20;
        }
    }
}
