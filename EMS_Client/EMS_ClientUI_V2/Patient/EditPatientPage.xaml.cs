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
    /// Interaction logic for EditPatientPage.xaml
    /// </summary>
    public partial class EditPatient : Page
    {

        Demographics demographics;
        Patient patientToUpdate;
        public delegate void RefreshScreen();
        RefreshScreen refreshScreen;

        public EditPatient(Demographics d, Patient p, RefreshScreen r)
        {
            Logging.Log("Edit patient is initiated and pop up is displayed");
            demographics = d;
            patientToUpdate = p;
            refreshScreen = r;
            InitializeComponent();

            this.DataContext = patientToUpdate;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            refreshScreen();
            Logging.Log("Edit patient pop up closed");
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Logging.Log("Patient data is updated using Edit Patient pop up window from PatientView page");
            patientToUpdate = (Patient)this.DataContext;
            demographics.UpdatePatient(patientToUpdate);
            BtnClose_Click(sender, e);
        }
    }
}
