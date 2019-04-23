/**
*  \file Scheduling.cs
*  \project INFO2180 - EMS System Term Project
*  \author The Char Stars - Alex Kozak
*  \date 2018-11-16
*  \brief Scheduling library class and main interface
*  
*  The functions in this file are used to setup the Scheduling Class in the Scheduling library. See class 
*  header comment for more information on the contents of this file
*/

using System;
using System.Collections.Generic;

/** 
* \namespace EMS_Library
*
* \brief <b>Brief Description</b> - This namespace holds all classes for use within the EMS_Client program
*
* \author <i>The Char Stars - Alex Kozak</i>
*/
namespace EMS_Library
{
    /** 
    * \class Scheduling
    *
    * \brief <b>Brief Description</b> - This class is meant to find, schedule and update appointments.
    * 
    * The Scheduling class is able to access all all weeks and view each appointment slot. When accessing appointment slots
    * it can schedule an appointment if not there or update if there already.
    * 
    * <b>NOTE:</b> Being able to schedule an appointment for a recall will be part of the the updating functionality.
    *
    * \author <i>The Char Stars - Alex Kozak</i>
    */
    public class Scheduling
    {
        public const string DATE_FORMAT = "yyyy-MM-dd";
        public const int DEFAULT_ID = -1;

        public Dictionary<int, Week> dSchedule = new Dictionary<int, Week>();                      /**< The dictionary of weeks representing the entire schedule of the database*/
        private Dictionary<int, Appointment> dAppointments = new Dictionary<int, Appointment>();    /**< The dictionary of all appointemtns in the database, scheduled and unscheduled*/

        /**
        * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - Get the week information from the database.
        * \details <b>Details</b>
        *
        * This method will access the database in order to get all the weeks with the appointments.
        * 
        * \param f - <b>FileIO</b> - The object to open the database file
        * 
        * <exception cref="ArgumentNullException">Exception can be thrown if ConvertTableToDictionary returns a null value, confirm non-null value.</exception>
        * 
        * \return none -<b>void</b> - this method returns nothing        
        */
        public Scheduling()
        {
            dAppointments = GetAppointmentsFromDatabase();
            dSchedule = GetScheduleFromDatabase();

            //DateTime dateTime = new DateTime(2018, 1, 1);
            //if (dSchedule.Count < 104) { ScheduleAppointment(new Appointment(-1, -1, -1, -1), dateTime, 0); }
            //for (int i = dSchedule.Count; i < 104; i++) UpdateAppointmentDate(dateTime.AddDays(i * 7), 0, 0);
        }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - Get all appointments from the database.
        * \details <b>Details</b>
        *
        * This method will access the database in order to get all the appointments.
        * 
        * \return none -<b>Dictionary<int, Appointment></b> - this method returns the dictionary of appointments
        */
        public static Dictionary<int, Appointment> GetAppointmentsFromDatabase()
        {
            Dictionary<int, Appointment> dApps = new Dictionary<int, Appointment>();
            Dictionary<string, string[]> dAppointmentBase = FileIO.ConvertTableToDictionary(FileIO.GetDataTable(FileIO.TableNames.Appointments));
            foreach (string appointmentID in dAppointmentBase.Keys) { if (!dApps.ContainsKey(Int32.Parse(appointmentID))) dApps.Add(Int32.Parse(appointmentID), new Appointment(dAppointmentBase[appointmentID])); }
            Logging.Log("Scheduling", "GetAppointmentsFromDatabase", string.Format("Received {0} appointments from the database.", dApps.Count));
            return dApps;
        }

        public void CheckInAppointment(int appointmentID)
        {
            dAppointments[appointmentID].CheckIn();
            SaveAppointmentsToDatabase();
        }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - Get all weeks from the database.
        * \details <b>Details</b>
        *
        * This method will access the database in order to get all the weeks.
        * 
        * \return none -<b>Dictionary<int, Week></b> - this method returns the dictionary of weeks
        */
        public static Dictionary<int, Week> GetScheduleFromDatabase()
        {
            Dictionary<int, Week> dSched = new Dictionary<int, Week>();
            Dictionary<string, string[]> dScheduleBase = FileIO.ConvertTableToDictionary(FileIO.GetDataTable(FileIO.TableNames.Schedule));
            foreach (string weekID in dScheduleBase.Keys) { dSched.Add(Int32.Parse(weekID), new Week(dScheduleBase[weekID])); }
            Logging.Log("Scheduling", "GetScheduleFromDatabase", string.Format("Received {0} weeks from the database.", dSched.Count));
            return dSched;
        }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - save all apointments
        * \details <b>Details</b>
        *
        * This method saves the appointments to the database
        * 
        * \return none - <b>VOID</b> - this method returns nothing
        */
        private void SaveAppointmentsToDatabase()
        {
            Dictionary<string, string[]> dSubmit = new Dictionary<string, string[]>();
            foreach (Appointment appointment in dAppointments.Values) { dSubmit.Add(appointment.AppointmentID.ToString(), appointment.ToStringArray()); }
            Logging.Log("Scheduling", "SaveAppointmentsToDatabase", string.Format("Submitting {0} appointments to the database.", dSubmit.Count));
            FileIO.SetDataTable(dSubmit, FileIO.TableNames.Appointments);
        }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - save all weeks
        * \details <b>Details</b>
        *
        * This method saves the schedule weeks to the database
        * 
        * \return none - <b>VOID</b> - this method returns nothing
        */
        private void SaveScheduleToDatabase()
        {
            Dictionary<string, string[]> dSubmit = new Dictionary<string, string[]>();
            foreach (Week week in dSchedule.Values) { dSubmit.Add(week.WeekID.ToString(), week.ToStringArray()); }
            Logging.Log("Scheduling", "SaveScheduleToDatabase", string.Format("Submitting {0} weeks to the database.", dSubmit.Count));
            FileIO.SetDataTable(dSubmit, FileIO.TableNames.Schedule);
        }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - Get the list of days of the month with the appointments.
        * \details <b>Details</b>
        *
        * This method will pull the weeks of the month in order to get the appointments to 
        * 
        * \return <b>List<Day></b> - return a list of days in the month.
        */
        public List<Day> GetDaysByMonth(DateTime month)
        {
            List<Day> lDays = new List<Day>();
            foreach (Week week in dSchedule.Values)
            {
                if (week.StartDate.Month == month.Month || week.StartDate.AddDays(7).Month == month.Month)
                {
                    foreach (Day day in week.dDays.Values) { if (week.StartDate.AddDays((int)day.GetDayOfWeek()).Month == month.Month) lDays.Add(day); }
                }
            }
            Logging.Log("Scheduling", "GetDaysByMonth", string.Format("Returning {0} days for month {1}.", lDays.Count, month.Month));
            return lDays;
        }

        /**
         * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - Get the list of weeks of the month with the appointments.
         * \details <b>Details</b>
         *
         * This method will pull the weeks of the month in order to get the appointments to 
         * 
         * \return <b>List<Week></b> - return a list of Week object in the month.
         */
        public List<Week> GetWeeksByMonth(DateTime month)
        {
            List<Week> lWeeks = new List<Week>();
            foreach (Week week in dSchedule.Values)
            {
                if ((week.StartDate.Year == month.Year && week.StartDate.Month == month.Month) || (week.StartDate.AddDays(6).Year == month.Year && week.StartDate.AddDays(6).Month == month.Month))
                {
                    lWeeks.Add(week);
                }
            }
            Logging.Log("Scheduling", "GetWeeksByMonth", string.Format("Returning {0} weeks for month {1}.", lWeeks.Count, month.Month));
            return lWeeks;
        }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - Get all appointments in the given month
        * \details <b>Details</b>
        *
        * This method will pull all appointments in the month
        * 
        * \return <b>List<Appointment></b> - return a list of all appointments scheduled in the month
        */
        public List<Appointment> GetAppointmentsByMonth(DateTime month)
        {
            List<Appointment> lAppointments = new List<Appointment>();
            foreach (Week week in dSchedule.Values)
            {
                if ((week.StartDate.Month == month.Month || week.StartDate.AddDays(7).Month == month.Month) && week.StartDate.Year == month.Year)
                {
                    foreach (Day day in week.dDays.Values)
                    {
                        if (week.StartDate.AddDays((int)day.GetDayOfWeek()).Month == month.Month)
                        {
                            foreach (Appointment appointment in day.GetAppointments()) { if (appointment.AppointmentID != -1) lAppointments.Add(appointment); }
                        }
                    }
                }
            }
            Logging.Log("Scheduling", "GetAppointmentsByMonth", string.Format("Returning {0} appointments for month {1}.", lAppointments.Count, month.Month));
            return lAppointments;
        }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - Get a single week of appointments.
        * \details <b>Details</b>
        *
        * This method will pull a single week from the schedule
        * 
        * \return <b>Week</b> - return a week object
        */
        public Week GetScheduleByWeek(DateTime startOfWeek)
        {
            startOfWeek = startOfWeek.AddDays(-((int)startOfWeek.DayOfWeek));
            foreach (Week week in dSchedule.Values)
            {
                if (week.StartDate.Date == startOfWeek.Date)
                {
                    Logging.Log("Scheduling", "GetScheduleByWeek", string.Format("Returning Week of {0} (WeekID: {1}).", startOfWeek.Date.ToString(DATE_FORMAT), week.WeekID));
                    return week;
                }
            }
            Logging.Log("Scheduling", "GetScheduleByDay", string.Format("Week not in schedule, adding new week to tblSchedule (start date: {0}).", startOfWeek.Date.ToString(DATE_FORMAT)));
            FileIO.AddRecordToDataTable(new Week(startOfWeek).ToStringArray(), FileIO.TableNames.Schedule);
            return null;            
        }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - Get the day object for a given date.
        * \details <b>Details</b>
        *
        * This method will pull a single day object.
        * 
        * \return <b>Day</b> - return a Day object
        */
        public Day GetScheduleByDay(DateTime requestDate)
        {
            DateTime startOfWeek = requestDate.AddDays(-((int)requestDate.DayOfWeek));
            foreach (Week week in dSchedule.Values)
            {
                if (week.StartDate.Date == startOfWeek.Date)
                {
                    Logging.Log("Scheduling", "GetScheduleByDay", string.Format("Returning Day {0} (WeekID: {1}).", requestDate.Date.ToString(DATE_FORMAT), week.WeekID));
                    return week.dDays[requestDate.DayOfWeek];
                }
            }
            Logging.Log("Scheduling", "GetScheduleByDay", string.Format("Day given is not in the schedule, adding new week to tblSchedule (start date: {0}).", startOfWeek.Date.ToString(DATE_FORMAT)));
            FileIO.AddRecordToDataTable(new Week(requestDate.AddDays(-((int)requestDate.DayOfWeek))).ToStringArray(), FileIO.TableNames.Schedule);            
            dSchedule = GetScheduleFromDatabase();
            return GetScheduleByDay(requestDate);
        }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - Get the day object for a given date.
        * \details <b>Details</b>
        *
        * This method will pull the appointments for a single day.
        * 
        * \return <b>Dat</b> - return a Day
        */
        public Day GetScheduleByDay(int year, int month, int day) { return GetScheduleByDay(new DateTime(year, month, day)); }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - Get the scheduled appointment by ID.
        * \details <b>Details</b>
        *
        * This method will pull the appointment by ID.
        * 
        * \return <b>Dat</b> - return an Appointment object
        */
        public Appointment GetScheduledAppointmentByAppointmentID(int appointmentID)
        {
            foreach (Week week in dSchedule.Values)
            {
                foreach (Day day in week.dDays.Values)
                {
                    foreach (Appointment appointment in day.GetAppointments())
                    {
                        if (appointment.AppointmentID == appointmentID)
                        {
                            Logging.Log("Scheduling", "GetScheduledAppointmentByAppointmentID", string.Format("Appointment (ID: {0}) found!", appointmentID));
                            return appointment;
                        }
                    }
                }
            }
            Logging.Log("Scheduling", "GetScheduledAppointmentByAppointmentID", string.Format("Appointment (ID: {0}) not found, returning null.", appointmentID));
            return new Appointment();
        }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - Get the Day object containing the appointmentID
        * \details <b>Details</b>
        *
        * This method will pull day containing the given appointmentID
        * 
        * \return <b>Day</b> - return a Day object
        */
        public Day GetDayByAppointmentID(int appointmentID)
        {
            foreach (Week week in dSchedule.Values)
            {
                foreach (Day day in week.dDays.Values)
                {
                    foreach (Appointment appointment in day.GetAppointments())
                    {
                        if (appointment.AppointmentID == appointmentID)
                        {
                            Logging.Log("Scheduling", "GetDayByAppointmentID", string.Format("Appointment (ID: {0}) found on Day (WeekID: {1}, Date: {2})", appointmentID, day.WeekID, GetDateFromDay(day).Date.ToString("yyy-MM-dd")));
                            return day;
                        }
                    }
                }
            }
            Logging.Log("Scheduling", "GetDayByAppointmentID", string.Format("Appointment (ID: {0}) not found, returning null.", appointmentID));
            return null;
        }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - Get the DateTime from a Day object
        * \details <b>Details</b>
        *
        * This method generates the DateTime of the given Date object.
        * 
        * \return <b>DateTime</b> - return a DateTime object
        */
        public DateTime GetDateFromDay(Day day)
        {
            DateTime dateTime = new DateTime(0);
            if (day != null && dSchedule.ContainsKey(day.WeekID)) { dateTime = dSchedule[day.WeekID].StartDate.AddDays((int)day.GetDayOfWeek()).Date; }
            return dateTime;
        }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - Get the DateTime from an appointmentID
        * \details <b>Details</b>
        *
        * This method generates the DateTime for the appointmentID
        * 
        * \return <b>DateTime</b> - return a DateTime object
        */
        public DateTime GetDateByAppointmentID(int appointmentID)
        {            
            return GetDateFromDay(GetDayByAppointmentID(appointmentID));
        }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - Search for an appointment by ID of the patient
        * \details <b>Details</b>
        *
        * This method will allow to search for an appointment by the appointmentID
        * 
        * <exception cref="IndexOutOfRangeException">Exception can be thrown if ID is used inapropriately, confirm within range.</exception>
        * 
        * \return <b>Appointment</b> - return the appointment object with the appointment information
        */
        public Appointment SearchForAppointment(int appointmentID)
        {
            if (dAppointments.ContainsKey(appointmentID))
            {
                Logging.Log("Scheduling", "SearchForAppointment", string.Format("Appointment (ID: {0}) found!", appointmentID));
                return dAppointments[appointmentID];
            }
            else
            {
                Logging.Log("Scheduling", "SearchForAppointment", string.Format("Appointment (ID: {0}) not found. Returning new appointment.", appointmentID));
                return new Appointment();
            }
        }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - returns all flagged appointments
        * \details <b>Details</b>
        *
        * This method searches the schedule for all appointments which are flagged for recall, and returns them as a list
        * 
        * <exception cref="IndexOutOfRangeException">Exception can be thrown if ID is used inapropriately, confirm within range.</exception>
        * 
        * \return <b>List<Appointment></Appointment></b> - return all flagged appointments
        */
        public List<Appointment> GetFlaggedAppointments()
        {
            List<Appointment> lAppointments = new List<Appointment>();
            foreach (Appointment appointment in dAppointments.Values) { if (appointment.RecallFlag > 0) lAppointments.Add(appointment); }
            Logging.Log("Scheduling", "GetFlaggedAppointments", string.Format("Returning {0} flagged appointments", lAppointments.Count));
            return lAppointments;
        }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - Changes the date and slot of the appointment to a new one
        * \details <b>Details</b>
        *
        * This method removes the initial and reschedules it with the same info to the new given place.
        * 
        * \return <b>bool</b> - if successfully rescheduled or not
        */
        public bool UpdateAppointmentDate(DateTime newDate, int newTimeSlot, int appointmentID)
        {
            Logging.Log("Scheduling", "UpdateAppointmentDate", string.Format("Attempting to move appointment (ID: {0}) to {1} in slot {2}", appointmentID, newDate.ToString(DATE_FORMAT),newTimeSlot));
            bool updateSuccessful = true;

            if (SearchForAppointment(appointmentID).AppointmentID == DEFAULT_ID)
            {
                Logging.Log("Scheduling", "UpdateAppointmentDate", string.Format("Appointment (ID: {0}) not found", appointmentID));
                updateSuccessful = false;
            }
            else
            {
                Day previousDay = GetDayByAppointmentID(appointmentID);
                if (ScheduleAppointment(dAppointments[appointmentID], newDate, newTimeSlot))
                {
                    if (previousDay != null) { dSchedule[previousDay.WeekID].dDays[previousDay.GetDayOfWeek()].DeleteAppointment(previousDay.GetTimeslotByAppointmentID(appointmentID), appointmentID); }
                    SaveScheduleToDatabase();
                    Logging.Log("Scheduling", "UpdateAppointmentDate", string.Format("Appointment (ID: {0}) found. Schedule successfully changed.", appointmentID));
                }
                else
                {
                    Logging.Log("Scheduling", "UpdateAppointmentDate", string.Format("Appointment (ID: {0}) found, but the destination timeslot was full. Schedule remains unchanged.", appointmentID));
                    updateSuccessful = false;
                }
            }
            return updateSuccessful;
        }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - Cancels an appointment with a given id
        * \details <b>Details</b>
        *
        * This method cancles the appointment given a valid appointmentID.
        * 
        * \return <b>bool</b> - if successfully removed or not
        */
        public bool CancelAppointment(int appointmentID, int timeSlot)
        {
            Logging.Log("Scheduling", "CancelAppointment", string.Format("Cancelling appointment (ID: {0})", appointmentID));
            bool updateSuccessful = true;

            if (SearchForAppointment(appointmentID).AppointmentID == DEFAULT_ID)
            {
                Logging.Log("Scheduling", "CancelAppointment", string.Format("Appointment (ID: {0}) not found", appointmentID));
                updateSuccessful = false;
            }
            else
            {
                updateSuccessful = GetDayByAppointmentID(appointmentID).DeleteAppointment(appointmentID, timeSlot, true);
                SaveScheduleToDatabase();
                if (updateSuccessful)
                {
                    Logging.Log("Scheduling", "CancelAppointment", string.Format("Appointment (ID: {0}) successfully deleted", appointmentID));
                }
                else { Logging.Log("Scheduling", "CancelAppointment", string.Format("Appointment (ID: {0}) not scheduled", appointmentID)); }
            }
            return updateSuccessful;
        }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - updates an appointment's internal information
        * \details <b>Details</b>
        *
        * This method updates the appointment to contin the new information
        * 
        * \return <b>bool</b> - if successfully updates or not
        */
        public bool UpdateAppointmentInfo(int appointmentID, int patientID, int dependantID, int recallFlag)
        {
            Logging.Log("Scheduling", "UpdateAppointmentInfo", string.Format("Updating Appointment (ID: {0}) to contain these values: (PatientID: {1}, DependantID: {2}, RecallFlag: {3})", appointmentID, patientID, dependantID, recallFlag));
            bool returnValue = true;

            if (SearchForAppointment(appointmentID).AppointmentID == DEFAULT_ID)
            {
                Logging.Log("Scheduling", "UpdateAppointmentInfo", string.Format("Appointment (ID: {0}) does not exist.", appointmentID));
                returnValue = false;
            }
            else
            {
                GetScheduledAppointmentByAppointmentID(appointmentID).UpdateAppointment(patientID, dependantID, recallFlag);
                dAppointments[appointmentID].PatientID = patientID;
                dAppointments[appointmentID].DependantID = dependantID;
                dAppointments[appointmentID].RecallFlag = recallFlag;
                SaveAppointmentsToDatabase();
            }
            return returnValue;
        }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - updates an appointment's internal information, only the recall flag
        * \details <b>Details</b>
        *
        * This method updates the appointment to contin the new recall flag
        * 
        * \return <b>bool</b> - if successfully updates or not
        */
        public bool UpdateAppointmentInfo(int appointmentID, int recallFlag)
        {
            Logging.Log("Scheduling", "UpdateAppointmentInfo", string.Format("Updating Appointment (ID: {0}) to contain these values: (RecallFlag: {1})", appointmentID, recallFlag));
            bool returnValue = true;
            Appointment a = SearchForAppointment(appointmentID);
            if (a.AppointmentID == DEFAULT_ID)
            {
                Logging.Log("Scheduling", "UpdateAppointmentInfo", string.Format("Appointment (ID: {0}) does not exist.", appointmentID));
                returnValue = false;              
            }
            else
            {
                dAppointments[appointmentID].RecallFlag = recallFlag;
                GetScheduledAppointmentByAppointmentID(appointmentID).UpdateAppointment(a.PatientID, a.DependantID, recallFlag);
                SaveAppointmentsToDatabase();
            }
            return returnValue;
        }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - Get all appointments for a patient.
        * \details <b>Details</b>
        *
        * This method returns a list of all appointments for a given patientID
        * 
        * \return <b>List<Appointment></Appointment></b> - return the list of appointments the patient is scheduled for
        */
        public List<Appointment> GetAllAppointmentsForPatient(int patientID)
        {
            Logging.Log("Scheduling", "GetAllAppointmentsForPatient", string.Format("Looking for all Appointments with patientID: {0}", patientID));
            List<Appointment> lAppointments = new List<Appointment>();
            if (patientID >= 0)
            {
                foreach (Week week in dSchedule.Values)
                {
                    foreach (Day day in week.dDays.Values)
                    {
                        foreach (Appointment appointment in day.GetAppointments()) { if (appointment.PatientID == patientID) lAppointments.Add(appointment); }
                    }
                }
                Logging.Log("Scheduling", "GetAllAppointmentsForPatient", string.Format("Found {0} Appointments with patientID: {1}", lAppointments.Count, patientID));
            }
            return lAppointments;
        }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - Schedule an appointment
        * \details <b>Details</b>
        *
        * This method will allow to schedule an appointment in a specific time slot of a specific day.
        * 
        * \return <b>Appointment</b> - return a code on whether the scheduling succeeded or failed.
        */
        public bool ScheduleAppointment(Appointment appointment, DateTime appointmentDate, int timeSlot)
        {
            if (appointment != null)
            {
                Logging.Log("Scheduling", "ScheduleAppointment", string.Format("Scheduling new Appointment (ID: {0}) for {1}, timeslot {2}", appointment.AppointmentID, appointmentDate.ToString(DATE_FORMAT), timeSlot));
                if (GetScheduleByDay(appointmentDate).AddAppointment(appointment, timeSlot))
                {
                    Logging.Log("Scheduling", "ScheduleAppointment", string.Format("Appointment (ID: {0}) scheduled successfully.", appointment.AppointmentID));
                    SaveScheduleToDatabase();
                    FileIO.AddRecordToDataTable(appointment.ToStringArray(), FileIO.TableNames.Appointments);
                    dAppointments = GetAppointmentsFromDatabase();
                    dSchedule = GetScheduleFromDatabase();
                    return true;
                }            
                Logging.Log("Scheduling", "ScheduleAppointment", string.Format("Appointment (ID: {0}) failed to schedule, timeslot full.", appointment.AppointmentID));
            }
            return false;
        }
    }
}