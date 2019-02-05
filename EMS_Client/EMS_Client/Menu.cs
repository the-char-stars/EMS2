/**
*  \file Menu.cs
*  \project EMS Project
*  \author The Char Stars - Tudor Lupu
*  \date 2018-11-21
*  \brief The logic behind the main menu. This is the menu that
*  calls all the other submenues
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS_Client.Interfaces;
using EMS_Client.MenuOptions;
using EMS_Client.MenuSpecificOptions;
using EMS_Library;

/** 
* \namespace EMS_Client
*
* \brief <b>Brief Description</b> - This namespace holds the user interface portion of the code base.
*
* \author <i>The Char Stars</i>
*/
namespace EMS_Client
{
    public enum MenuCodes
    {
        MAINMENU = 0,
        SCHEDULING,
        PATIENTS,
        BILLING
    }

    public enum FooterButtonCodes
    {
        HELP = 1,
        BACK = 5,
        QUIT = 9
    }

    /** 
  * \class Menu
  *
  * \brief <b>Brief Description</b> - This class is meant to create the logic behind the system main menu in order to access it's resources
  * 
  * The Menu class creates the main menu object and contains all of its logic and submenues. This is used all throughout the system.
  *
  * \author <i>The Char Stars - Tudor Lupu</i>
  */
    class Menu
    {
        #region private fields
        private static List<IOption[]> _listOfMenus = new List<IOption[]>();
        private static string _menuTitle { get; set; }
        private static int _selectedItem { get; set; }        

        private const int MAX_TITLE_SIZE = 20;
        private const int MAX_MIDBOX_HEIGHT = 20;
        private const int MENU_BOX_WIDTH = 25;
        #endregion

        #region public fields
        public MenuCodes _selectedMenu { get; set; }
        #endregion

        /**
        * \brief <b>Brief Description</b> - Menu - Constructor <b><i>class method</i></b> - This method is used when only the selected menu and title are known
        * \details <b>Details</b>
        *
        * This takes in the currently selected menu and its title and sets them
        * 
        * \return <b>void</b>
        */
        public Menu(MenuCodes menu, string title)
        {
            _selectedItem = 0;
            _selectedMenu = menu;
            _menuTitle = title;

            CreateMenuOptionList();
        }

        /**
        * \brief <b>Brief Description</b> - Menu - Constructor <b><i>class method</i></b> - This method is used when only the selected menu, title and index of selected item are known
        * \details <b>Details</b>
        *
        * This takes in the currently selected menu, its title and what the index of the currently selected item is
        * 
        * \return <b>void</b>
        */
        public Menu(MenuCodes menu, string title, int selectedItem)
        {
            _selectedItem = selectedItem;
            _selectedMenu = menu;
            _menuTitle = title;

            CreateMenuOptionList();
        }

        /**
        * \brief <b>Brief Description</b> - Menu <b><i>class method</i></b> - This method generates the content inside of the menu box
        * \details <b>Details</b>
        * 
        * \return <b>List<string></b> - the list of the lines inside the menu box
        */
        public List<string> CreateMenuContentList()
        {
            IOption[] options = _listOfMenus[(int)_selectedMenu];

            // generate the top part of the menu box
            List<string> content = new List<string>
            {
                Container.GetContainerTop(_menuTitle, MENU_BOX_WIDTH)
            };

            // generate the menu options
            for (int index = 0; index < options.Length; index++)
            {
                // initialize the prefix with the number of the option
                string selectPrefix = (index + 1).ToString() + ".";

                // check if the current menu item is selected
                if (index == _selectedItem)
                {
                    // change prefix if selected
                    selectPrefix = "■ ";
                }

                // add line to menu list
                content.Add(String.Format("{0, 4} {1, -20}{2}", selectPrefix,
                    options[index].Description, Container.VERTICAL));
            }

            // fill the rest of the menu with empty space
            content = Container.DisplayRestOfMenu(content, MENU_BOX_WIDTH);

            return content;
        }

        /**
        * \brief <b>Brief Description</b> - DisplayMenu <b><i>class method</i></b> - This method displays the menu with the selected option highlighted
        * \details <b>Details</b>
        * 
        * \return <b>void</b>
        */
        public void DisplayMenu()
        {
            IOption[] options = _listOfMenus[(int)_selectedMenu];
            string[] content = new string[Console.WindowHeight - 10];

            // display the top of the menu
            Container.DisplayContainerTop(_menuTitle, MENU_BOX_WIDTH);

            // display all the lines inside of the menu
            for (int index = 0; index < options.Length; index++)
            {
                string selectPrefix = (index + 1).ToString() + ".";

                // check if the current menu item is selected
                if (index == _selectedItem)
                {
                    // change prefix and make text white
                    selectPrefix = "■ ";
                    Console.ForegroundColor = ConsoleColor.White;
                }

                // format the content of the menu line
                content[index] = String.Format("{0, 4} {1, -20}{2}", selectPrefix,
                    options[index].Description, Container.VERTICAL);

                // display the menu line
                Console.WriteLine(content[index]);

                // set the line colour back to default
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }

            // display the rest of the menu as blank lines
            Container.DisplayRestOfMenu(content, MENU_BOX_WIDTH);
        }

        /**
        * \brief <b>Brief Description</b> - ChangeMenu <b><i>class method</i></b> - This method changes the menu based on the key pressed
        * \details <b>Details</b>
        * 
        * This takes in the key the user pressed, scheduling library, demographics library, billing library
        * 
        * \return <b>bool</b> - true if menu properties were changed 
        */
        public bool ChangeMenu(ConsoleKey key, Scheduling scheduling, Demographics demographics, Billing billing)
        {
            // switch index to number key pressed
            if (Input.IsValidChoice(key, _listOfMenus[(int)_selectedMenu].Length) &&
                Input.KeyToNumber(key) - 1 != _selectedItem)
            {
                _selectedItem = Input.KeyToNumber(key) - 1;
                return true;
            }

            switch (key)
            {
                // if up arrow decrement index
                case (ConsoleKey.UpArrow):
                    if (_selectedItem >= 1)
                    {
                        _selectedItem--;
                        return true;
                    }
                    break;
                // if down arrow increment index
                case (ConsoleKey.DownArrow):
                    if (_selectedItem <= (_listOfMenus[(int)_selectedMenu].Length - 2))
                    {
                        _selectedItem++;
                        return true;
                    }
                    break;
                // if F9 attempt to exit
                case (ConsoleKey.F9):
                    ExitCommand exit = new ExitCommand();
                    exit.Execute(scheduling, demographics, billing);
                    return true;
                // if escape go back to previous menu
                case (ConsoleKey.Escape):
                    if ((int)_selectedMenu > 0)
                    {
                        _selectedMenu = MenuCodes.MAINMENU;
                        _selectedItem = 0;
                        _menuTitle = "main menu";
                        return true;
                    }
                    break;
                // if enter select the current menu option
                case (ConsoleKey.Enter):                    
                    _listOfMenus[(int)_selectedMenu][(int)_selectedItem].Execute(scheduling, demographics, billing);
                    Console.Clear();

                    // set the new menu properties
                    if (_listOfMenus[(int)MenuCodes.MAINMENU].Contains(_listOfMenus[(int)_selectedMenu][(int)_selectedItem]))
                    {
                        _menuTitle = _listOfMenus[(int)_selectedMenu][(int)_selectedItem].Description;
                        _selectedMenu = (MenuCodes)_selectedItem + 1;
                        _selectedItem = 0;
                    }
                                      
                    return true;
            }

            return false;
        }

        /**
        * \brief <b>Brief Description</b> - CreateMenuOptionList <b><i>class method</i></b> - This method generates the menu and submenus option lists
        * \details <b>Details</b>
        * 
        * \return <b>void</b>
        */
        static void CreateMenuOptionList()
        {
            _listOfMenus.Clear();

            // MAIN MENU
            IOption[] mainMenu = new IOption[]
            {
                new SchedulingCommand(),
                new PatientsCommand(),
                new BillingCommand()
            };

            // SCHEDULING MENU
            IOption[] schedulingMenu = new IOption[]
            {
                new ScheduleApptCommand(),
                new ScheduleRecallCommand()
            };

            // PATIENT MENU
            IOption[] patientMenu = new IOption[]
            {
                new AddPatientCommand(),
                new UpdatePatientCommand(),
                new SearchPatientListCommand()
            };

            // BILLING MENU
            IOption[] billingMenu = new IOption[]
            {
                new GenerateMonthlyReportCommand(),
                new ReconcileMonthlyCommand(),
                new ViewReconcileSummaryCommand()
            };

            // add the option lists to the main list of menus
            _listOfMenus.Add(mainMenu);
            _listOfMenus.Add(schedulingMenu);
            _listOfMenus.Add(patientMenu);
            _listOfMenus.Add(billingMenu);
        }
    }
}
