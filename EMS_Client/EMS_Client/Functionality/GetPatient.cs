/**
 * \file GetPatient.cs
*  \project INFO2180 - EMS System Term Project
*  \author The Char Stars - Tudor Lupu
*  \date 2018-12-4
*  \brief The menu to search for a patient
*  
*  This class displays the patient search screen and lets
*  the user select a patient out of the results.
*/

using EMS_Client.MenuOptions;
using EMS_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EMS_Client.Functionality
{
    public enum SearchOption { NOSEARCH = -1, NAMESEARCH = 1, HCNSEARCH }

    /** 
    * \class GetPatient
    *
    * \brief <b>Brief Description</b> - This class is the functionality behind searching and selecting a patient
    * 
    * The GetPatient class simply just asks for a patient and then returns all paitnets found by the information.
    * 
    * \author <i>The Char Stars - Tudor Lupu</i>
    */
    public class GetPatient
    {
        #region private fields
        private static bool _return;
        private static int _selectedInputField;
        private static SearchOption _selectedSearch;
        private static Pair<int, int> _inputPosition;
        private static List<Pair<string, string>> _searchMenu;
        private static List<Pair<string, string>> _namesSearchContent;
        private static List<Pair<string, string>> _hcnSearchContent;
        private enum NameSearchFields { FNAME = 1, LNAME, SEARCH };
        private enum HCNSearchFields { HCN = 1, SEARCH };
        #endregion

        #region public fields

        #endregion

        static string Description => "Search Patient";

        /**
        * \brief <b>Brief Description</b> - Get <b><i>class method</i></b> - the main method used to get the patient.
        * \details <b>Details</b>
        *
        * This gets the current selected menu, the title of the menu, the selected option in that menu and the block
        * selected
        * 
        * \return <b>Patient</b> - the patient that was selected by the user search
        */
        public static Patient Get(MenuCodes menu, string title, int selectedIndex, int blockID = -1)
        {
            _return = false;
            _selectedInputField = 1;
            _inputPosition = new Pair<int, int>(6, 50);
            _selectedSearch = SearchOption.NOSEARCH;

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
            Container.DisplayContent(_searchMenu, selectedIndex, _selectedInputField, menu, title, Description);

            // run the main loop and return its return
            return MainLoop(menu, title, selectedIndex, blockID);
        }

        /**
        * \brief <b>Brief Description</b> - FormatInputLines <b><i>class method</i></b> - Formats the input fields
        * \details <b>Details</b>
        *
        * This takes in the contents of the menu that it needs to format
        * 
        * \return <b>void</b>
        */
        static void FormatInputLines(List<Pair<string, string>> content)
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
        * This takes in the menu that is currently selected, the title of the menu, the index of the selected menu option
        * 
        * \return <b>Patient</b> - selected patient by the user
        */
        static Patient MainLoop(MenuCodes menu, string title, int selectedIndex, int blockID)
        {
            Console.CursorVisible = false;
            ConsoleKey userInput;
            Patient patient = null;

            while (!_return)
            {
                _return = false;
                userInput = Console.ReadKey(true).Key;
                Thread.Sleep(1);    // sleep for just a tiny bit so the menu has time to reprint. avoids visual glitches

                // check and change based on what button the user pressed
                if (ChangeInputField(userInput))
                {
                    // check if the user requested to return
                    if (_return) { break; }

                    // check if the user requested to search
                    if (_selectedSearch != SearchOption.NOSEARCH)
                    {
                        switch (_selectedSearch)
                        {
                            // user chose to search by name
                            case SearchOption.NAMESEARCH:
                                patient = SearchNames(menu, title, selectedIndex, blockID);
                                break;
                            // user chose to search by health card number
                            case SearchOption.HCNSEARCH:
                                patient = SearchHCN(menu, title, selectedIndex, blockID);
                                break;
                        }
                        if (patient != null) { break; }

                        // set it back to not searching
                        _selectedSearch = SearchOption.NOSEARCH;
                    }

                    // display the now changed menu
                    Container.DisplayContent(_searchMenu, selectedIndex, _selectedInputField, menu, title, Description);
                }
            }      
            
            // clear the console and return the patient
            Console.Clear();
            return patient;
        }

        /**
        * \brief <b>Brief Description</b> - SearchNames <b><i>class method</i></b> - the functionality to search by names
        * \details <b>Details</b>
        *
        * This takes in the menu that is currently selected, the title of the menu, the index of the selected menu option
        * 
        * \return <b>Patient</b> - selected patient by the user
        */
        static Patient SearchNames(MenuCodes menu, string title, int selectedIndex, int blockID)
        {
            _selectedInputField = 1;
            _inputPosition.First = 6;
            _inputPosition.Second = 50;

            // make sure the input fields are clear
            Input.ClearInputFields(_namesSearchContent);

            // display the menu
            Container.DisplayContent(_namesSearchContent, selectedIndex, _selectedInputField, menu, title, Description);

            ConsoleKey keyPressed;
            do
            {
                keyPressed = Console.ReadKey(true).Key;

                // act according to what key the user pressed
                switch (keyPressed)
                {
                    // up arrow was pressed therefore change indexes
                    case ConsoleKey.UpArrow:
                        if (_selectedInputField > 1)
                        {
                            _selectedInputField--;
                            _inputPosition.First--;
                        }
                        break;
                    // down arrow was pressed therefore change indexes
                    case ConsoleKey.DownArrow:
                        if (_selectedInputField < _namesSearchContent.Count)
                        {
                            _selectedInputField++;
                            _inputPosition.First++;
                        }
                        break;
                    // user decided to select one of the menu options
                    case ConsoleKey.Enter:
                        switch (_selectedInputField)
                        {
                            // user picked to enter a first or last name
                            case (int)NameSearchFields.FNAME:
                            case (int)NameSearchFields.LNAME:
                                Console.CursorTop = _inputPosition.First;
                                Console.CursorLeft = _inputPosition.Second;
                                Pair<InputRetCode, string> temp = Input.GetInput(_namesSearchContent[_selectedInputField - 1].Second, 40, MenuOptions.InputType.Strings);
                                if ((temp.First & InputRetCode.SAVE) != 0)
                                {
                                    _namesSearchContent[_selectedInputField - 1].Second = temp.Second;
                                }
                                break;
                            // user decided to do a search based on the fields above
                            case (int)NameSearchFields.SEARCH:
                                Demographics demographics = new Demographics();

                                // get a list of patients that match the criteria
                                List<Patient> returnedPatients = demographics.GetPatientByName(_namesSearchContent[0].Second, _namesSearchContent[1].Second);

                                // loop through all found records if there are any
                                for (int i = 0; i < returnedPatients.Count; i++) { if (returnedPatients[i].PatientID == blockID) { returnedPatients.RemoveAt(i); } }

                                // some records were found
                                if (returnedPatients.Count > 0)
                                {
                                    return BrowseRecords(returnedPatients, menu, title, selectedIndex, blockID);
                                }
                                // no records were found
                                else
                                {
                                    PrintErrorOnLine(_inputPosition, "No record found with those names.", 0, 41);
                                    Console.ReadKey();
                                }
                                break;
                        }
                        break;
                }

                //  redisplay the menu
                Container.DisplayContent(_namesSearchContent, selectedIndex, _selectedInputField, menu, title, Description);

            } while (keyPressed != ConsoleKey.Escape);

            // reset the selected field
            _selectedInputField = 1;

            return null;
        }

        /**
        * \brief <b>Brief Description</b> - SearchHCN <b><i>class method</i></b> - the functionality to search by hcn
        * \details <b>Details</b>
        *
        * This takes in the menu that is currently selected, the title of the menu, the index of the selected menu option
        * 
        * \return <b>Patient</b> - selected patient by the user
        */
        static Patient SearchHCN(MenuCodes menu, string title, int selectedIndex, int blockID)
        {
            _selectedInputField = 1;
            _inputPosition.First = 7;
            _inputPosition.Second = 50;

            // make sure the input fields are clear
            Input.ClearInputFields(_hcnSearchContent);

            // display the contents of the menu
            Container.DisplayContent(_hcnSearchContent, selectedIndex, _selectedInputField, menu, title, Description);

            ConsoleKey keyPressed;
            do
            {
                keyPressed = Console.ReadKey(true).Key;

                // act according to the key pressed by the user
                switch (keyPressed)
                {
                    // up arrow pressed therefore change index
                    case ConsoleKey.UpArrow:
                        if (_selectedInputField > 1) { _selectedInputField--; }
                        break;
                    // down arrow pressed therefore change index
                    case ConsoleKey.DownArrow:
                        if (_selectedInputField < _hcnSearchContent.Count) { _selectedInputField++; }
                        break;
                    // user decided to select a button
                    case ConsoleKey.Enter:
                        switch (_selectedInputField)
                        {
                            // user selected to enter a health card number
                            case (int)HCNSearchFields.HCN:
                                // set the cursor to the right spot
                                Console.CursorTop = _inputPosition.First - 1;
                                Console.CursorLeft = _inputPosition.Second;

                                // get the health card number
                                Pair<InputRetCode, string> temp = Input.GetInput(_hcnSearchContent[_selectedInputField - 1].Second, 12, InputType.Strings | InputType.Ints);

                                // check if the user saved or canceled their input
                                if ((temp.First & InputRetCode.SAVE) != 0)
                                {
                                    _hcnSearchContent[_selectedInputField - 1].Second = temp.Second;
                                }
                                break;
                            // user selected to search for the health card number above
                            case (int)HCNSearchFields.SEARCH:
                                Demographics demographics = new Demographics();

                                // find a patient with a matching health card number
                                Patient returnedPatient = demographics.GetPatientByHCN(_hcnSearchContent[0].Second);
                                List<Patient> patients = new List<Patient>();

                                // check if there was a patient with that HCN
                                if (returnedPatient != null)
                                {
                                    patients.Add(returnedPatient);
                                    return BrowseRecords(patients, menu, title, selectedIndex, blockID);
                                }
                                else
                                {
                                    PrintErrorOnLine(_inputPosition, "No record found with that number.", 0, 41);
                                    Console.ReadKey();
                                }

                                break;
                        }
                        break;
                }

                // re-display the menu
                Container.DisplayContent(_hcnSearchContent, selectedIndex, _selectedInputField, menu, title, Description);

            } while (keyPressed != ConsoleKey.Escape);

            // reset the selected index
            _selectedInputField = 1;

            return null;
        }

        /**
        * \brief <b>Brief Description</b> - BrowseRecords <b><i>class method</i></b> - the functionality to browse through the records
        * \details <b>Details</b>
        *
        * This takes in a list of patients the menu that is currently selected, the title of the menu, the index of the selected menu option
        * 
        * \return <b>Patient</b> - selected patient record
        */
        static public Patient BrowseRecords(List<Patient> patients, MenuCodes menu, string title, int selectedIndex, int blockID = -1)
        {
            int patientIndex = 1;
            ConsoleKey keyPressed = new ConsoleKey();
            List<Pair<string, string>> patientDisplayList = new List<Pair<string, string>>();

            // insert all found patients into a list which is used to display the patients
            foreach (Patient patient in patients)
            {
                patientDisplayList.Add(new Pair<string, string>(patient.ToString(), ""));
            }

            do
            {
                // display the patient list
                Container.DisplayContent(patientDisplayList, selectedIndex, patientIndex, menu, title, Description);

                keyPressed = Console.ReadKey(true).Key;

                switch (keyPressed)
                {
                    // user pressed down key therefore change index
                    case ConsoleKey.DownArrow:
                        if (patientIndex < patients.Count) { patientIndex++; }
                        break;
                    // user pressed up key therefore change index
                    case ConsoleKey.UpArrow:
                        if (patientIndex > 1) { patientIndex--; }
                        break;
                    // user chose a patient record
                    case ConsoleKey.Enter:
                        // return the selected patient recordd
                        return patients[patientIndex - 1];
                }


            } while (keyPressed != ConsoleKey.Escape);

            // user canceled therefore no patient was selected
            return null;
        }

        /**
        * \brief <b>Brief Description</b> - PrintErrorOnLine <b><i>class method</i></b> - displays the error on the same line as the input field
        * \details <b>Details</b>
        *
        * This takes the initial position of where the cursor it currently is at, the error message to display, how far it should be and in which column
        * 
        * \return <b>bool</b> - returns false because there was an error
        */
        static bool PrintErrorOnLine(Pair<int, int> pInitialPosition, string message, int lineOffset, int columnOffset)
        {
            // set the cursor to the position to which it will display the message
            Console.CursorTop = pInitialPosition.First + lineOffset;
            Console.CursorLeft = Console.WindowWidth - message.Length - 2;

            // display the red message
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(message);
            Console.ForegroundColor = ConsoleColor.DarkGray;

            return false;
        }

        /**
        * \brief <b>Brief Description</b> - ChangeInputField <b><i>class method</i></b> - change the input field selected based on user keys pressed
        * \details <b>Details</b>
        *
        * This takes in the key pressed for the main menu and acts according to what that key was
        * 
        * \return <b>bool</b> - returns true if the input field has changed false otherwise
        */
        private static bool ChangeInputField(ConsoleKey keyPressed)
        {
            switch (keyPressed)
            {
                // if user pressed up arrow key and the index can go up, change index
                //  and return that there was a change
                case (ConsoleKey.UpArrow):
                    if (_selectedInputField > 1)
                    {
                        _selectedInputField--;
                        _inputPosition.First--;
                        return true;
                    }
                    break;
                // if user pressed down arrow key and the index can go down, change index
                //  and return that there was a change
                case (ConsoleKey.DownArrow):
                    if (_selectedInputField < _searchMenu.Count)
                    {
                        _selectedInputField++;
                        _inputPosition.First++;
                        return true;
                    }
                    break;
                // if user selected a field, set the selected field index and
                // return that there should be a change
                case (ConsoleKey.Enter):
                    _selectedSearch = (SearchOption)_selectedInputField;
                    return true;
                // if the user chose to cancel, set the return to true and 
                // return that there should be a change
                case (ConsoleKey.Escape):
                    _return = true;
                    return true;
            }

            return false;
        }
    }
}
