using Klase.Pomocne_klase;
using SlojServisa;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

namespace Veterinarska_Ambulanta_WPF.Prozori
{
    public partial class Pacijenti : Window
    {
        private readonly PacijentPravila _pacijentRepo;
        private readonly VeterinarPravila _veterinarRepo;
        private readonly InputSterilisanCheck _proveriUnosZaSterilizaciju;
        private DispatcherTimer debounceTimer;

        public Pacijenti()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _pacijentRepo = new PacijentPravila();
            _veterinarRepo = new VeterinarPravila();
            _proveriUnosZaSterilizaciju = new InputSterilisanCheck();
            binDataGrid();
        }

        private void ButtonNazad_Click(object sender, RoutedEventArgs e)
        {
            new Meni().Show();
            Close();
        }

        private void DataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (DataGrid.SelectedItem is SlojPodataka.Klase.Pacijenti pacijent)
            {
                TextboxIdPacijenta.Text = pacijent.IDPacijenta.ToString();
                TextboxIme.Text = pacijent.Ime;
                TextboxVlasnik.Text = pacijent.Vlasnik;
                TextboxBrojCipa.Text = pacijent.BrojCipa;
                TextboxBrojTelefona.Text = pacijent.BrojTelefona;
                ComboboxPol.Text = pacijent.Pol;
                TextboxAlergije.Text = pacijent.Alergije;
                TextboxSterilisan.Text = pacijent.Sterilisan == true ? "Da" : "Ne";
                TextboxTezinaKg.Text = pacijent.TezinaKg.ToString();
                ComboboxVeterinar.SelectedValue = pacijent.IDVeterinara;
            }
        }

        private void binDataGrid()
        {
            DataGrid.ItemsSource = _pacijentRepo.VratiSvePacijente();
        }

        private void ButtonDodaj_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidirajUnos())
            {
                MessageBox.Show("Molimo Vas da popunite sva polja za pacijenta.");
                return;
            }

            var pacijent = new SlojPodataka.Klase.Pacijenti
            {
                Ime = TextboxIme.Text,
                Vlasnik = TextboxVlasnik.Text,
                BrojCipa = TextboxBrojCipa.Text,
                BrojTelefona = TextboxBrojTelefona.Text,
                Pol = ComboboxPol.Text,
                Alergije = TextboxAlergije.Text,
                Sterilisan = _proveriUnosZaSterilizaciju.proveraUnosaZaSterilizaciju(TextboxSterilisan.Text),
                TezinaKg = int.Parse(TextboxTezinaKg.Text),
                IDVeterinara = int.Parse(ComboboxVeterinar.SelectedValue.ToString())
            };

            var poruka = _pacijentRepo.DodajPacijenta(pacijent);
            if (!poruka.Uspeh)
            {
                MessageBox.Show(poruka.Poruka);
                return;
            }
            binDataGrid();
            ponistiUnosTxt();
        }

        private bool ValidirajUnos()
        {
            if (string.IsNullOrWhiteSpace(TextboxIme.Text)) return false;
            if (string.IsNullOrWhiteSpace(TextboxVlasnik.Text)) return false;
            if (string.IsNullOrWhiteSpace(TextboxBrojCipa.Text)) return false;
            if (string.IsNullOrWhiteSpace(TextboxBrojTelefona.Text)) return false;
            if (string.IsNullOrWhiteSpace(ComboboxPol.Text)) return false;
            if (string.IsNullOrWhiteSpace(TextboxTezinaKg.Text)) return false;
            if (!int.TryParse(TextboxTezinaKg.Text, out _)) return false;
            if (ComboboxVeterinar.SelectedValue == null) return false;
            return true;
        }

        private void ButtonIzmeni_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidirajUnos())
            {
                MessageBox.Show("Molimo Vas da popunite polja za izmenu");
                return;
            }

            var p = new SlojPodataka.Klase.Pacijenti
            {
                IDPacijenta = Convert.ToInt32(TextboxIdPacijenta.Text),
                Ime = TextboxIme.Text,
                Vlasnik = TextboxVlasnik.Text,
                BrojCipa = TextboxBrojCipa.Text,
                BrojTelefona = TextboxBrojTelefona.Text,
                Pol = ComboboxPol.Text,
                Alergije = TextboxAlergije.Text,
                Sterilisan = _proveriUnosZaSterilizaciju.proveraUnosaZaSterilizaciju(TextboxSterilisan.Text),
                TezinaKg = int.Parse(TextboxTezinaKg.Text),
                IDVeterinara = int.Parse(ComboboxVeterinar.SelectedValue.ToString())
            };

            var poruka = _pacijentRepo.IzmeniPacijenta(p);
            if (!poruka.Uspeh)
            {
                MessageBox.Show(poruka.Poruka);
                return;
            }
            binDataGrid();
            ponistiUnosTxt();
        }

        private void ponistiUnosTxt()
        {
            TextboxIdPacijenta.Text = "";
            TextboxIme.Text = "";
            TextboxVlasnik.Text = "";
            TextboxBrojCipa.Text = "";
            TextboxBrojTelefona.Text = "";
            ComboboxPol.Text = "";
            TextboxAlergije.Text = "";
            TextboxSterilisan.Text = "";
            TextboxTezinaKg.Text = "";
            ComboboxVeterinar.Text = "";
        }

        private void ButtonObrisi_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextboxIdPacijenta.Text))
            {
                MessageBox.Show("Niste uneli ID");
                return;
            }
            int id = Convert.ToInt32(TextboxIdPacijenta.Text);
            _pacijentRepo.ObrisiPacijenta(id);
            binDataGrid();
            ponistiUnosTxt();
        }

        private void ComboboxVeterinar_Loaded(object sender, RoutedEventArgs e)
        {
            ComboboxVeterinar.ItemsSource = _veterinarRepo.VratiSveAktivneVeterinare();
            ComboboxVeterinar.DisplayMemberPath = "Ime";
            ComboboxVeterinar.SelectedValuePath = "IDVeterinara";
        }

        private void pretragaPacijenataSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (debounceTimer == null)
            {
                debounceTimer = new DispatcherTimer();
                debounceTimer.Interval = TimeSpan.FromMilliseconds(750);

                debounceTimer.Tick += (s, es) =>
                {
                    debounceTimer.Stop();

                    string tekst = pretragaPacijenataSearch.Text.Trim();
                    var view = CollectionViewSource.GetDefaultView(DataGrid.ItemsSource);

                    if (string.IsNullOrWhiteSpace(tekst))
                    {
                        view.Filter = null;
                        return;
                    }

                    if (Regex.IsMatch(tekst, @"^\d{15}$"))
                    {
                        view.Filter = item =>
                        {
                            var pacijent = item as SlojPodataka.Klase.Pacijenti;
                            return pacijent != null && pacijent.BrojCipa == tekst;
                        };
                    }
                    else if (Regex.IsMatch(tekst, @"^[\p{L}\s-]+$"))
                    {
                        string[] delovi = tekst.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        view.Filter = item =>
                        {
                            var pacijent = item as SlojPodataka.Klase.Pacijenti;
                            if (pacijent == null) return false;

                            string ime = pacijent.Ime?.ToLowerInvariant() ?? "";
                            string vlasnik = pacijent.Vlasnik?.ToLowerInvariant() ?? "";

                            return delovi.All(deo =>
                                ime.Contains(deo.ToLowerInvariant()) || vlasnik.Contains(deo.ToLowerInvariant()));
                        };
                    }
                    else
                    {
                        MessageBox.Show("Unos mora biti broj čipa (15 cifara) ili ime ljubimca/vlasnika.",
                                        "Neispravan unos", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                };
            }

            debounceTimer.Stop();
            debounceTimer.Start();
        }

        private void KartonButton_Click(object sender, RoutedEventArgs e)
        {
            var brojCipa = TextboxBrojCipa.Text?.Trim();

            if (string.IsNullOrWhiteSpace(brojCipa))
            {
                MessageBox.Show("Morate uneti broj čipa ljubimca.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var pacijent = _pacijentRepo.vratiPacijentaPoBrojuCipa(brojCipa);

            if (pacijent == null)
            {
                MessageBox.Show("Pacijent sa unetim brojem čipa ne postoji u bazi.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            new KartonLjubimca(brojCipa).Show();
        }
    }
}
