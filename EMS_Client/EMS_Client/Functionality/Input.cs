/**
 * \file Input.cs
*  \project INFO2180 - EMS System Term Project
*  \author The Char Stars - Tudor Lupu
*  \date 2018-12-4
*  \brief The menu that takes user input for the input fields
*  
*  This class is used throughout the program wherever there is
*  a form that requires the user to enter information
*/

using EMS_Client.MenuOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS_Client
{
    [Flags] public enum InputRetCode
    {
        SAVE = 1,
        UP = 2,
        DOWN = 4,
        TAB = 5
    }

    /** 
    * \class Input
    *
    * \brief <b>Brief Description</b> - This class is used to get user input for an input field
    * 
    * The Input class simply places the cursor and asks for user for a specific type of input
    * 
    * \author <i>The Char Stars - Tudor Lupu</i>
    */
    public class Input
    {
        private static char[] seperators = { '.', '/', '-', ',' };
        private const int OFFSET = 48;

        /**
        * \brief <b>Brief Description</b> - GetInput <b><i>class method</i></b> - main method used by outside methods to get the input
        * \details <b>Details</b>
        *
        * This takes in the current text that is in the input field(if any), the maximum allowed length for the input and what type of
        * input it should allow
        * 
        * \return <b>Pair<InputRetCode, string></b> - information the return and the return itself
        */
        public static Pair<InputRetCode, string> GetInput(string textInField, int maxFieldLength, InputType inputType)
        {
            Console.CursorVisible = true;
            ConsoleKeyInfo keyPressed = default(ConsoleKeyInfo);
            ConsoleModifiers keyModifiers = default(ConsoleModifiers);
            Int32 currentCursorPosition = Console.CursorLeft;

            //  the container returned. starts off with a successful message
            Pair<InputRetCode, string> retContainer = new Pair<InputRetCode, string>(InputRetCode.SAVE, textInField);

            int startingConsole = Console.CursorLeft;
            string oldValue = "";

            // run while the user doesnt decide to save
            while (keyPressed.Key != ConsoleKey.Enter && keyPressed.Key != ConsoleKey.Tab && retContainer.First == InputRetCode.SAVE)
            {
                // check if there should already be text in the input field             
                if (retContainer.Second != oldValue)
                {
                    Console.CursorVisible = false;
                    Console.CursorLeft = startingConsole;

                    // fill the input field with the value that should be there already
                    Console.Write(retContainer.Second + " \b");

                    Console.CursorVisible = true;
                    oldValue = retContainer.Second;
                }

                //  read the key pressed by the user and any modifier
                keyPressed = Console.ReadKey(true);
                keyModifiers = keyPressed.Modifiers;

                //  check if user is ready to move on to the next one
                if (!(keyPressed.Key == ConsoleKey.Enter) && !(keyPressed.Key == ConsoleKey.Tab))
                {
                    switch (keyPressed.Key)
                    {
                        case ConsoleKey.Escape:
                            //  check if the user wants to cancel this selection
                            retContainer.First = (InputRetCode)0;
                            break;
                        case ConsoleKey.UpArrow:
                            //  check if the user wants to cancel this current selection and go to the one above
                            retContainer.First = InputRetCode.UP;
                            break;
                        case ConsoleKey.DownArrow:
                            //  check if the user wants to cancel this current selection and go the the one below
                            retContainer.First = InputRetCode.DOWN;
                            break;;
                        case ConsoleKey.Backspace:
                            //  check if the user wants to erase a character
                            if (retContainer.Second.Length > 0)
                            {
                                retContainer.Second = retContainer.Second.Remove(retContainer.Second.Length - 1);
                                currentCursorPosition = Console.CursorLeft;
                                Console.Write(" \b");
                                Console.CursorLeft = currentCursorPosition;
                            }                            
                            break;
                        default:
                            //  check if shift is not pressed
                            char kc = keyPressed.KeyChar;
                            if (retContainer.Second.Length < maxFieldLength)
                            {
                                if ((inputType & InputType.Strings) != 0 && (char.IsLetter(kc) || kc == 32))
                                {
                                    retContainer.Second += char.ToUpper(kc);
                                }
                                if ((inputType & InputType.Ints) != 0 && char.IsDigit(kc))
                                {
                                    retContainer.Second += kc;
                                }
                                if ((inputType & InputType.Seperators) != 0 && seperators.Contains(kc))
                                {
                                    retContainer.Second += kc;
                                }
                            }
                            break;
                    }
                }

                // if the user pressed tab then it saves input and drops to the input field below
                if (keyPressed.Key == ConsoleKey.Tab) { retContainer.First = InputRetCode.SAVE | InputRetCode.DOWN; }
            }

            Console.CursorVisible = false;
            return retContainer;
        }

        /**
        * \brief <b>Brief Description</b> - ClearInputFields <b><i>class method</i></b> - clears the input fields
        * \details <b>Details</b>
        *
        * This takes in the current list of content that is being display for the input fields
        * 
        * \return <b>List<Pair<string, string>></b> - the cleared input field
        */
        public static List<Pair<string, string>> ClearInputFields(List<Pair<string, string>> content)
        {
            // loop through all lines and clear their current value
            foreach (Pair<string, string> line in content) { line.Second = ""; }

            return content;
        }

        /**
        * \brief <b>Brief Description</b> - IsValidChoice <b><i>class method</i></b> - checks if the input is a valid key pressed
        * \details <b>Details</b>
        *
        * This takes the key that was pressed and how many choices there are in the menu
        * 
        * \return <b>bool</b> - true/false depending on whether it is valid or not
        */
        public static bool IsValidChoice(ConsoleKey keyPressed, int menuChoices)
        {
            // check if the key is within the number key is within range
            return KeyToNumber(keyPressed) > 0 && KeyToNumber(keyPressed) <= menuChoices;
        }

        /**
        * \brief <b>Brief Description</b> - KeyToNumber <b><i>class method</i></b> - converts a key pressed to an integer
        * \details <b>Details</b>
        *
        * This takes a key pressed and converts it to an integer. Used to convert number keys to their int values
        * 
        * \return <b>int</b> - the integer of the key pressed
        */
        public static int KeyToNumber(ConsoleKey keyPressed)
        {
            return (int)keyPressed - OFFSET;
        }
    }
}
