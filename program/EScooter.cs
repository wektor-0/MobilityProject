using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace program
{
    internal class EScooter
    {
        internal static int _iddistributor = 1;
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

        public EScooter(int Hoechstgeschwindigkeit)
        {
            EScooterId = _iddistributor++;
            this.Hoechstgeschwindigkeit = Hoechstgeschwindigkeit;
        }
    }
}

