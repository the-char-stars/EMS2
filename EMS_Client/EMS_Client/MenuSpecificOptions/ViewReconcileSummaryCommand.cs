/**
 * \file UpdatePatientCommand.cs
*  \project INFO2180 - EMS System Term Project
*  \author The Char Stars - Tudor Lupu
*  \date 2018-12-4
*  \brief The menu option to view the reconcile summary
*  
*  This class is the option in the billing menu to 
*  view the reconcile summary.
*/

using EMS_Client.Interfaces;
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
    * \class ViewReconcileSummaryCommand
    *
    * \brief <b>Brief Description</b> - This class is meant to create all the boxes and display the content inside of them.
    * 
    * The Container class takes in what type of menu and its contents to display and then proceeds to display it.
    *
    * \author <i>The Char Stars - Tudor Lupu</i>
    */
    class ViewReconcileSummaryCommand : IOption
    {
        #region public fields
        public string Description => "Reconcile Summary";
        #endregion

        /**
        * \brief <b>Brief Description</b> - Execute <b><i>class method</i></b> - An overload of the entry point into the reconcile summary viewing menu
        * \details <b>Details</b>
        *
        * This takes in the existing patient information, if it should return a patient and the demographics library
        * 
        * \return <b>void</b>
        */
        public void Execute(Scheduling scheduling, Demographics demographics, Billing billing)
        {           
            Container.DisplayContent(new List<Pair<string, string>>() { { new Pair<string, string>("", "") } },
                                            2, 1, MenuCodes.BILLING, "Billing", Description);

            // let the user pick a month
            DateTime getMonth = DatePicker.GetDate(scheduling, DateTime.Today, false);

            // check if user did not cancel during month selection screen
            if (getMonth.Ticks != 0)
            {
                // generate the government file name
                string date = string.Format("{0}{1}govFile.txt", getMonth.Year, getMonth.Month);

                // get the list of the report contents
                List<string> report = billing.ReconcileMonthlyBilling(date);
                List<Pair<string, string>> content = new List<Pair<string, string>>();

                // create the content using the lines in the report
                foreach (string line in report)
                {
                    content.Add(new Pair<string, string>(line, ""));
                }


                // display the contents of the report
                Container.DisplayContent(content, 2, -1, MenuCodes.BILLING, "Billing", Description);

                Console.ReadKey();

            }
        }
    }
}
