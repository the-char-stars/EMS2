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
using EMS_Library;
using MaterialDesignThemes.Wpf;

namespace EMS_ClientUI_V2
{
    /// <summary>
    /// Interaction logic for MainMenuPage.xaml
    /// </summary>
    public partial class MainMenuPage : Page
    {
        #region PRIVATE
        // FRAMES
        Frame ContentFrame { get; set; }
        Frame ExtraOptionMenuFrame { get; set; }

        // PAGES
        SchedulingView schedulingView { get; set; }
        BillingView billingView { get; set; }
        PatientView patientView;
        #endregion

        public MainMenuPage(Frame content, Frame extraMenu, DialogHost dialogHost, Demographics demographics, Scheduling scheduling, Billing billing, Snackbar snackBar)
        {
            Logging.Log("Initiating MainMenuPage");
            InitializeComponent();
            ContentFrame = content;
            ExtraOptionMenuFrame = extraMenu;

            patientView = new PatientView(demographics, dialogHost);
            schedulingView = new SchedulingView(scheduling, demographics, billing, dialogHost);
            billingView = new BillingView(scheduling, demographics, dialogHost, billing, snackBar);
        }

        private void BtnBilling_Checked(object sender, RoutedEventArgs e)
        {
            Logging.Log("Billing selected from MainMenuPage");
            ContentFrame.Content = billingView;
            ExtraOptionMenuFrame.Content = null;
        }

        private void BtnScheduling_Checked(object sender, RoutedEventArgs e)
        {
            Logging.Log("Scheduling selected from MainMenuPage");
            ContentFrame.Content = schedulingView;
            ExtraOptionMenuFrame.Content = null;
        }

        private void BtnPatients_Checked(object sender, RoutedEventArgs e)
        {
            Logging.Log("Patients selected from MainMenuPage");
            ContentFrame.Content = patientView;
            ExtraOptionMenuFrame.Content = null;
        }
    }
}
