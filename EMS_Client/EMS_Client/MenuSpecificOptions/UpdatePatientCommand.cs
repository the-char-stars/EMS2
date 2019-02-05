/**
 * \file UpdatePatientCommand.cs
*  \project INFO2180 - EMS System Term Project
*  \author The Char Stars - Tudor Lupu
*  \date 2018-12-4
*  \brief The menu option to update a patient's record
*  
*  This class is the option in the patients menu to 
*  update a patient's record in the database.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS_Client.Functionality;
using EMS_Client.Interfaces;
using EMS_Client.MenuOptions;
using EMS_Library;

/** 
* \namespace EMS_Client
*
* \brief <b>Brief Description</b> - This namespace holds the user interface portion of the code base.
*
* \author <i>The Char Stars</i>
*/
namespace EMS_Client.MenuSpecificOptions
{
    /** 
    * \class UpdatePatientCommand
    *
    * \brief <b>Brief Description</b> - This class is a sub menu of patients used to update a patient in the database
    * 
    * The UpdatePatientCommand class displays the menu asking for the new user information and once received changes
    * it in the database too
    *
    * \author <i>The Char Stars - Tudor Lupu</i>
    */
    class UpdatePatientCommand : IOption
    {
        #region private fields
        #endregion

        #region public fields
        public string Description => "Update Patient";
        #endregion

        /**
        * \brief <b>Brief Description</b> - Execute <b><i>class method</i></b> - Entry point into the update patient menu
        * \details <b>Details</b>
        *
        * This takes in the scheduling, demographics and billing libraries
        * 
        * \return <b>void</b>
        */
        public void Execute(Scheduling scheduling, Demographics demographics, Billing billing)
        {
            // display the getting patient menu and get the requested patient
            Patient patient = GetPatient.Get(MenuCodes.PATIENTS, "Patients", 1);
            Patient updatedPatient = new Patient();

            do
            {
                // check if the user canceled during the patient selection screen
                if (patient == null) { break; }

                // run the get patient menu but instead of adding it to the database it returns it
                AddPatientCommand addPatientCommand = new AddPatientCommand();
                updatedPatient = addPatientCommand.Execute(patient.ShowInfo(), true, demographics);

                // check if the user canceled during data entry
                if (updatedPatient != null)
                {
                    // update the patient information in the database
                    updatedPatient.PatientID = patient.PatientID;
                    demographics.UpdatePatient(updatedPatient);

                    // display a success message
                    Container.DisplayContent(new List<Pair<string, string>>() { { new Pair<string, string>("Patient updated successfully.", "")} },
                        0, 1, MenuCodes.PATIENTS, "Patients", Description);

                    // wait for user to read message and confirm
                    Console.ReadKey();
                    break;
                }
                else
                {
                    // display error message
                    Container.DisplayContent(new List<Pair<string, string>>() { { new Pair<string, string>("An error was encountered while updating patient.", "") } },
                        0, 1, MenuCodes.PATIENTS, "Patients", Description);

                    // wait for user to read message and confirm
                    Console.ReadKey();
                    break;
                }

            } while (patient != null && updatedPatient != null);           
        }
    }
}
