/**
 * \file ReconcileMonthlyCommand.cs
*  \project INFO2180 - EMS System Term Project
*  \author The Char Stars - Tudor Lupu
*  \date 2018-12-4
*  \brief The menu option to reconcile a month.
*  
*  This class is the option in the billing menu to 
*  reconcile a month.
*/

using EMS_Client.Interfaces;
using EMS_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS_Client.MenuSpecificOptions
{
    /** 
    * \class ReconcileMonthlyCommand
    *
    * \brief <b>Brief Description</b> - This class is a sub menu of billing used to reconcile a month
    * 
    * The ReconcileMonthlyCommand class displays a date picker and once selected it attempts to reconcile
    * that month
    * 
    * \author <i>The Char Stars - Tudor Lupu</i>
    */
    class ReconcileMonthlyCommand : IOption
    {
        public string Description => "Reconcile Month";

        /**
        * \brief <b>Brief Description</b> - Execute <b><i>class method</i></b> - Entry point into the reconcile month menu
        * \details <b>Details</b>
        *
        * This takes in the scheduling, demographics and billing libraries
        * 
        * \return <b>void</b>
        */
        public void Execute(Scheduling scheduling, Demographics demographics, Billing billing)
        {           
            // display a clear screen
            Container.DisplayContent(new List<Pair<string, string>>() { { new Pair<string, string>("", "") } },
                                            1, 1, MenuCodes.BILLING, "Billing", Description);

            // get the month to generate the report for
            DateTime getMonth = DatePicker.GetDate(scheduling, DateTime.Today, false);

            // check if the user didnt cancel the month selection
            if (getMonth.Ticks != 0)
            {
                // format the name of the text file
                string date = string.Format("{0}{1}govFile.txt", getMonth.Year, getMonth.Month);

                // generate the report for the reconciled month
                List<string> report = billing.ReconcileMonthlyBilling(date);               

                // display the success message
                Container.DisplayContent(new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("Report successfully generated!", "") }, 1, -1, MenuCodes.BILLING, "Billing", Description);

                // wait for confirmation from user that they read the message
                Console.ReadKey();

            }
        }
    }
}
