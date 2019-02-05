/**
 * \file SelectedAppointmentSlot.cs
*  \project INFO2180 - EMS System Term Project
*  \author The Char Stars - Tudor Lupu
*  \date 2018-12-4
*  \brief The calendar with the appointment slot selection
*  
*  This class is used throughout the program wherever there is
*  a form that requires the user to enter information
*/

using EMS_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS_Client.Functionality
{
    /** 
    * \class SelectAppointmentSlot
    *
    * \brief <b>Brief Description</b> - This class is used to select an appointment slot in the calendar
    * look like
    * 
    * The SelectAppointmentSlot class is used to display a clander which lets the user pick a date and 
    * appointment slot in order to schedule an appointment.
    * 
    * \author <i>The Char Stars - Tudor Lupu</i>
    */
    class SelectAppointmentSlot
    {
        #region private fields
        string[] _months = DatePicker._months;
        #endregion

        #region public fields
        #endregion

        public static string Description => "View Appointments";

        /**
        * \brief <b>Brief Description</b> - Get <b><i>class method</i></b> - main method used by outside methods to get the appointment
        * \details <b>Details</b>
        *
        * This takes in the scheduling and demographics libraries
        * 
        * \return <b>Pair<Day, int></b> - the appointment slot
        */
        public static Pair<Day, int> Get(Scheduling scheduling, Demographics demographics)
        {
            Console.CursorVisible = true;
            Container.DisplayContent(new List<KeyValuePair<string, string>>(), 0, -1, MenuCodes.SCHEDULING, "Scheduling", Description);
            DateTime date = DateTime.Today;

            while (true)
            { 
                // get the month from the user
                date = DatePicker.GetDate(scheduling, date, false);

                // check if the user didnt cancel the date selection
                if (date != new DateTime(0))
                {
                    do
                    {        
                        // get the day from the user
                        DateTime selectedDate = DatePicker.GetDate(scheduling, date, true);

                        // check if the user canceled the date selection
                        if (selectedDate == new DateTime(0)) { break; }                        

                        // get the selected day
                        Day selectedDay = scheduling.GetScheduleByDay(selectedDate);

                        // get a list of appointments on that day
                        List<Appointment> apptList = selectedDay.GetAppointments();

                        List<Pair<string, string>> apptContent = new List<Pair<string, string>>();

                        // loop through all appointments on that day
                        foreach (Appointment appointment in apptList)
                        {
                            string line = "";

                            // if the appointment slot is not taken, sets it to be displayed as (EMPTY)
                            if (appointment.AppointmentID == -1 && appointment.PatientID == -1) { line = "(EMPTY)"; }
                            else
                            {
                                // get information about the main patient
                                Patient mainPatient = demographics.GetPatientByID(appointment.PatientID);

                                // get information about the dependant of the main patient
                                Patient dependantPatient = demographics.GetPatientByID(appointment.DependantID);

                                // format the main patient information
                                line = string.Format("{0}, {1} - {2}", mainPatient.LastName, mainPatient.FirstName, mainPatient.HCN);

                                // if the patient has a dependant, adds it to the patient information line
                                if (dependantPatient != null)
                                {
                                    line += string.Format(" > {0}, {1} - {2}", dependantPatient.LastName, dependantPatient.FirstName, dependantPatient.HCN);
                                }
                            }

                            // adds the patient line to the appointment content that will be displayed
                            apptContent.Add(new Pair<string, string>(line, ""));
                        }

                        int selectedIndex = 1;

                        // display all appointments
                        Container.DisplayContent(apptContent, 0, selectedIndex, MenuCodes.SCHEDULING, "Scheduling", Description);
                        ConsoleKey keyPressed = new ConsoleKey();
                        Console.CursorVisible = false;

                        // loop until user cancels
                        do
                        {
                            keyPressed = Console.ReadKey(true).Key;
                            switch (keyPressed)
                            {
                                // down arrow pressed therefore go to appt below
                                case ConsoleKey.DownArrow:
                                    if (selectedIndex < apptContent.Count) { selectedIndex++; }
                                    break;
                                // up arrow pressed therefore go to appt above
                                case ConsoleKey.UpArrow:
                                    if (selectedIndex > 1) { selectedIndex--; }
                                    break;
                                // enter pressed therefore return the selected appointment
                                case ConsoleKey.Enter:
                                    return new Pair<Day, int>(selectedDay, selectedIndex - 1);
                            }

                            // re-display the appointment selection
                            Container.DisplayContent(apptContent, 0, selectedIndex, MenuCodes.SCHEDULING, "Scheduling", Description);

                        } while (keyPressed != ConsoleKey.Escape);

                    } while (true);
                }
                // exit because user canceled date selection
                else { break; }
            }

            // user canceled therefore return a blank selection
            return new Pair<Day, int>(null, -1);
        }

        /**
        * \brief <b>Brief Description</b> - Get <b><i>class method</i></b> - overload main method used by outside methods to get the appointment
        * \details <b>Details</b>
        *
        * This take sin the scheduling library, the demographics library, the date when an appointment should start, the selected menu, the selected option
        * in that menu and the title of the menu
        * 
        * \return <b>Pair<Day, int></b> - the appointment slot
        */
        public static Pair<Day, int> Get(Scheduling scheduling, Demographics demographics, DateTime startDate, int selectedMenu, MenuCodes menu, string title)
        {
            Console.CursorVisible = true;
            Container.DisplayContent(new List<KeyValuePair<string, string>>(), selectedMenu, -1, menu, title, Description);
            DateTime date = startDate;

            // check to make sure the date passed in is a valid one
            if (date != new DateTime(0))
            {
                do
                {
                    // get a date starting at the date passed in as a parameter
                    date = DatePicker.GetDate(scheduling, date, true);

                    // check if the user canceled the date selection process
                    if (date == new DateTime(0)) { break; }

                    // get the schedule for that day
                    Day selectedDay = scheduling.GetScheduleByDay(date);

                    // get a list of appointments for that day
                    List<Appointment> apptList = selectedDay.GetAppointments();
                    List<Pair<string, string>> apptContent = new List<Pair<string, string>>();

                    // loop through all appointments on that day
                    foreach (Appointment appointment in apptList)
                    {
                        string line = "";

                        // if the appointment slot is not taken, sets it to be displayed as (EMPTY)
                        if (appointment.AppointmentID == -1 && appointment.PatientID == -1) { line = "(EMPTY)"; }
                        else
                        {
                            // get information about the main patient
                            Patient mainPatient = demographics.GetPatientByID(appointment.PatientID);

                            // get information about the dependant of the main patient
                            Patient dependantPatient = demographics.GetPatientByID(appointment.DependantID);

                            // format the main patient information
                            line = string.Format("{0}, {1} - {2}", mainPatient.LastName, mainPatient.FirstName, mainPatient.HCN);


                            // if the patient has a dependant, adds it to the patient information line
                            if (dependantPatient != null)
                            {
                                line += string.Format(" > {0}, {1} - {2}", dependantPatient.LastName, dependantPatient.FirstName, dependantPatient.HCN);
                            }
                        }

                        // adds the patient line to the appointment content that will be displayed
                        apptContent.Add(new Pair<string, string>(line, ""));
                    }

                    int selectedIndex = 1;

                    // display all appointments
                    Container.DisplayContent(apptContent, selectedMenu, selectedIndex, menu, title, Description);
                    ConsoleKey keyPressed = new ConsoleKey();
                    Console.CursorVisible = false;

                    // loop until user cancels
                    do
                    {
                        keyPressed = Console.ReadKey(true).Key;
                        switch (keyPressed)
                        {
                            // down arrow pressed therefore go to appt below
                            case ConsoleKey.DownArrow:
                                if (selectedIndex < apptContent.Count) { selectedIndex++; }
                                break;
                            // up arrow pressed therefore go to appt above
                            case ConsoleKey.UpArrow:
                                if (selectedIndex > 1) { selectedIndex--; }
                                break;
                            // enter pressed therefore return the selected appointment
                            case ConsoleKey.Enter:
                                return new Pair<Day, int>(selectedDay, selectedIndex - 1);
                        }

                        // re-display the appointment selection
                        Container.DisplayContent(apptContent, selectedMenu, selectedIndex, menu, title, Description);

                    } while (keyPressed != ConsoleKey.Escape);

                } while (true);
            }

            // user canceled therefore return a blank selection
            return new Pair<Day, int>(null, -1);
        }
    }
}
