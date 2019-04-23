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
using System.IO;

namespace EMS_ClientUI_V2
{
    /// <summary>
    /// Interaction logic for SchedulingView.xaml
    /// </summary>
    public partial class BillingView : Page
    {
        DialogHost dh;
        Scheduling scheduling;
        Demographics demographics;
        Billing billing;
        Snackbar snackBar;

        public BillingView(Scheduling sched, Demographics demo, DialogHost dialogHost, Billing b, Snackbar sb)
        {
            InitializeComponent();
            dh = dialogHost;
            scheduling = sched;
            demographics = demo;
            billing = b;
            snackBar = sb;
            tbReportTitle.Header = "";
            UpdateReportTree();
            
        }

        private void BtnGenerateMonthlyReport_Click(object sender, RoutedEventArgs e)
        {
            CalendarPicker calendarPicker = new CalendarPicker(GenerateMonthlyReport);
            Frame f = new Frame() { Content = calendarPicker };
            dh.ShowDialog(f);         

        }

        private void GenerateMonthlyReport (DateTime dt)
        {
            // check if the report generation was successful
            if (billing.GenerateMonthlyBillingFile(scheduling, demographics, dt.Year, dt.Month))
            {
                snackBar.MessageQueue.Enqueue("SUCCESSFULLY CREATED");
            }
            else
            {
                snackBar.MessageQueue.Enqueue("DID NOT GENERATE");
            }

            UpdateReportTree();
        }

        private void BtnReconcileMonthlyReport_Click(object sender, RoutedEventArgs e)
        {
            CalendarPicker calendarPicker = new CalendarPicker(ReconcileMonthlyReport);
            Frame f = new Frame() { Content = calendarPicker };
            dh.ShowDialog(f);
        }

        private void ReconcileMonthlyReport(DateTime dt)
        {
            // format the name of the text file
            string date = string.Format(@"Reports\{0,2:D2}{1,2:D2}", dt.Year, dt.Month);
            // check if the report generation was successful
            List<string> ls = billing.GenerateMonthlyBillingSummary(date);
            if (ls.Count != 0)
            {
                snackBar.MessageQueue.Enqueue("SUCCESSFULLY CREATED");
            }
            else
            {
                snackBar.MessageQueue.Enqueue("DID NOT GENERATE");
            }
            tbReportDisplay.Text = "";
            tbReportTitle.Content = "Monthly Report Summary";
            foreach (string s in ls)
            {
                tbReportDisplay.Text += s + '\n';
            }
            UpdateReportTree();

        }

        private void UpdateReportTree()
        {
            if (!Directory.Exists("Reports")) { Directory.CreateDirectory("Reports"); }
            tvcMonthlyBillingReport.Items.Clear();
            tvcGovResponseReports.Items.Clear();

            List<string> reportFiles = new List<string>(Directory.EnumerateFiles(@"Reports"));

            foreach (string s in reportFiles)
            {
                string name = s.Split('\\')[1];

                if (name.Contains("gov"))
                {

                    tvcGovResponseReports.Items.Add(name);
                }

                if (name.Contains("Monthly"))
                {
                    tvcMonthlyBillingReport.Items.Add(name);
                }
            }

            tvReports.InvalidateVisual();
        }

        private void TvReports_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            string filePath = "Reports/" + tvReports.SelectedValue.ToString();
            tbReportTitle.Header = tvReports.SelectedValue.ToString();
            if (File.Exists(filePath))
            {
                tbReportDisplay.Text = File.ReadAllText(filePath);
            }
            else
            {
                tbReportDisplay.Text = "";
                tbReportTitle.Header = "";
            }
        }
    }
}
