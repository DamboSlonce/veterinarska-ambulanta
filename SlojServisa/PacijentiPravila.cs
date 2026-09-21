using Klase;
using SlojPodataka;
using SlojPodataka.Klase;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SlojServisa
{
    public class PacijentPravila
    {
        private readonly PacijentiDB _repo;

        public PacijentPravila()
        {
            _repo = new PacijentiDB();
        }

        public List<Pacijenti> VratiSvePacijente() => _repo.GetAll();

        public Obavestenje DodajPacijenta(Pacijenti pacijent)
        {
            var ispravan = ProveraPodatakaPacijenta(pacijent);
            if (ispravan.Uspeh)
                _repo.Insert(pacijent);
            return ispravan;
        }

        public Obavestenje IzmeniPacijenta(Pacijenti pacijent)
        {
            var ispravan = ProveraPodatakaPacijenta(pacijent);
            if (ispravan.Uspeh)
                _repo.Update(pacijent);
            return ispravan;
        }

        public Pacijenti vratiPacijentaPoBrojuCipa(string brojCipa) => _repo.findPacijent(brojCipa);
        public void ObrisiPacijenta(int id) => _repo.Delete(id);

        internal Obavestenje ProveraPodatakaPacijenta(Pacijenti pacijent)
        {
            if (string.IsNullOrWhiteSpace(pacijent.Ime))
                return new Obavestenje { Uspeh = false, Poruka = "Ime ljubimca je obavezno." };

            if (!Regex.IsMatch(pacijent.Ime, @"^[A-Za-zČčĆćŠšĐđŽž ]{1,50}$"))
                return new Obavestenje { Uspeh = false, Poruka = "Limit za ime ljubimca je 50 karaktera i ne može da sadrži brojeve" };

            if (string.IsNullOrWhiteSpace(pacijent.Vlasnik))
                return new Obavestenje { Uspeh = false, Poruka = "Ime vlasnika je obavezno." };

            if (!Regex.IsMatch(pacijent.Vlasnik, @"^[A-Za-zČčĆćŠšĐđŽž ]{1,50}$"))
                return new Obavestenje { Uspeh = false, Poruka = "Limit za ime vlasnika je 50 karaktera i ne može da sadrži brojeve" };

            if (!Regex.IsMatch(pacijent.BrojCipa ?? "", @"^\d{15}$"))
                return new Obavestenje { Uspeh = false, Poruka = "Broj čipa mora da ima 15 brojeva" };

            var postojeci_pacijent = _repo.findPacijentByID(pacijent.IDPacijenta);

            if (postojeci_pacijent == null)
            {
                if (_repo.checkIfBrojCipaExists(pacijent.BrojCipa))
                    return new Obavestenje { Uspeh = false, Poruka = "Ljubimac sa ovim brojem čipa: " + pacijent.BrojCipa + " već postoji" };
            }
            else if (postojeci_pacijent.BrojCipa != pacijent.BrojCipa && _repo.checkIfBrojCipaExists(pacijent.BrojCipa))
            {
                return new Obavestenje { Uspeh = false, Poruka = "Ljubimac sa ovim brojem čipa: " + pacijent.BrojCipa + " već postoji" };
            }

            if (string.IsNullOrWhiteSpace(pacijent.BrojTelefona))
                return new Obavestenje { Uspeh = false, Poruka = "Broj telefona vlasnika je obavezan." };

            if (!Regex.IsMatch(pacijent.BrojTelefona, @"^[\d\+\-\(\)\s]{1,20}$"))
                return new Obavestenje { Uspeh = false, Poruka = "Primeri kako treba da izgleda broj telefona: +381641234567, 064-123-4567" };

            if (string.IsNullOrWhiteSpace(pacijent.Pol))
                return new Obavestenje { Uspeh = false, Poruka = "Pol ljubimca je obavezan." };

            if (pacijent.Sterilisan == null)
                return new Obavestenje
                {
                    Uspeh = false,
                    Poruka = "Odgovor za sterilizaciju nije prepoznatljiv. Dostupni odgovori: da, jeste, sterilisan, kastriran, yes, true, 1 ili ne, nije, no, false, 0, /"
                };

            if (pacijent.TezinaKg <= 0)
                return new Obavestenje { Uspeh = false, Poruka = "Težina ljubimca mora biti veća od 0 kg." };

            if (pacijent.IDVeterinara <= 0)
                return new Obavestenje { Uspeh = false, Poruka = "Veterinar mora biti izabran." };

            return new Obavestenje { Uspeh = true, Poruka = "Uspešno je dodat pacijent" };
        }
    }
}
