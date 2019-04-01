/**
*  \file Patient.cs
*  \project INFO2180 - EMS System Term Project
*  \author The Char Stars - Divyangbhai Dankhara
*  \date 2018-11-16
*  \brief Patient class definition and functions
*  
*  The functions in this file are used to setup the Patient Class in the Demographics library. See class 
*  header comment for more information on the contents of this file
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Globalization;

namespace EMS_Library
{
    /** 
    * \class Patient
    *
    * \brief <b>Brief Description</b> - This class is meant to validate the user information and store the correct information.
    * 
    * This Patient class will be invoked by the demographic class and it has reference to the Demographics class so it can access the patient roster.
    * patient class also have ability to get the all other data when patient enter HeadOfHouse number into the their information.
    * 
    *  
    * <b>NOTE:</b> The intention is to have a single demographic object to create, search, update the patient and find the Dependants of the given patient. 
    * Because of This, the demographic class should be initialized once, then simply passed through to all other classes which require it.
    *
    * \author <i>The Char Stars - Divyangbhai Dankhara</i>
    */
    public class Patient
    {        
        private int _patientID;                 
        private string _firstName = null;
        private string _lastName = null;
        private string _hcn = null;
        private string _mInitial = null;
        private DateTime _dateOfBirth = DateTime.Now.Date;
        private string _sex = null;
        private string _headOfHouse = null;
        private string _addressLine1 = null;
        private string _addressLine2 = null;
        private string _city = null;
        private string _province = null;
        private string _phoneNum = null;
        private string _postalCode = null;

        private Demographics demographics;

        /**
        * \brief <b>Brief Description</b> Patient - ToString <b><i>class method</i></b> - This is the class ToStirng method which will returns class 
        * \details <b>Details</b>
        * 
        * This is ToString function which will returns the one string which contains the  LastName, FirstName, HCN, AddressLine1, PhoneNum which is formated with '-' separates in it.
        * 
        * \param none - <b>none</b> - This ToString function will not take any parameters
        * 
        * \return String <i>String</i> Containing LastName, FirstName, HCN, AddressLine1, PhoneNum.
        * 
        */
        public override string ToString()
        {
            return string.Format("{0}, {1} - {2} - {3} - {4}", LastName, FirstName, HCN, AddressLine1, PhoneNumber);
        }

        /**
        * \brief <b>Brief Description</b> Patient - Constructor <b><i>class method</i></b> - This method is used to instantiate a new Patient object with given a set of attribute values.
        * \details <b>Details</b>
        *
        * Constructor for the Patient class, it gets the Demographics class object assign it to the local variables to use so it can use the demographic class method to get access to the patient roster.
        * 
        * \param d - <b>Demographics</b> - This is object of demographic class which Patient class will use to access patient roster.
        * 
	    * \return As This is a <i>constructor</i> for the Patient class, nothing is returned
        * 
        */
        public Patient(Demographics d = null)
        {
            if (d == null) d = new Demographics();
            Logging.Log("Patient", "Patient",  "Initializing New the Patient object with default constructor");
            this.DateOfBirth = DateTime.Now.AddDays(-1);
            demographics = d;
            // default constructor
            _patientID = -1;
        }

        /**
        * \brief <b>Brief Description</b> Patient - Constructor <b><i>class method</i></b> - This method is used to instantiate a new Patient object with given a set of attribute values.
        * \details <b>Details</b>
        *
        * Constructor for the Patient class, it gets the Demographics class object assign it to the local variables to use so it can use the demographic class method to get access to the patient roster. and here patient class constructor also getting the string array which contain patient inflammation which came from database
        * 
        * \param d - <b>Demographics</b> - This is object of demographic class which Patient class will use to access patient roster.
        * 
        * \param patientRecord - <b>string[]</b> - This is a String array which contain the patient information which will used to create a Patient object. 
        * 
        * \param isNew - <b>bool</b> this the bool that indicates that is this a new patient object or not.
        * 
        * \return As This is a <i>constructor</i> for the Patient class, nothing is returned
        * 
        */
        public Patient(Demographics d, string[] patientRecord, bool isNew = false)
        {
            Logging.Log("Patient", "Patient" , "Initialize the Patient object with preexisting Patient data");
            demographics = d;
            int i = 0;

            if (isNew) { PatientID = FileIO.GenerateTableID(FileIO.TableNames.Patients); }
            else { PatientID = Int32.Parse(patientRecord[i++]); }
            FirstName = patientRecord[i++];
            LastName = patientRecord[i++];
            HCN = patientRecord[i++];
            MInitial = patientRecord[i++];
            DateOfBirth = DateTime.Parse(patientRecord[i++]);
            Sex = patientRecord[i++];
            HeadOfHouse = patientRecord[i++];
            AddressLine1 = patientRecord[i++];
            AddressLine2 = patientRecord[i++];
            City = patientRecord[i++];
            Province = patientRecord[i++];
            PhoneNumber = patientRecord[i++];
            PostalCode = patientRecord[i++];
        }

        /**
        * \brief <b>Brief Description</b> Patient - Constructor <b><i>class method</i></b> - This method is used to instantiate a new Patient object with given a set of attribute values.
        * \details <b>Details</b>
        *
        * Constructor for the Patient class, it gets the Demographics class object assign it to the local variables to use so it can use the demographic class method to get access to the patient roster. and here patient class constructor also getting the string array which contain patient inflammation which came from database
        * 
        * \param d - <b>Demographics</b> - This is object of demographic class which Patient class will use to access patient roster.
        * 
        * \param patientRecord - <b>string[]</b> - This is a String array which contain the patient information which will used to create a Patient object. 
        * 
        * \param ID - <b>int</b> - this is a integer which contains the PatientID of Patient In database 
        * 
        * \return As This is a <i>constructor</i> for the Patient class, nothing is returned
        * 
        */
        public Patient(Demographics d, string[] patientRecord, int ID)
        {
            Logging.Log("Patient", "Patient", "Initialize the Patient object with preexisting Patient data");
            demographics = d;
            int i = 0;

            PatientID = ID;
            FirstName = patientRecord[i++];
            LastName = patientRecord[i++];
            HCN = patientRecord[i++];
            MInitial = patientRecord[i++];
            DateOfBirth = DateTime.Parse(patientRecord[i++]);
            Sex = patientRecord[i++];
            HeadOfHouse = patientRecord[i++];
            AddressLine1 = patientRecord[i++];
            AddressLine2 = patientRecord[i++];
            City = patientRecord[i++];
            Province = patientRecord[i++];
            PhoneNumber = patientRecord[i++];
            PostalCode = patientRecord[i++];
        }

        /**
        * \brief <b>Brief Description</b> Patient - Constructor <b><i>class method</i></b> - This method used to return all the data member as string array.
        * \details <b>Details</b>
        *  
        * This method is used when used when patient about to submit to the database at that this method used to get the string array if the patient information.
        * 
        * \return ToStringArray <i>string[]</i> for the Patient class.
        * 
        */
        public string[] ToStringArray()
        {
            return new string[] { this._patientID.ToString(), this._firstName, this._lastName, this._hcn, this._mInitial, this._dateOfBirth.ToString(), this._sex, this._headOfHouse, this._addressLine1,this._addressLine2,this._city,this._province,this._phoneNum, this._postalCode };
        }

        /**
        * \brief <b>Brief Description</b> - Patient <b><i>class method</i></b> - THis method will be called 
        * \details <b>Details</b>
        *
        * ShowInfo method will make the list which contains the Information Type and Actual information of patients FirstName, Last name, health Card, Middle Initial, Date of birth, Gender, Head Of house, Address Line1, Address Line2, City, Province, and PhoneNumber and returns it to the UI class so UI class can display it to the screen.
        * 
        * \return List - <b>List</b> - THis method will returns List which have key value pair, it contains the Information type and Actual information of the Patients information. 
        *     
        */
        public List<KeyValuePair<string, string>> ShowInfo()
        {
            Logging.Log("Patient", "ShowInfo" , String.Format("Printing information of patient {0} to screen",PatientID)); 
            List<KeyValuePair<string, string>> lkvp = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("First Name".PadRight(20, ' ') + ": ", FirstName),
                new KeyValuePair<string, string>("Last Name".PadRight(20, ' ') + ": ", LastName),
                new KeyValuePair<string, string>("Health Card".PadRight(20, ' ') + ": ", HCN),           
                new KeyValuePair<string, string>("Middle Initial".PadRight(20, ' ') + ": ", MInitial),
                new KeyValuePair<string, string>("Date Of Birth".PadRight(20, ' ') + ": ", DateOfBirth.ToString()),
                new KeyValuePair<string, string>("Gender".PadRight(20, ' ') + ": ", Sex),
                new KeyValuePair<string, string>("Head Of House".PadRight(20, ' ') + ": ", HeadOfHouse),
                new KeyValuePair<string, string>("Address Line 1".PadRight(20, ' ') + ": ", AddressLine1),
                new KeyValuePair<string, string>("Address Line 2".PadRight(20, ' ') + ": ", AddressLine2),
                new KeyValuePair<string, string>("City".PadRight(20, ' ') + ": ", City),
                new KeyValuePair<string, string>("Province".PadRight(20, ' ') + ": ", Province),
                new KeyValuePair<string, string>("Phone Number".PadRight(20, ' ') + ": ", PhoneNumber)
            };

            return lkvp;
        }

        /**
        * \brief <b>Brief Description</b> - Patient <b><i>class accessors</i></b> - this accessors used to get and set the PatientID for patient class.
        * \details <b>Details</b>
        *
        * This accessors used to validate and set the value of the PatientID
        * 
        * \param PatientID - <b>int</b> - This method takes int PatientID
        * 
        * \return patientID - <b>int</b> - This accessors return the PatientID which stored in object Data member
        * 
        */
        public int PatientID
        {
            get { return _patientID; }
            set { if ((value > -1)) _patientID = value; else { Logging.Log("Patient", "PatientID", String.Format( "Failed assigning Patient ID : {0}",value)); } }
        }

        /**
        * \brief <b>Brief Description</b> - Patient <b><i>class accessors</i></b> - this accessors used to get and set the Patient FirstName for patient class.
        * \details <b>Details</b>
        *
        * This accessors used to validate and set the value of the FirstName
        * 
        * \param FirstName - <b>string</b> - This method takes string 
        * 
        * \return FirstName - <b>String</b> - This accessors return the PatientID which stored in object Data member
        * 
        */
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                value = (value ?? "").ToUpper();
                // check the first name and set it to the first name;
                Regex rx = new Regex("^[a-zA-Z.-]+$");
                Match mc = rx.Match(value);
                if (mc.Success) _firstName = value;
                else { Logging.Log("Patient", "FirstName", String.Format( "Failed assigning Patient's FirstName : {0}",value)); }
            }
        }

        /**
        * \brief <b>Brief Description</b> - Patient <b><i>class accessors</i></b> - this accessors used to get and set the Patient LastName for patient class.
        * \details <b>Details</b>
        *
        * This accessors used to validate and set the value of the LastName
        * 
        * \param LastName - <b>string</b> - This method returns String LastName
        * 
        * \return LastName - <b>String</b> - This accessors return the PatientID which stored in object Data member
        * 
        */
        public string LastName
        {
            get { return _lastName; }
            set
            {
                value = (value ?? "").ToUpper();
                Regex rx = new Regex("^[a-zA-Z.-]+$");
                Match mc = rx.Match(value);
                if (mc.Success) { _lastName = value; }
                else { Logging.Log("Patient", "LastName", String.Format( "Failed assigning Patient's LastName: {0}",value)); }
            }
        }

        /**
        * \brief <b>Brief Description</b> - Patient <b><i>class accessors</i></b> - this accessors used to get and set the Patient HCN for patient class.
        * \details <b>Details</b>
        *
        * This accessors used to validate and set the value of the HCN
        * 
        * \param HCN - <b>string</b> - This method takes string HCN
        * 
        * \return HCN - <b>String</b> - This accessors return the HCN which stored in object Data member
        * 
        */
        public string HCN
        {
            get { return _hcn; }
            set
            {
                _hcn = (value ?? "").ToUpper();
            }
        }

        /**
        * \brief <b>Brief Description</b> - Patient <b><i>class accessors</i></b> - this accessors used to get and set the Patient Minitial for patient class.
        * \details <b>Details</b>
        *
        * This accessors used to validate and set the value of the Minitial
        * 
        * \param Minitial -<b>string</b> - This method takes string Minitial
        * 
        * \return Minitial - <b>String</b> - This accessors return the Minitial which stored in object Data member
        * 
        */
        public string MInitial
        {
            get { return _mInitial; }
            set
            {
                value = (value ?? "").ToUpper();
                Regex rx = new Regex("^[a-zA-Z]?$");
                Match mc = rx.Match(value);
                if (mc.Success) { _mInitial = value; }
                else { Logging.Log("Patient", "MInitial", String.Format("Failed assigning Patient's MInitial : {0}",value)); }
            }
        }

        /**
        * \brief <b>Brief Description</b> - Patient <b><i>class accessors</i></b> - this accessors used to get and set the Patient DateOfBirth for patient class.
        * \details <b>Details</b>
        *
        * This accessors used to validate and set the value of the DateOfBirth
        * 
        * \param DateOfBirth - <b>string</b> - This method takes string DateOfBirth
        * 
        * \return DateOfBirth - <b>String</b> - This accessors return the DateOfBirth which stored in object Data member
        * 
        */
        public DateTime DateOfBirth
        {
            get { return _dateOfBirth.Date; }
            set
            {
                _dateOfBirth = value;               
            }
        }

        public string DOBString { get { return _dateOfBirth.ToShortDateString(); } }

        /**
        * \brief <b>Brief Description</b> - Patient <b><i>class accessors</i></b> - this accessors used to get and set the Patient Sex for patient class.
        * \details <b>Details</b>
        *
        * This accessors used to validate and set the value of the gender
        * 
        * \param sex - <b>string</b> - This method takes string gender
        * 
        * \return sex - <b>String</b> - This accessors return the gender which stored in object Data member
        * 
        */
        List<string> validSexes = new List<string> { "M", "F", "X", "MALE", "FEMALE", "OTHER" };

        public string Sex
        {
            get { return _sex; }
            set
            {
                
                value = (value ?? "").ToString().ToUpper();
                if (validSexes.Contains(value)) { _sex = value; }
                else { Logging.Log("Patient", "Sex", String.Format("Failed assigning Patient's Sex : {0}",value)); }
            }
        }

        /**
        * \brief <b>Brief Description</b> - Patient <b><i>class accessors</i></b> - this accessors used to get and set the Patient HeadOfHouse for patient class.
        * \details <b>Details</b>
        *
        * This accessors used to validate and set the value of the HeadOfHouse
        * 
        * \param HeadOfHouse - <b>string</b> - This method takes string HeadOfHouse
        * 
        * \return HeadOfHouse - <b>String</b> - This accessors return the HeadOfHouse which stored in object Data member
        * 
        */
        public string HeadOfHouse
        {
            get { return _headOfHouse; }
            set
            {
                value = (value ?? "").ToUpper().ToString();
                _headOfHouse = value;
                Regex rx = new Regex("^([0-9]{10}[a-zA-Z]{2}$)");
                Match mc = rx.Match(value);
                if (mc.Success)
                {
                    Patient headOfHouseHold = demographics.GetPatientByHCN(value);
                    if(headOfHouseHold != null)
                    {
                        _addressLine1 = headOfHouseHold.AddressLine1;
                        _addressLine2 = headOfHouseHold.AddressLine2;
                        _city = headOfHouseHold.City;
                        _phoneNum = headOfHouseHold.PhoneNumber;
                        _province = headOfHouseHold.Province;                        
                    }
                    else
                    {
                        Logging.Log("Patient", "HeadOfHouse", String.Format("Head of house with HCN number {0} does not exist in database",value));
                    }

                }
                else { Logging.Log("Patient", "HeadOfHouse", String.Format("Failed to find head-of-house-hold from patient roster : {0}",value)); }
            }
        }

        /**
        * \brief <b>Brief Description</b> - Patient <b><i>class accessors</i></b> - this accessors used to get and set the Patient AddressLine1 for patient class.
        * \details <b>Details</b>
        *
        * This accessors used to validate and set the value of the AddressLine1
        * 
        * \param DateOfBirth - <b>string</b> - This method takes string AddressLine1
        * 
        * \return DateOfBirth - <b>String</b> - This accessors return the AddressLine1 which stored in object Data member
        * 
        */
        public string AddressLine1
        {
            get { return _addressLine1; }
            set
            {
                value = (value ?? "").ToUpper().ToString();
                Regex rx = new Regex("^[a-zA-Z0-9\\s.-]+$");
                Match mc = rx.Match(value);
                if (mc.Success) { _addressLine1 = value; }
                else { Logging.Log("Patient", "AddressLine1", string.Format("Failed assigning Patient's Address1 : {0}",value)); }
            }
        }

        /**
         * \brief <b>Brief Description</b> - Patient <b><i>class accessors</i></b> - this accessors used to get and set the Patient AddressLine2 for patient class.
         * \details <b>Details</b>
         *
         * This accessors used to validate and set the value of the AddressLine2
         * 
         * \param AddressLine2 - <b>string</b> - This method takes string AddressLine2
         * 
         * \return AddressLine2 - <b>String</b> - This accessors return the AddressLine2 which stored in object Data member
         * 
         */
        public string AddressLine2
        {
            get { return _addressLine2; }
            set
            {
                value = (value ?? "").ToUpper().ToString();
                Regex rx = new Regex("^[a-zA-Z0-9\\s.-]+$");
                Match mc = rx.Match(value);
                if (mc.Success) { _addressLine2 = value; }
                else{ Logging.Log("Patient", "AddressLine2", String.Format("Failed assigning Patient's Address2 : {0}",value)); }
            }
        }

        /**
         * \brief <b>Brief Description</b> - Patient <b><i>class accessors</i></b> - this accessors used to get and set the Patient City for patient class.
         * \details <b>Details</b>
         *
         * This accessors used to validate and set the value of the city
         * 
         * \param city - <b>string</b> - This method takes string city
         * 
         * \return city - <b>String</b> - This accessors return the city which stored in object Data member
         * 
         */
        public string City
        {
            get { return _city; }
            set
            {
                value = value.ToUpper().ToString();
                Regex rx = new Regex("^[a-zA-Z]+$");
                Match mc = rx.Match(value);
                if (mc.Success) { _city = value; }
                else { Logging.Log("Patient", "City", String.Format("Failed assigning Patient's City : {0}",value)); }
            }
        }

        /**
         * \brief <b>Brief Description</b> - Patient <b><i>class accessors</i></b> - this accessors used to get and set the Patient City for patient class.
         * \details <b>Details</b>
         *
         * This accessors used to validate and set the value of the city
         * 
         * \param city - <b>string</b> - This method takes string city
         * 
         * \return city - <b>String</b> - This accessors return the city which stored in object Data member
         * 
         */
        public string PostalCode
        {
            get { return _postalCode; }
            set { _postalCode = (value ?? "").ToUpper().ToString(); }
        }

        /**
         * \brief <b>Brief Description</b> - Patient <b><i>class accessors</i></b> - this accessors used to get and set the Patient Province for patient class.
         * \details <b>Details</b>
         *
         * This accessors used to validate and set the value of the Province
         * 
         * \param Province - <b>string</b> - This method takes string Province
         * 
         * \return Province - <b>String</b> - This accessors return the Province which stored in object Data member
         * 
         */
        public string Province
        {
            get { return _province; }
            set
            {
                value = (value ?? "").ToUpper().ToString();
                string[] provinces = { "AB", "BC", "MB", "NB", "NL", "NS", "NT", "NU", "ON", "PE", "QC", "SK", "YT"};
                if (provinces.Contains(value)) { _province = value; }
                else { Logging.Log("Patient", "Province", String.Format("Failed assigning Patient's Province : {0}",value)); }
            }
        }

        /**
          * \brief <b>Brief Description</b> - Patient <b><i>class accessors</i></b> - this accessors used to get and set the Patient PhoneNum for patient class.
          * \details <b>Details</b>
          *
          * This accessors used to validate and set the value of the PhoneNum
          * 
          * \param PhoneNum - <b>string</b> - This method takes string PhoneNum
          * 
          * \return PhoneNum - <b>String</b> - This accessors return the PhoneNum which stored in object Data member
          * 
          */
        public string PhoneNumber
        {
            get { return _phoneNum; }
            set { _phoneNum = value; }
        }

        bool isPostalCodeValid()
        {
            Regex rx = new Regex("^[A-Z][0-9][A-Z][-]?[0-9][A-Z][0-9]$");
            return rx.IsMatch(_postalCode ?? "");
        }

        bool isHCNValid()
        {
            Regex rx = new Regex("^([0-9]{10}[a-zA-Z]{2}$)");
            return rx.IsMatch(HCN ?? "");
        }

        bool isPhoneNumberValid()
        {
            return new Regex("^[0-9]{3}[ ./-]?[0-9]{3}[ ./-]?[0-9]{4}$").IsMatch(_phoneNum ?? "");
        }

        /**
          * \brief <b>Brief Description</b> - Patient <b><i>class method</i></b> - this is a method used to check that patient is read to submit
          * \details <b>Details</b>
          *
          * This method check that each required field if filled with data if it is not than it will return false.
          * 
          * \return true/false - <b>Bool</b> - This method will return true when all the required filed are filled in with data and returns false when the one or many of required filed are empty
          * 
          */
        public bool IsReadyToSave()
        {
            bool isReady = true;

            if (this.FirstName == null) { isReady = false; }
            if (isReady && this.HCN == null) { isReady = isReady && isHCNValid(); }
            if (isReady && this.LastName == null) { isReady = false; }
            if (isReady && this.DateOfBirth == null) { isReady = false; }
            if (isReady && this.Sex == null) { isReady = false; }
            if (isReady && this.PostalCode == null) { isReady = isReady && isPostalCodeValid(); }            
            if (isReady) { if (!isPhoneNumberValid()) _phoneNum = ""; }

            return isReady;
        }
    }
}
