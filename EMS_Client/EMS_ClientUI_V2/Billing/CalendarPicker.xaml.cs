using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for MonthlyReportPage.xaml
    /// </summary>
    public partial class CalendarPicker : Page
    {
        public delegate void CompletionFunction (DateTime t);

        CompletionFunction completionFunction;

        private DateTime newdate = DateTime.Now;

        public CalendarPicker(CompletionFunction cf)
        {
            InitializeComponent();

            completionFunction = cf;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {

            DialogHost.CloseDialogCommand.Execute(null, null);

        }

        private void BtnOkay_Click(object sender, RoutedEventArgs e)
        {
            if (newdate != null)
            {
                completionFunction(newdate);
                DialogHost.CloseDialogCommand.Execute(null, null);
            }
        }

        private void TheCalendar_DisplayModeChanged(object sender, CalendarModeChangedEventArgs e)
        {

            if (theCalendar.DisplayMode == System.Windows.Controls.CalendarMode.Month)
            {
                newdate = theCalendar.DisplayDate;
                theCalendar.DisplayMode = System.Windows.Controls.CalendarMode.Year;
            }
        }

        private void TheCalendar_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.Captured is CalendarItem)
            {
                Mouse.Capture(null);
            }
        }
    }
}
