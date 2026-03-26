using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace program
{
    internal class Nutzer
    {
        internal static int _iddistributor = 1;
        private int _nutzer_id;
        private string _vorname;
        private string _nachname;
        private string _email;
        private string _pw;
        private decimal _guthaben;
        private int _fuehrerschein_nr;

        public int NutzerId
        {
            get { return _nutzer_id; }
            set { _nutzer_id = value; }
        }
        public string Vorname
        {
            get { return _vorname; }
            set { _vorname = value; }
        }
        public string Nachname
        {
            get { return _nachname; }
            set { _nachname = value; }
        }
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        public string Pw
        {
            get { return _pw; }
            set { _pw = value; }
        }
        public decimal Guthaben
        {
            get { return _guthaben; }
            set { _guthaben = value; }
        }
        public int FuehrerscheinNr
        {
            get { return _fuehrerschein_nr; }
            set { _fuehrerschein_nr = value; }
        }

        public Nutzer(string Vorname, string Nachname, string Email, string Pw, decimal Guthaben, int FuehrerscheinNr)
        {
            NutzerId = _iddistributor++;
            this.Vorname = Vorname;
            this.Nachname = Nachname;
            this.Email = Email;
            this.Pw = Pw;
            this.Guthaben = Guthaben;
            this.FuehrerscheinNr = FuehrerscheinNr;
        }
    }
}

