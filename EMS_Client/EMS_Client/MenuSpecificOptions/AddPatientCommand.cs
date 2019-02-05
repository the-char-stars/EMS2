/**
 * \file AddPatientCommand.cs
*  \project INFO2180 - EMS System Term Project
*  \author The Char Stars - Tudor Lupu
*  \date 2018-12-4
*  \brief The menu option to add a patient to the database
*  
*  This class is the option in the patient menu to add a patient.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EMS_Client.Interfaces;
using EMS_Library;

namespace EMS_Client.MenuOptions
{
    [Flags] public enum InputType { Strings = 1, Ints = 2, Seperators = 4 }

    /** 
    * \class AddPatientCommand
    *
    * \brief <b>Brief Description</b> - This class is a sub menu of patients used to add a patient to the database
    * 
    * The AddPatientCommand class displays the menu asking for the user information and once received adds them to
    * the database
    *
    * \author <i>The Char Stars - Tudor Lupu</i>
    */
    class AddPatientCommand : IOption
    {
        #region private fields
        private bool _return;
        private int _selectedInputField;
        private Pair<int, int> _inputPosition;
        private List<Pair<string, string>> _content;
        private List<Pair<int, InputType>> _contentLength;
        #endregion

        #region public fields
        public string Description => "Add Patient";
        #endregion

        /**
        * \brief <b>Brief Description</b> - Execute <b><i>class method</i></b> - Entry point into the patient add menu
        * \details <b>Details</b>
        *
        * This takes in the scheduling, demographics and billing libraries
        * 
        * \return <b>void</b>
        */
        public void Execute(Scheduling scheduling, Demographics demographics, Billing billing)
        {
            _return = false;
            _selectedInputField = 1;

            // position of where the first input line should start
            _inputPosition = new Pair<int, int>(6, 60);

            // all the input fields
            _content = new List<Pair<string, string>>()
            {
                { new Pair<string, string>("First Name", "") },
                { new Pair<string, string>("Last Name", "") },
                { new Pair<string, string>("Health Card Number", "") },
                { new Pair<string, string>("Middle Name Initial", "") },
                { new Pair<string, string>("Date of Birth", "") },
                { new Pair<string, string>("Gender", "") },
                { new Pair<string, string>("Head of House", "") },
                { new Pair<string, string>("Address Line 1", "") },
                { new Pair<string, string>("Address Line 2", "") },
                { new Pair<string, string>("City", "") },
                { new Pair<string, string>("Province", "") },
                { new Pair<string, string>("Phone Number", "") },
                { new Pair<string, string>(" >> SAVE PATIENT <<", "") }
            };

            // the max allowed content length and the input type of that length
            _contentLength = new List<Pair<int, InputType>>()
            {
                new Pair<int, InputType>(40,InputType.Strings | InputType.Seperators),
                new Pair<int, InputType>(40,InputType.Strings | InputType.Seperators),
                new Pair<int, InputType>(12,InputType.Strings | InputType.Ints),
                new Pair<int, InputType>(1,InputType.Strings),
                new Pair<int, InputType>(10,InputType.Ints | InputType.Seperators),
                new Pair<int, InputType>(1,InputType.Strings),
                new Pair<int, InputType>(12,InputType.Strings | InputType.Ints),
                new Pair<int, InputType>(40,InputType.Strings | InputType.Ints | InputType.Seperators),
                new Pair<int, InputType>(40,InputType.Strings | InputType.Ints | InputType.Seperators),
                new Pair<int, InputType>(40,InputType.Strings),
                new Pair<int, InputType>(2,InputType.Strings),
                new Pair<int, InputType>(12,InputType.Ints | InputType.Seperators),
            };

            // space out and add colons to input lines
            FormatInputLines();

            // display the input lines
            Container.DisplayContent(_content, 0, _selectedInputField, MenuCodes.PATIENTS, "Patients", Description);

            // start the logic behind getting the input and saving it
            MainLoop(demographics);
        }

        /**
        * \brief <b>Brief Description</b> - Execute <b><i>class method</i></b> - An overload of the entry point into the adding of a patient
        * \details <b>Details</b>
        *
        * This takes in the existing patient information, if it should return a patient and the demographics library
        * 
        * \return <b>Patient</b> - the patient that was just added to the database
        */
        public Patient Execute(List<KeyValuePair<string, string>> existingPatient, bool returnPatient, Demographics demographics)
        {
            _return = false;
            _selectedInputField = 1;
            _inputPosition = new Pair<int, int>(6, 50);
            _content = new List<Pair<string, string>>();

            // creates the content with the information
            foreach (KeyValuePair<string, string> line in existingPatient)
            {
                _content.Add(new Pair<string, string>(line.Key, line.Value));
            }

            // adds the button to the content so it also gets displayed with it
            _content.Add(new Pair<string, string>(" >> SAVE PATIENT <<", ""));

            // the max allowed content length and the input type of that length
            _contentLength = new List<Pair<int, InputType>>()
            {
                new Pair<int, InputType>(40,InputType.Strings | InputType.Seperators),
                new Pair<int, InputType>(40,InputType.Strings | InputType.Seperators),
                new Pair<int, InputType>(12,InputType.Strings | InputType.Ints),
                new Pair<int, InputType>(1,InputType.Strings),
                new Pair<int, InputType>(10,InputType.Ints | InputType.Seperators),
                new Pair<int, InputType>(1,InputType.Strings),
                new Pair<int, InputType>(12,InputType.Strings | InputType.Ints),
                new Pair<int, InputType>(40,InputType.Strings | InputType.Ints | InputType.Seperators),
                new Pair<int, InputType>(40,InputType.Strings | InputType.Ints | InputType.Seperators),
                new Pair<int, InputType>(40,InputType.Strings),
                new Pair<int, InputType>(2,InputType.Strings),
                new Pair<int, InputType>(12,InputType.Ints | InputType.Seperators),
            };

            // space out and add colons to input lines
            FormatInputLines();

            // display the input lines
            Container.DisplayContent(_content, 0, _selectedInputField, MenuCodes.PATIENTS, "Patients", Description);

            // start the logic behind getting the input and saving it
            return MainLoop(demographics, returnPatient);
        }

        /**
        * \brief <b>Brief Description</b> - FormatInputLines <b><i>class method</i></b> - Makes the input field information look nice
        * \details <b>Details</b>
        *
        * This will add the padding and the expected format of certain fields to the field text that is being displayed before the
        * input point for the user
        * 
        * \return <b>void</b>
        */
        private void FormatInputLines()
        {
            // loop through all the input lines
            for (int index = 0; index < _content.Count; index++)
            {
                // check if the line does not already contain a colon or is not a save button
                if (!_content[index].First.Contains(":") &&
                    !_content[index].First.Contains("SAVE PATIENT") &&
                    _content[index].First != "")
                {
                    // check if date of birth, if it is then not only set its padding but also add the expected date format
                    if (_content[index].First == "Date of Birth")
                    {
                        _content[index].First = string.Format("{0}{1, -20}{2, 10}{3}", " ", _content[index].First, "(DDMMYYYY)", ":");
                    }
                    // check if phone number, if it is then not only set its padding but also add the expected date format
                    else if(_content[index].First == "Phone Number")
                    {
                        _content[index].First = string.Format("{0}{1, -15}{2, 15}{3}", " ", _content[index].First, "(XXX-XXX-XXXX)", ":");
                    }
                    // otherwise simply just set the padding of the field
                    else
                    {
                        _content[index].First = string.Format("{0}{1, -30}{2}", " ", _content[index].First, ":");
                    }                
                }

                // if the value associated with the field is null just set it to an empty string
                if (_content[index].Second == null)
                {
                    _content[index].Second = "";
                }
            }
        }

        /**
        * \brief <b>Brief Description</b> - MainLoop <b><i>class method</i></b> - The main logic behind the getting a patient added to the database menu
        * \details <b>Details</b>
        *
        * This takes in the demographics library and whether it needs to return the added patient
        * 
        * \return <b>Patient</b> - the patient added if it was requested
        */
        private Patient MainLoop(Demographics demographics, bool returnPatient = false)
        {
            Patient newPatient = new Patient(demographics);

            const int ADDBUTTON = 13; // index of the button

            //  format the first value to look nice
            ConsoleKey userInput;
            Pair<int, int> pInitialPosition = new Pair<int, int>(6,50);
            Pair<InputRetCode, string> temp = new Pair<InputRetCode, string>(0, "");
            do
            {               
                _return = false;

                // if the user presses tab then treat it as enter
                if (temp.First == InputRetCode.TAB && _selectedInputField != ADDBUTTON) { userInput = ConsoleKey.Enter; }
                else { userInput = Console.ReadKey(true).Key; }

                // delay changing so content has enough time to redisplay avoiding any visual glitches
                Thread.Sleep(1);

                // get the input for the selected field if the user pressed enter and it is not the button
                if (userInput == ConsoleKey.Enter &&
                _selectedInputField != ADDBUTTON)
                {
                    // position the cursor
                    Console.CursorTop = _inputPosition.First;
                    Console.CursorLeft = _inputPosition.Second;

                    // get the input for the selected field
                    temp = Input.GetInput(_content[_selectedInputField - 1].Second, _contentLength[_selectedInputField - 1].First, _contentLength[_selectedInputField - 1].Second);

                    // check if the user saved their input choice
                    if ((temp.First & InputRetCode.SAVE) != 0)
                    {
                        // save the input choice into the form
                        _content[_selectedInputField - 1].Second = temp.Second;
                    }
                    // user canceled their input 
                    else
                    {
                        // reset the position of the cursor
                        Console.CursorTop = 0;
                        Console.CursorLeft = 0;

                        // re-display the form, overwriting the old one therefore not seeing the input that was just canceled
                        Container.DisplayContent(_content, 0, _selectedInputField, MenuCodes.PATIENTS, "Patients", Description);
                    }

                    // check if the user wants to change index to field above
                    if ((temp.First & InputRetCode.UP) != 0)
                    {
                        userInput = ConsoleKey.DownArrow;
                    }

                    // check if the user wants to change index to field below
                    if ((temp.First & InputRetCode.DOWN) != 0)
                    {
                        userInput = ConsoleKey.DownArrow;
                    }
                }
                // user selected to add the patient information they just entered
                else if (userInput == ConsoleKey.Enter && _selectedInputField == ADDBUTTON)
                {
                    bool validInfo = true;
                    int errorOffset = 41, lineOffset = 6;                   

                    // get the value of the head of house from the form
                    newPatient.HeadOfHouse = _content[lineOffset].Second;
                    // check if it is a valid head of house number
                    if ((newPatient.HeadOfHouse != _content[lineOffset].Second) && _content[lineOffset].Second != "")
                    {
                        validInfo = PrintErrorOnLine(pInitialPosition, "Head of House error", lineOffset, errorOffset);
                    }
                    else
                    {
                        // get the patient information of the head of house
                        Patient headOfHouse = demographics.GetPatientByHCN(newPatient.HeadOfHouse);

                        // check if the head of house exists and the field was not left blank
                        if (newPatient.HeadOfHouse != "" && headOfHouse != null)
                        {
                            // the head of house is valid therefore filling in some of the fields with their information

                            // ADDRESS LINE 1
                            lineOffset++;
                            newPatient.AddressLine1 = headOfHouse.AddressLine1;
                            _content[lineOffset].Second = newPatient.AddressLine1;

                            // ADDRESS LINE 2
                            lineOffset++;
                            if (headOfHouse.AddressLine2 != null)
                            {
                                newPatient.AddressLine2 = headOfHouse.AddressLine2;
                                _content[lineOffset].Second = newPatient.AddressLine2;
                            }

                            // CITY LINE
                            lineOffset++;
                            newPatient.City = headOfHouse.City;
                            _content[lineOffset].Second = newPatient.City;

                            // PROVINCE
                            lineOffset++;
                            newPatient.Province = headOfHouse.Province;
                            _content[lineOffset].Second = newPatient.Province;

                            // PHONE NUMBER
                            lineOffset++;
                            newPatient.PhoneNum = headOfHouse.PhoneNum;
                            _content[lineOffset].Second = newPatient.PhoneNum;

                            Container.DisplayContent(_content, 0, _selectedInputField, MenuCodes.PATIENTS, "Patients", Description);

                        }
                        // the head of house was either not set or doesnt exist therefore user has to enter rest of information
                        else
                        {
                            // ADDRESS LINE 1
                            lineOffset++;
                            if (newPatient.AddressLine1 == null) { newPatient.AddressLine1 = _content[lineOffset].Second; }
                            if (newPatient.AddressLine1 != _content[lineOffset].Second.ToUpper() || newPatient.AddressLine1 == "")
                            {
                                validInfo = PrintErrorOnLine(pInitialPosition, "Address error", lineOffset, errorOffset);
                            }

                            // ADDRESS LINE 2
                            lineOffset++;
                            if (newPatient.AddressLine2 == null) { newPatient.AddressLine2 = _content[lineOffset].Second; }
                            if ((newPatient.AddressLine2 != _content[lineOffset].Second) && _content[lineOffset].Second != "")
                            {
                                validInfo = PrintErrorOnLine(pInitialPosition, "Address error", lineOffset, errorOffset);
                            }

                            // CITY LINE
                            lineOffset++;
                            if (newPatient.City == null) { newPatient.City = _content[lineOffset].Second; }
                            if (newPatient.City != _content[lineOffset].Second || newPatient.City == "")
                            {
                                validInfo = PrintErrorOnLine(pInitialPosition, "City error", lineOffset, errorOffset);
                            }

                            // PROVINCE
                            lineOffset++;
                            if (newPatient.Province == null) { newPatient.Province = _content[lineOffset].Second; }
                            if (newPatient.Province != _content[lineOffset].Second || newPatient.Province == "")
                            {
                                validInfo = PrintErrorOnLine(pInitialPosition, "Province error", lineOffset, errorOffset);
                            }

                            // PHONE NUMBER
                            lineOffset++;
                            if (newPatient.PhoneNum == null) { newPatient.PhoneNum = _content[lineOffset].Second; }
                            if (newPatient.PhoneNum != _content[lineOffset].Second || newPatient.PhoneNum == "")
                            {
                                validInfo = PrintErrorOnLine(pInitialPosition, "Phone number error", lineOffset, errorOffset);
                            }
                        }

                        lineOffset = 0;

                        // set the rest of the patient information and display any error messages if there were any
                        // FIRST NAME
                        newPatient.FirstName = _content[lineOffset].Second;
                        if (newPatient.FirstName != _content[lineOffset].Second.ToUpper() || newPatient.FirstName == "")
                        {
                            validInfo = PrintErrorOnLine(pInitialPosition, "First name error", lineOffset, errorOffset);
                        }

                        // LAST NAME
                        lineOffset++;
                        newPatient.LastName = _content[lineOffset].Second;
                        if (newPatient.LastName != _content[lineOffset].Second.ToUpper() || newPatient.LastName == "")
                        {
                            validInfo = PrintErrorOnLine(pInitialPosition, "Last name error", lineOffset, errorOffset);
                        }

                        // HCN
                        lineOffset++;
                        newPatient.HCN = _content[lineOffset].Second;
                        if (newPatient.HCN != _content[lineOffset].Second.ToUpper() || newPatient.HCN == "")
                        {
                            validInfo = PrintErrorOnLine(pInitialPosition, "Health card number error", lineOffset, errorOffset);
                        }

                        // MIDDLE INITIAL
                        lineOffset++;
                        newPatient.MInitial = _content[lineOffset].Second;
                        if (newPatient.MInitial != _content[lineOffset].Second.ToUpper() && newPatient.MInitial == "")
                        {
                            validInfo = PrintErrorOnLine(pInitialPosition, "Middle initial error", lineOffset, errorOffset);
                        }

                        //  DATE OF BIRTH
                        lineOffset++;
                        newPatient.DateOfBirth = _content[lineOffset].Second;
                        if (newPatient.DateOfBirth != _content[lineOffset].Second.ToUpper() || newPatient.DateOfBirth == "")
                        {
                            validInfo = PrintErrorOnLine(pInitialPosition, "Date of Birth error", lineOffset, errorOffset);
                        }

                        // GENDER 
                        lineOffset++;
                        newPatient.Sex = _content[lineOffset].Second;
                        if (newPatient.Sex != _content[lineOffset].Second.ToUpper() || newPatient.Sex == "")
                        {
                            validInfo = PrintErrorOnLine(pInitialPosition, "Gender error", lineOffset, errorOffset);
                        }
                    }

                    // check if all the info was valid and the patient does not need to be returned
                    if (validInfo && !returnPatient)
                    {
                        // add the new patient to the database
                        demographics.AddNewPatient(newPatient);

                        // display the message the the patient was successfully added
                        Container.DisplayContent(new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("Patient successfully added!", "") }, 0, _selectedInputField, MenuCodes.PATIENTS, "Patients", Description);

                        // wait for the user to confirm that they have seen the message
                        Console.ReadKey();
                        break;
                    }
                    // check if all the info was valid and return the patient
                    else if(validInfo && returnPatient)
                    {
                        return newPatient; 
                    }
                }

                // change the menu based on the key pressed by the user
                if (ChangeInputField(userInput))
                {
                    // check if the user requested to return
                    if (_return) { break; }

                    // redisplay the content with the changes that were requested
                    Container.DisplayContent(_content, 0, _selectedInputField, MenuCodes.PATIENTS, "Patients", Description);
                }

            } while (!_return);

            // clear console before returning
            Console.Clear();
            return null;
        }


        /**
        * \brief <b>Brief Description</b> - PrintErrorOnLine <b><i>class method</i></b> - Display an error message on the same line as the input line
        * \details <b>Details</b>
        *
        * This takes in the position of the cursor, the error message that needs to be displayed, how pushed the message should be and in what column
        * 
        * \return <b>bool</b> - false because there was an error
        */
        private bool PrintErrorOnLine(Pair<int, int> pInitialPosition, string message, int lineOffset, int columnOffset)
        {
            // position the cursor to the right spot
            Console.CursorTop = pInitialPosition.First + lineOffset;
            Console.CursorLeft = Console.WindowWidth - message.Length - 2;

            // display the error message in red
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(message);
            Console.ForegroundColor = ConsoleColor.DarkGray;

            return false;
        }

        /**
        * \brief <b>Brief Description</b> - ChangeInputField <b><i>class method</i></b> - Changes the index of the selected field or cancels the current menu
        * \details <b>Details</b>
        *
        * This will take in the demographics library and whether it needs to return the added patient
        * 
        * \return <b>bool</b> true if there was a change, false otherwise
        */
        private bool ChangeInputField(ConsoleKey keyPressed)
        {
            switch (keyPressed)
            {
                // decrement the index if user pressed up key
                case (ConsoleKey.UpArrow):
                    if (_selectedInputField > 1)
                    {
                        _selectedInputField--;
                        _inputPosition.First--;
                        return true;
                    }
                    break;
                // increment the index if the user pressed the down key
                case (ConsoleKey.DownArrow):
                    if (_selectedInputField < _content.Count)
                    {
                        _selectedInputField++;
                        _inputPosition.First++;
                        return true;
                    }
                    break;
                // set the return as true because the user wants to return
                case (ConsoleKey.Escape):
                    _return = true;
                    return true;
            }

            return false;
        }
    }
}
