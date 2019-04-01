﻿using System;
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

namespace EMS_ClientUI_V2
{
    /// <summary>
    /// Interaction logic for MainMenuPage.xaml
    /// </summary>
    public partial class MainMenuPage : Page
    {
        #region PRIVATE
        // FRAMES
        Frame ContentFrame { get; set; }
        Frame ExtraOptionMenuFrame { get; set; }

        // PAGES
        PatientMenuPage PatientsMenu { get; set; }
        SchedulingMenuPage SchedulingMenu { get; set; }
        BillingMenuPage BillingMenu { get; set; }
        PatientView pv;
        #endregion

        public MainMenuPage(Frame content, Frame extraMenu, DialogHost dialogHost, Demographics d)
        {
            InitializeComponent();
            ContentFrame = content;
            ExtraOptionMenuFrame = extraMenu;

            pv = new PatientView(d, dialogHost);
            PatientsMenu = new PatientMenuPage(ContentFrame, dialogHost, d);
            SchedulingMenu = new SchedulingMenuPage(ContentFrame, dialogHost);
            BillingMenu = new BillingMenuPage(ContentFrame, dialogHost);
        }

        private void BtnBilling_Checked(object sender, RoutedEventArgs e)
        {
            ExtraOptionMenuFrame.Content = BillingMenu;
        }

        private void BtnScheduling_Checked(object sender, RoutedEventArgs e)
        {
            ExtraOptionMenuFrame.Content = SchedulingMenu;
        }

        private void BtnPatients_Checked(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = pv;
            ExtraOptionMenuFrame.Content = null;
        }
    }
}
