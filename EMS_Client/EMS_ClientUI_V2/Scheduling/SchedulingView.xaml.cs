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
    /// Interaction logic for SchedulingView.xaml
    /// </summary>
    public partial class SchedulingView : Page
    {
        Scheduling scheduling;
        Demographics demographics;
        public SchedulingView(Scheduling s, Demographics d)
        {
            scheduling = s;
            demographics = d;
            InitializeComponent();
            int i = 1;
            foreach (Appointment a in scheduling.GetScheduleByDay(DateTime.Now).GetAppointments())
            {
                lvTodaysSchedule.Children.Add(new AppointmentCard(a, demographics, scheduling, i++));
            }
        }

        private void BtnUpdateAppointment_Click(object sender, RoutedEventArgs e)
        {
            // update the selected appointment
        }

        private void CalSelectedDate_DisplayDateChanged(object sender, EventArgs e)
        {
            lvTodaysSchedule.Children.Clear();
            int i = 1;

            foreach (Appointment a in scheduling.GetScheduleByDay(calSelectedDate.SelectedDate.Value).GetAppointments())
            {
                lvTodaysSchedule.Children.Add(new AppointmentCard(a, demographics, scheduling, i++));
            }
        }
    }
}
