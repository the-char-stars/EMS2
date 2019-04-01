using EMS_Library;
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
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        Demographics demographics = new Demographics();
        public MainPage()
        {
            InitializeComponent();

            // display the main menu with the three main buttons
            MainMenuFrame.Content = new MainMenuPage(
                this.ContentFrame, this.ExtraOptionMenuFrame, this.mainDialogueHost, demographics);
        }
    }
}
