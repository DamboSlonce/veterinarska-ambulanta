using Klase;
using SlojPodataka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SlojServisa
{
    public class TerminPravila
    {
        private readonly TerminDB _repo;

        public TerminPravila()
        {
            _repo = new TerminDB();
        }

        public List<Termin> VratiSveTermine() => _repo.GetAll();

        public Obavestenje DodajTermin(Termin termin)
        {
            var ispravan = ProveraPodatakaZaTermin(termin);
            if (ispravan.Uspeh)
                _repo.Insert(termin);
            return ispravan;
        }

        public Obavestenje IzmeniTermin(Termin termin)
        {
            var ispravan = ProveraPodatakaZaTermin(termin);
            if (ispravan.Uspeh)
                _repo.Update(termin);
            return ispravan;
        }

        public void ObrisiTermin(int id) => _repo.Delete(id);

        public List<Termin> DajSveTerminePacijenta(int id) => _repo.GetTerminiFromPacijent(id);

        public bool ProveriAktivnostVeterinara(int id) => _repo.DaLiJeVeterinarAktivan(id);

        internal Obavestenje ProveraPodatakaZaTermin(Termin termin)
        {
            if (!_repo.DaLiJeVeterinarAktivan(termin.IDVeterinara))
                return new Obavestenje { Uspeh = false, Poruka = "Ne može da se unese taj veterinar. Veterinar nije aktivan" };

            if (termin.Datum < DateTime.Today)
                return new Obavestenje { Uspeh = false, Poruka = "Ne može pregled u prošlom vremenu" };

            if (termin.Datum > DateTime.Today.AddYears(1))
                return new Obavestenje { Uspeh = false, Poruka = "Ne može pregled da se zakaze vise od godinu dana" };

            string vremeTekst = termin.Vreme.ToString(@"hh\:mm");
            if (!Regex.IsMatch(vremeTekst, @"^(?:[01]\d|2[0-3]):[0-5]\d$"))
                return new Obavestenje { Uspeh = false, Poruka = "Vreme mora da se predstavi u ovom formatu hh:mm, npr: 10:15" };

            if (termin.Vreme.Hours < 8 || termin.Vreme.Hours > 21)
                return new Obavestenje { Uspeh = false, Poruka = "Vreme mora biti u formatu HH:mm između 08:00 i 20:59." };

            if (string.IsNullOrWhiteSpace(termin.VrstaUsluge))
                return new Obavestenje { Uspeh = false, Poruka = "Morate da upisete vrstu usluge (npr. vakcinacija, pregled, sterilizacija)!" };

            var zakazaniTermini = _repo.GetAll();

            bool postojiKonflikt = zakazaniTermini.Any(t =>
                t.IDTermina != termin.IDTermina &&
                t.Datum.Date == termin.Datum.Date &&
                t.IDVeterinara == termin.IDVeterinara &&
                Math.Abs((t.Vreme - termin.Vreme).TotalMinutes) < 45);

            if (postojiKonflikt)
            {
                return new Obavestenje
                {
                    Uspeh = false,
                    Poruka = "Već postoji termin kod istog veterinara na isti dan u razmaku manjem od 45 minuta."
                };
            }

            return new Obavestenje { Uspeh = true, Poruka = "Uspešno je dodat entitet" };
        }
    }
}
