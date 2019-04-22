/**
*  \file Day.cs
*  \project INFO2180 - EMS System Term Project
*  \author The Char Stars - Alex Kozak
*  \date 2018-11-16
*  \brief Day class definition and functions
*  
*  The functions in this file are used to setup the Day Class in the Scheduling library. See class 
*  header comment for more information on the contents of this file
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS_Library
{
    /** 
     * \class Day
     *
     * \brief <b>Brief Description</b> - This class is meant to a day's worth of appointments
     * 
     * The Day class is able to store 6 or 2 AppointmentIDs depending on the day of the week. This will also
     * facilitate the query for timeslots, and the changing of dates and times for the appointments.
     *
     * \author <i>The Char Stars - Alex Kozak</i>
     */
    public class Day
    {
        #region Class Variables
        private readonly DayOfWeek dayOfWeek;
        private List<Appointment> lAppointments = new List<Appointment>();
        public int WeekID;
        #endregion

        #region Constructors
        public Day()
        {
            WeekID = -1;
            dayOfWeek = DayOfWeek.Sunday;
            AddEmptyAppointments();
        }

        public Day(DayOfWeek dayOfWeek, int weekID = -1)
        {
            WeekID = weekID;
            this.dayOfWeek = dayOfWeek;
            AddEmptyAppointments();
        }

        public Day(DayOfWeek dayOfWeek, List<Appointment> lAppointments, int weekID = -1)
        {
            WeekID = weekID;
            this.dayOfWeek = dayOfWeek;
            if (lAppointments == null) AddEmptyAppointments();
            else this.lAppointments = lAppointments;
        }

        private void AddEmptyAppointments()
        {
            int numAppointments = Week.WEEKDAY_TIME_SLOTS;
            if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday) numAppointments = Week.WEEKEND_TIME_SLOTS;
            while (numAppointments-- > 0) { lAppointments.Add(new Appointment()); }
        }
        #endregion

        public List<Appointment> GetAppointments() { return lAppointments; }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - adds a new appointemnt into a slot on the day
        * \details <b>Details</b>
        *
        * adds an appointment if available to the selected slot
        * 
        * \return <b>bool</b> - if successfully added
        */
        public bool AddAppointment(Appointment appointment, int timeSlot)
        {
            bool isSuccess = false;
            if (appointment != null && timeSlot < lAppointments.Count && timeSlot >= 0)
            {
                if (lAppointments[timeSlot].AppointmentID == -1)
                {
                    isSuccess = true;
                    if (appointment.AppointmentID == -1)
                    {
                        appointment.AppointmentID = FileIO.GenerateTableID(FileIO.TableNames.Appointments);
                    }
                    lAppointments[timeSlot] = appointment;
                }
                Logging.Log("Day", "AddAppointment", string.Format("Appointment {0} ", appointment.AppointmentID) + (isSuccess ? "successfully added." : "failed to add. Appointment is already filled."));
            }
            return isSuccess;
        }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - Gets timeslot
        * \details <b>Details</b>
        *
        * Returns teh timeslot given the appointmentID
        * 
        * \return <b>int</b> - the slot index of the appointment.
        */
        public int GetTimeslotByAppointmentID(int appointmentID)
        {
            int timeSlot = 0;
            foreach (Appointment a in lAppointments)
            {
                if (a.AppointmentID == appointmentID) break;
                timeSlot++;
            }
            if (timeSlot == lAppointments.Count) { timeSlot = -1; }
            Logging.Log("Day", "GetTimeslotByAppointmentID", string.Format("Appointment is at index {0}. (WeekID: {1}, DayOfWeek: {2})", timeSlot, WeekID, (int)dayOfWeek));
            return timeSlot;
        }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - updates appointment in a given slot
        * \details <b>Details</b>
        *
        * replaces the appointment in the given slot with a new appointment object
        * 
        * \return <b>bool</b> - return the list of appointments the patient is scheduled for
        */
        public bool UpdateAppointment(Appointment a, int timeSlot)
        {
            bool isSuccess = false;
            if (a != null)
            {
                if (timeSlot < lAppointments.Count && timeSlot >= 0)
                {
                    if (lAppointments[timeSlot].AppointmentID == a.AppointmentID)
                    {
                        isSuccess = true;
                        lAppointments[timeSlot] = a;
                        Logging.Log("Day", "UpdateAppointment", string.Format("Successfully updated Appointment to update, invalid timeslot {0}", timeSlot));
                    }
                    else { Logging.Log("Day", "UpdateAppointment", string.Format("Failed to update, Appointment with ID {0} not found", a.AppointmentID)); }
                }
                else { Logging.Log("Day", "UpdateAppointment", string.Format("Failed to update, invalid timeslot {0}", timeSlot)); }
            }
            else { Logging.Log("Day", "UpdateAppointment", "Failed to update, Appointment given is null"); }
            return isSuccess;
        }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - returns if the timeslot is open
        * \details <b>Details</b>
        *
        * Returns if the requested timeslot is open or not.
        * 
        * \return <b>bool</b> - if the slot is open.
        */
        public bool IsTimeslotOpen(int timeSlot)
        {
            bool isOpen = false;
            if (timeSlot < lAppointments.Count && timeSlot >= 0)
            {
                isOpen = (lAppointments[timeSlot].AppointmentID == -1);
            }
            return isOpen;
        }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - Deletes appointment in a given slot
        * \details <b>Details</b>
        *
        * replaces the appointment in the given slot with a blank apointment
        * 
        * \return <b>bool</b> - return the if the appointment was deleted or not
        */
        public bool DeleteAppointment(int appointmentID, int timeSlot, bool isAppointmentID = false)
        {
            if (isAppointmentID)
            {
                for (int i = 0; i < lAppointments.Count; i++)
                {
                    if (lAppointments[i].AppointmentID == appointmentID)
                    {
                        Logging.Log("Day", "DeleteAppointment", string.Format("Appointment {0} deleted.", lAppointments[timeSlot].AppointmentID));
                        lAppointments[i] = new Appointment(-1, -1, -1, 0);
                        return true;
                    }
                }
                Logging.Log("Day", "DeleteAppointment", string.Format("Invalid AppointmentID {0}.", timeSlot));
            }
            else
            {
                if (timeSlot < lAppointments.Count && timeSlot >= 0)
                {
                    Logging.Log("Day", "DeleteAppointment", string.Format("Appointment {0} deleted.", lAppointments[timeSlot].AppointmentID));
                    lAppointments[timeSlot] = new Appointment();
                    return true;
                }
                else { Logging.Log("Day", "DeleteAppointment", string.Format("Invalid timeSlot {0}.", timeSlot)); }
            }
            return false;
        }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - formats the date info into a common format
        * \details <b>Details</b>
        *
        * Used in the saving of the schedule to the database
        * 
        * \return <b>string</b> - formatted day
        */
        public string ToScheduleString()
        {
            string outputString = "";
            for (int i = 0; i < lAppointments.Count; i++)
            {
                outputString += lAppointments[i].AppointmentID.ToString() + ",";
            }
            return outputString;
        }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - if there are free slots
        * \details <b>Details</b>
        *
        * returns if there are one or more free slots on the day
        * 
        * \return <b>bool</b> 
        */
        public bool HasFreeSlot()
        {
            foreach (Appointment appointment in lAppointments) { if (appointment.AppointmentID == -1) return true; }
            return false;
        }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - gets number of free slots
        * \details <b>Details</b>
        *
        * returns the number of free slots on the day
        * 
        * \return <b>int</b> - the number of free slots on that day.
        */
        public int NumberOfFreeSlots()
        {
            int numFreeSlots = 0;
            foreach (Appointment appointment in lAppointments) { if (appointment.AppointmentID == -1) numFreeSlots++; }
            Logging.Log("Day", "NumberOfFreeSlot", string.Format("{0} free appointment slots.", numFreeSlots));
            return numFreeSlots;
        }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - gets number of free slots
        * \details <b>Details</b>
        *
        * returns the number of free slots on the day
        * 
        * \return <b>int</b> - the number of free slots on that day.
        */
        public int NumberOfScheduledAppointments()
        {
            int numFreeSlots = lAppointments.Count;
            foreach (Appointment appointment in lAppointments) { if (appointment.AppointmentID == -1) numFreeSlots--; }
            Logging.Log("Day", "NumberOfFreeSlot", string.Format("{0} free appointment slots.", numFreeSlots));
            return numFreeSlots;
        }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - gets the day of the week field
        * \details <b>Details</b>
        *
        * returns the dayOfWeek
        * 
        * \return <b>DayOfWeek</b> - the day of the week this day object represents.
        */
        public DayOfWeek GetDayOfWeek() { return dayOfWeek; }
    }
}
