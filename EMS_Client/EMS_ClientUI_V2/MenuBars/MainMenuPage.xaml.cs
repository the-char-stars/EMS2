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

        public MainMenuPage(Frame content, Frame extraMenu, DialogHost dialogHost, Demographics d, Scheduling scheduling, Billing b, Snackbar sb)
        {
            InitializeComponent();
            ContentFrame = content;
            ExtraOptionMenuFrame = extraMenu;

            patientView = new PatientView(d, dialogHost);
            schedulingView = new SchedulingView(scheduling, d);
            billingView = new BillingView(scheduling, d, dialogHost, b, sb);
        }

        private void BtnBilling_Checked(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = billingView;
            ExtraOptionMenuFrame.Content = null;
        }

        private void BtnScheduling_Checked(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = schedulingView;
            ExtraOptionMenuFrame.Content = null;
        }

        private void BtnPatients_Checked(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = patientView;
            ExtraOptionMenuFrame.Content = null;
        }
    }
}
