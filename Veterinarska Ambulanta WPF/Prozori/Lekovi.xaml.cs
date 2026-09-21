using SlojPodataka.Klase;
using SlojServisa;
using System.Windows;
using System.Windows.Controls;

namespace Veterinarska_Ambulanta_WPF.Prozori
{
    public partial class Lekovi : Window
    {
        private readonly LekPravila _lekRepo;

        public Lekovi()
        {
            InitializeComponent();
            _lekRepo = new LekPravila();
            binDataGrid();
        }

        private void ButtonNazad_Click(object sender, RoutedEventArgs e)
        {
            new Meni().Show();
            Close();
        }

        private void DataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (DataGrid.SelectedItem is Lek lek)
            {
                TextboxIdLeka.Text = lek.IDLeka.ToString();
                TextboxNaziv.Text = lek.Naziv;
                TextboxProizvodjac.Text = lek.Proizvodjac;
                TextboxJacina.Text = lek.Jacina;
                TextboxDoziranje.Text = lek.Doziranje;
            }
        }

        private void binDataGrid()
        {
            DataGrid.ItemsSource = _lekRepo.VratiSveAktivneLekove();
            DataGrid.Items.Refresh();
        }

        private void ButtonDodaj_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidirajUnosLeka())
            {
                MessageBox.Show("Niste uneli sve podatke o leku");
                return;
            }

            var lek = new Lek
            {
                Naziv = TextboxNaziv.Text,
                Proizvodjac = TextboxProizvodjac.Text,
                Jacina = TextboxJacina.Text,
                Doziranje = TextboxDoziranje.Text
            };

            var poruka = _lekRepo.DodajLek(lek);
            if (!poruka.Uspeh)
            {
                MessageBox.Show(poruka.Poruka);
                return;
            }
            binDataGrid();
            ponistiUnosTxt();
        }

        private bool ValidirajUnosLeka()
        {
            if (string.IsNullOrWhiteSpace(TextboxNaziv.Text)) return false;
            if (string.IsNullOrWhiteSpace(TextboxProizvodjac.Text)) return false;
            if (string.IsNullOrWhiteSpace(TextboxJacina.Text)) return false;
            if (string.IsNullOrWhiteSpace(TextboxDoziranje.Text)) return false;
            return true;
        }

        private void ButtonIzmeni_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidirajUnosLeka())
            {
                MessageBox.Show("Niste uneli sve podatke o leku");
                return;
            }

            var lek = new Lek
            {
                IDLeka = int.Parse(TextboxIdLeka.Text),
                Naziv = TextboxNaziv.Text,
                Proizvodjac = TextboxProizvodjac.Text,
                Jacina = TextboxJacina.Text,
                Doziranje = TextboxDoziranje.Text
            };

            var poruka = _lekRepo.IzmeniLek(lek);
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
            TextboxIdLeka.Text = "";
            TextboxNaziv.Text = "";
            TextboxProizvodjac.Text = "";
            TextboxJacina.Text = "";
            TextboxDoziranje.Text = "";
        }

        private void ButtonObrisi_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextboxIdLeka.Text))
            {
                MessageBox.Show("Niste uneli ID");
                return;
            }

            int id = int.Parse(TextboxIdLeka.Text);
            _lekRepo.ObrisiLek(id);
            MessageBox.Show("Lek je deaktiviran");
            binDataGrid();
            ponistiUnosTxt();
        }
    }
}
