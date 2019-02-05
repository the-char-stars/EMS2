/**
*  \file BillingRecord.cs
*  \project INFO2180 - EMS System Term Project
*  \author The Char Stars - Attila Katona
*  \date 2018-11-16
*  \brief BillingRecord class definition and functions
*  
*  The functions in this file are used to setup the BillingRecord Class in the Billing library. See class 
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
   * \class BillingRecord
   *
   * \brief <b>Brief Description</b> - This class is meant to handle all the billing record enquiries
   * 
   * The BillingRecord class will hold all values that coorelate to the cost of each billing code and the effective date of that billing code to ensure that the 
   * ministry is not being billed for procedures that are not covered from the date of the appointment. All exceptions will be caught
   * using a try catch and logged by the logging class, Logging.cs.
   * 
   * \author <i>The Char Stars - Attila Katona</i>
   */
    public class BillingRecord
    {        
        public string BillingCode { get; set; }         /**< The billing code for the record in scope*/
        public DateTime EffectiveDate { get; set; }     /**< The date the billing code is active from*/
        public double Cost { get; set; }                /**< The monatery cost related to the bill code*/

        /**
         * \brief <b>Brief Description</b> - <b><i>Constructor</i></b> - Called upon to begin the process of handling all billing records
         * \details <b>Details</b>
         *
         * Constructor for the BillingRecord, it will need a string array holding the data for billingCode, effectiveDate and cost
         * 
         * \param s - <b>string[]</b> - This is the string array holding the data for the billingCode, effectiveDate and cost. Any errors will be logged using the logging class
         *        
         * \return As this is a <i>constructor</i> for the Billing class, nothing is returned
         * 
         * <exception cref="IndexOutOfRangeException">Thrown if nothing the string array has no values</exception>
         * <exception cref="FormatException">Exception can be thrown if the s[2] value is not a number value or s[1] is in an invalid format for the DateTime, try/catch block, and construction of the class is continued.</exception>
         */
        public BillingRecord(string [] s)
        {
            try
            {
                BillingCode = s[0].ToString();
                EffectiveDate = DateTime.ParseExact(s[1].ToString(),"yyyymmdd", CultureInfo.InvariantCulture);                
                Cost = Double.Parse(s[2].ToString()) / 10000;
            }
            catch (Exception e)
            {
                Logging.Log(e, "BillingRecord", "Constructor", "FAILED parsing from data row");
            }
        }
        /**
        * \brief <b>Brief Description</b> - Billing<b><i>class method</i></b> - This checks any DateTime and validates the given time to confirm it is after the effective date.
        * \details <b>Details</b>
        *
        * This method will take a DateTime, which will be the date of appointment or something regarding the EMS Billing code. It will validate the date to ensure it is
        * after the effectiveDate of the billingCode to ensure the procedure done to the patient will be paid for by the Ministry of Health.
        * 
        * \return <b>bool</b> - The resulting true or false if the DateTime.Compare is less than 0 or not. If under 0 the effectiveDate is earlier than the checkDate.
        */
        public bool IsValidDate(DateTime checkDate)
        {
            return DateTime.Compare(EffectiveDate, checkDate) < 0;
        }
    }
}