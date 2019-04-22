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
    /// Interaction logic for AppointmentCard.xaml
    /// </summary>
    public delegate void UpdateDisplay(DateTime dt);
    public partial class AppointmentCard : UserControl
    {
        public Appointment appointment;
        Scheduling scheduling;
        Demographics demographics;
        public Patient primary = null, dependant = null;
        DialogHost dialogHost;
        int timeSlot;
        DateTime selectedDate;

        UpdateDisplay updateDisplay;
        public AppointmentCard(Appointment a, Demographics d, Scheduling s, DialogHost dh, int slot, DateTime dt, UpdateDisplay upd)
        {
            InitializeComponent();
            updateDisplay = upd;
            dialogHost = dh;
            appointment = a;
            scheduling = s;
            demographics = d;
            timeSlot = slot;
            selectedDate = dt;

            if (appointment.AppointmentID == -1)
            {
                tbTitle.Text = "Unscheduled Appointment";
                chipPrimaryPatient.Visibility = Visibility.Hidden;
                chipSecondaryPatient.Visibility = Visibility.Hidden;

                btnCancelAppointment.Content = "Schedule Appointment";
                btnCancelAppointment.Click += ScheduleAppointment;

                btnEditAppointment.Visibility = Visibility.Hidden;

                if (DateTime.Compare(selectedDate.Date, DateTime.Now.Date) < 0)
                {
                    cardBackground.Background = Brushes.LightGray;
                    btnCancelAppointment.Visibility = Visibility.Hidden;
                    btnEditAppointment.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                czState.Mode = ColorZoneMode.PrimaryLight;
                primary = d.GetPatientByID(a.PatientID);
                dependant = d.GetPatientByID(a.DependantID);

                tbTitle.Text = string.Format("Slot {0}", slot);

                chipPrimaryPatient.Content = string.Format("{0} {1}", primary.FirstName, primary.LastName);
                chipPrimaryPatient.Icon = primary.FirstName[0];

                if (dependant != null)
                {
                    chipSecondaryPatient.Content = string.Format("{0} {1}", dependant.FirstName, dependant.LastName);
                    chipSecondaryPatient.Icon = dependant.FirstName[0];
                }
                else
                {
                    chipSecondaryPatient.Visibility = Visibility.Hidden;
                }

                if (DateTime.Compare(selectedDate.Date, DateTime.Now.Date) < 0)
                {
                    cardBackground.Background = Brushes.LightGray;
                    czState.Mode = ColorZoneMode.PrimaryDark;
                    btnEditAppointment.Visibility = Visibility.Hidden;
                    btnCancelAppointment.Content = "Add Codes / Notes";
                    btnCancelAppointment.Click += AddCodesAndNotes;
                }
                else
                {
                    btnEditAppointment.Click += EditAppointment;
                    btnCancelAppointment.Click += CancelAppointment;
                }
            }
        }

        void ScheduleAppointment(object sender, EventArgs e)
        {
            Frame f = new Frame() { Content = new AppointmentScheduler(scheduling, demographics, appointment, selectedDate, timeSlot, updateDisplay) };
            dialogHost.ShowDialog(f);
        }

        void EditAppointment(object sender, EventArgs e)
        {

        }

        void CancelAppointment(object sender, EventArgs e)
        {
            scheduling.CancelAppointment(appointment.AppointmentID, timeSlot);
            updateDisplay(selectedDate);
        }

        void AddCodesAndNotes(object sender, EventArgs e)
        {

        }
    }
}
