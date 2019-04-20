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
        SchedulingMenuPage SchedulingMenu { get; set; }
        BillingView BillingMenu { get; set; }
        PatientView pv;
        #endregion

        public MainMenuPage(Frame content, Frame extraMenu, DialogHost dialogHost, Demographics d, Scheduling scheduling, Billing b, Snackbar sb)
        {
            InitializeComponent();
            ContentFrame = content;
            ExtraOptionMenuFrame = extraMenu;

            pv = new PatientView(d, dialogHost);
            SchedulingMenu = new SchedulingMenuPage(ContentFrame, dialogHost);
            BillingMenu = new BillingView(scheduling, d, dialogHost, b, sb);
        }

        private void BtnBilling_Checked(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = BillingMenu;
            ExtraOptionMenuFrame.Content = null;
        }

        private void BtnScheduling_Checked(object sender, RoutedEventArgs e)
        {
            ExtraOptionMenuFrame.Content = SchedulingMenu;
        }

        private void BtnPatients_Checked(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = pv;
            ExtraOptionMenuFrame.Content = null;
        }
    }
}
