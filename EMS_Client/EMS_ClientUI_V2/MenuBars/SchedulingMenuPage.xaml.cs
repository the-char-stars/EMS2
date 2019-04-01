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
    /// Interaction logic for SchedulingMenuPage.xaml
    /// </summary>
    public partial class SchedulingMenuPage : Page
    {
        #region PRIVATE
        Frame ContentFrame { get; set; }
        ViewAppointments ViewAppointmentsPage { get; set; }
        ScheduleRecall ScheduleRecallPage { get; set; }
        #endregion

        public SchedulingMenuPage(Frame frame, DialogHost dialogHost)
        {
            InitializeComponent();

            ViewAppointmentsPage = new ViewAppointments();
            ScheduleRecallPage = new ScheduleRecall();
            ContentFrame = frame;
        }

        private void ViewAppts_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = ViewAppointmentsPage;
        }

        private void SchedRecall_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = ScheduleRecallPage;
        }
    }
}
