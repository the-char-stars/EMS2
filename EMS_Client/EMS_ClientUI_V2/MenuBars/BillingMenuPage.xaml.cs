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
    /// Interaction logic for BillingMenuPage.xaml
    /// </summary>
    public partial class BillingMenuPage : Page
    {
        #region PRIVATE
        Frame ContentFrame { get; set; }
        MonthlyReport MonthlyReportPage { get; set; }
        ReconcileMonthly ReconcileMonthlyPage { get; set; }
        ReconcileSummary ReconcileSummaryPage { get; set; }
        #endregion

        public BillingMenuPage(Frame frame, DialogHost dialogHost)
        {
            InitializeComponent();
            MonthlyReportPage = new MonthlyReport();
            ReconcileMonthlyPage = new ReconcileMonthly();
            ReconcileSummaryPage = new ReconcileSummary();
            ContentFrame = frame;
        }

        private void MonthlyReport_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = MonthlyReportPage;
        }

        private void ReconcileMonthly_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = ReconcileMonthlyPage;
        }

        private void ReconcileSummary_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = ReconcileSummaryPage;
        }
    }
}
