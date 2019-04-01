using EMS_Library;
using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;


namespace EMS_ClientUI_V2
{
    /// <summary>
    /// Interaction logic for PatientOptionPage.xaml
    /// </summary>
    public partial class PatientMenuPage : Page
    {
        #region PRIVATE
        Frame ContentFrame { get; set; }
        EditPatient EditPatientPage { get; set; }
        SearchPatient SearchPatientPage { get; set; }
        DialogHost DialogHost;
        Demographics demographics;
        #endregion

        public PatientMenuPage(Frame frame, DialogHost dialogHost, Demographics d)
        {
            demographics = d;
            InitializeComponent();
            DialogHost = dialogHost;
            EditPatientPage = new EditPatient();
            SearchPatientPage = new SearchPatient();
            ContentFrame = frame;
        }

        private void AddPatienBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame f = new Frame { Content = new AddPatient(demographics) };
            DialogHost.ShowDialog(f);
            //ContentFrame.Content = AddPatientPage;
        }

        private void EditPatientBtn_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = EditPatientPage;
        }

        private void SearchPatientBtn_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = SearchPatientPage;
        }
    }
}
