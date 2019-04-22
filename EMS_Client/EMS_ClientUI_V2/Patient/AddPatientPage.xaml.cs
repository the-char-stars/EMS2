using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using EMS_Library;
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
    /// Interaction logic for AddNewPatient.xaml
    /// </summary>
    public partial class AddPatient : Page
    {
        Demographics demographics;
        Patient addingPatient = new Patient();
        public delegate void RefreshScreen();
        RefreshScreen refreshScreen;

        public AddPatient(Demographics d, RefreshScreen r)
        {
            Logging.Log("Add a patient pop up window is initiated");
            refreshScreen = r;
            demographics = d;
            this.DataContext = addingPatient;
            InitializeComponent();
            dpDateOfBirth.BlackoutDates.Add(new CalendarDateRange(DateTime.Now, DateTime.MaxValue));
            //btnAddNewPatient.Command = DialogHost.CloseDialogCommand;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Logging.Log("Closed AddPatient pop up");
            refreshScreen();
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        private void BtnAddNewPatient_Click(object sender, RoutedEventArgs e)
        {
            Logging.Log("Add Patient button is pressed");
            if (addingPatient.IsReadyToSave())
            {
                demographics.AddNewPatient(addingPatient);
                Logging.Log("Patient is added to database");
                refreshScreen();
                DialogHost.CloseDialogCommand.Execute(null, null);
            }
            else
            {
                Logging.Log("Could not add Patient from AddPatient pop up page");
                tbFirstName.Text = tbFirstName.Text;
                tbLastName.Text = tbLastName.Text;
                tbHealthCard.Text = tbHealthCard.Text;
                tbPostalCode.Text = tbPostalCode.Text;
                cbGender.SelectedItem = cbGender.SelectedItem;
            }
        }

        private void TbHeadOfHouse_TextChanged(object sender, TextChangedEventArgs e)
        {
            Patient p = demographics.GetPatientByHCN(tbHeadOfHouse.Text);
            if (p != null)
            {
                tbAddress1.Text = p.AddressLine1;
                tbAddress2.Text = p.AddressLine2;
                tbCity.Text = p.City;
                tbPhoneNumber.Text = p.PhoneNumber;
                tbPostalCode.Text = p.PostalCode;
                cbProvince.SelectedValue = p.Province;
            }
            this.DataContext = addingPatient;
        }
    }
}
