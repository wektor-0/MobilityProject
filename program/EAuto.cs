using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace program
{
    internal class EAuto : EFahrzeug
    {

        internal static int _iddistributor = 1;
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

        public EAuto(int Sitzplaetze, string Kennzeichen)
        {
            EAutoId = _iddistributor++;
            this.Sitzplaetze = Sitzplaetze;
            this.Kennzeichen = Kennzeichen;
        }
    }
}
