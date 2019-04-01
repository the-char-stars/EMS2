using EMS_Library;
using MaterialDesignThemes.Wpf;
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
    /// Interaction logic for PatientView.xaml
    /// </summary>
    public partial class PatientView : Page
    {
        Demographics demographics;
        DialogHost dialogHost;
        int NumPatients = 10;
        public PatientView(Demographics d, DialogHost dh)
        {
            demographics = d;
            dialogHost = dh;
            InitializeComponent();
            tbNumPatients.DataContext = NumPatients;
            lvPatients.ItemsSource = demographics.dPatientRoster.Values;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame f = new Frame() { Content = new AddPatient(demographics) };
            dialogHost.ShowDialog(f);
        }
    }
}
