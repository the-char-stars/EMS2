/**
 * \file ScheduleRecallCommand.cs
*  \project INFO2180 - EMS System Term Project
*  \author The Char Stars - Tudor Lupu
*  \date 2018-12-4
*  \brief The menu option to schedule the recall appointments.
*  
*  This class is the option in the scheduling menu to 
*  schedule a recall appointment
*/

using EMS_Client.Functionality;
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
    * \class ScheduleRecallCommand
    *
    * \brief <b>Brief Description</b> - This class is a sub menu of scheduling used to reschedule a recall appointment
    * 
    * The ScheduleRecallCommand class displays all appointments that are set to recall and allows the user to select one 
    * in order to reschedule it.
    * 
    * \author <i>The Char Stars - Tudor Lupu</i>
    */
    class ScheduleRecallCommand : IOption
    {
        #region public fields
        public string Description => "Schedule Recall";
        #endregion

        /**
        * \brief <b>Brief Description</b> - Execute <b><i>class method</i></b> - An overload of the entry point into the scheduling of a recall menu
        * \details <b>Details</b>
        *
        * This takes in the existing patient information, if it should return a patient and the demographics library
        * 
        * \return <b>void</b>
        */
        public void Execute(Scheduling scheduling, Demographics demographics, Billing billing)
        {
            // get a list of all appointments that are flagged for recall
            List<Appointment> appointments = scheduling.GetFlaggedAppointments();
            List<Pair<string, string>> content = new List<Pair<string, string>>();

            // if there are any appointments flagged
            if (appointments.Count > 0)
            {
                // give menu to browse through those records and select an appointment
                Appointment selectedAppt = BrowseRecords(appointments, MenuCodes.SCHEDULING, "Scheduling", 1);

                // check if the user didnt cancel during record browse
                if (selectedAppt != null)
                {
                    // get the date of when the recall is allowed to start being scheduled
                    DateTime date = scheduling.GetDateByAppointmentID(selectedAppt.AppointmentID).AddDays(selectedAppt.RecallFlag * 7);

                    // display the content
                    Container.DisplayContent(content, 1, 1, MenuCodes.SCHEDULING, "Scheduling", Description);

                    // get a new selected appointment for the recall
                    Pair<Day, int> selectedRecallDate = SelectAppointmentSlot.Get(scheduling, demographics, date, 1, MenuCodes.SCHEDULING, Description);
                    Day selectedDay = selectedRecallDate.First;
                    int slot = selectedRecallDate.Second;

                    // check if the user didnt cancel during recall appointment scheduling
                    if (selectedDay != null)
                    {
                        // get the appointment slot based on the selected day
                        Appointment SelectedAppointment = selectedDay.GetAppointments()[slot];
                        DateTime selectedDate = scheduling.GetDateFromDay(selectedDay);

                        // check if appointment is not already taken
                        if (SelectedAppointment.AppointmentID == -1)
                        {
                            // get the patient from the selected appointment
                            Patient searchResult = demographics.GetPatientByID(selectedAppt.PatientID);

                            // check if patient was found
                            if (searchResult != null)
                            {
                                // schedule appointment and check if successful
                                if (scheduling.ScheduleAppointment(new Appointment(searchResult.PatientID, -1, 0), selectedDate, slot))
                                {
                                    // update the appointment information
                                    scheduling.UpdateAppointmentInfo(selectedAppt.AppointmentID, 0);

                                    Container.DisplayContent(new List<Pair<string, string>>() { { new Pair<string, string>("Appointment scheduled successfully!", "") } },
                                        1, 0, MenuCodes.SCHEDULING, "Scheduling", Description);
                                }
                                else
                                {
                                    Container.DisplayContent(new List<Pair<string, string>>() { { new Pair<string, string>("An error was encountered while scheduling appointment.", "") } },
                                        1, 0, MenuCodes.SCHEDULING, "Scheduling", Description);
                                }
                            }
                        }
                        // the appointment is already taken
                        else
                        {
                            Container.DisplayContent(new List<Pair<string, string>>() { { new Pair<string, string>("Appointment slot is already taken.", "") } },
                                        1, 0, MenuCodes.SCHEDULING, "Scheduling", Description);
                        }                        
                    }
                }
            }
            else
            {
                Container.DisplayContent(new List<Pair<string, string>>() { { new Pair<string, string>("No appointments flagged for recall.", "") } },
                                        1, 0, MenuCodes.SCHEDULING, "Scheduling", Description);
            }
            Console.ReadKey();
        }

        /**
        * \brief <b>Brief Description</b> - BrowseRecords <b><i>class method</i></b> - This method allows the browsing thorugh the found appointments
        * \details <b>Details</b>
        *
        * This takes in a list of the found appointments, the menu that should be displayed, the title of the menu and the index of the selected item in the menu
        * 
        * \return <b>void</b>
        */
        public Appointment BrowseRecords(List<Appointment> appointments, MenuCodes menu, string title, int selectedIndex)
        {
            int apptIndex = 1;
            ConsoleKey keyPressed = new ConsoleKey();
            List<Pair<string, string>> content = new List<Pair<string, string>>();

            // information to be displayed about the appointment
            string[] apptInfo = new string[4]
                    {
                        "Appointment ID: ",
                        "Patient ID: ",
                        "Dependant ID: ",
                        "Recall: "
                    };

            // loop through all appointments
            foreach (Appointment appt in appointments)
            {
                string line = "";

                // set appointment to empty if not set
                if (appt.AppointmentID == -1 && appt.PatientID == -1) { line = "(EMPTY)"; }
                else
                {   // display information of appointments that are not taken
                    for (int index = 0; index < 4; index++)
                    {
                        line += apptInfo[index] + appt.ToStringArray()[index].PadRight(5, ' ');
                    }
                }

                // add appointment information tho the content list
                content.Add(new Pair<string, string>(line, ""));
            }

            do
            {
                // display the content
                Container.DisplayContent(content, selectedIndex, apptIndex, menu, title, Description);

                keyPressed = Console.ReadKey(true).Key;

                switch (keyPressed)
                {
                    // increment index if arrow up pressed
                    case ConsoleKey.DownArrow:
                        if (apptIndex < appointments.Count) { apptIndex++; }
                        break;
                    // decrement index if arrow down pressed
                    case ConsoleKey.UpArrow:
                        if (apptIndex > 1) { apptIndex--; }
                        break;
                    // return selected appointment if enter pressed
                    case ConsoleKey.Enter:
                        return appointments[apptIndex - 1];
                }

            } while (keyPressed != ConsoleKey.Escape);

            return null;
        }
    }
}
