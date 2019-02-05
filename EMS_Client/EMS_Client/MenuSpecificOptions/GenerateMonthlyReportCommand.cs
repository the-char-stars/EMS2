/**
 * \file GenerateMonthlyReportCommand.cs
*  \project INFO2180 - EMS System Term Project
*  \author The Char Stars - Tudor Lupu
*  \date 2018-12-4
*  \brief The menu option to generate a monthly report.
*  
*  This class is the option in the billing menu to 
*  generate a monthly report.
*/

using EMS_Client.Interfaces;
using EMS_Client.MenuOptions;
using EMS_Client;
using EMS_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/** 
* \namespace EMS_Client
*
* \brief <b>Brief Description</b> - This namespace holds the user interface portion of the code base.
*
* \author <i>The Char Stars</i>
*/
namespace EMS_Client.MenuSpecificOptions
{
    /** 
    * \class GenerateMonthlyReportCommand
    *
    * \brief <b>Brief Description</b> - This class is a sub menu of billing used to generate a monthly report
    * 
    * The GenerateMonthlyReportCommand class displays a date picker and once selected it attempts to generate
    * a monthly report for the selected month.
    * 
    * \author <i>The Char Stars - Tudor Lupu</i>
    */
    class GenerateMonthlyReportCommand : IOption
    {
        public string Description => "Monthly Report";


        /**
        * \brief <b>Brief Description</b> - Execute <b><i>class method</i></b> - Entry point into the generate monthly report menu
        * \details <b>Details</b>
        *
        * This takes in the scheduling, demographics and billing libraries
        * 
        * \return <b>void</b>
        */
        public void Execute(Scheduling scheduling, Demographics demographics, Billing billing)
        {
            // display a clear screen 
            Container.DisplayContent(new List<Pair<string, string>>() { { new Pair<string, string>("", "")} }, 0, -1, MenuCodes.BILLING, "Billing", Description);

            // get the month to generate the report for
            DateTime getMonth = DatePicker.GetDate(scheduling, DateTime.Today, false);

            // check if the user didnt cancel the month selection
            if (getMonth.Ticks != 0) {

                // check if the report generation was successful
                if(billing.GenerateMonthlyBillingFile(scheduling, demographics, getMonth.Year, getMonth.Month))
                {
                    Container.DisplayContent(new List<Pair<string, string>>() { { new Pair<string, string>("Report successfuly generated!", "")} }, 0, -1, MenuCodes.BILLING, "Billing", Description);
                }
                else
                {
                    Container.DisplayContent(new List<Pair<string, string>>() { { new Pair<string, string>("Report failed to generate.", "") } }, 0, -1, MenuCodes.BILLING, "Billing", Description);
                }

                // wait for confirmation from user that they read the message
                Console.ReadKey();
            }
        }
    }
}
