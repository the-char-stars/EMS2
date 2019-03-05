using System.Windows;
using System.Windows.Controls;


namespace EMS_ClientUI_V2
{
    /// <summary>
    /// Interaction logic for PatientOptionPage.xaml
    /// </summary>
    public partial class PatientOptionPage : Page
    {
        Frame contentFrame;

        AddNewPatient anp = new AddNewPatient();
        public PatientOptionPage(Frame frm)
        {
            InitializeComponent();
            contentFrame = frm;
        }

        private void AddPatienBtn_Click(object sender, RoutedEventArgs e)
        {
            contentFrame.Content = anp;
        }
    }
}
