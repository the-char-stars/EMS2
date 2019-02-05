/**
 * \file ScheduleRecallCommand.cs
*  \project INFO2180 - EMS System Term Project
*  \author The Char Stars - Tudor Lupu
*  \date 2018-12-4
*  \brief The menu option to search for a patient
*  
*  This class is the option in the scheduling menu to 
*  search for a patient in the database.
*/

using EMS_Client.Functionality;
using EMS_Client.Interfaces;
using EMS_Client.MenuOptions;
using EMS_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    public enum SearchOption { NOSEARCH = -1, NAMESEARCH = 1, HCNSEARCH }

    /** 
    * \class SearchPatientListCommand
    *
    * \brief <b>Brief Description</b> - This class is a sub menu of patients used to just search for a patient in the database
    * 
    * The SearchPatientListCommand class displays the search menu asking for first&last names or hcn and displays all matching
    * patients. Also provides the option of selection a patient
    * 
    * \author <i>The Char Stars - Tudor Lupu</i>
    */
    class SearchPatientListCommand : IOption
    {
        #region private fields
        private int _selectedInputField;
        private List<Pair<string, string>> _searchMenu;
        private List<Pair<string, string>> _namesSearchContent;
        private List<Pair<string, string>> _hcnSearchContent;
        private enum NameSearchFields { FNAME = 1, LNAME, SEARCH };
        private enum HCNSearchFields { HCN = 1, SEARCH};
        #endregion

        #region public fields
        public string Description => "Search Patient";
        #endregion

        /**
        * \brief <b>Brief Description</b> - Execute <b><i>class method</i></b> - Entry point into the search patient list menu
        * \details <b>Details</b>
        *
        * This takes in the scheduling, demographics and billing libraries
        * 
        * \return <b>void</b>
        */
        public void Execute(Scheduling scheduling, Demographics demographics, Billing billing)
        {
            // the menu that allows the user to select between searching by first and last names or HCN
            _searchMenu = new List<Pair<string, string>>()
            {
                { new Pair<string, string>(" > First & Last name", "") },
                { new Pair<string, string>(" > Health Card Number", "") }
            };

            // the menu for the searching by first and last names
            _namesSearchContent = new List<Pair<string, string>>()
            {
                { new Pair<string, string>("First Name", "") },
                { new Pair<string, string>("Last Name", "") },
                { new Pair<string, string>(" >> SEARCH PATIENT <<", "") }
            };

            // the menu for the searching by health card number
            _hcnSearchContent = new List<Pair<string, string>>()
            {
                { new Pair<string, string>("Health Card Number", "") },
                { new Pair<string, string>(" >> SEARCH PATIENT <<", "") }
            };

            //  format the input lines so they look like input lines
            FormatInputLines(_namesSearchContent);
            FormatInputLines(_hcnSearchContent);

            // display the search menu
            Container.DisplayContent(_searchMenu, 2, _selectedInputField, MenuCodes.PATIENTS, "Patients", Description);

            // run the main loop
            MainLoop();
        }

        /**
        * \brief <b>Brief Description</b> - FormatInputLines <b><i>class method</i></b> - Formats the input fields
        * \details <b>Details</b>
        *
        * This takes in the contents of the menu that it needs to format
        * 
        * \return <b>void</b>
        */
        private void FormatInputLines(List<Pair<string, string>> content)
        {
            // for the lines that dont already contain a ':' or the lines that are not a search patient button add the colon
            for (int index = 0; index < content.Count; index++)
            {
                if (!content[index].First.Contains(":") &&
                    !content[index].First.Contains("SEARCH PATIENT") &&
                    content[index].First != "")
                {
                    content[index].First = string.Format("{0}{1, -20}{2}", " ", content[index].First, ":");
                }
            }
        }

        /**
        * \brief <b>Brief Description</b> - MainLoop <b><i>class method</i></b> - main logic behind the patient search
        * \details <b>Details</b>
        * 
        * \return <b>void</b>
        */
        private void MainLoop()
        {
            Console.CursorVisible = false;
            while (true)
            {
                // display the getting patient menu and get the requested patient
                Patient resultPatient = GetPatient.Get(MenuCodes.PATIENTS, "Patients", 2);

                // check if user canceled the patient get action
                if (resultPatient != null)
                {
                    // list of all the patient information that needs to be displayed
                    List<KeyValuePair<string, string>> infoToDisplay = resultPatient.ShowInfo();

                    // display the found patient information
                    Container.DisplayContent(infoToDisplay, 2, _selectedInputField, MenuCodes.PATIENTS, "Patients", Description);

                    // wait for user confirmation that they are done reading
                    Console.ReadKey();
                }
                // exit if canceled
                else { break; }
            }

            Console.Clear();
        }

    }
}
