using SlojPodataka.Klase;
using SlojServisa;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Veterinarska_Ambulanta_WPF.Prozori
{
    public partial class Veterinari : Window
    {
        private readonly VeterinarPravila _veterinarRepo;

        public Veterinari()
        {
            InitializeComponent();
            _veterinarRepo = new VeterinarPravila();
            binDataGrid();
        }

        private void ButtonNazad_Click(object sender, RoutedEventArgs e)
        {
            new Meni().Show();
            Close();
        }

        private void DataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (DataGrid.SelectedItem is Veterinar veterinar)
            {
                TextboxIdVeterinara.Text = veterinar.IDVeterinara.ToString();
                TextboxIme.Text = veterinar.Ime;
                TextboxPrezime.Text = veterinar.Prezime;
                TextboxJMBG.Text = veterinar.JMBG;
                TextboxBrojTelefona.Text = veterinar.BrojTelefona;
                TextboxEmail.Text = veterinar.Email;
            }
        }

        private void binDataGrid()
        {
            DataGrid.ItemsSource = _veterinarRepo.VratiSveAktivneVeterinare();
        }

        private void ButtonDodaj_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidirajUnosVeterinara())
            {
                MessageBox.Show("Niste uneli sve podatke");
                return;
            }

            var novi = new Veterinar
            {
                Ime = TextboxIme.Text,
                Prezime = TextboxPrezime.Text,
                JMBG = TextboxJMBG.Text,
                BrojTelefona = TextboxBrojTelefona.Text,
                Email = TextboxEmail.Text
            };

            var poruka = _veterinarRepo.DodajVeterinara(novi);
            if (!poruka.Uspeh)
            {
                MessageBox.Show(poruka.Poruka);
                return;
            }
            binDataGrid();
            ponistiUnosTxt();
        }

        private bool ValidirajUnosVeterinara()
        {
            if (string.IsNullOrWhiteSpace(TextboxIme.Text)) return false;
            if (string.IsNullOrWhiteSpace(TextboxPrezime.Text)) return false;
            if (string.IsNullOrWhiteSpace(TextboxJMBG.Text)) return false;
            if (string.IsNullOrWhiteSpace(TextboxBrojTelefona.Text)) return false;
            if (string.IsNullOrWhiteSpace(TextboxEmail.Text)) return false;
            return true;
        }

        private void ButtonIzmeni_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidirajUnosVeterinara())
            {
                MessageBox.Show("Niste uneli sve podatke");
                return;
            }

            var izmena = new Veterinar
            {
                IDVeterinara = Convert.ToInt32(TextboxIdVeterinara.Text),
                Ime = TextboxIme.Text,
                Prezime = TextboxPrezime.Text,
                JMBG = TextboxJMBG.Text,
                BrojTelefona = TextboxBrojTelefona.Text,
                Email = TextboxEmail.Text
            };

            var poruka = _veterinarRepo.IzmeniVeterinara(izmena);
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
            TextboxIdVeterinara.Text = "";
            TextboxIme.Text = "";
            TextboxPrezime.Text = "";
            TextboxJMBG.Text = "";
            TextboxBrojTelefona.Text = "";
            TextboxEmail.Text = "";
        }

        private void ButtonObrisi_Click(object sender, RoutedEventArgs e)
        {
            var obrisi = new Veterinar
            {
                IDVeterinara = Convert.ToInt32(TextboxIdVeterinara.Text),
                Ime = TextboxIme.Text,
                Prezime = TextboxPrezime.Text,
                JMBG = TextboxJMBG.Text,
                BrojTelefona = TextboxBrojTelefona.Text,
                Email = TextboxEmail.Text
            };
            var poruka = _veterinarRepo.ObrisiVeterinara(obrisi);
            if (!poruka.Uspeh)
            {
                MessageBox.Show(poruka.Poruka);
                return;
            }
            binDataGrid();
            ponistiUnosTxt();
        }
    }
}
