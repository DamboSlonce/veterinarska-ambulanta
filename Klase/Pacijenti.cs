using System;

namespace SlojPodataka.Klase
{
    public class Pacijenti
    {
        public int IDPacijenta { get; set; }
        public string Ime { get; set; }
        public string Vlasnik { get; set; }
        public string BrojCipa { get; set; }
        public string BrojTelefona { get; set; }
        public string Pol { get; set; }
        public string Alergije { get; set; }
        public bool? Sterilisan { get; set; }
        public int TezinaKg { get; set; }
        public int IDVeterinara { get; set; }
    }
}
