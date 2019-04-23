using EMS_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EMS_ClientUI_V2
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        Demographics demographics = new Demographics();
        Scheduling scheduling = new Scheduling();
        Billing billing = new Billing();
        MainWindow mainWindow;

        public MainPage(string Username, FileIO.AccessLevel accessLevel, MainWindow mw)
        {
            mainWindow = mw;
            Logging.Log("Initialize MainPage");
            InitializeComponent();
            if (accessLevel > FileIO.AccessLevel.Root)
            {
                lbiBackup.Visibility = Visibility.Hidden;
                lbiBackup.Height = 0;
            }

            lbiLogOut.Selected += LbiLogOut_Selected;
            lbiBackup.Selected += LbiBackup_Selected;
            chipUser.Content = Username;
            // display the main menu with the three main buttons
            MainMenuFrame.Content = new MainMenuPage(
                this.ContentFrame, this.ExtraOptionMenuFrame, this.mainDialogueHost, demographics, scheduling, billing, ErrorMessage);
            this.Background = new ImageBrush(new BitmapImage(new Uri("../../Images/Background3.jpg", UriKind.Relative)));
        }

        private void LbiBackup_Selected(object sender, RoutedEventArgs e)
        {
            FileIO.BackupDatabase(FileIO.currentDataSet);
        }

        private void LbiLogOut_Selected(object sender, RoutedEventArgs e)
        {
            LogInPage login = new LogInPage(mainWindow) { Owner = mainWindow };
            login.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            login.ShowDialog();

            if (login.wasClosed)
            {
                mainWindow.Close();
            }
        }
    }
}
