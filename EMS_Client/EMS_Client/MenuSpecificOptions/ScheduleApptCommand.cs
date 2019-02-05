/**
 * \file ScheduleApptCommand.cs
*  \project INFO2180 - EMS System Term Project
*  \author The Char Stars - Tudor Lupu
*  \date 2018-12-4
*  \brief The menu option to schedule an appointment.
*  
*  This class is the option in the scheduling menu to 
*  schedule an appointment.
*/

using EMS_Client.Functionality;
using EMS_Client.Interfaces;
using EMS_Client.MenuOptions;
using EMS_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS_Client.MenuSpecificOptions
{
    /** 
    * \class ScheduleApptCommand
    *
    * \brief <b>Brief Description</b> - This class is a sub menu of scheduling used to schedule an appointment
    * 
    * The ScheduleApptCommand class displays the calendar and allows the user to select an appointment slot 
    * 
    * \author <i>The Char Stars - Tudor Lupu</i>
    */
    class ScheduleApptCommand : IOption
    {
        #region private fields
        string[] _months = DatePicker._months;
        private enum ApptMenuOptions
        {
            ADDBILLINGCODE = 0,
            FLAGRECALL,
            ADDDEPENDANT,
            REMOVEDEPENDANT,
            CHANGEDATE,
            CANCELAPPT      
        }
        #endregion

        #region public fields
        public string Description => "View Appointments";
        #endregion

        /**
        * \brief <b>Brief Description</b> - Execute <b><i>class method</i></b> - An overload of the entry point into the appointment scheduling menu
        * \details <b>Details</b>
        *
        * This takes in the existing patient information, if it should return a patient and the demographics library
        * 
        * \return <b>void</b>
        */
        public void Execute(Scheduling scheduling, Demographics demographics, Billing billing)
        {
            while (true)
            {
                // let the user select an appointment
                Pair<Day, int> kvp = SelectAppointmentSlot.Get(scheduling, demographics);
                Day selectedDay = kvp.First;
                int slot = kvp.Second;

                // check if the selected day is valid
                if (selectedDay != null)
                {
                    // get appointment
                    Appointment SelectedAppointment = selectedDay.GetAppointments()[slot];

                    // get a date for the appointment
                    DateTime selectedDate = scheduling.GetDateFromDay(selectedDay);

                    // check if there is no patient scheduled for that slot
                    if (SelectedAppointment.AppointmentID == -1)
                    {
                        while (true)
                        {
                            // let user select a patient to book for the appointment slot
                            Patient searchResult = GetPatient.Get(MenuCodes.SCHEDULING, "Scheduling", 0);

                            // check if user did not cancel during patient selection
                            if (searchResult != null)
                            {
                                // check if appoint could be scheduled successfully
                                if (scheduling.ScheduleAppointment(new Appointment(searchResult.PatientID, -1, 0), selectedDate, slot))
                                {
                                    Container.DisplayContent(new List<Pair<string, string>>() { { new Pair<string, string>("Appointment scheduled successfully!", "") } },
                                        0, 0, MenuCodes.SCHEDULING, "Scheduling", Description);
                                }
                                else
                                {
                                    Container.DisplayContent(new List<Pair<string, string>>() { { new Pair<string, string>("An error was encountered while scheduling appointment.", "") } },
                                        0, 0, MenuCodes.SCHEDULING, "Scheduling", Description);
                                }

                                Console.ReadKey();
                            }
                            else { break; }
                        }
                    }
                    // appointment slot is taken
                    else
                    {
                        const int BUTTONCOUNT = 6;
                        int selectedButton = 0;
                        Patient scheduledPatient, scheduledDependant;

                        while (selectedButton != -1)
                        {
                            // get the patient scheduled for that appointment slot and its dependant
                            scheduledPatient = demographics.GetPatientByID(SelectedAppointment.PatientID);
                            scheduledDependant = demographics.GetPatientByID(SelectedAppointment.DependantID);

                            List<KeyValuePair<string, string>> content = new List<KeyValuePair<string, string>>
                            {
                                // add patient info
                                new KeyValuePair<string, string>("", ""),
                                new KeyValuePair<string, string>("Patient: ", scheduledPatient.ToString())
                            };

                            // check if patient has dependant
                            if (scheduledDependant != null)
                            {
                                // append dependant info to patient info
                                content.Add(new KeyValuePair<string, string>("Dependant: ", scheduledDependant.ToString()));
                            }

                            // add some space
                            content.Add(new KeyValuePair<string, string>("", ""));

                            int initialButtonIndex = content.Count + 1;

                            // add the buttons for the scheduled appointment
                            content.Add(new KeyValuePair<string, string>(" >> ADD BILLING CODE << ", ""));
                            content.Add(new KeyValuePair<string, string>(" >> FLAG FOR RECALL << ", ""));
                            content.Add(new KeyValuePair<string, string>((scheduledDependant != null) ? " >> CHANGE DEPENDANT << " : " >> ADD DEPENDANT << ", ""));
                            content.Add(new KeyValuePair<string, string>(" >> REMOVE DEPEDANT << ", ""));
                            content.Add(new KeyValuePair<string, string>(" >> CHANGE DATE << ", ""));
                            content.Add(new KeyValuePair<string, string>(" >> CANCEL APPOINTMENT << ", ""));

                            selectedButton = initialButtonIndex;

                            // display the content of the content with the buttons
                            Container.DisplayContent(content, 0, initialButtonIndex, MenuCodes.SCHEDULING, "Scheduling", Description);
                            selectedButton = ScheduledPatientMenu(content, BUTTONCOUNT, initialButtonIndex);

                            switch ((ApptMenuOptions)(selectedButton - initialButtonIndex))
                            {
                                // ADD BILLING CODES
                                case ApptMenuOptions.ADDBILLINGCODE:
                                    int returnStatus = 0;

                                    Patient patientToBill = scheduledPatient;

                                    // check if patient has dependant to bill
                                    if (SelectedAppointment.DependantID != -1)
                                    {
                                        // show depandat as option to bill
                                        patientToBill = GetPatient.BrowseRecords(new List<Patient>()
                                    { demographics.GetPatientByID(SelectedAppointment.PatientID), demographics.GetPatientByID(SelectedAppointment.DependantID) }, MenuCodes.SCHEDULING, Description, 0);
                                    }

                                    // if patient/dependant exists
                                    if (patientToBill != null)
                                    {
                                        // add billing codes until an invalid one is entered
                                        while ((returnStatus = AddBillingCode(SelectedAppointment.AppointmentID.ToString(), patientToBill.PatientID.ToString(), billing)) != -1)
                                        {
                                            if (returnStatus == 1)
                                            {
                                                Container.DisplayContent(new List<Pair<string, string>>() { { new Pair<string, string>("Billing code were entered successfuly!", "") } },
                                                0, selectedButton, MenuCodes.SCHEDULING, "Scheduling", Description);
                                            }
                                            else if (returnStatus == 0)
                                            {
                                                Container.DisplayContent(new List<Pair<string, string>>() { { new Pair<string, string>("Invalid billing code.", "") } },
                                                0, selectedButton, MenuCodes.SCHEDULING, "Scheduling", Description);
                                            }

                                            Console.ReadKey();
                                        }
                                    }

                                    break;
                                // FLAG FOR RECALL
                                case ApptMenuOptions.FLAGRECALL:
                                    Container.DisplayContent(new List<Pair<string, string>>() { { new Pair<string, string>("Enter the new recall flag:", "") } },
                                                0, selectedButton, MenuCodes.SCHEDULING, "Scheduling", Description);

                                    Console.CursorTop = 6;
                                    Console.CursorLeft = 55;

                                    // get the recall flag
                                    Pair<InputRetCode, string> inputReturn = Input.GetInput(SelectedAppointment.RecallFlag.ToString(), 2, InputType.Ints);

                                    // if the user saved their flag instead of exiting
                                    if ((inputReturn.First & InputRetCode.SAVE) != 0)
                                    {
                                        // print status update on whether the flag was changed
                                        if (scheduling.UpdateAppointmentInfo(SelectedAppointment.AppointmentID, int.Parse(inputReturn.Second)))
                                        {
                                            Container.DisplayContent(new List<Pair<string, string>>() { { new Pair<string, string>("Recall flag updated!", "") } },
                                                0, selectedButton, MenuCodes.SCHEDULING, "Scheduling", Description);
                                        }
                                        else
                                        {
                                            Container.DisplayContent(new List<Pair<string, string>>() { { new Pair<string, string>("Recall flag could not be updated.", "") } },
                                                0, selectedButton, MenuCodes.SCHEDULING, "Scheduling", Description);
                                        }

                                        Console.ReadKey();
                                    }

                                    break;

                                // ADD DEPENDANT
                                case ApptMenuOptions.ADDDEPENDANT:
                                    // get the patient to add dependant to
                                    Patient dependant = GetPatient.Get(MenuCodes.SCHEDULING, "Scheduling", 0, SelectedAppointment.PatientID);

                                    // check if user did not exit during patient search
                                    if (dependant != null)
                                    {
                                        // add dependant to patient
                                        SelectedAppointment.DependantID = dependant.PatientID;
                                        scheduling.UpdateAppointmentInfo(SelectedAppointment.AppointmentID, SelectedAppointment.PatientID,
                                            SelectedAppointment.DependantID, SelectedAppointment.RecallFlag);

                                        Container.DisplayContent(new List<Pair<string, string>>() { { new Pair<string, string>("Dependant successfully added!", "") } },
                                            0, selectedButton, MenuCodes.SCHEDULING, "Scheduling", Description);
                                        Console.ReadKey();
                                    }
                                    break;

                                // REMOVE DEPENDANT
                                case ApptMenuOptions.REMOVEDEPENDANT:
                                    // check if appointment has dependant associated
                                    if (SelectedAppointment.DependantID == -1)
                                    {
                                        Container.DisplayContent(new List<Pair<string, string>>() { { new Pair<string, string>("Appointment has no dependant associated.", "") } },
                                        0, -1, MenuCodes.SCHEDULING, "Scheduling", Description);
                                    }
                                    else
                                    {
                                        // remove dependant
                                        SelectedAppointment.DependantID = -1;
                                        scheduling.UpdateAppointmentInfo(SelectedAppointment.AppointmentID, SelectedAppointment.PatientID,
                                            SelectedAppointment.DependantID, SelectedAppointment.RecallFlag);

                                        Container.DisplayContent(new List<Pair<string, string>>() { { new Pair<string, string>("Dependant successfully removed!", "") } },
                                            0, -1, MenuCodes.SCHEDULING, "Scheduling", Description);
                                    }

                                    Console.ReadKey();
                                    break;
                                // CHANGE APPT DATE
                                case ApptMenuOptions.CHANGEDATE:
                                    // get the new appointment slot
                                    Pair<Day, int> newAppointmentSlot = SelectAppointmentSlot.Get(scheduling, demographics);

                                    // check if user did not cancel during appointment selection
                                    if (newAppointmentSlot.First != null)
                                    {
                                        Appointment newSelectedAppointment = selectedDay.GetAppointments()[newAppointmentSlot.Second];
                                        DateTime newSelectedDate = scheduling.GetDateFromDay(newAppointmentSlot.First);

                                        // check if appointent could be scheduled to new date
                                        if (scheduling.UpdateAppointmentDate(newSelectedDate, newAppointmentSlot.Second, SelectedAppointment.AppointmentID))
                                        {
                                            Container.DisplayContent(new List<Pair<string, string>>() { { new Pair<string, string>("Appointment date successfully changed!", "") } },
                                                0, -1, MenuCodes.SCHEDULING, "Scheduling", Description);
                                        }
                                        else
                                        {
                                            Container.DisplayContent(new List<Pair<string, string>>() { { new Pair<string, string>("Appointment date could not be changed.", "") } },
                                                0, -1, MenuCodes.SCHEDULING, "Scheduling", Description);
                                        }

                                        Console.ReadKey();
                                    }
                                    break;
                                // CANCEL APPT
                                case ApptMenuOptions.CANCELAPPT:
                                    // attempt to cancel appointment
                                    if (scheduling.CancelAppointment(SelectedAppointment.AppointmentID))
                                    {
                                        Container.DisplayContent(new List<Pair<string, string>>() { { new Pair<string, string>("Appointment successfully canceled!", "") } },
                                                0, -1, MenuCodes.SCHEDULING, "Scheduling", Description);
                                    }
                                    else
                                    {
                                        Container.DisplayContent(new List<Pair<string, string>>() { { new Pair<string, string>("Appointment could not be canceled.", "") } },
                                                0, -1, MenuCodes.SCHEDULING, "Scheduling", Description);
                                    }

                                    Console.ReadKey();

                                    break;
                            }
                        }
                    }
                }
                else { break; }
            }
        }

        /**
        * \brief <b>Brief Description</b> - AddBillingCode <b><i>class method</i></b> - This method adds a billing code to a patient
        * \details <b>Details</b>
        *
        * This takes in the appointment ID, the id of the patient and the billing library
        * 
        * \return <b>int</b> - status flag
        */
        private int AddBillingCode(string aptID, string patientID, Billing billing)
        {
            // ask for a billing code
            Container.DisplayContent(new List<Pair<string, string>>() { { new Pair<string, string>("Enter the billing code: ", "")} },
                                0, 1, MenuCodes.SCHEDULING, "Scheduling", Description);

            // set cursor
            Console.CursorTop = 6;
            Console.CursorLeft = 55;

            // ask the user to enter a billing code
            Pair<InputRetCode, string> inputReturn = Input.GetInput("", 4, InputType.Ints | InputType.Strings);

            // check if the user saved the billing code
            if ((inputReturn.First & InputRetCode.SAVE) != 0 &&
                inputReturn.Second.Length == 4)
            {
                // add billing code to record
                if (billing.AddNewRecord(aptID, patientID, inputReturn.Second))
                {
                    return 1;
                }
                return 0;
            }

            return -1;
        }

        /**
        * \brief <b>Brief Description</b> - ScheduledPatientMenu <b><i>class method</i></b> - This method controls the index in the patient scheduling menu
        * \details <b>Details</b>
        *
        * This takes in the content of the big container, how many buttons there are and which one is the selected button
        * 
        * \return <b>void</b>
        */
        private int ScheduledPatientMenu(List<KeyValuePair<string, string>> content, int buttonCount, int selectedButton = 13)
        {
            ConsoleKey keyPressed = new ConsoleKey();
            int currentSelectedButton = selectedButton;

            do
            {
                keyPressed = Console.ReadKey().Key;
                switch (keyPressed)
                {
                    // decrement index when up arrow is pressed
                    case ConsoleKey.UpArrow:
                        if (currentSelectedButton  > selectedButton ) { currentSelectedButton--; }
                        break;
                    // increment index when down arrow is pressed
                    case ConsoleKey.DownArrow:
                        if (currentSelectedButton < selectedButton + buttonCount - 1) { currentSelectedButton++; }
                        break;
                    // return the selected button when enter is pressed
                    case ConsoleKey.Enter:
                        return currentSelectedButton;
                }

                // re-display menu
                Container.DisplayContent(content,
                                0, currentSelectedButton, MenuCodes.SCHEDULING, "Scheduling", Description);

            } while (keyPressed != ConsoleKey.Escape);

            return -1;
        }
    }
}
