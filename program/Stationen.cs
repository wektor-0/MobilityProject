using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace program
{
    internal class Stationen
    {
        internal static int _iddistributor = 1;
        private int _stationen_id;
        private string _adresse;
        private int _kapazitaet;

        public int StationenId
        {
            get { return _stationen_id; }
            set { _stationen_id = value; }
        }
        public string Adresse
        {
            get { return _adresse; }
            set { _adresse = value; }
        }
        public int Kapazitaet
        {
            get { return _kapazitaet; }
            set { _kapazitaet = value; }
        }

        public Stationen(string Adresse, int Kapazitaet)
        {
            StationenId = _iddistributor++;
            this.Adresse = Adresse;
            this.Kapazitaet = Kapazitaet;
        }
    }
}

