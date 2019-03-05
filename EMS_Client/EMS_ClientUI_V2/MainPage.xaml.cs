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
        PatientOptionPage pot;
        public MainPage()
        {
            InitializeComponent();
            pot = new PatientOptionPage(this.MainFrame);
        }

        private void PatientsBtn_Click(object sender, RoutedEventArgs e)
        {
            OptionsFrame.Content = pot;

        }

        private void SchedulingBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BillingBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
