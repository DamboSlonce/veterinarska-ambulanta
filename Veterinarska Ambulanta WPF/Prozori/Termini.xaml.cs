using Klase;
using SlojServisa;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Veterinarska_Ambulanta_WPF.Prozori
{
    public partial class Termini : Window
    {
        private readonly TerminPravila _terminRepo;
        private readonly VeterinarPravila _veterinarRepo;
        private readonly PacijentPravila _pacijentRepo;

        public Termini()
        {
            InitializeComponent();
            _terminRepo = new TerminPravila();
            _veterinarRepo = new VeterinarPravila();
            _pacijentRepo = new PacijentPravila();
            binDataGrid();
        }

        private void ButtonNazad_Click(object sender, RoutedEventArgs e)
        {
            new Meni().Show();
            Close();
        }

        private void DataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (DataGrid.SelectedItem is Termin termin)
            {
                TextboxIdTermina.Text = termin.IDTermina.ToString();
                DatepickerDatum.Text = termin.Datum.ToString("yyyy-MM-dd");
                TextboxVreme.Text = termin.Vreme.ToString(@"hh\:mm");
                TextboxVrstaUsluge.Text = termin.VrstaUsluge;
                ComboboxPacijent.SelectedValue = termin.IDPacijenta;
                ComboboxVeterinar.SelectedValue = termin.IDVeterinara;

                if (!_terminRepo.ProveriAktivnostVeterinara(termin.IDVeterinara))
                {
                    MessageBox.Show("Trenutno je veterinar neaktivan", "Informacija", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void binDataGrid()
        {
            DataGrid.ItemsSource = _terminRepo.VratiSveTermine();
        }

        private void ponistiUnosTxt()
        {
            TextboxIdTermina.Text = "";
            DatepickerDatum.Text = "";
            TextboxVreme.Text = "";
            TextboxVrstaUsluge.Text = "";
            ComboboxPacijent.Text = "";
            ComboboxVeterinar.Text = "";
        }

        private void ButtonDodaj_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidirajUnosTermina())
            {
                MessageBox.Show("Niste uneli sve podatke za termin!");
                return;
            }

            var noviTermin = new Termin
            {
                Datum = DateTime.Parse(DatepickerDatum.Text).Date,
                Vreme = TimeSpan.Parse(TextboxVreme.Text),
                VrstaUsluge = TextboxVrstaUsluge.Text,
                IDPacijenta = Convert.ToInt32(ComboboxPacijent.SelectedValue.ToString()),
                IDVeterinara = Convert.ToInt32(ComboboxVeterinar.SelectedValue.ToString())
            };

            var poruka = _terminRepo.DodajTermin(noviTermin);
            if (!poruka.Uspeh)
            {
                MessageBox.Show(poruka.Poruka);
                return;
            }
            binDataGrid();
            ponistiUnosTxt();
        }

        private bool ValidirajUnosTermina()
        {
            if (string.IsNullOrWhiteSpace(DatepickerDatum.Text)) return false;
            if (!DateTime.TryParse(DatepickerDatum.Text, out _)) return false;
            if (string.IsNullOrWhiteSpace(TextboxVreme.Text)) return false;
            if (!TimeSpan.TryParse(TextboxVreme.Text, out _)) return false;
            if (string.IsNullOrWhiteSpace(TextboxVrstaUsluge.Text)) return false;
            if (ComboboxPacijent.SelectedValue == null) return false;
            if (ComboboxVeterinar.SelectedValue == null) return false;
            return true;
        }

        private void ButtonObrisi_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(TextboxIdTermina.Text, out int id))
            {
                _terminRepo.ObrisiTermin(id);
                MessageBox.Show("Podaci su uspešno obrisani!");
                binDataGrid();
                ponistiUnosTxt();
            }
        }

        private void ButtonIzmeni_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidirajUnosTermina())
            {
                MessageBox.Show("Niste uneli sve podatke za termin!");
                return;
            }

            var termin = new Termin
            {
                IDTermina = int.Parse(TextboxIdTermina.Text),
                Datum = DateTime.Parse(DatepickerDatum.Text),
                Vreme = TimeSpan.Parse(TextboxVreme.Text),
                VrstaUsluge = TextboxVrstaUsluge.Text,
                IDPacijenta = Convert.ToInt32(ComboboxPacijent.SelectedValue),
                IDVeterinara = Convert.ToInt32(ComboboxVeterinar.SelectedValue)
            };

            var poruka = _terminRepo.IzmeniTermin(termin);
            if (!poruka.Uspeh)
            {
                MessageBox.Show(poruka.Poruka);
                return;
            }
            MessageBox.Show("Podaci su uspešno izmenjeni!");
            binDataGrid();
            ponistiUnosTxt();
        }

        private void ComboboxVeterinar_Loaded(object sender, RoutedEventArgs e)
        {
            ComboboxVeterinar.ItemsSource = _veterinarRepo.VratiSveAktivneVeterinare();
            ComboboxVeterinar.DisplayMemberPath = "Ime";
            ComboboxVeterinar.SelectedValuePath = "IDVeterinara";
        }

        private void ComboboxPacijent_Loaded(object sender, RoutedEventArgs e)
        {
            ComboboxPacijent.ItemsSource = _pacijentRepo.VratiSvePacijente();
            ComboboxPacijent.DisplayMemberPath = "Ime";
            ComboboxPacijent.SelectedValuePath = "IDPacijenta";
        }

        private void FiltrirajTermineComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var izabrano = FiltrirajTermineComboBox.SelectedItem as ComboBoxItem;
            if (izabrano == null) return;

            string vremenskiPeriod = izabrano.Content.ToString();
            var view = CollectionViewSource.GetDefaultView(DataGrid.ItemsSource);

            DateTime danas = DateTime.Today;
            DateTime krajnjiDatum;

            switch (vremenskiPeriod)
            {
                case "Danas":
                    krajnjiDatum = danas;
                    break;
                case "7 dana":
                    krajnjiDatum = danas.AddDays(7);
                    break;
                case "30 dana":
                    krajnjiDatum = danas.AddDays(30);
                    break;
                default:
                    view.Filter = null;
                    return;
            }

            view.Filter = item =>
            {
                var termin = item as Termin;
                return termin != null &&
                       termin.Datum.Date >= danas &&
                       termin.Datum.Date <= krajnjiDatum;
            };
        }
    }
}
