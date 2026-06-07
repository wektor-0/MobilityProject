using System;
using System.Data.SQLite;

namespace program
{
    [SQLiteFunction(Name = "SQL_BERECHNE_BETRAG", Arguments = 3, FuncType = FunctionType.Scalar)]
    public class SqlBerechneBetrag : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            if (args[0] == DBNull.Value || args[1] == DBNull.Value || args[2] == DBNull.Value)
                return 0m;

            string fahrzeugTyp = args[0].ToString().ToLower();
            int kilometer = Convert.ToInt32(args[1]);
            decimal tarif = Convert.ToDecimal(args[2]);

            decimal grundgebuehr = 0m;
            if (fahrzeugTyp == "auto") grundgebuehr = 5.00m;
            else if (fahrzeugTyp == "scooter") grundgebuehr = 2.00m;
            else if (fahrzeugTyp == "bike") grundgebuehr = 1.00m;

            return grundgebuehr + (kilometer * tarif);
        }
    }

    [SQLiteFunction(Name = "SQL_PRUEFE_LADESTATUS", Arguments = 2, FuncType = FunctionType.Scalar)]
    public class SqlPruefeLadestatus : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            int? zielStationId = args[0] == DBNull.Value ? (int?)null : Convert.ToInt32(args[0]);
            int akku = Convert.ToInt32(args[1]);
            if (zielStationId.HasValue && akku < 90)
            {
                return "laden";
            }

            return "bereit";
        }
    }
}