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
    /// Interaction logic for MainMenuPage.xaml
    /// </summary>
    public partial class MainMenuPage : Page
    {
        #region PRIVATE
        // FRAMES
        Frame ContentFrame { get; set; }
        Frame ExtraOptionMenuFrame { get; set; }

        // PAGES
        PatientMenuPage PatientsMenu { get; set; }
        SchedulingMenuPage SchedulingMenu { get; set; }
        BillingMenuPage BillingMenu { get; set; }
        #endregion

        public MainMenuPage(Frame content, Frame extraMenu)
        {
            InitializeComponent();
            ContentFrame = content;
            ExtraOptionMenuFrame = extraMenu;

            PatientsMenu = new PatientMenuPage(ContentFrame);
            SchedulingMenu = new SchedulingMenuPage(ContentFrame);
            BillingMenu = new BillingMenuPage(ContentFrame);
        }

        private void PatientsBtn_Click(object sender, RoutedEventArgs e)
        {
            // show the submenu of the patients main menu
            ExtraOptionMenuFrame.Content = PatientsMenu;
        }

        private void SchedulingBtn_Click(object sender, RoutedEventArgs e)
        {
            // show the submenu of scheduling main menu
            ExtraOptionMenuFrame.Content = SchedulingMenu;
        }

        private void BillingBtn_Click(object sender, RoutedEventArgs e)
        {
            // show the submenu of the billing main menu
            ExtraOptionMenuFrame.Content = BillingMenu;
        }
    }
}
