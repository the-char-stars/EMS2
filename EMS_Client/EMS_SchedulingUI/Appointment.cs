/**
*  \file Appointment.cs
*  \project INFO2180 - EMS System Term Project
*  \author The Char Stars - Alex Kozak
*  \date 2018-11-16
*  \brief Appointment class definition and functions
*  
*  The functions in this file are used define a class used to store all the necessary parts of
*  an apointment. This is primarily used in the Scheduling library.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS_Library
{
   /** 
   * \class Appointment
   *
   * \brief <b>Brief Description</b> - This class is meant to help package appointment information
   * 
   * The Appointment class packages the appointment information in order to make it easier to store, update and view
   * each appointment. It is used all throughout the program but will be prominently used by the scheduling class.
   *
   * \author <i>The Char Stars - Alex Kozak</i>
   */
    public class Appointment
    {
        public int AppointmentID { get; set; }  /**< The unique ID by which each appointment can be found by. */
        public int PatientID { get; set; }      /**< The unique ID by which each patient can be found by. */
        public int DependantID { get; set; }    /**< The unique ID by which the person that showed up to the appointment with the patient can be found by. */
        public int RecallFlag { get; set; }     /**< The flag that indicates how long until the patient needs to be recalled for another appointment. */
        public string AppointmentNotes { get; set; }     
        public int IsCheckedIn { get; set; }     

        /**
        * \brief <b>Brief Description</b> - Program <b><i>Constructor</i></b> - constructs with string array
        * \details <b>Details</b>
        *
        * parses string array for Appointment information
        */
        public Appointment(string[] appointmentInfo)
        {
            if (appointmentInfo != null)
            {
                try
                {
                    AppointmentID = Int32.Parse(appointmentInfo[0]);
                    PatientID = Int32.Parse(appointmentInfo[1]);
                    DependantID = Int32.Parse(appointmentInfo[2]);
                    RecallFlag = Int32.Parse(appointmentInfo[3]);
                    IsCheckedIn = Int32.Parse(appointmentInfo[4]);
                }
                catch (FormatException e) { Logging.Log(e, "Appointment", "Constructor", "FormatException"); }
                catch (ArgumentNullException e) { Logging.Log(e, "Appointment", "Constructor", "ArgumentNullException"); }
            }
        }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>Constructor</i></b> - constructs without appointmentID
        * \details <b>Details</b>
        *
        * sets values given and automatically gets appointmentID
        */
        public Appointment(int patientID, int dependantID, int recallFlag)
        {
            AppointmentID = FileIO.GenerateTableID(FileIO.TableNames.Appointments);
            PatientID = patientID;
            DependantID = dependantID;
            RecallFlag = recallFlag;
            IsCheckedIn = 0;
        }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>Constructor</i></b> - constructs with appointmentID
        * \details <b>Details</b>
        *
        * sets values given and manually sets appointmentID
        */
        public Appointment(int appointmentID, int patientID, int dependantID, int recallFlag)
        {
            AppointmentID = appointmentID;
            PatientID = patientID;
            DependantID = dependantID;
            RecallFlag = recallFlag;
            IsCheckedIn = 0;
        }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>Constructor</i></b> - constructs empty
        * \details <b>Details</b>
        *
        * sets values to defaults
        */
        public Appointment()
        {
            AppointmentID = -1;
            PatientID = -1;
            DependantID = -1;
            RecallFlag = -1;
            IsCheckedIn = 0;
        }

        public void CheckIn()
        {
            IsCheckedIn = 1;
        }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - updates info
        * \details <b>Details</b>
        *
        * updates the patientID, DependantID, and recallFlag values, all given
        * 
        * \return <b>VOID</b>
        */
        public void UpdateAppointment(int patientID, int dependantID, int recallFlag)
        {
            PatientID = patientID;
            DependantID = dependantID;
            RecallFlag = recallFlag;
        }

        /**
        * \brief <b>Brief Description</b> - Program <b><i>class method</i></b> - Gets the string array representation 
        * \details <b>Details</b>
        *
        * Gets the string array representation of the Appointment object for saving to the database.
        * 
        * \return <b>string[]</b> - the string array
        */
        public string[] ToStringArray()
        {
            return new string[] { AppointmentID.ToString(), PatientID.ToString(), DependantID.ToString(), RecallFlag.ToString(), IsCheckedIn.ToString() };
        }
    }
}