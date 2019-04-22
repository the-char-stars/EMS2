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
    /// Interaction logic for SchedulingView.xaml
    /// </summary>
    public partial class SchedulingView : Page
    {
        Scheduling scheduling;
        Demographics demographics;
        DialogHost dialogHost;
        Billing billing;
        public SchedulingView(Scheduling s, Demographics d, Billing b, DialogHost dh)
        {
            billing = b;
            Logging.Log("Scheduling View Initiated");

            dialogHost = dh;
            scheduling = s;
            demographics = d;
            InitializeComponent();
            updateAppointments(DateTime.Today);
        }

        private void BtnUpdateAppointment_Click(object sender, RoutedEventArgs e)
        {
            Logging.Log("Update Appointment Button clicked");
            // update the selected appointment
        }

        void updateAppointments(DateTime dt)
        {
            lvTodaysSchedule.Children.Clear();
            int i = 1;//MAGIC NUMBER

            foreach (Appointment a in scheduling.GetScheduleByDay(dt).GetAppointments())
            {
                lvTodaysSchedule.Children.Add(new AppointmentCard(a, demographics, scheduling, billing, dialogHost, i++, dt, updateAppointments));
            }

            Logging.Log("Appointments are updated");
        }

        private void CalSelectedDate_DisplayDateChanged(object sender, EventArgs e)
        {
            updateAppointments(calSelectedDate.SelectedDate.Value);
        }
    }
}
