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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Logging.Log("Initializing MainWindow");
            InitializeComponent();
            this.Background = new ImageBrush(new BitmapImage(new Uri("../../Images/Background2.jpg", UriKind.Relative)));

        }
        public void changeMainFrame(Page page)
        {

        }
    }
}
