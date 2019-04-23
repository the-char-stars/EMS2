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

namespace EMS_ClientUI_V2
{
    /// <summary>
    /// Interaction logic for AppointmentScheduler.xaml
    /// </summary>
    public partial class AppointmentEditor : UserControl
    {
        Appointment appointment;
        Scheduling scheduling;
        Demographics demographics;
        Patient primary, dependant;
        UpdateDisplay updateDisplay;
        Billing billing;

        DateTime selectedDate;
        int timeSlot;
        bool isUpdate;

        public AppointmentEditor(Scheduling s, Demographics d, Appointment a, Billing b, DateTime da, int ts, UpdateDisplay upd, bool ud = false)
        {
            Logging.Log("AppointmentScheduler is initiated");

            InitializeComponent();
            billing = b;
            isUpdate = ud;
            updateDisplay = upd;
            timeSlot = ts;
            selectedDate = da;
            scheduling = s;
            demographics = d;
            appointment = a;
            
            foreach(BillingRecord br in billing.allBillingCodes.Values)
            {
                cbBillingCodes.Items.Add(br);
            }

            tbPrimaryPatient.Text = demographics.GetPatientByID(appointment.PatientID).GetName();
            btnPrimaryAdd.Click += BtnPrimaryAdd_Click;

            foreach(ApptBillRecord br in billing.GetApptBillRecords(appointment.AppointmentID))
            {
                Chip c = new Chip();
                c.Margin = new Thickness(4);
                c.Content = br.BillingCode;

                if (Int32.Parse(br.PatientID) == appointment.PatientID)
                {
                    wpBillingCodesPrimary.Children.Add(c);
                }                
                else
                {
                    wpBillingCodesSecondary.Children.Add(c);
                }
            }

            if (a.DependantID == -1)
            {
                btnSecondaryAdd.Visibility = Visibility.Hidden;

            }
            else
            {
                tbDependantPatient.Text = demographics.GetPatientByID(appointment.DependantID).GetName();
                btnSecondaryAdd.Click += BtnSecondaryAdd_Click; ;
            }
        }

        private void BtnSecondaryAdd_Click(object sender, RoutedEventArgs e)
        {
            if (cbBillingCodes.SelectedValue != null)
            {
                Chip c = new Chip();
                c.Margin = new Thickness(4);
                c.DeleteClick += C_DeleteClick;
                c.Tag = wpBillingCodesSecondary;
                c.Content = cbBillingCodes.SelectedValue;
                c.IsDeletable = true;
                wpBillingCodesSecondary.Children.Add(c);
            }
        }

        private void C_DeleteClick(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)((Chip)sender).Tag).Children.Remove((Chip)sender);
        }

        private void BtnPrimaryAdd_Click(object sender, RoutedEventArgs e)
        {
            if (cbBillingCodes.SelectedValue != null)
            {
                Chip c = new Chip();
                c.Margin = new Thickness(4);
                c.DeleteClick += C_DeleteClick;
                c.Tag = wpBillingCodesPrimary;
                c.Content = cbBillingCodes.SelectedValue;
                c.IsDeletable = true;
                wpBillingCodesPrimary.Children.Add(c);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Logging.Log("Cancel button pressed in AppointmentScheduler pop up window");
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            Logging.Log("Okay button pressed in Appointment Scheduler");
            foreach (Chip c in wpBillingCodesPrimary.Children)
            {
                if (c.Tag != null)
                {
                    billing.AddNewRecord(appointment.AppointmentID.ToString(), appointment.PatientID.ToString(), ((BillingRecord)c.Content).BillingCode);
                }
            }

            foreach (Chip c in wpBillingCodesSecondary.Children)
            {
                if (c.Tag != null)
                {
                    billing.AddNewRecord(appointment.AppointmentID.ToString(), appointment.DependantID.ToString(), ((BillingRecord)c.Content).BillingCode);
                }
            }
            updateDisplay(selectedDate);
            DialogHost.CloseDialogCommand.Execute(null, null);
        }
    }
}
