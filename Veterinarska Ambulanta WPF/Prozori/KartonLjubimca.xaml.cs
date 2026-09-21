using Klase;
using SlojServisa;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Veterinarska_Ambulanta_WPF.Prozori
{
    public partial class KartonLjubimca : Window
    {
        private readonly PacijentPravila _pacijentRepo;
        private readonly TerminPravila _terminiRepo;
        private readonly PregledPravila _preglediRepo;

        public KartonLjubimca(string brojCipa)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _pacijentRepo = new PacijentPravila();
            _terminiRepo = new TerminPravila();
            _preglediRepo = new PregledPravila();
            binDataGrid(brojCipa);
        }

        private void binDataGrid(string brojCipa)
        {
            var pacijent = _pacijentRepo.vratiPacijentaPoBrojuCipa(brojCipa);
            ImeLabel.Content = "Ime ljubimca: " + pacijent.Ime;
            VlasnikLabel.Content = "Vlasnik: " + pacijent.Vlasnik;
            CipLabel.Content = "Broj čipa: " + pacijent.BrojCipa;
            TelefonLabel.Content = "Telefon: " + pacijent.BrojTelefona;
            PolLabel.Content = "Pol: " + pacijent.Pol;
            AlergijaLabel.Content = "Alergija: " + pacijent.Alergije;
            SterilisanLabel.Content = "Sterilisan: " + pacijent.Sterilisan;
            TezinaLabel.Content = "Težina: " + pacijent.TezinaKg + " kg";

            var termini = _terminiRepo.DajSveTerminePacijenta(pacijent.IDPacijenta);
            dgTermini.ItemsSource = termini;

            List<Pregled> pregledi = new List<Pregled>();
            foreach (var termin in termini)
            {
                pregledi.AddRange(_preglediRepo.DajSvePregledePacijenta(termin.IDTermina));
            }
            dgPregledi.ItemsSource = pregledi;

            if (termini.Count == 0)
            {
                MessageBox.Show("Trenutno nema ništa uneto od termina i pregleda za ovog ljubimca", "Informacija", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void dgPregledi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var pregled = dgPregledi.SelectedItem as Pregled;
            if (pregled == null) return;
            txtIzvestaj.Text = pregled.Izvestaj;
        }
    }
}
