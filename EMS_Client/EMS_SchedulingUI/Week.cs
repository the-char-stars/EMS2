/**
*  \file Week.cs
*  \project INFO2180 - EMS System Term Project
*  \author The Char Stars - Alex Kozak
*  \date 2018-11-16
*  \brief Week class definition and functions
*  
*  The functions in this file are used to setup the Week Class in the Scheduling library. See class 
*  header comment for more information on the contents of this file
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS_Library
{
    /** 
   * \class Week
   *
   * \brief <b>Brief Description</b> - This class is meant to store a list of all the appointments for a specific week.
   * 
   * The Week class is able to store 34 AppointmentIDs in order to be able to access each Appointment object for the timeslots
   * of that week.
   *
   * \author <i>The Char Stars - Alex Kozak</i>
   */
    public class Week
    {
        public int WeekID;                  // unique WeekID
        public DateTime StartDate;          // The sunday of the week
        public string[] AppointmentList;    // 34 AppointmentIDs and a check value at the end "END"
        public List<Appointment> lAppointments = new List<Appointment>();
        public Dictionary<DayOfWeek, Day> dDays = new Dictionary<DayOfWeek, Day>();

        public const int WEEKDAY_TIME_SLOTS = 6;
        public const int WEEKEND_TIME_SLOTS = 2;
        public const string END_CHECK = "END";
        public const char DEFAULT_DELIMETER = ',';
        public const int TOTAL_TIME_SLOTS = WEEKDAY_TIME_SLOTS * 5 + WEEKEND_TIME_SLOTS * 2;

        /**
        * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - Convert a string array into the Week information
        * \details <b>Details</b>
        *
        * This method will take a string array containing the information for the week and converts it into a Week object
        * 
        * \param weekData - <b>string[]</b> - The object to log the actions to the log file
        * 
        * \return none - <b>void</b> - this method returns nothing        
        */
        public Week(string[] weekData)
        {
            if (weekData != null)
            {
                WeekID = (Int32.Parse(weekData[0]));
                DateTime.TryParse(weekData[1].ToString(), out StartDate);
                //StartDate = DateTime.ParseExact(weekData[1].ToString(), Scheduling.DATE_FORMAT, CultureInfo.InvariantCulture);
                AppointmentList = weekData[2].Split(DEFAULT_DELIMETER);
            }
            else
            {
                WeekID = -1;
                StartDate = new DateTime(0);
                AppointmentList = GetDefaultAppointmentListString().Split(DEFAULT_DELIMETER);
            }
            foreach (string appointmentID in AppointmentList)
            {
                if (appointmentID != END_CHECK)
                {
                    if (Int32.Parse(appointmentID) == -1) { lAppointments.Add(new Appointment()); }
                    else
                    {
                        Dictionary<int, Appointment> dapp = Scheduling.GetAppointmentsFromDatabase();
                        int apptID = Int32.Parse(appointmentID);
                        if (dapp.ContainsKey(apptID)) lAppointments.Add(dapp[apptID]);
                        else
                        {
                            lAppointments.Add(new Appointment());
                            Logging.Log("Week", "Constructor", string.Format("Schedule contains appointment with ID {0}. No associated appointment.", apptID));
                        }
                    }
                }
            }            

            if (AppointmentList[TOTAL_TIME_SLOTS] == END_CHECK)
            {
                int currentIndex = 0;
                foreach (DayOfWeek dayOfWeek in Enum.GetValues(typeof(DayOfWeek)))
                {
                    int numTimeSlots = WEEKDAY_TIME_SLOTS;
                    if (dayOfWeek == DayOfWeek.Sunday || dayOfWeek == DayOfWeek.Saturday) { numTimeSlots = WEEKEND_TIME_SLOTS; }
                    dDays.Add(dayOfWeek, new Day(dayOfWeek, lAppointments.GetRange(currentIndex, numTimeSlots), WeekID));
                    currentIndex += numTimeSlots;
                }
            }
            else
            {
                Logging.Log("Week", "Constructor", AppointmentList);
            }
        }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - generate a blank week given a DateTime object
        * \details <b>Details</b>
        *
        * This method will take a DateTime object and generate an empty week for that
        * 
        * \return none - <b>void</b> - this method returns nothing        
        */
        public Week(DateTime start)
        {
            WeekID = -1;
            StartDate = start;
            AppointmentList = GetDefaultAppointmentListString().Split(',');
            foreach (string apptId in AppointmentList) { if (apptId != END_CHECK) lAppointments.Add(new Appointment()); }

            if (AppointmentList[TOTAL_TIME_SLOTS] == END_CHECK)
            {
                int currentIndex = 0;
                foreach (DayOfWeek dayOfWeek in Enum.GetValues(typeof(DayOfWeek)))
                {
                    int numTimeSlots = WEEKDAY_TIME_SLOTS;
                    if (dayOfWeek == DayOfWeek.Sunday || dayOfWeek == DayOfWeek.Saturday) { numTimeSlots = WEEKEND_TIME_SLOTS; }
                    dDays.Add(dayOfWeek, new Day(dayOfWeek, lAppointments.GetRange(currentIndex, numTimeSlots), WeekID));
                    currentIndex += numTimeSlots;
                }
            }
            else
            {
                Logging.Log("Week", "Constructor", AppointmentList);
            }
        }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - gets number of free slots in the week
        * \details <b>Details</b>
        *
        * returns the number of free slots in the week
        * 
        * \return <b>int</b> - the number of free slots in the week.
        */
        public int NumberOfFreeSlots()
        {
            int i = 0;
            foreach (Appointment a in lAppointments) { if (a.AppointmentID == -1) i++; }
            return i;
        }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - Generates default string value for the week
        * \details <b>Details</b>
        *
        * returns the default week content string
        * 
        * \return <b>strnng</b> - the empty week string of -1 values
        */
        public static string GetDefaultAppointmentListString()
        {
            return string.Concat(Enumerable.Repeat(Scheduling.DEFAULT_ID.ToString() + DEFAULT_DELIMETER, TOTAL_TIME_SLOTS)) + END_CHECK;
        }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - Gets the string array representation 
        * \details <b>Details</b>
        *
        * Gets the string array representation of the Week object for saving to the database.
        * 
        * \return <b>string[]</b> - the string array
        */
        public string[] ToStringArray()
        {
            string scheduleString = "";
            foreach(Day day in dDays.Values) { scheduleString += day.ToScheduleString(); }
            scheduleString += END_CHECK;
            return new string[] { WeekID.ToString(), StartDate.ToString(Scheduling.DATE_FORMAT), scheduleString };
        }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - Gets the first available day in the week 
        * \details <b>Details</b>
        *
        * Gets the next day object with an available timeslot
        * 
        * \return <b>Day</b> - the free day
        */
        public Day GetFirstAvaiableDay()
        {
            foreach(Day day in dDays.Values)
            {
                if (day.HasFreeSlot())
                {
                    Logging.Log("Week", "GetFirstAvaiableDay", string.Format("Week (WeekID: {0}) has day {1} available ", WeekID, (int)day.GetDayOfWeek()));
                    return day;
                }
            }
            Logging.Log("Week", "GetFirstAvaiableDay", string.Format("Week (WeekID: {0}) has no available days. ", WeekID));
            return null;
        }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - Gets all available days
        * \details <b>Details</b>
        *
        * Gets all days in the week with available slots
        * 
        * \return <b>List<Day></b> - The list of free days
        */
        public List<Day> GetAllAvailableDays()
        {
            List<Day> lDays = new List<Day>();
            foreach (Day day in dDays.Values) { if (day.HasFreeSlot()) { lDays.Add(day); } }
            Logging.Log("Week", "GetAllAvailableDays", string.Format("Week (WeekID: {0}) has {1} available days. ", WeekID, lDays.Count));
            return lDays;
        }
    }
}