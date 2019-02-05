/**
*  \file Container.cs
*  \project EMS Project
*  \author The Char Stars - Tudor Lupu
*  \date 2018-11-21
*  \brief Creates all the needed boxes for the UI
*  
*  This file contain the user inteface for the 
*  entire program, inclusing the accessing and 
*  implementation of all of the class libraries.
*/

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
    [Flags] public enum FooterFlags { Back = 1, Quit = 2 }

    /** 
    * \class Container
    *
    * \brief <b>Brief Description</b> - This class is meant to create all the boxes and display the content inside of them.
    * 
    * The Container class takes in what type of menu and its contents to display and then proceeds to display it.
    *
    * \author <i>The Char Stars - Tudor Lupu</i>
    */
    public class Container
    {
        #region private fields
        public const char BRCORNER = '┘';
        public const char BLCORNER = '└';
        public const char TRCORNER = '┐';
        public const char TLCORNER = '┌';
        public const char HORIZONTAL = '─';
        public const char VERTICAL = '│';
        public const char LJOINT = '├';
        public const char MJOINT = '┴';
        public const char RJOINT = '┤';
        #endregion

        /**
        * \brief <b>Brief Description</b> - DisplayContainerTop <b><i>class method</i></b> - Only displays the top part of a container
        * \details <b>Details</b>
        *
        * This takes in the title of the container and its width
        * 
        * \return <b>void</b>
        */
        public static void DisplayContainerTop(string title, int width)
        {
            // generate the top part of the container
            string topBar = GetContainerTop(title, width);

            // display it in white
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(topBar);
            Console.ForegroundColor = ConsoleColor.DarkGray;
        }

        /**
        * \brief <b>Brief Description</b> - GetContainerTop <b><i>class method</i></b> - Generates the top part of a container
        * \details <b>Details</b>
        *
        * This takes in the title of the container and its width
        * 
        * \return <b>string</b> - the container top
        */
        public static string GetContainerTop(string title, int width)
        {
            // generate the bar based on the width and add the title of it to the left
            string topBar = "" + TLCORNER;
            topBar = topBar.PadRight(2, HORIZONTAL);
            topBar += title.ToUpper();
            topBar = topBar.PadRight(width, HORIZONTAL);
            topBar += TRCORNER;

            return topBar;
        }

        /**
        * \brief <b>Brief Description</b> - DisplayHeader <b><i>class method</i></b> - Displays the header box of the UI
        * \details <b>Details</b>
        *
        * This takes in the title that is displayedd int he middle of the header
        * 
        * \return <b>void</b>
        */
        public static void DisplayHeader(string title)
        {
            string horizontalLine = new string(HORIZONTAL, Console.WindowWidth -3);
            string emptyLine = string.Format("{0}{1}{2}", VERTICAL, "".PadRight(Console.WindowWidth - 3, ' '), VERTICAL);

            // display the header in white
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(TLCORNER + horizontalLine + TRCORNER);
            Console.WriteLine(emptyLine);
            Console.WriteLine("{0} {1}{2}", VERTICAL, title.PadRight(Console.WindowWidth - 4, ' '), VERTICAL);
            Console.WriteLine(emptyLine);
            Console.WriteLine(BLCORNER + horizontalLine + BRCORNER);
            Console.ForegroundColor = ConsoleColor.DarkGray;

        }

        /**
        * \brief <b>Brief Description</b> - AddContentBox <b><i>class method</i></b> - Displays the menu box on the left together with the middle box and its content
        * \details <b>Details</b>
        *
        * This takes in the title of the middle box, the index of the selected option in the menu, the menu box and the content itself
        * 
        * \return <b>List<string></b> - middle section of the UI
        */
        public static List<string> AddContentBox(string title, int indexOfSelectedOption, List<string> menu, List<Pair<string, string>> content)
        {
            // generate the top of the middle container
            List<string> fullMiddleBox = new List<string>
            {
                menu[0] + " " + GetContainerTop(title, Console.WindowWidth - 29)
            };

            
            for (int index = 0; index < menu.Count-1; index++)
            {
                // check if it should fill the rest of the container with empty space or still add content
                if (index >= content.Count)
                {
                    // add empty space together with the '<' which points to the current option selected in the menu
                    fullMiddleBox.Add(menu[index + 1] + string.Format("{0}{1}{2}", (index == indexOfSelectedOption) ? "<" + VERTICAL : " "+ VERTICAL,
                "".PadRight(Console.WindowWidth - 30), VERTICAL));
                }
                else
                {
                    // add content together with the '<' which points to the current option selected in the menu
                    fullMiddleBox.Add(menu[index + 1] + string.Format("{0}{1}{2}", (index == indexOfSelectedOption) ? "<"+ VERTICAL : " " + VERTICAL,
                (content[index].First + content[index].Second).PadRight(Console.WindowWidth - 30), VERTICAL));
                }                   
            }

            return fullMiddleBox;
        }

        /**
        * \brief <b>Brief Description</b> - DisplayFooterContent <b><i>class method</i></b> - Displays the content of the footer
        * \details <b>Details</b>
        *
        * This takes in the flag with what buttons should be displayed and whether the content box is shown
        * 
        * \return <b>void</b>
        */
        public static void DisplayFooterContent(FooterFlags flags, bool isConetentBoxShown = false)
        {
            // create the top part of the container
            string topBar = string.Format("{0}{1}{2}{3}{4}", TLCORNER, "".PadRight(24, HORIZONTAL), MJOINT, "".PadRight(Console.WindowWidth - 28, HORIZONTAL), TRCORNER);

            // if the content box is shown give the top bar of the footer some extra joints in order to connect
            if (isConetentBoxShown) { topBar = string.Format("{0}{1}{2}{3}{4}{5}{6}", TLCORNER, "".PadRight(24, HORIZONTAL), MJOINT, HORIZONTAL, MJOINT, "".PadRight(Console.WindowWidth - 30, HORIZONTAL), RJOINT); }
            Console.WriteLine(topBar);

            // set the buttons based on the flags
            string back = ((flags & FooterFlags.Back) != 0) ? "ESC[BACK]" : "";
            string quit = ((flags & FooterFlags.Quit) != 0) ? "F9[QUIT]" : "";

            //  build the footer content depending on what buttons were passed in
            string footer = (quit == "") ? 
                string.Format("{0} {1}{2} {3}", Container.VERTICAL, back.PadRight(Console.WindowWidth / 2), quit.PadLeft(Console.WindowWidth / 2), Container.VERTICAL) :
                string.Format("{0} {1}{2} {3}", Container.VERTICAL, back.PadRight(20, ' '), quit.PadLeft(Console.WindowWidth - quit.Length - 17, ' '), Container.VERTICAL);

            // display the footer
            Console.WriteLine(footer);
            Console.WriteLine(string.Format("{0}{1}{2}", BLCORNER, "".PadRight(Console.WindowWidth - 3, HORIZONTAL), BRCORNER));
        }

        /**
        * \brief <b>Brief Description</b> - DisplayContent <b><i>class method</i></b> - Display the right menu box together with the content box and its contents
        * \details <b>Details</b>
        *
        * This takes in the content that needs to be displayed, the index of the item that is selected in the menu, the line that should be highlighted inside of the
        * container, the menu that should be displayed, the title of the menu and the title of the middle container
        * 
        * \return <b>void</b>
        */
        public static void DisplayContent(List<KeyValuePair<string, string>> content, int selectedMenuItem, int selectedInput,
            MenuCodes menuToDisplay, string menuTitle, string actionTitle)
        {
            List<Pair<string, string>> convertedList = new List<Pair<string, string>>();

            // converts the KeyValuePair to a Pair
            foreach(KeyValuePair<string, string> line in content)
            {
                convertedList.Add(new Pair<string, string>(line.Key, line.Value));
            }

            // call the overloaded DisplayContent with the Pair instead of the KeyValuePair
            DisplayContent(convertedList, selectedMenuItem, selectedInput, menuToDisplay, menuTitle, actionTitle);
        }

        /**
        * \brief <b>Brief Description</b> - DisplayContent <b><i>class method</i></b> - Display the right menu box together with the content box and its contents
        * \details <b>Details</b>
        *
        * This takes in the content that needs to be displayed, the index of the item that is selected in the menu, the line that should be highlighted inside of the
        * container, the menu that should be displayed, the title of the menu and the title of the middle container
        * 
        * \return <b>void</b>
        */
        public static void DisplayContent(List<Pair<string, string>> content, int selectedMenuItem, int selectedInput, 
            MenuCodes menuToDisplay, string menuTitle, string actionTitle)
        {
            // set the menu that needs to be displayed
            Menu menu = new Menu(menuToDisplay, menuTitle, selectedMenuItem);

            // generate the contents of the menu
            List<string> menuList = menu.CreateMenuContentList();
            List<string> fullContent = new List<string>();

            // set the cursor at the top of the window
            Console.CursorTop = 0;
            Console.CursorLeft = 0;

            // display the header with the default title
            Container.DisplayHeader("EMS System 2.0 - A system by Attila, Alex, Divyang and Tudor");

            // add the content box to the menu
            fullContent = Container.AddContentBox(actionTitle, selectedMenuItem, menuList, content);

            for (int count = 0; count < fullContent.Count; count++)
            {
                // display the menu part as grayed out
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(fullContent[count].Substring(0, 26));
                Console.ForegroundColor = ConsoleColor.White;

                // make the border of the middle container white
                if (fullContent[count].Substring(26).Contains(Container.HORIZONTAL) &&
                    !fullContent[count].Substring(26).Contains("" + Container.HORIZONTAL + Container.BRCORNER))
                {
                    Console.WriteLine(fullContent[count].Substring(26));
                }
                else
                {
                    Console.Write(fullContent[count].Substring(26, 2));
                    Console.ForegroundColor = (count == selectedInput) ? ConsoleColor.White : ConsoleColor.DarkGray;
                    Console.WriteLine(fullContent[count].Substring(28));
                }
            }

            // display the footer
            Container.DisplayFooterContent(FooterFlags.Back | FooterFlags.Quit, true);
        }


        /**
        * \brief <b>Brief Description</b> - DisplayRestOfMenu <b><i>class method</i></b> - Fills the rest of the menu with empty space
        * \details <b>Details</b>
        *
        * This takes in the options that the menu is displaying and the width of the menu
        * 
        * \return <b>void</b>
        */
        public static void DisplayRestOfMenu(string[] options, int width)
        {
            // set the non set menu options to blank lines
            foreach(string option in options)
            {
                if(option == null)
                {
                    Console.WriteLine("{0}{1}", "".PadRight(width, ' '), VERTICAL);
                }
            }
        }


        /**
        * \brief <b>Brief Description</b> - DisplayRestOfMenu <b><i>class method</i></b> - Fills the rest of the menu with empty space
        * \details <b>Details</b>
        *
        * This takes in a list of the options that the menu is displaying and the width of the menu
        * 
        * \return <b>List<string></b> - the new menu list containing the blank lines
        */
        public static List<string> DisplayRestOfMenu(List<string> options, int width)
        {
            int widthOffset = 9;

            // current length of the menu options
            int optionsLength = options.Count;

            // fill rest of menu with blank lines
            for (int count = 0; count < (Console.WindowHeight - (widthOffset + optionsLength)); count++)
            {
                options.Add(string.Format("{0}{1}", "".PadRight(width, ' '), VERTICAL));
            }

            return options;
        }
    }
}
