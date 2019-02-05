/**
 * \file Logging.cs
*  \project INFO2180 - EMS System Term Project
*  \author The Char Stars - Alex Kozak
*  \date 2018-11-16
*  \brief Logging class definition and function declarations
*  
*  The functions in this file are used to setup the static Logging Class in the Support library. See class 
*  header comment for more information on the contents of this file
*/

using System;
using System.IO;

namespace EMS_Library
{
    /** 
    * \class Logging
    *
    * \brief <b>Brief Description</b> - This class is meant to facilitate the logging of function calls, errors, and various other messages from within the EMS system.    
    * 
    * The Logging class has access to the <i>logFilePath</i> constant which is the determinant for where all of the log files will be stored. This class is 
    * apart of the EMS_Library namespace, which is common throughout all of the the non-UI based libraries found in the EMS System. The primary use of this class 
    * is to log messages and other points of data throughout the system as easily and as accessable as possible. The <i>Logging.Log()</i> method is overloaded with
    * multiple formats to allow for the use of a single function in many different circumstances. 
    * 
    * <b>NOTE:</b> This is a <b>static</b> class so it cannot be instantiated. 
    *
    * \author <i>The Char Stars - Alex Kozak</i>
    */
    public static class Logging
    {
        private const string logFilePath = "./Log";               /**< The constant path to the log file folder*/
        private const string fileNameFormat = "{0}/ems.{1}.log";  /**< The format string for the log files filename.*/
        //private string currentLogFilePath;                      /**< The string for the current path to the log file. Important so the file can be closed between writes, allowing for it to be read while the program is still running.*/
        //private string currentDay;                              /**< The string representation of the current day, a pivotal part of the logfile name.*/

        /**
        * \brief <b>Brief Description</b> - Logging <b><i>class method</i></b> - This creates the required direcory for the Logging class
        * \details <b>Details</b>
        *
        * This creates the required direcory for the Logging class using the logFilePath constant string to determine the name of the directory to create
        * 
        * \return none -<b>void</b> - this method returns nothing        
        */
        private static void CreateLogFileDirectory() { System.IO.Directory.CreateDirectory(logFilePath); }

        /**
        * \brief <b>Brief Description</b> - <b><i>Class method</i></b> - Called to confirm that there is a log file generated for the current day
        * \details <b>Details</b>
        *
        * The generate function updates the currentDay/currentLogFilePath values to be consistent with the current day. If the file does not 
        * already exist, it creates the file with the propper name. 
        * 
        * \param none -<b>void</b> - this method takes no parameters
        * 
        * \return none -<b>void</b> - this method returns nothing   
        * 
        * <exception cref="UnauthorizedAccessException">Thrown if the attempted creation currentLogFilePath is inaccessable. If thrown, should return false to inform the caller.</exception>
        * <exception cref="FormatException">Thrown if the given date format is invalid, should be fine, but possible to break, therefore will be put in a try/catch block.</exception>
        * 
        */
        private static string GenerateLogFile()
        {
            CreateLogFileDirectory();
            string currentLogFilePath = string.Format(fileNameFormat, logFilePath, DateTime.Now.ToString("yyyy-MM-dd"));
            if (!File.Exists(currentLogFilePath)) { File.CreateText(currentLogFilePath).Close(); }
            return currentLogFilePath;
        }

        /**
        * \brief <b>Brief Description</b> - Log <b><i>class method</i></b> - This function logs the message given to the log file.
        * \details <b>Details</b>
        *
        * This is the worker function for the overloaded Log functions, all formatting strings to pass to this class. The function
        * adds the current date and time to the start of the message, and properly formats any information coming in. The resulting
        * formatted message is appended to the end of the current log file, which can be updated by the chechDate() function call
        * at the start of the function.
        * 
        * \return none -<b>void</b> - this method returns nothing        
        * 
        * <exception cref="FileNotFoundException">Thrown if currentLogFilePath is not found. will try/catch block this just in case. ConfirmCorrectLogFile should confirm the creation of the file though.</exception>
        * <exception cref="UnauthorizedAccessException">Thrown if currentLogFilePath is inaccessable. If thrown, do not log the message.</exception>
        * 
        * \see ConfirmCorrectLogFile(), Log(string, string, string[]), Log(string, string, string = ""), Log(Exception, string, string, string = "")
        */
        public static void Log(string logMessage)
        {                 
            StreamWriter logStream = File.AppendText(GenerateLogFile());
            logStream.WriteLine(string.Format("{0} {1}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), logMessage));
            logStream.Close();
        }

        /**
        * \brief <b>Brief Description</b> - Log <b><i>class method</i></b> - This function logs the message given to the log file.
        * \details <b>Details</b>
        *
        * This function formats the className, methodName, and string[] extraInfo into a single string to be passed to the main Log function.
        * 
        * \param className -<b>string</b> - The string representation of the class name
        * \param methodName -<b>string</b> - The string representation of the method name
        * \param extraInfo -<b>string[]</b> - The array of strings for any extra information to be added
        * 
        * \return none -<b>void</b> - this method returns nothing        
        * 
        * \see ConfirmCorrectLogFile(), Log(string), Log(string, string, string = ""), Log(Exception, string, string, string = "")
        */
        public static void Log(string className, string methodName, string[] extraInfo)
        {
            Log(string.Format("{0}.{1} - {{2}}", className, methodName, string.Join(", ", extraInfo ?? (new string[0]))));
        }

        /**
        * \brief <b>Brief Description</b> - Log <b><i>class method</i></b> - This function logs the message given to the log file.
        * \details <b>Details</b>
        *
        * This function formats the className, methodName, and <b>optional</b> string extraInfo into a single string to be passed to the main Log function.
        * 
        * \param className -<b>string</b> - The string representation of the class name
        * \param methodName -<b>string</b> - The string representation of the method name
        * \param extraInfo -<b>string</b> - The <i>optional</i> string for any extra information to be added
        * 
        * \return none -<b>void</b> - this method returns nothing        
        * 
        * \see ConfirmCorrectLogFile(), Log(string), Log(string, string, string[]), Log(Exception, string, string, string = "")
        */
        public static void Log(string className, string methodName, string extraInfo = "")
        {
            Log(string.Format("{0}.{1} logging event - {2}", className, methodName, extraInfo));
        }

        /**
        * \brief <b>Brief Description</b> - Log <b><i>class method</i></b> - This function logs the message given to the log file.
        * \details <b>Details</b>
        *
        * This function formats the exception message, className, methodName, and <b>optional</b> string extraInfo into a 
        * single string to be passed to the main Log function.
        * 
        * \param e -<b>Exception</b> - The exception to get the message from
        * \param className -<b>string</b> - The string representation of the class name
        * \param methodName -<b>string</b> - The string representation of the method name
        * \param extraInfo -<b>string</b> - The <i>optional</i> string for any extra information to be added
        * 
        * \return none -<b>void</b> - this method returns nothing        
        * 
        * <exception cref="ArgumentNullException">Exception e can throw an error as it attempts to get the message of the exception, thus confirm that e is not null before use.</exception>
        * 
        * \see ConfirmCorrectLogFile(), Log(string), Log(string, string, string[]), Log(string, string, string = "")
        */
        public static void Log(Exception e, string className, string methodName, string extraInfo = "")
        {
            Log(string.Format("ERROR in {0}.{1}: {2} - {3}", className, methodName, e != null ? e.Message : "", extraInfo));
        }
    }
}