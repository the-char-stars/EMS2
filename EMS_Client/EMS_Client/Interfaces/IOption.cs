/**
 * \file IOption.cs
*  \project INFO2180 - EMS System Term Project
*  \author The Char Stars - Tudor Lupu
*  \date 2018-12-4
*  \brief The interfact for all the commands
*  
*  This is a basic interface for all the commands
*  to use(menu options).
*/

using EMS_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS_Client.Interfaces
{
    /** 
    * \interface IOption
    *
    * \brief <b>Brief Description</b> - This interface is used to define a basic layout of how menu commands should
    * look like
    * 
    * The IOption interface is simply just defining a layout for all the menu options and sub options. Every menu
    * command uses this layout.
    * 
    * \author <i>The Char Stars - Tudor Lupu</i>
    */
    interface IOption
    {
        // the description of the menu item
        string Description { get; }

        // the main entry point of that menu item
        void Execute(Scheduling scheduling, Demographics demographics, Billing billing);
    }
}
