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
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using EMS_Library;

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
        private int loginAttempts { get; set; }
        private MainWindow mainWindow { get; set; }
        private readonly TimeSpan loginAttemptInterval = TimeSpan.FromSeconds(60);
        private readonly Stopwatch loginStopwatch = new Stopwatch();
        private readonly int maxLoginAttempts = 3;
        #endregion

        public LogInPage(MainWindow main)
        {
            Logging.Log("Login page initiated.");
            InitializeComponent();
            wasClosed = false;
            isValidPass = false;
            mainWindow = main;
            loginAttempts = 0;
            loginStopwatch.Start();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            wasClosed = (isValidPass) ? false : true;
        }

        private bool CanSignIn()
        {
            if(this.loginStopwatch.IsRunning && this.loginStopwatch.Elapsed >= this.loginAttemptInterval)
            {
                return true;
            }

            return false;
        }

        private void SignIn_Click(object sender, RoutedEventArgs e)
        {

            Logging.Log("User attempting to log in");

            // check if the user has tried to log in more than 3 times and is on cooldown
            if (loginAttempts >= maxLoginAttempts && !CanSignIn())
            {
                loginErrorMessage.Text = "Too many attempts in the past minute!";
                return;
            }
            // check if the cooldown has expired and the user should be able to try to log in again
            else if(loginAttempts >= maxLoginAttempts && CanSignIn())
            {
                loginErrorMessage.Text = "";
                loginAttempts = 0;
            }

            if (FileIO.CheckUser(userName.Text, userPassword.Password.ToString()))
            {

                isValidPass = true;
                mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                if (mainWindow != null)
                {
                    Logging.Log("User logged in");
                    // display the main page on the main window
                    mainWindow.ContentFrame.Content = new MainPage();

                    // close the log in window
                    this.Close();
                }

                loginErrorMessage.Text = "Error encountered while logging you in. Please try again!";
                Logging.Log("Error encountered when logging in");
            }
            else
            {
                loginErrorMessage.Text = "Username/Password invalid. Try again!";
                Logging.Log("Username/Password invalid");
                loginAttempts++;
                loginStopwatch.Restart();
            }
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            Logging.Log("Closing app from LoginPage");
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
