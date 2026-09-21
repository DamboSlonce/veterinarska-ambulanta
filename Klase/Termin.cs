using System;

namespace Klase
{
    public class Termin
    {
        public int IDTermina { get; set; }
        public DateTime Datum { get; set; }
        public string VrstaUsluge { get; set; }
        public int IDPacijenta { get; set; }
        public int IDVeterinara { get; set; }
        public TimeSpan Vreme { get; set; }
    }
}
