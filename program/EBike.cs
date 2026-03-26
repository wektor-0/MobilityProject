using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace program
{
    internal class EBike
    {
        internal static int _iddistributor = 1;
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

        public EBike(bool HatKorb)
        {
            EBikeId = _iddistributor++;
            this.HatKorb = HatKorb;
        }
    }
}

