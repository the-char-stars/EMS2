/**
*  \file ApptBillRecord.cs
*  \project INFO2180 - EMS System Term Project
*  \author The Char Stars - Attila Katona
*  \date 2018-11-16
*  \brief ApptBillRecord class definition and functions
*  
*  The functions in this file are used to setup the ApptBillRecord Class in the Billing library. See class 
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
   * \class ApptBillRecord
   *
   * \brief <b>Brief Description</b> - This class is meant to be a middle man of Billing class and the joining classes using the Billing
   * 
   * The ApptBillRecord class will hold the appointmentID, patientID, billingCode. These fields will have getters and setters for use by the UI or any other
   * class under the namespace EMS_Library. All exceptions will be caught using a try catch and logged by the logging class, Logging.cs.
   * 
   * \author <i>The Char Stars - Attila Katona</i>
   */
    public class ApptBillRecord
    {
        public string AppointmentID { get; set; } /**< The ID of the appointment for reference*/
        public string PatientID { get; set; } /**< The ID of the patient for reference*/
        public string BillingCode { get; set; } /**< The code to reference for the cost for the procedure*/
        public string AppointmentBillingID { get; set; } /**< The code to reference for the cost for the procedure*/

        /**
         * \brief <b>Brief Description</b> - <b><i>Constructor</i></b> - Called upon to begin the process of handling all appointment billing records
         * \details <b>Details</b>
         *
         * Constructor for the AptBillRecord, it will need a string array holding the data for appointmentID, patientID and billingCode.
         * 
         * \param s - <b>string[]</b> - This is the string array holding the data for the appointet, patient and billing code. Any errors will be logged using the logging class
         *        
         * \return As this is a <i>constructor</i> for the Billing class, nothing is returned
         * 
         * <exception cref="System.IndexOutOfRangeException">Thrown if nothing the string array has no values</exception>
         */
        public ApptBillRecord (string[] s)
        {
            try
            {
                AppointmentBillingID = s[0].ToString();
                AppointmentID = s[1].ToString();
                PatientID = s[2].ToString();
                BillingCode = s[3].ToString();
            }
            catch (Exception e)
            {
                Logging.Log(e, "BillingRecord", "Constructor", "Error parsing from data row");
            }
        }
        /**
        * \brief <b>Brief Description</b> - Billing<b> <i>class method</i></b> - This makes a string array of data needed to store in the database
        * \details <b>Details</b>
        *
        * This method will take the appointmentID, patientID and billingCode.
        * 
        * \return <b>string[]</b> - The resulting string array   
        */
        public string[] ToStringArray()
        {
            string[] tmp = { AppointmentBillingID, AppointmentID, PatientID, BillingCode };
            return tmp;
        }
    }
}
