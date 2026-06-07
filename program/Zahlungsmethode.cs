using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace program
{
    internal class Zahlungsmethode
    {
        private int _zm_id;
        private string _typ;

        public int ZmId
        {
            get { return _zm_id; }
            set { _zm_id = value; }
        }
        public string Typ
        {
            get { return _typ; }
            set { _typ = value; }
        }

        public Zahlungsmethode(int id, string Typ)
        {
            ZmId = id;
            this.Typ = Typ;
        }
    }
}

