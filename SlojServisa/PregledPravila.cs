using Klase;
using SlojPodataka;
using System.Collections.Generic;

namespace SlojServisa
{
    public class PregledPravila
    {
        private readonly PregledDB _repo;

        public PregledPravila()
        {
            _repo = new PregledDB();
        }

        public List<Pregled> VratiSvePreglede() => _repo.GetAll();

        public Obavestenje DodajPregled(Pregled pregled)
        {
            var ispravan = PravilaZaDodavanjePreglede(pregled);
            if (ispravan.Uspeh)
                _repo.Insert(pregled);
            return ispravan;
        }

        public Obavestenje IzmeniPregled(Pregled pregled)
        {
            var ispravan = PravilaZaDodavanjePreglede(pregled);
            if (ispravan.Uspeh)
                _repo.Update(pregled);
            return ispravan;
        }

        public void ObrisiPregled(int id) => _repo.Delete(id);

        public bool ProveriAktivnostLeka(int id) => _repo.DaLiJeLekAktivan(id);

        public List<Pregled> DajSvePregledePacijenta(int id) => _repo.GetTerminiFromPacijent(id);

        internal Obavestenje PravilaZaDodavanjePreglede(Pregled pregled)
        {
            if (string.IsNullOrWhiteSpace(pregled.Izvestaj))
                return new Obavestenje { Uspeh = false, Poruka = "Izvestaj mora da se napise!" };

            if (pregled.IDTermina <= 0)
                return new Obavestenje { Uspeh = false, Poruka = "Greska prilikom čitanja ID termina." };

            var proveriTermin = _repo.GetTerminWithPregled(pregled.IDTermina);
            if (proveriTermin == null)
                return new Obavestenje { Uspeh = false, Poruka = "Ne postoji dati termin, proverite tabelu termini" };

            if (pregled.IDLeka <= 0)
                return new Obavestenje { Uspeh = false, Poruka = "Greska prilikom čitanja ID leka." };

            return new Obavestenje { Uspeh = true, Poruka = "Uspešno je dodat entitet" };
        }
    }
}
