﻿/**
 * \file BillingCommand.cs
*  \project INFO2180 - EMS System Term Project
*  \author The Char Stars - Tudor Lupu
*  \date 2018-12-4
*  \brief The main menu of the Billing
*  
*  This class serves as only the main billing menu.
*  There is no functionality
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS_Client.Interfaces;
using EMS_Library;

namespace EMS_Client.MenuOptions
{
    /** 
    * \class PatientsCommand
    *
    * \brief <b>Brief Description</b> - This class is a main menu option with no functionality
    * 
    * The PatientsCommand class has no functionality. It is only used to display the right menu options
    * in the main menu
    * 
    * \author <i>The Char Stars - Tudor Lupu</i>
    */
    class BillingCommand : IOption
    {
        #region public fields
        public string Description => "Billing";
        #endregion

        /**
        * \brief <b>Brief Description</b> - Execute <b><i>class method</i></b> - does nothing
        * \details <b>Details</b>
        *
        * This method is there only for the structure of the menu. It is one of the major menu
        * selections therefore it has no code behind. It it mostly accessed for its description.
        * 
        * \return <b>void</b>
        */
        public void Execute(Scheduling scheduling, Demographics demographics, Billing billing)
        {
        }
    }
}
