using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EMS_ClientUI_V2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private void App_Startup(object sender, StartupEventArgs e)
        {

            // start the main window
            MainWindow main = new MainWindow();
            main.Show();

            // start the login page
            LogInPage login = new LogInPage(main) { Owner = main };
            login.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            login.ShowDialog();

            // check if the login window was closed
            if (login.wasClosed)
            {
                main.Close();
            }

        }

    }
}
