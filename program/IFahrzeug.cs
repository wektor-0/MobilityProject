using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace program
{
    public interface IFahrzeug
    {

        int GetAkkustand();
        decimal BerechnePreis(int minuten);
        bool IstVerfuegbar();
    }
}
