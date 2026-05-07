using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace program
{
    internal class Funktionen
    {
        private Datenbank _db = Datenbank.Instance;


        // Registriet einen neuen Nutzer in der Datenbank
        public void NutzerRegistrieren(Nutzer n)
        {
            using (var conn = Datenbank.Instance.GetConnection())
            {
                conn.Open();
                string sql = "INSERT INTO nutzer (vorname, nachname, email, guthaben, fuehrerschein_nr) " +
                             "VALUES (@v, @n, @e, @g, @f);";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@v", n.Vorname);
                    cmd.Parameters.AddWithValue("@n", n.Nachname);
                    cmd.Parameters.AddWithValue("@e", n.Email);
                    cmd.Parameters.AddWithValue("@g", n.Guthaben);
                    cmd.Parameters.AddWithValue("@f", n.FuehrerscheinNr);

                    cmd.ExecuteNonQuery();
                }
            }
        }


        // Schliesst eine aktive Miete ab und speichert die fnalen Werte
        // Finale Preis wird dann im Code mit BerechnePreis gerechnet
        public void MieteBeenden(int buchungId, decimal berechneterPreis, int endAkku)
        {
            using (var conn = Datenbank.Instance.GetConnection())
            {
                conn.Open();
                string sql = @"UPDATE buchungen 
                       SET endzeit = DATETIME('now'), 
                           betrag = @preis, 
                           end_akku = @akku, 
                           abgeschlossen = 1 
                       WHERE buchung_id = @bid;";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@preis", berechneterPreis);
                    cmd.Parameters.AddWithValue("@akku", endAkku);
                    cmd.Parameters.AddWithValue("@bid", buchungId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}