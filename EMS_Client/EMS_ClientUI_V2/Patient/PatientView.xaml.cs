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


        public PatientView(Demographics d, DialogHost dh)
        {
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
            Frame f = new Frame() { Content = new AddPatient(demographics, refreshTable) };
            dialogHost.ShowDialog(f);           
        }

        private void refreshTable()
        {
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
            if (lvPatients.SelectedValue != null)
            {
                spSelectedPatient.DataContext = lvPatients.SelectedValue;
            }
        }
    }
}
