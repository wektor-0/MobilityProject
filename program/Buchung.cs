using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace program
{
    internal class Buchung
    {
        internal static int _iddistributor = 1;
        private int _buchung_id;
        private DateTime _startzeit;
        private DateTime? _endzeit;
        private int _start_akku;
        private int _end_akku;
        private decimal _betrag;
        private decimal _distanz;
        private bool _abgeschlossen;
        private string _status;
        private int _fk_zahlungsmethoden_id;
        private int _fk_nutzer_id;
        private int _fk_efahrzeuge_id;

        public int FK_Zahlungsmethoden_Id 
        {
            get {return _fk_zahlungsmethoden_id;} 
            set { _fk_zahlungsmethoden_id = value; }
        }
        public int FK_Nutzer_Id
        {
            get { return _fk_nutzer_id; }
            set { _fk_nutzer_id = value; }
        }
        public int FK_Efahrzeuge_Id
        {
            get { return _fk_efahrzeuge_id; }
            set { _fk_efahrzeuge_id = value; }
        }
        public int BuchungId
        {
            get { return _buchung_id; }
            set { _buchung_id = value; }
        }
        public DateTime Startzeit
        {
            get { return _startzeit; }
            set { _startzeit = value; }
        }
        public DateTime? Endzeit
        {
            get { return _endzeit; }
            set { _endzeit = value; }
        }
        public int StartAkku
        {
            get { return _start_akku; }
            set { _start_akku = value; }
        }
        public int EndAkku
        {
            get { return _end_akku; }
            set { _end_akku = value; }
        }
        public decimal Betrag
        {
            get { return _betrag; }
            set { _betrag = value; }
        }
        public decimal Distanz
        {
            get { return _distanz; }
            set { _distanz = value; }
        }
        public bool Abgeschlossen
        {
            get { return _abgeschlossen; }
            set { _abgeschlossen = value; }
        }
        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public Buchung(DateTime Startzeit, DateTime? Endzeit, int StartAkku, int EndAkku, decimal Betrag, decimal Distanz, bool Abgeschlossen, string Status)
        {
            BuchungId = _iddistributor++;
            this.Startzeit = Startzeit;
            this.Endzeit = Endzeit;
            this.StartAkku = StartAkku;
            this.EndAkku = EndAkku;
            this.Betrag = Betrag;
            this.Distanz = Distanz;
            this.Abgeschlossen = Abgeschlossen;
            this.Status = Status;
        }
        public Buchung(int id,int FK_Efahrzeuge_Id, int FK_Zahlungsmethoden_Id, int FK_Nutzer_Id, DateTime Startzeit, DateTime? Endzeit, int StartAkku, int EndAkku, decimal Betrag, decimal Distanz, bool Abgeschlossen, string Status)
        {
            BuchungId = id;
            this.FK_Efahrzeuge_Id = FK_Efahrzeuge_Id;
            this.FK_Nutzer_Id = FK_Nutzer_Id;
            this.FK_Zahlungsmethoden_Id = FK_Zahlungsmethoden_Id;
            this.Startzeit = Startzeit;
            this.Endzeit = Endzeit;
            this.StartAkku = StartAkku;
            this.EndAkku = EndAkku;
            this.Betrag = Betrag;
            this.Distanz = Distanz;
            this.Abgeschlossen = Abgeschlossen;
            this.Status = Status;
        }
    }
}

