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
        AddPatient AddPatientPage { get; set; }
        EditPatient EditPatientPage { get; set; }
        SearchPatient SearchPatientPage { get; set; }
        #endregion

        public PatientMenuPage(Frame frame)
        {
            InitializeComponent();
            AddPatientPage = new AddPatient();
            EditPatientPage = new EditPatient();
            SearchPatientPage = new SearchPatient();
            ContentFrame = frame;
        }

        private void AddPatienBtn_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = AddPatientPage;
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
