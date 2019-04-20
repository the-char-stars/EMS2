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

namespace EMS_ClientUI_V2
{
    /// <summary>
    /// Interaction logic for AppointmentCard.xaml
    /// </summary>
    public partial class AppointmentCard : UserControl
    {
        public Appointment appointment;
        public Patient primary = null, dependant = null;
        public AppointmentCard(Appointment a, Demographics d, Scheduling s, int slot)
        {
            InitializeComponent();
            appointment = a;
            if (appointment.AppointmentID == -1)
            {
                tbTitle.Text = "Unscheduled Appointment";
                chipPrimaryPatient.Visibility = Visibility.Hidden;
                chipSecondaryPatient.Visibility = Visibility.Hidden;

                btnCancelAppointment.Content = "Schedule Appointment";
                btnCancelAppointment.Click += ScheduleAppointment;

                btnEditAppointment.Visibility = Visibility.Hidden;
            }
            else
            {
                primary = d.GetPatientByID(a.PatientID);
                dependant = d.GetPatientByID(a.DependantID);

                tbTitle.Text = string.Format("Slot {0}", slot);

                chipPrimaryPatient.Content = string.Format("{0} {1}", primary.FirstName, primary.LastName);
                chipPrimaryPatient.Icon = primary.FirstName[0];

                chipSecondaryPatient.Content = string.Format("{0} {1}", dependant.FirstName, dependant.LastName);
                chipSecondaryPatient.Icon = dependant.FirstName[0];

                btnEditAppointment.Click += EditAppointment;
                btnCancelAppointment.Click += CancelAppointment;
            }

        }

        void ScheduleAppointment(object sender, EventArgs e)
        {

        }

        void EditAppointment(object sender, EventArgs e)
        {

        }

        void CancelAppointment(object sender, EventArgs e)
        {

        }
    }
}
