using System;
using System.Windows;
using SlojServisa;

namespace Veterinarska_Ambulanta_WPF
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                BazaServis.ProveriKonekciju();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Aplikacija nije uspela da se poveže na bazu.\n\n" +
                    "Potrebno je jedno od sledećeg:\n" +
                    "- SQL Server Express (localhost\\SQLEXPRESS)\n" +
                    "- LocalDB (instalira se uz Visual Studio)\n\n" +
                    "Detalji: " + ex.Message,
                    "VetKeep - greška konekcije",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                Shutdown();
                return;
            }

            base.OnStartup(e);
            var login = new MainWindow();
            login.Show();
        }
    }
}
