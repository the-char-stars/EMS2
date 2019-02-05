/**
 * \file DatePicker.cs
*  \project INFO2180 - EMS System Term Project
*  \author The Char Stars - Tudor Lupu
*  \date 2018-12-4
*  \brief The menu that asks the user to enter a date
*  
*  This class provides the capability to select a date in the main menu
*/

using EMS_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS_Client
{
    /** 
    * \class DatePicker
    *
    * \brief <b>Brief Description</b> - This class is asking the user to pick a date
    * 
    * The DatePicker class simply just displays a date and lets the user browse through
    * until the are set and cofirm a date.
    * 
    * \author <i>The Char Stars - Tudor Lupu</i>
    */
    public class DatePicker
    {
        #region private fields
        int _year;
        int _month;
        int _day;

        static DatePicker _picker;

        const int NUMBEROFMONTHS = 12;
        #endregion

        #region public fields
        public static string[] _months = new string[NUMBEROFMONTHS]
            {
                "January",
                "February",
                "March",
                "April",
                "May",
                "June",
                "July",
                "August",
                "September",
                "October",
                "November",
                "December"
            };
        #endregion

        private DatePicker()
        {
            _year = 2018;
            _month = 1;
            _day = 1;
        }

        /**
        * \brief <b>Brief Description</b> - GetDate <b><i>class method</i></b> - Entry point into getting date
        * \details <b>Details</b>
        *
        * This takes in the scheduling library, the date to which it should be set initialy and a bool indicating
        * whether it should ask for a day or not.
        * 
        * \return <b>DateTime</b> - The date selected
        */
        public static DateTime GetDate(Scheduling scheduling, DateTime startDate, bool getDay = true)
        {
            _picker = new DatePicker();
            return _picker.GetDay(scheduling, startDate, getDay);
        }

        private KeyValuePair<int, int> startPos = new KeyValuePair<int, int>(32, 6);

        /**
        * \brief <b>Brief Description</b> - GoToWindowOrigin <b><i>class method</i></b> - Sets the cursor to the right spot
        * \details <b>Details</b>
        *
        * This takes the X and the Y where the cursor should be set. If they are not passed in then it is assumed the user
        * is requesting for it to be set in the top left corner of the screen.
        * 
        * \return <b>void</b>
        */
        private void GoToWindowOrigin(int left = 0, int top = 0)
        {
            Console.CursorLeft = startPos.Key + left;
            Console.CursorTop = startPos.Value + top;
        }

        /**
        * \brief <b>Brief Description</b> - DisplayCurrentCalendar <b><i>class method</i></b> - Displays the calendar of the selected date
        * \details <b>Details</b>
        *
        * This takes the scheduling library, the date of the month the calendar should be set to and offsets for the display.
        * 
        * \return <b>void</b>
        */
        private void DisplayCurrentCalendar(Scheduling scheduling, DateTime dateTime, int xOffset = 5, int yOffset = 5)
        {
            Container.DisplayContent(ConvertIntoSingleList(GenerateCalendarList(scheduling.GetWeeksByMonth(dateTime), scheduling, dateTime.Month), xOffset, yOffset), 0, -1, MenuCodes.SCHEDULING, "Scheduling", "Select Date");
        }

        /**
        * \brief <b>Brief Description</b> - printInstructions <b><i>class method</i></b> - Prints the instructions on how to use the date picker
        * \details <b>Details</b>
        *
        * This takes in whether the date picker is also asking for a day and display the right information based on that.
        * 
        * \return <b>void</b>
        */
        private void printInstructions(bool getDay)
        {
            GoToWindowOrigin();
            string dayHelp = "Up/Down: +/- 1 day  |  Right/Left: +/- 7 days  |  M/N: +/- 1 month";

            // check which help it should display
            if (getDay) { Console.Write(dayHelp); }
            else { Console.Write("Up/Down: +/- 1 month".PadRight(dayHelp.Length)); }
        }

        /**
        * \brief <b>Brief Description</b> - GetDay <b><i>class method</i></b> - Selection of the day
        * \details <b>Details</b>
        *
        * This takes in the scheduling library, the date at which it should start getting input and a bool
        * indicating whether it will be getting the day too and not only just the month.
        * 
        * \return <b>DateTime</b> - the day selected
        */
        private DateTime GetDay(Scheduling scheduling, DateTime startDate, bool getDay)
        {
            Console.CursorVisible = true;
            DateTime retDate = startDate;
            DateTime displayDate = retDate;
            if (getDay) { DisplayCurrentCalendar(scheduling, displayDate); }
            while (true)
            {
                this._year = retDate.Year;
                this._month = retDate.Month;
                this._day = retDate.Day;

                // keep track of the date
                if (retDate.Month != displayDate.Month && getDay)
                {
                    displayDate = retDate;
                    DisplayCurrentCalendar(scheduling, displayDate);
                }

                Console.ForegroundColor = ConsoleColor.White;
                int xOffset = getDay ? 12 : 0;

                // print the instructions and move the cursor to the right spot
                printInstructions(getDay);
                GoToWindowOrigin(xOffset, 3);

                // ddisplay the date picking with the right current info
                if (getDay) { Console.Write("{0, -10} {1, -2}, {2}", _months[this._month - 1], this._day, this._year); }
                else { Console.Write("{0, -10}, {1, -20}", _months[this._month - 1], this._year); }
                GoToWindowOrigin(xOffset + (getDay ? 12 : 0), 3);

                ConsoleKey keyPressed = Console.ReadKey(true).Key;

                // read the key input based on what buttons are displayed
                if (getDay)
                {
                    switch (keyPressed)
                    {
                        // go the the day after the current one
                        case ConsoleKey.UpArrow:
                            retDate = retDate.AddDays(1);
                            continue;

                        // go to the day before the current one
                        case ConsoleKey.DownArrow:
                            retDate = retDate.AddDays(-1);
                            continue;

                        // go to the week before the current one
                        case ConsoleKey.LeftArrow:
                            retDate = retDate.AddDays(-7);
                            continue;

                        // go to the week after the current one
                        case ConsoleKey.RightArrow:
                            retDate = retDate.AddDays(7);
                            continue;

                        // go the the month after the current one
                        case ConsoleKey.M:
                            retDate = retDate.AddMonths(1);
                            continue;

                        // go to the month before the current one
                        case ConsoleKey.N:
                            retDate = retDate.AddMonths(-1);
                            continue;
                    }
                }
                else
                {
                    switch (keyPressed)
                    {
                        // go the the month after the current one
                        case ConsoleKey.UpArrow:
                            retDate = retDate.AddMonths(1);
                            continue;

                        // go to the month before the current one
                        case ConsoleKey.DownArrow:
                            retDate = retDate.AddMonths(-1);
                            continue;
                    }
                }

                // check if the user would like to select the date or cancel
                if (keyPressed == ConsoleKey.Enter) { break; }
                else if (keyPressed == ConsoleKey.Escape) { return DateTime.MinValue; }
            }

            return new DateTime(this._year, this._month, this._day);
        }

        /**
        * \brief <b>Brief Description</b> - GenerateCalendarList <b><i>class method</i></b> - Generates the contents of the calendar
        * \details <b>Details</b>
        *
        * This takes in the weeks in the selected month, the scheduling library and which month it is
        * 
        * \return <b>List<string[,]></b> - the contents of the calendar line by line
        */
        private static List<string[,]> GenerateCalendarList(List<Week> month, Scheduling scheduling, int monthInt)
        {
            List<string[]> retCalendar = new List<string[]>();

            // loop through every week in the month
            for (int week = 0; week < month.Count; week++)
            {
                // add a week for the calendar to the calendar
                retCalendar.Add(new string[7]);

                // loop through every day in the week
                for (int day = 0; day < 7; day++)
                {
                    // get the day object of the current day
                    Day dayOfWeek = month[week].GetAllAvailableDays()[day];

                    // create a string with the day number and number of free appointments on that day
                    if (scheduling.GetDateFromDay(dayOfWeek).Month == monthInt)
                    {
                        retCalendar[week][day] = string.Format("{0},({1})", scheduling.GetDateFromDay(dayOfWeek).Day, dayOfWeek.NumberOfScheduledAppointments());
                    }
                }
            }

            return FormatCalendar(retCalendar);
        }

        /**
        * \brief <b>Brief Description</b> - FormatCalendar <b><i>class method</i></b> - Puts the calendar content into the table
        * \details <b>Details</b>
        *
        * This takes the calendar with each week and days and adds each day into its own box for the calendar
        * 
        * \return <b>List<string[,]></b> - the formatted calendar content
        */
        private static List<string[,]> FormatCalendar(List<string[]> calendar)
        {
            List<string[,]> formattedCalendar = new List<string[,]>();

            // loop through every week of the month
            for (int week = 0; week < calendar.Count; week++)
            {
                // create an array withing the week array that contains the 4 slices in which
                // info will be displayed for the day
                formattedCalendar.Add(new string[7, 4]);

                // loop through every day slice so each slice in a day is set for every day in that week
                for (int daySlice = 0; daySlice < 4; daySlice++)
                {
                    // loop through the days in the week and setting the same index slice for all
                    for (int day = 0; day < 7; day++)
                    {
                        string format = "";

                        // check which slice it is on
                        switch (daySlice)
                        {
                            // first part of the day box
                            case 0:
                                format = string.Format("      {0}", Container.VERTICAL);
                                break;
                            // second part of the day box
                            case 1:
                                if (calendar[week][day] == null) { format = string.Format("      {0}", Container.VERTICAL); break; }
                                string pulledDay = calendar[week][day].Split(',')[0];
                                format = string.Format("  {0}  {1}",
                                    (pulledDay.Length == 1) ? "0" + pulledDay : pulledDay,
                                    Container.VERTICAL);
                                break;
                            // third part of the day box
                            case 2:
                                if (calendar[week][day] == null) { format = string.Format("      {0}", Container.VERTICAL); break; }
                                string apptNumber = calendar[week][day].Substring(calendar[week][day].IndexOf('(') + 1, 1);
                                format = string.Format(" ({0}) {1}",
                                    (apptNumber.Length == 1) ? "0" + apptNumber : apptNumber,
                                    Container.VERTICAL);
                                break;
                            // forth part of the day box
                            case 3:
                                format = string.Format("{0}{1}", "".PadRight(6, Container.HORIZONTAL), Container.BRCORNER);
                                break;
                        }

                        // insert the formatted slice in the right spot
                        formattedCalendar[week][day, daySlice] = format;
                    }
                }
            }

            return formattedCalendar;
        }

        /**
        * \brief <b>Brief Description</b> - ConvertIntoSingleList <b><i>class method</i></b> - converts the calendar into a displayable content list
        * \details <b>Details</b>
        *
        * This takes in the formatted calendar with the boxes and X&Y offsets for the calendar. The formatted calendar is converted into a list of
        * pairs in order to make it easier to display using the displaycontainer method
        * 
        * \return <b>List<Pair<string, string>></b> - the content to be displayed
        */
        private static List<Pair<string, string>> ConvertIntoSingleList(List<string[,]> calendar, int xOffset, int yOffset)
        {
            List<Pair<string, string>> retContent = new List<Pair<string, string>>();

            // add the Y offset
            for (int i = 0; i < yOffset; i++) { retContent.Add(new Pair<string, string>("", "")); }

            // add the the day above each column
            retContent.Add(new Pair<string, string>("".PadLeft(xOffset) + string.Format(" {0, -5}  {1, -5}  {2, -5}  {3, -5}  {4, -5}  {5, -5}  {6, -5}", "SUN", "MON", "TUE", "WED", "THU", "FRI", "SAT"), ""));

            // loop through all the weeks, days and slices and adds them as pairs to the content
            for (int week = 0; week < calendar.Count; week++)
            {
                for (int daySlice = 0; daySlice < 4; daySlice++)
                {
                    string line = "";

                    for (int day = 0; day < 7; day++)
                    {
                        line += calendar[week][day, daySlice];
                    }

                    retContent.Add(new Pair<string, string>("".PadLeft(xOffset) + line, ""));
                }
            }

            return retContent;
        }

        /**
        * \brief <b>Brief Description</b> - FormatMonth <b><i>class method</i></b> - formats the current selected month to look presentable
        * \details <b>Details</b>
        *
        * This takes in a month name and formats it.
        * 
        * \return <b>string</b> - the formatted month string
        */
        private static string FormatMonth(string month)
        {
            return string.Format("{0, 5}{1, -15}{2}", "<", month, ">");
        }
    }
}
