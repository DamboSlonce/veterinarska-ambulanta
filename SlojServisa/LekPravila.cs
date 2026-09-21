using Klase;
using SlojPodataka;
using SlojPodataka.Klase;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SlojServisa
{
    public class LekPravila
    {
        private readonly LekDB _repo;

        public LekPravila()
        {
            _repo = new LekDB();
        }

        public List<Lek> VratiSveLekove() => _repo.GetAll();
        public List<Lek> VratiSveAktivneLekove() => _repo.GetAllActiveLek();

        public Obavestenje DodajLek(Lek lek)
        {
            var ispravan = ProveraPodatakaZaLek(lek);
            if (ispravan.Uspeh)
                _repo.Insert(lek);
            return ispravan;
        }

        public Obavestenje IzmeniLek(Lek lek)
        {
            var ispravan = ProveraPodatakaZaLek(lek);
            if (ispravan.Uspeh)
                _repo.Update(lek);
            return ispravan;
        }

        public void ObrisiLek(int id) => _repo.SoftDelete(id);

        internal Obavestenje ProveraPodatakaZaLek(Lek lek)
        {
            if (string.IsNullOrWhiteSpace(lek.Naziv))
                return new Obavestenje { Uspeh = false, Poruka = "Naziv leka je obavezan." };

            if (!Regex.IsMatch(lek.Naziv, @"^[A-Za-zČčĆćŠšĐđŽž0-9\- ]{1,100}$"))
                return new Obavestenje { Uspeh = false, Poruka = "Naziv leka može imati do 100 karaktera i ne sme sadržati specijalne znakove." };

            if (string.IsNullOrWhiteSpace(lek.Proizvodjac))
                return new Obavestenje { Uspeh = false, Poruka = "Proizvođač leka je obavezan." };

            if (!Regex.IsMatch(lek.Proizvodjac, @"^[A-Za-zČčĆćŠšĐđŽž0-9\- ]{1,100}$"))
                return new Obavestenje { Uspeh = false, Poruka = "Proizvođač može imati do 100 karaktera i ne sme sadržati specijalne znakove." };

            if (string.IsNullOrWhiteSpace(lek.Jacina))
                return new Obavestenje { Uspeh = false, Poruka = "Jačina leka je obavezna." };

            if (!Regex.IsMatch(lek.Jacina, @"^[0-9]+(mg|g|ml|mcg)$"))
                return new Obavestenje { Uspeh = false, Poruka = "Jačina mora biti broj sa jedinicom mg|g|ml|mcg (npr. 500mg)." };

            if (string.IsNullOrWhiteSpace(lek.Doziranje))
                return new Obavestenje { Uspeh = false, Poruka = "Doziranje leka je obavezno." };

            if (!Regex.IsMatch(lek.Doziranje, @"^[A-Za-z0-9ČčĆćŠšĐđŽž\-\/ ]{1,100}$"))
                return new Obavestenje { Uspeh = false, Poruka = "Doziranje može imati do 100 karaktera i ne sme sadržati specijalne znakove." };

            return new Obavestenje { Uspeh = true, Poruka = "Uspešno je dodat entitet" };
        }
    }
}
