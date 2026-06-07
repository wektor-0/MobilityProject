using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace program
{
    internal class Station
    {
        private int _stationen_id;
        private string _adresse;
        private int _kapazitaet;
        private int _fk_orte_id;


        public int Fk_Orte_Id
        {
            get { return _fk_orte_id; }
            set { _fk_orte_id = value; }
        }

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

        public Station(int id, int Fk_Orte_Id, string Adresse, int Kapazitaet)
        {
            StationenId = id;
            this.Fk_Orte_Id = Fk_Orte_Id;
            this.Adresse = Adresse;
            this.Kapazitaet = Kapazitaet;
        }

    }
}

