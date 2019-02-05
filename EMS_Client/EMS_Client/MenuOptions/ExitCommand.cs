/**
 * \file ExitCommand.cs
*  \project INFO2180 - EMS System Term Project
*  \author The Char Stars - Tudor Lupu
*  \date 2018-12-4
*  \brief The main menu of the Exit quit command
*  
*  This class serves as the menu that asks the user
*  if they want to quit
*/

using EMS_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS_Client.MenuOptions
{
    /** 
    * \class ExitCommand
    *
    * \brief <b>Brief Description</b> - This class is a main menu option used to exit the program
    * 
    * The ExitCommand class is used to exit the program. Before exiting it displays an "Are you sure" screen
    * in order to get user confirmation.
    * 
    * \author <i>The Char Stars - Tudor Lupu</i>
    */
    class ExitCommand : Interfaces.IOption
    {
        #region public fields
        public string Description => "EXIT";
        #endregion

        /**
        * \brief <b>Brief Description</b> - Execute <b><i>class method</i></b> - gets confirmation that the user does indeed want to exit
        * \details <b>Details</b>
        *
        * This method is there only for the structure of the menu. It is one of the major menu
        * selections therefore it has no code behind. It it mostly accessed for its description.
        * 
        * \return <b>void</b>
        */
        public void Execute(Scheduling scheduling, Demographics demographics, Billing billing)
        {
            if (ExitConfirmation())
            {
                Environment.Exit(0);
            }
        }

        /**
        * \brief <b>Brief Description</b> - ExitConfirmation <b><i>class method</i></b> - gets confirmation that the user does indeed want to exit
        * \details <b>Details</b>
        *
        * This method requests the user to enter either 'Y' for Yes or 'N' for No on whether it should exit or not.
        * 
        * \return <b>void</b>
        */
        public bool ExitConfirmation()
        {
            // clear everything so the focus is on the confirmation screen
            Console.Clear();

            // loop until one of the two buttons was pressed
            do
            {             
                // display the message
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Are you sure you want to exit? (Y/N)");

                // wait for a key response
                ConsoleKey input = Console.ReadKey(true).Key;
                Console.Clear();

                // the user would like to exit
                if (input == ConsoleKey.Y)
                {
                    return true;
                }
                // the user does not want to exit
                else if (input == ConsoleKey.N)
                {
                    return false;
                }
                // wrong button was pressed
                else
                {                  
                    Console.BackgroundColor = ConsoleColor.Red;                   
                    Console.WriteLine("Error: Invalid Key pressed!");
                    Console.BackgroundColor = ConsoleColor.Black;                    

                }
            } while (true);
        }
    }
}
