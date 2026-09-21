using System.Windows;
using Veterinarska_Ambulanta_WPF.Prozori;

namespace Veterinarska_Ambulanta_WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string username = TextboxKorisnickoIme.Text;
            string password = PasswordfieldLozinka.Password;

            if (username == "Veterinar" && password == "1234")
            {
                Meni menipage = new Meni();
                menipage.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Pogrešno korisničko ime ili lozinka!");
            }
        }
    }
}
