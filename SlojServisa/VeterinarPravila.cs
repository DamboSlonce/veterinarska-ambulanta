using Klase;
using SlojPodataka;
using SlojPodataka.Klase;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SlojServisa
{
    public class VeterinarPravila
    {
        private readonly VeterinarDB _repo;

        public VeterinarPravila()
        {
            _repo = new VeterinarDB();
        }

        public List<Veterinar> VratiSveVeterinare() => _repo.GetAll();
        public List<Veterinar> VratiSveAktivneVeterinare() => _repo.GetAllActiveVeterinar();

        public Obavestenje DodajVeterinara(Veterinar veterinar)
        {
            var ispravan = ProveraPodatakaZaVeterinara(veterinar);
            if (ispravan.Uspeh)
                _repo.Insert(veterinar);
            return ispravan;
        }

        public Obavestenje IzmeniVeterinara(Veterinar veterinar)
        {
            var ispravan = ProveraPodatakaZaVeterinara(veterinar);
            if (ispravan.Uspeh)
                _repo.Update(veterinar);
            return ispravan;
        }

        public Obavestenje ObrisiVeterinara(Veterinar veterinar)
        {
            var ispravan = ProveraDaLiSeSmeObrisatiVeterinar(veterinar);
            if (ispravan.Uspeh)
                _repo.SoftDelete(veterinar.IDVeterinara);
            return ispravan;
        }

        internal Obavestenje ProveraPodatakaZaVeterinara(Veterinar veterinar)
        {
            if (string.IsNullOrWhiteSpace(veterinar.Ime))
                return new Obavestenje { Uspeh = false, Poruka = "Ime veterinara je obavezno." };

            if (!Regex.IsMatch(veterinar.Ime, @"^[A-Za-zČčĆćŠšĐđŽž ]{1,50}$"))
                return new Obavestenje { Uspeh = false, Poruka = "Limit za ime veterinara je 50 karaktera i ne može da sadrži brojeve" };

            if (string.IsNullOrWhiteSpace(veterinar.Prezime))
                return new Obavestenje { Uspeh = false, Poruka = "Prezime veterinara je obavezno." };

            if (!Regex.IsMatch(veterinar.Prezime, @"^[A-Za-zČčĆćŠšĐđŽž ]{1,50}$"))
                return new Obavestenje { Uspeh = false, Poruka = "Limit za prezime veterinara je 50 karaktera i ne može da sadrži brojeve" };

            if (!Regex.IsMatch(veterinar.JMBG ?? "", @"^\d{13}$"))
                return new Obavestenje { Uspeh = false, Poruka = "JMBG mora da ima 13 brojeva" };

            var postojeci_veterinar = _repo.findVeterinaraByID(veterinar.IDVeterinara);

            if (postojeci_veterinar == null)
            {
                if (_repo.checkIfJMBGExists(veterinar.JMBG))
                    return new Obavestenje { Uspeh = false, Poruka = "Osoba sa ovim JMBG:" + veterinar.JMBG + " već postoji" };
            }
            else if (postojeci_veterinar.JMBG != veterinar.JMBG && _repo.checkIfJMBGExists(veterinar.JMBG))
            {
                return new Obavestenje { Uspeh = false, Poruka = "Osoba sa ovim JMBG:" + veterinar.JMBG + " već postoji" };
            }

            if (string.IsNullOrWhiteSpace(veterinar.BrojTelefona))
                return new Obavestenje { Uspeh = false, Poruka = "Broj telefona veterinara je obavezan." };

            if (!Regex.IsMatch(veterinar.BrojTelefona, @"^[\d\+\-\(\)\s]{1,20}$"))
                return new Obavestenje { Uspeh = false, Poruka = "Primeri kako treba da izgleda broj telefona: +381641234567, 064-123-4567" };

            if (string.IsNullOrWhiteSpace(veterinar.Email))
                return new Obavestenje { Uspeh = false, Poruka = "Email veterinara je obavezan." };

            if (!Regex.IsMatch(veterinar.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return new Obavestenje { Uspeh = false, Poruka = "Email adresa nije validna." };

            var proveri_email = _repo.findVeterinaraByID(veterinar.IDVeterinara);

            if (proveri_email == null)
            {
                if (_repo.checkIfEmailForVeterinarExists(veterinar.Email))
                    return new Obavestenje { Uspeh = false, Poruka = "Email za ovog veterinara već postoji." };
            }
            else if (proveri_email.Email != veterinar.Email && _repo.checkIfEmailForVeterinarExists(veterinar.Email))
            {
                return new Obavestenje { Uspeh = false, Poruka = "Email za ovog veterinara već postoji. Veterinar koji ga ima je: " + proveri_email.IDVeterinara + " " + proveri_email.Ime + " " + proveri_email.Prezime };
            }

            return new Obavestenje { Uspeh = true, Poruka = "Uspešno je dodat entitet" };
        }

        internal Obavestenje ProveraDaLiSeSmeObrisatiVeterinar(Veterinar veterinar)
        {
            return new Obavestenje { Uspeh = true, Poruka = "Uspešno je obrisan veterinar" };
        }
    }
}
