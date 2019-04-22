using EMS_Library;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        ObservableCollection<Patient> patientRoster = new ObservableCollection<Patient>();
        Patient selectedPatient;

        public PatientView(Demographics d, DialogHost dh)
        {
            Logging.Log("Initiated PatientView page");
            demographics = d;
            dialogHost = dh;
            InitializeComponent();
            tbNumPatients.Text = demographics.dPatientRoster.Count.ToString();
            lvPatients.ItemsSource = patientRoster;
            foreach(Patient p in demographics.dPatientRoster.Values)
            {
                patientRoster.Add(p);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Logging.Log("User selected AddPatient button");

            Frame f = new Frame() { Content = new AddPatient(demographics, refreshTable) };
            dialogHost.ShowDialog(f);           
        }

        private void refreshTable()
        {
            Logging.Log("Refreshing patientRoster Dictionary");

            patientRoster.Clear();
            foreach (Patient p in demographics.dPatientRoster.Values)
            {
                patientRoster.Add(p);
            }
            tbNumPatients.Text = demographics.dPatientRoster.Count.ToString();
            lvPatients.InvalidateVisual();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            Logging.Log("Searching for Patient");

            string firstSearch = tbFirstNameSeach.Text;
            patientRoster.Clear();
            foreach (Patient p in demographics.dPatientRoster.Values)
            {
                if (p.FirstName.Contains(tbFirstNameSeach.Text.ToUpper())
                    && p.LastName.Contains(tbLastNameSearch.Text.ToUpper())
                    && (cbGenderSearch.SelectedValue == null || cbGenderSearch.SelectedValue.ToString() == "ALL" || p.Sex.Contains(cbGenderSearch.SelectedValue.ToString().ToUpper()))
                    && p.HCN.Contains(tbHealthCard.Text.ToUpper()))
                {
                    patientRoster.Add(p);
                }
            }
            lvPatients.InvalidateVisual();
            btnSeachBadge.Badge = patientRoster.Count;
        }

        private void tbSearchTermChanged(object sender, RoutedEventArgs e)
        {
            int searchCount = 0;
            foreach (Patient p in demographics.dPatientRoster.Values)
            {
                if (p.FirstName.Contains(tbFirstNameSeach.Text.ToUpper()) 
                    && p.LastName.Contains(tbLastNameSearch.Text.ToUpper())
                    && (cbGenderSearch.SelectedValue == null || cbGenderSearch.SelectedValue.ToString() == "ALL" || p.Sex.Contains(cbGenderSearch.SelectedValue.ToString().ToUpper()))
                    && p.HCN.Contains(tbHealthCard.Text.ToUpper()))
                {
                    searchCount++;
                }
            }
            btnSeachBadge.Badge = searchCount.ToString();
        }

        private void LvPatients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Logging.Log("Patient selected from the ListView in the PatientView");

            if (lvPatients.SelectedValue != null)
            {
                spSelectedPatient.DataContext = lvPatients.SelectedValue;
                selectedPatient = (Patient)lvPatients.SelectedValue;
                
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Logging.Log("Edit Patient button was clicked in PatientView page");

            if (spSelectedPatient.DataContext != null)
            {
                Frame f = new Frame() { Content = new EditPatient(demographics,selectedPatient, refreshTable) };
                dialogHost.ShowDialog(f);
            }
        }
    }
}
