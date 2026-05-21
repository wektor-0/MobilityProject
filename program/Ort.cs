using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace program
{
    internal class Ort
    {
        internal static int _iddistributor = 1;
        private int _plz;
        private string _name;
        private int _orte_id;

        public int OrteId 
        {
            get { return _orte_id;}
            set { _orte_id = value; }
        }
        public int Plz
        {
            get { return _plz; } 
            set { _plz = value; }
        }
        public string Name 
        {
            get { return _name; }
            set { _name = value; }
        }

        public Ort(int Plz, string Name) 
        {
            OrteId = _iddistributor++;
            this.Plz = Plz;
            this.Name = Name;
        }
        public Ort(int id, int Plz, string Name)
        {
            OrteId = id;
            this.Plz = Plz;
            this.Name = Name;
        }
    }
}
