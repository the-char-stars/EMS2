/**
*  \file Client.cs
*  \project EMS Project
*  \author The Char Stars - Tudor Lupu
*  \date 2018-11-21
*  \brief The UI for the program, and the startup 
*  project for the solution
*  
*  This file contain the user inteface for the 
*  entire program, inclusing the accessing and implementation of all of the class libraries.
*/

using EMS_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

/** 
* \namespace EMS_Client
*
* \brief <b>Brief Description</b> - This namespace holds the user interface portion of the code base.
*
* \author <i>The Char Stars</i>
*/
namespace EMS_Client
{    
    /** 
   * \class Client
   *
   * \brief <b>Brief Description</b> - This class is meant to help navigate the system and access it's resources
   * 
   * The Program class ties everything together with the use of a as modern as possible console UI. The goal is to simply provide
   * the user with a smooth and intuitive interface to minimize keyboard strokes. It allows users to schedule/update appointments,
   * generate billing reports, find/update the patient roster and more.
   * 
   * <b>NOTE:</b> The intention is to have the menu
   *
   * \author <i>The Char Stars - Tudor Lupu</i>
   */
    class Client
    {

        #region private fields
        // this code for the window maximization was taken from:
        // https://www.gamedev.net/forums/topic/584288-how-can-i-make-a-console-app-full-screen-in-c/
        [DllImport("kernel32.dll", ExactSpelling = true)]

        private static extern IntPtr GetConsoleWindow();
        private static IntPtr ThisConsole = GetConsoleWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]

        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int HIDE = 0;
        private const int MAXIMIZE = 3;
        private const int MINIMIZE = 6;
        private const int RESTORE = 9;
        #endregion


        static void Main(string[] args)
        {
            // start the console in full screen
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            ShowWindow(ThisConsole, MAXIMIZE);

            // create the menu object
            Menu menu = new Menu(MenuCodes.MAINMENU, "main menu");

            // set the default look of the console
            SetUpConsole("EMS SYSTEM 2.0");

            // display the main header
            Container.DisplayHeader("EMS System 2.0 - A system by Attila, Alex, Divyang and Tudor");

            // display the main menu
            menu.DisplayMenu();

            // display the footer with only the quit button
            Container.DisplayFooterContent(FooterFlags.Quit);

            Scheduling scheduling = new Scheduling();
            Demographics demographics = new Demographics();
            Billing billing = new Billing();

            ConsoleKey input = Console.ReadKey(true).Key;
            do
            {
                // if the key the user pressed changed something
                if (menu.ChangeMenu(input, scheduling, demographics, billing))
                {
                    // hide the cursor and reset its position
                    Console.CursorTop = 0;
                    Console.CursorLeft = 0;
                    Console.CursorVisible = false;

                    // re-display the header
                    Container.DisplayHeader("EMS System 2.0 - A system by Attila, Alex, Divyang and Tudor");

                    // display the changed menu
                    menu.DisplayMenu();

                    // display the right footer depending on what menu the user is on
                    if (menu._selectedMenu == MenuCodes.MAINMENU) { Container.DisplayFooterContent(FooterFlags.Quit); }
                    else { Container.DisplayFooterContent(FooterFlags.Back | FooterFlags.Quit); }
                }

                // read another key from the user
                input = Console.ReadKey(true).Key;

            } while (input != ConsoleKey.X);


        }

        /**
        * \brief <b>Brief Description</b> - SetUpConsole <b><i>class method</i></b> - Set the default look of the console
        * \details <b>Details</b>
        *
        * This takes in the tile of the console and sets it
        * 
        * \return <b>void</b>
        */
        private static void SetUpConsole(string title)
        {
            // set the title of the console
            Console.Title = title;
            Console.CursorVisible = false;

            // set the colours of the console
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.DarkGray;

            Console.OutputEncoding = System.Text.Encoding.Unicode;

            Console.Clear();
        }

    }
}
