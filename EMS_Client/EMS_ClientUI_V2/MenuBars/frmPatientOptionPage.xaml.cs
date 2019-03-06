using System.Windows;
using System.Windows.Controls;


namespace EMS_ClientUI_V2
{
    /// <summary>
    /// Interaction logic for PatientOptionPage.xaml
    /// </summary>
    public partial class PatientOptionPage : Page
    {
        #region PRIVATE
        Frame ContentFrame { get; set; }
        AddNewPatient AddNewPatientPage { get; set; }
        #endregion

        public PatientOptionPage(Frame frame)
        {
            InitializeComponent();
            AddNewPatientPage = new AddNewPatient();
            ContentFrame = frame;
        }

        private void AddPatienBtn_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = AddNewPatientPage;
        }
    }
}
