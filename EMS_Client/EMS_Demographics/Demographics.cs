/**
*  \file Demographics.cs
*  \project INFO2180 - EMS System Term Project
*  \author The Char Stars - Divyangbhai Dankhara
*  \date 2018-11-16
*  \brief Primary interaction with the Demographic Library
*  
*  This file contain the definition of the demographic class which is inherited by the patient class 
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS_Library
{

    /** 
    * \class Demographics
    *
    * \brief <b>Brief Description</b> - This class is meant to perform things like create patient, update patient, search patient from patient roster as well add the newly created patient to the database.
    * 
    * The Demographics class has access to the <i>Patient class</i>. This class is also 
    * apart of the EMS_Library namespace, which is common throughout all of the non-UI based libraries found in the EMS System. The primary purpose of This 
    * library is to provide functionality like create new patient search patient give list of dependents of the patient and add the newly created patient to the database through fileIO.library
    *  
    * <b>NOTE:</b> The intention is to have a single demographic object to create, search, update the patient and find the Dependants of the given patient. 
    * Because of This, the demographic class should be initialized once, then simply passed through to all other classes which require it.
    *
    * \author <i>The Char Stars - Divyangbhai Dankhara</i>
    */
    public class Demographics
    {
        public Dictionary<int, Patient> dPatientRoster = new Dictionary<int, Patient>(); /**< > */

        /**
        * \brief <b>Brief Description</b> Demographics - Constructor <b><i>class method</i></b> - This method is used to instantiate a new Demographics object with given a set of attribute values and instantiate base class object.
        * \details <b>Details</b>
        *
        * Constructor for the Demographics class, it gets the fileIO class object and logging object and assign it to the local variables to use.
        * 
        * \param f - <b>FileIO</b> - This object used to get patient from database and save patient in database file.
        * 
	    * \return As This is a <i>constructor</i> for the Demographics class, nothing is returned
        * 
        * \see GetPatientList();
        */
        public Demographics()
        {
            Logging.Log("Demographics", "Demographics", "Initialize the demographics object");
            GetPatientList();
        }



        /**
        * \brief <b>Brief Description</b> - Demographics <b><i>class method</i></b> - This method is used to add the newly created patient to the database. and update the patient roster
        * \details <b>Details</b>
        *
        * This method adds the newly created patients to the database and update the patient roster with GetPatientList Function which will refill the patient roster
        * 
        * \param patient -<b>Patient</b> - This method takes the newly created patient object to add that in database
        * 
        * \return none - <b>void</b> - This method returns nothing
        * 
        * \see GetPatientList();
        */
        public void AddNewPatient(Patient patient)
        {
            if(patient != null)
            {
                FileIO.AddRecordToDataTable(patient.ToStringArray(), FileIO.TableNames.Patients);
                Logging.Log("Demographics", "AddNewPatient", String.Format("Adding new patient with PatientHCN {0} to the database file", patient.HCN));
                GetPatientList();

            }
        }



        /**
        * \brief <b>Brief Description</b> - Demographics <b><i>class method</i></b> - This method is used to create new patient Object.
        * \details <b>Details</b>
        *
        * This method creates and instantiate patient object and return that object reference to the UI
        * 
        * \param none - <b>void</b> - This method takes no parameters
        * 
        * \return <b>Patient</b> - This method returns the newly created patient object reference 
        */
        public Patient CreateNewPatient()
        {
            try
            {
                Patient newPatient = new Patient(this);
                Logging.Log("Demographics", "CreateNewPatient", "Creating new patient Object");
                return newPatient;
            }
            catch (Exception e)
            {
                Logging.Log("Demographics", "CreateNewPatient", String.Format("Error occur while creating new patient object : {0}", e.ToString()));
                return null;
            }

        }



        /**
        * \brief <b>Brief Description</b> - Demographics <b><i>class method</i></b> - This method is used to find the patient by it HCN number from patient roster.
        * \details <b>Details</b>
        *
        * This method iterates through the entire patient roster and find the Patient with the same HCN number which is given in the parameter 
        * 
        * \param headOfHouse - <b>string</b> - This method takes a string which contain the HCN number of patient which patient have to find from the patient roster.
        * 
        * \return <b>Patient</b> - return the patient object which matches with given HCN number or return NULL. 
        */
        public Patient GetPatientByHCN(string HCNno)
        {
            if (HCNno != null)
            {
                HCNno = HCNno.ToUpper();
                Logging.Log("Demographics", "GetPatientByHCN", String.Format("Searching the Patient by their PatientHCN number from patient roster : {0}", HCNno));
                foreach (Patient hod in dPatientRoster.Values)
                {
                    if (hod.HCN == HCNno)
                    {
                        return hod;
                    }
                }
                Logging.Log("Demographics", "GetPatientByHCN", String.Format("Failed searching the Patient by their PatientHCN number : {0} from patient roster", HCNno));
                
            }
            return null;
        }



        /**
        * \brief <b>Brief Description</b> - Demographics <b><i>class method</i></b> - This method is used to find the Patient with same Firstname and lastname
        * \details <b>Details</b>
        *
        * This method iterates through the entire patient roster and find the Patient with the same Firstname and LastName which is given in the parameter 
        * 
        * \param FName - <b>string</b> - This method takes a string which contain the FirstName of patient. and find the patient with same FirstName from the patient roster.
        * 
        * \param LName - <b>string</b> - This method takes a string which contain the LastName of patient. and find the patient with same LastName from the patient roster.
        * 
        * \param isExact - <b>bool</b> - this is bool which tells that is the first and last name that is exact or not
        *
        * \return ListOfPatient <b>List<Patient></b> - return the List of patient which matches with given FirstName and LastName 
        */
        public List<Patient> GetPatientByName(string FName, string LName, bool isExact = false)
        {
            List<Patient> ListOfPatient = new List<Patient>();
            if ((FName != null) && (LName != null))
            {
                FName = FName.ToUpper();
                LName = LName.ToUpper();                
                Logging.Log("Demographics", "GetPatientByName", String.Format("Search Patient by their FileName : {0} and LastName : {1} from patient roster", FName, LName));

                foreach (Patient hod in dPatientRoster.Values)
                {
                    if (hod.FirstName == FName && hod.LastName == LName)
                    {
                        ListOfPatient.Add(hod);
                    }
                    else if (!isExact && ((hod.FirstName.Contains(FName) && FName != "") || (hod.LastName.Contains(LName) && LName != "")))
                    {
                        ListOfPatient.Add(hod);
                    }
                }
                Logging.Log("Demographics", "GetPatientByName", String.Format("{0} Patients found", ListOfPatient.Count()));                
            }
            return ListOfPatient;
        }


        /**
        * \brief <b>Brief Description</b> - Demographics <b><i>class method</i></b> - This method is used to find the Patient with same Pid which is given in parameter
        * \details <b>Details</b>
        *
        * This method checks through the patient roster and find the Patient with the same patientID which is given in the parameter 
        * 
        * \param ID - <b>int</b> - This method takes a int which contain the patientID of patient. and find the patient with same PatientID from the patient roster.
        *
        * \return <b>Patient</b> - return the patient which matches with given PatientID
        */
        public Patient GetPatientByID(int ID)
        {
            Logging.Log("Demographics", "GetPatientByID", String.Format("Searching patient by the PatientID : {0} in patient roster", ID));
            if (dPatientRoster.ContainsKey(ID))
            {
                return dPatientRoster[ID];
            }
            Logging.Log("Demographics", "GetPatientByID", String.Format("Failed Searching patient by the PatientID : {0} in patient roster", ID));
            return null;
        }



        /**
        * \brief <b>Brief Description</b> - Demographics <b><i>class method</i></b> - this method used to save new patient or updated patient to the patient roster
        * \details <b>Details</b>
        *
        * This method will first check that the patient is available in patient roster. If it is, then it will update the patient in roster. If it is new patient then it will add new patient to the patient roster. 
        *  
        * \param patient - <b>Patient</b> - this method takes no parameters
        * 
        * \return none - <b>void</b> - this method returns nothing
        */
        public void UpdatePatient(Patient patient)
        {
            if(patient != null)
            {
                dPatientRoster[patient.PatientID] = patient;
                FileIO.UpdateRecordFromTable(patient.ToStringArray(), FileIO.TableNames.Patients);
                Logging.Log("Demographics", "UpdatePatient", "Update the patient to the patient roster");
            }
            else
            {
                dPatientRoster.Add(patient.PatientID, patient);
                FileIO.AddRecordToDataTable(patient.ToStringArray(), FileIO.TableNames.Patients);
                Logging.Log("Demographics", "UpdatePatient", "Adding the patient to the patient roster");
            }            
        }



        /**
        * \brief <b>Brief Description</b> - Demographics <b><i>class method</i></b> - this method used to fill the data into patient roster from database
        * \details <b>Details</b>
        *
        * This method first clear patient roster and iterate through entire dictionary which it gets from the fileIO class and iterate through it and fill data into  patient roster. 
        *  
        * \param none - <b>void</b> - this method takes no parameters
        * 
        * \return none - <b>void</b> - this method returns nothing
        */
        public void GetPatientList()
        {
            dPatientRoster.Clear();
            Dictionary<string, string[]> patientRecords = FileIO.ConvertTableToDictionary(FileIO.GetDataTable(FileIO.TableNames.Patients));
            /// here we get the get the database and make a patient roster whch will help us to find update the user

            Logging.Log("Demographics", "GetPatientList", "Get all the Patient from the database and add to the patient roster");

            foreach (string[] patientRecord in patientRecords.Values)
            {
                Patient newPatient = new Patient(this, patientRecord);

                if (dPatientRoster.ContainsKey(newPatient.PatientID))
                {
                    dPatientRoster[newPatient.PatientID] = newPatient;
                }
                else
                {
                    dPatientRoster.Add(newPatient.PatientID, newPatient);
                }
            }
        }




        /**
        * \brief <b>Brief Description</b> - Demographics <b><i>class method</i></b> - This method used to get the list of the all the dependent
        * \details <b>Details</b>
        *
        * This method will call the GetDependants() method to get the list of the all Dependant then one by one it will print all the Dependant to the screen.
        *  
        * \param pHeadOfHouse - <b>Patient</b> - the patient object which is houseHold
        * 
        * \return <b>List<Patient></b> - this method will return list of all the dependents.
        */
        public List<Patient> GetDependants(Patient pHeadOfHouse)
        {
            if(pHeadOfHouse != null)
            {
                Logging.Log("Demographics", "GetDependants", String.Format("Searching Patient dependents with HouseHoldNumber : {0} from patient roster and make a list of patient Dependants", pHeadOfHouse.HCN));

                List<Patient> lDependants = new List<Patient>();
                /// here we search all the dependents into the patientRoster and make the list of dependent the 
                foreach (Patient dependant in dPatientRoster.Values)
                {
                    if (pHeadOfHouse.HCN == dependant.HeadOfHouse)
                    {
                        lDependants.Add(dependant);
                        Logging.Log("Demographics", "GetDependants", String.Format("Successfully searched Patient dependents with HouseHoldNumber : {0} from patient roster and added to the dependents list", pHeadOfHouse.HCN));
                    }
                }
                return lDependants;

            }
            return null;
        }



        /**
        * \brief <b>Brief Description</b> - Demographics <b><i>class method</i></b> - This method used to get the patientID by their HCN
        * \details <b>Details</b>
        *
        * The method will iterate through patient roster and find the patient with same PatientHCN and return the patients PatientID 
        *  
        * \param headOfHouse - <b>Patient</b> - HCN number of patient which we need to found
        * 
        * \return <b>int</b> - this method will return PatientID which we found from the HCN number
        */
        public int GetPatientIDByHCN(string headOfHouse)
        {
            if(headOfHouse != null)
            {
                Logging.Log("Demographics", "GetPatientIDByHCN", String.Format("Searching the PatientID PatientHCN number : {0} from patient roster", headOfHouse));
                foreach (Patient hod in dPatientRoster.Values)
                {
                    if (hod.HCN == headOfHouse)
                    {
                        return hod.PatientID;
                    }
                }
                Logging.Log("Demographics", "GetPatientIDByHCN", String.Format("Failed searching the Patient by their PatientHCN number :{0} from patient roster and returning -1", headOfHouse));
                return -1;

            }
            return -1;
        }
    }
}
