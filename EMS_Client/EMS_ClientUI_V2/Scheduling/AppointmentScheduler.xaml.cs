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
    /// Interaction logic for AppointmentScheduler.xaml
    /// </summary>
    public partial class AppointmentScheduler : UserControl
    {
        Appointment appointment;
        Scheduling scheduling;
        Demographics demographics;
        Patient primary, dependant;
        UpdateDisplay updateDisplay;

        DateTime selectedDate;
        int timeSlot;

        public AppointmentScheduler(Scheduling s, Demographics d, Appointment a, DateTime da, int ts, UpdateDisplay upd)
        {
            InitializeComponent();
            updateDisplay = upd;
            timeSlot = ts;
            selectedDate = da;
            scheduling = s;
            demographics = d;
            appointment = a;
            foreach(Patient p in demographics.dPatientRoster.Values)
            {
                cbPrimaryPatient.Items.Add(p);
            }
            cbSecondaryPatient.IsEnabled = false;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        private void CbPrimaryPatient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            primary = (Patient)cbPrimaryPatient.SelectedItem;
            if (cbPrimaryPatient.SelectedItem != null)
            {
                cbSecondaryPatient.Items.Clear();
                foreach (Patient p in demographics.GetDependants(primary))
                {
                    cbSecondaryPatient.Items.Add(p);
                }
            }
        }

        private void CbSecondaryPatient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dependant = (Patient)cbSecondaryPatient.SelectedItem;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (cbPrimaryPatient.SelectedItem != null)
            {
                appointment.PatientID = primary.PatientID;
                if (cbSecondaryPatient.SelectedItem != null)
                {
                    appointment.DependantID = dependant.PatientID;
                }
                scheduling.ScheduleAppointment(appointment, selectedDate, timeSlot - 1);
                updateDisplay(selectedDate);
                DialogHost.CloseDialogCommand.Execute(null, null);
            }
        }
    }
}
