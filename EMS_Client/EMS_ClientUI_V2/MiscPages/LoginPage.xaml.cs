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
using System.Windows.Shapes;

namespace EMS_ClientUI_V2
{
    /// <summary>
    /// Interaction logic for LogInPage.xaml
    /// </summary>
    public partial class LogInPage : Window
    {
        #region PUBLIC
        public bool wasClosed { get; set; }
        #endregion

        #region PRIVATE
        private bool isValidPass { get; set; }
        private MainWindow mainWindow { get; set; }
        #endregion

        public LogInPage(MainWindow main)
        {
            InitializeComponent();
            wasClosed = false;
            isValidPass = false;
            mainWindow = main;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            wasClosed = (isValidPass) ? false : true;
        }

        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            // check if user is valid -----> PLACEHOLDER VALUES. MUST BE CHANGED
            if (userName.Text == "admin" && userPassword.Password.ToString() == "123")
            {
                isValidPass = true;
                mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                if (mainWindow != null)
                {
                    // display the main page on the main window
                    mainWindow.ContentFrame.Content = new MainPage();

                    // close the log in window
                    this.Close();
                }

                loginErrorMessage.Text = "Error encountered while logging you in. Please try again!";
            }
            else
            {
                loginErrorMessage.Text = "Username/Password invalid. Try again!";
            }
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UserPassword_KeyUp(object sender, KeyEventArgs e)
        {
            // attempt to sign if the user presses enter
            if(e.Key == System.Windows.Input.Key.Enter)
            {
                SignIn_Click(sender, e);
            }
        }
    }
}
