using System.Windows;

namespace Veterinarska_Ambulanta_WPF.Prozori
{
    public partial class Meni : Window
    {
        public Meni()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void ButtonIzlogujteSe_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }

        private void ButtonPacijenti_Click(object sender, RoutedEventArgs e)
        {
            new Pacijenti().Show();
            Close();
        }

        private void ButtonTermini_Click(object sender, RoutedEventArgs e)
        {
            new Termini().Show();
            Close();
        }

        private void ButtonPregledi_Click(object sender, RoutedEventArgs e)
        {
            new Pregledi().Show();
            Close();
        }

        private void ButtonLekovi_Click(object sender, RoutedEventArgs e)
        {
            new Lekovi().Show();
            Close();
        }

        private void ButtonVeterinari_Click(object sender, RoutedEventArgs e)
        {
            new Veterinari().Show();
            Close();
        }
    }
}
