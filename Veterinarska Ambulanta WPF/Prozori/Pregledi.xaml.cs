using Klase;
using SlojServisa;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Veterinarska_Ambulanta_WPF.Prozori
{
    public partial class Pregledi : Window
    {
        private readonly PregledPravila _pregledRepo;

        public Pregledi()
        {
            InitializeComponent();
            _pregledRepo = new PregledPravila();
            binDataGrid();
        }

        private void ButtonNazad_Click(object sender, RoutedEventArgs e)
        {
            new Meni().Show();
            Close();
        }

        private void DataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            var pregled = DataGrid.SelectedItem as Pregled;
            if (pregled == null) return;

            ComboboxLek.SelectedValue = pregled.IDLeka;
            TextboxIdPregleda.Text = pregled.IDPregleda.ToString();
            IzvestajTextBox.Text = pregled.Izvestaj;
            TextboxIdTermina.Text = pregled.IDTermina.ToString();

            if (!_pregledRepo.ProveriAktivnostLeka(pregled.IDLeka))
            {
                MessageBox.Show("Selektovan lek je trenutno van upotrebe (neaktivan)", "Informacija", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void binDataGrid()
        {
            DataGrid.ItemsSource = _pregledRepo.VratiSvePreglede();
        }

        private void ButtonDodaj_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidirajUnosPregleda())
            {
                MessageBox.Show("Niste uneli sve podatke ili su podaci neispravnog formata");
                return;
            }

            var pregled = new Pregled
            {
                Izvestaj = IzvestajTextBox.Text,
                IDTermina = int.Parse(TextboxIdTermina.Text),
                IDLeka = Convert.ToInt32(ComboboxLek.SelectedValue)
            };

            var poruka = _pregledRepo.DodajPregled(pregled);
            if (!poruka.Uspeh)
            {
                MessageBox.Show(poruka.Poruka);
                return;
            }
            binDataGrid();
            ponistiUnosTxt();
        }

        private bool ValidirajUnosPregleda()
        {
            if (string.IsNullOrWhiteSpace(IzvestajTextBox.Text)) return false;
            if (string.IsNullOrWhiteSpace(TextboxIdTermina.Text)) return false;
            if (!int.TryParse(TextboxIdTermina.Text, out _)) return false;
            if (ComboboxLek.SelectedValue == null) return false;
            return true;
        }

        private void ButtonIzmeni_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidirajUnosPregleda())
            {
                MessageBox.Show("Niste uneli sve podatke.");
                return;
            }

            var pregled = new Pregled
            {
                IDPregleda = int.Parse(TextboxIdPregleda.Text),
                Izvestaj = IzvestajTextBox.Text,
                IDTermina = int.Parse(TextboxIdTermina.Text),
                IDLeka = Convert.ToInt32(ComboboxLek.SelectedValue)
            };

            var poruka = _pregledRepo.IzmeniPregled(pregled);
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
            TextboxIdPregleda.Text = "";
            IzvestajTextBox.Text = "";
            TextboxIdTermina.Text = "";
            ComboboxLek.Text = "";
        }

        private void ButtonObrisi_Click(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(TextboxIdPregleda.Text);
            _pregledRepo.ObrisiPregled(id);
            binDataGrid();
            ponistiUnosTxt();
        }

        private void ComboboxLek_Loaded(object sender, RoutedEventArgs e)
        {
            ComboboxLek.ItemsSource = new LekPravila().VratiSveAktivneLekove();
            ComboboxLek.DisplayMemberPath = "Naziv";
            ComboboxLek.SelectedValuePath = "IDLeka";
        }
    }
}
