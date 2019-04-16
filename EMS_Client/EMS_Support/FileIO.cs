/**
 * \file FileIO.cs
*  \project INFO2180 - EMS System Term Project
*  \author The Char Stars - Alex Kozak
*  \date 2018-11-16
*  \brief FileIO class definition and function declarations
*  
*  The functions in this file are used to setup the FileIO Class in the Support library. See class header comment for more information on the contents of this file
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace EMS_Library
{
    /** 
    * \class FileIO
    *
    * \brief <b>Brief Description</b> - This class is meant to facilitate the access to the database and other external files from all other libraries in the system
    * 
    * The FileIO class has access to the <i>databasePath</i> constant string, which is the determinant for where the database will be stored. This class is also 
    * apart of the EMS_Library namespace, which is common throughout all of the the non-UI based libraries found in the EMS System. The primary purpose of this 
    * library is to provide all of the other libraries in the system wih the opportunity to interact with external files, whether it be the database where
    * all application information is stored, or government generated files. The primary method for moving information is through the XML reader/writer, 
    * and generic implementations to change the DataSet data into usable peices of data in the form of either a DataTable or a Dictionary.
    * 
    * <b>NOTE:</b> The intention is to have a single FileIO object in an entire system to prevent file writing issues with the single common database. 
    * Because of this, the FileIO class should be initialized once, then simply passed through to all other classes which require it.
    *
    * \author <i>The Char Stars - Alex Kozak</i>
    */
    public class FileIO
    {
        #region Enums
        public enum TableNames : byte { Patients, Schedule, BillingCodes, Appointments, AppointmentBills, Users };     /**<\enum The Enums to reference which table the function wishes to access/modify.*/
        public enum FileInputFormat : byte { MasterBillingCode, BillingOutput, GovernmentResponse };                   /**<\enum The Enums to reference which parse type to use when parsing input files.*/
        public enum AccessLevel : int { InvalidCredentials = -1, Root, Physician, Reception };                         /**<\enum The Enums representing the level of access the user has*/
        #endregion

        #region Constant Variables
        private const string databasePath = "./DBase";              /**< The constant path to the database folder*/
        private const string databaseName = "DBase.xml";            /**< The name of the database file.*/
        private const string masterFileName = "masterFile.txt";     /**< The name of the master file.*/
        private const string backupFolderName = "Backups";          /**< The name of the Backups directory.*/
        private const string DatabaseConnectionString = "Server=2A314-E07;Database=EMS2;User Id=sa;Password=Conestoga1;";
        private const string databaseInfoFileName = "DBInfo";
        private const string currentDatabaseVersion = "0.0.3";

        private const string databaseFullPathFormat = "{0}/{1}";    /**< The stored full path of the database.*/
        #endregion

        #region Dictionaries
        public static readonly Dictionary<TableNames, string[]> dColumns = new Dictionary<TableNames, string[]>
            {
                { TableNames.Patients, new string[] { "PatientID", "FirstName", "LastName", "HCN", "MInitial", "DateOfBirth", "Gender", "HeadOfHouse", "AddressLine1", "AddressLine2", "City", "Province", "PhoneNum", "PostalCode" } },
                { TableNames.Schedule, new string[] { "AppointmentID", "AppointmentDate", "AppointmentTimeSlot" } },
                { TableNames.AppointmentBills, new string[] { "appointmentBillingID", "appointmentID", "patientID", "billingCode" } },
                { TableNames.BillingCodes, new string[] { "BillingCode", "Effective_Date", "Cost" } },
                { TableNames.Appointments, new string[] { "AppointmentID", "PatientID", "DependantID", "recallFlag" , "AppointmentNotes" } },
                { TableNames.Users, new string[] { "UserID", "UserName", "Password", "AccessLevel" } }
            };  /**< The names of the columns in each TableNames enum.*/

        private static readonly Dictionary<TableNames, string> dTableNames = new Dictionary<TableNames, string>
            {
                { TableNames.Patients, "tblPatients" },
                { TableNames.Schedule, "tblSchedule" },
                { TableNames.AppointmentBills, "tblAppointmentBillingRecords" },
                { TableNames.BillingCodes, "tblBillingCodes" },
                { TableNames.Appointments, "tblAppointments" },
                { TableNames.Users, "tblUsers" }
            };  /**< The string representation of the TableNames tables.*/

        private static readonly Dictionary<FileInputFormat, Delegate> dStringParse = new Dictionary<FileInputFormat, Delegate>
            {
                { FileInputFormat.MasterBillingCode, new Func<string, KeyValuePair<string, string[]>>(ParseMasterBillingCodeString) },
                { FileInputFormat.GovernmentResponse, new Func<string, KeyValuePair<string, string[]>>(ParseGovernmentResponseString) },
                { FileInputFormat.BillingOutput, new Func<string, KeyValuePair<string, string[]>>(ParseBillingOutputString) }
            };  /**< The functions used to parse each of the FileInputFormat file types*/
        #endregion

        /**
        * \brief <b>Brief Description</b> - ToString <b><i>override class method</i></b> - This returns the full relative path to the database file
        * \details <b>Details</b>
        *
        * This is the override for the ToString function for the FileIO class.
        * 
        * \return <b>string</b> - The full relative path to the database
        */
        public static string GetFullPath() { return string.Format(databaseFullPathFormat, databasePath, databaseName); }

        /**
        * \brief <b>Brief Description</b> - FileIO <b><i>class method</i></b> - Checks to see if there have been any structural changes to the database
        * \details <b>Details</b>
        *
        * Checks to see if there have been any structural changes to the database and migrates the data over to the new scheme
        * 
        * \return <b>VOID</b>
        */
        public static void CheckForUpdates()
        {
            CreateDatabaseDirectory();
            string dbInfoPath = string.Format(databaseFullPathFormat, databasePath, databaseInfoFileName);
            bool shouldConvert = true;

            if (File.Exists(dbInfoPath)) { shouldConvert = (File.ReadAllLines(dbInfoPath)[0] != currentDatabaseVersion); }
            else { File.WriteAllText(dbInfoPath, currentDatabaseVersion); }

            if (shouldConvert)
            {
                DataSet newDataSet = GenerateEmptyDataset();
                DataSet oldDataSet = GetDataSet(true);
                foreach (DataTable dt in oldDataSet.Tables)
                {
                    List<DataColumn> ldc = new List<DataColumn>();
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (!newDataSet.Tables[dt.TableName].Columns.Contains(dc.ColumnName))
                        {
                            ldc.Add(dc);
                        }
                    }
                    foreach (DataColumn dc in ldc) { dt.Columns.Remove(dc.ColumnName); }
                    foreach (DataColumn dc in newDataSet.Tables[dt.TableName].Columns)
                    {
                        if (!dt.Columns.Contains(dc.ColumnName))
                        {
                            dt.Columns.Add(dc.ColumnName);
                        }
                    }
                    foreach (DataRow dr in dt.Rows)
                    {
                        newDataSet.Tables[dt.TableName].Rows.Add(ConvertRowToStringArray(dr));
                    }
                }
                GenerateBackupDatabase();
                File.Delete(dbInfoPath);
                File.WriteAllText(dbInfoPath, currentDatabaseVersion);
                SaveDatabase(newDataSet);
            }
        }

        /**
        * \brief <b>Brief Description</b> - FileIO <b><i>class method</i></b> - Generate a backup database
        * \details <b>Details</b>
        *
        * This function generates a backup database in a backups directory
        * 
        * \return <b>VOID</b>
        */
        public static void GenerateBackupDatabase()
        {
            File.Copy(GetFullPath(), string.Format("{0}/{1}/{2}{3}", databasePath, backupFolderName, DateTime.Now.ToString("yyyy_MM_dd__hh_mm_ss__"), databaseName));
        }

        /**
        * \brief <b>Brief Description</b> - FileIO <b><i>class method</i></b> - This function attempts to log the user into the database
        * \details <b>Details</b>
        *
        * This function takes a username/password and attempts to login to the system
        * 
        * \return <b>AccessLevel</b> - An enum with the user's credentials
        */
        public static AccessLevel Login(string userName, string password)
        {
            List<string[]> ls = GetTableRecord(userName, "UserName", TableNames.Users);
            foreach (string[] sAr in ls)
            {
                if (sAr[2] == password) { return (AccessLevel)Int32.Parse(sAr[3]); }
            }
            return AccessLevel.InvalidCredentials;
        }

        /**
        * \brief <b>Brief Description</b> - FileIO <b><i>class method</i></b> - This creates the required direcory for the FileIO class
        * \details <b>Details</b>
        *
        * This creates the required direcory for the FileIO class using the databasePath constant string to determine the name of the directory to create
        * 
        * \return none -<b>void</b> - this method returns nothing        
        */
        private static void CreateDatabaseDirectory()
        {
            Directory.CreateDirectory(databasePath);
            Directory.CreateDirectory(string.Format("{0}/{1}", databasePath, backupFolderName));
        }

        /**
        * \brief <b>Brief Description</b> - FileIO <b><i>class method</i></b> - This function gets the column names of a given table
        * \details <b>Details</b>
        *
        * This function gets the column names of a given table as a dictionary of string[]
        * 
        * \return <b>Dictionary<TableNames, string[]></b> - The dictionary of table names
        */
        private static Dictionary<TableNames, string[]> GetColumnNames()
        {

            return dColumns;
        }

        /**
        * \brief <b>Brief Description</b> - FileIO <b><i>class method</i></b> - This function gets the dataset from the DBase.xml file
        * \details <b>Details</b>
        *
        * This function retreives the hard stored information in the database
        * 
        * \return <b>Dataset</b> - The stored DataSet from the hard storage file
        */
        private static DataSet GetDataSet(bool isCheck = false)
        {
            CreateDatabaseDirectory();
            if (!isCheck) CheckForUpdates();
            DataSet ds = new DataSet();
            string databaseFullPath = string.Format(databaseFullPathFormat, databasePath, databaseName);
            if (File.Exists(databaseFullPath))
            {
                // If the file already exists, simply read the data into the DataSet object
                ds.ReadXml(databaseFullPath, XmlReadMode.ReadSchema);
            }
            else
            {
                // If the file doesn't exist, create it and populate the file with an empty XML schema
                File.CreateText(databaseFullPath).Close();
                ds = GenerateEmptyDataset();
                ds.WriteXml(databaseFullPath, XmlWriteMode.WriteSchema);
                Logging.Log("FileIO", "Constructor", "Database does not exist, creating a new empty database.");
            }
            return ds;
        }

        /**
        * \brief <b>Brief Description</b> - FileIO <b><i>class method</i></b> - This function prints the given DataSet object to the console
        * \details <b>Details</b>
        *
        * This function takes the dictionaries for table names and column names and generates an empty
        * DataSet with the format defined by the dictionaries
        * 
        * \param none -<b>void</b> - this method takes no parameters
        * 
        * \return <b>Dataset</b> - The empty dataset with the structure of the DataSet contained in the database
        */
        public static DataSet GenerateEmptyDataset()
        {
            Logging.Log("FileIO", "generateEmptyDataset");
            DataSet emptyDataSet = new DataSet();
            foreach (TableNames table in dTableNames.Keys)
            {
                DataTable dataTable = new DataTable(dTableNames[table]);
                foreach (string columnTitle in GetColumnNames()[table]) { dataTable.Columns.Add(columnTitle); }
                emptyDataSet.Tables.Add(dataTable);
            }
            return emptyDataSet;
        }

        /**
        * \brief <b>Brief Description</b> - FileIO <b><i>class method</i></b> - This function prints the given DataSet object to the console
        * \details <b>Details</b>
        *
        * This is an overloaded function to take a given DataSet and print it to the console. It prints each table individually, with the columns
        * dynamically generated as well as the column names. The tables are seperated by a blank line.
        * 
        * \param printSet -<b>DataSet</b> - The DataSet to print
        * 
        * \return none -<b>void</b> - this method returns nothing
        * 
        * <exception cref="System.ArgumentNullException">Thrown if nothing the printSet is null. Must confirm not null as null values cannot have a .DataSetName and .Tables value.</exception>
        * 
        * \see PrintDataSet(), PrintTable()
        */
        public static List<string> PrintDataSet(DataSet printSet)
        {
            List<string> ls = new List<string>();
            if (printSet != null)
            {
                Logging.Log("FileIO", "printDataSet", string.Format("Printing DataSet {0}. {1} Tables:", printSet.DataSetName, printSet.Tables.Count));
                foreach (DataTable dataTable in printSet.Tables)
                {
                    ls.Add(string.Format("{0}:\n", dataTable.TableName));
                    PrintTable(dataTable, ls);
                    ls.Add("\n");
                }
            }
            return ls;
        }

        /**
        * \brief <b>Brief Description</b> - FileIO <b><i>class method</i></b> - This function prints the library's DataSet object to the console
        * \details <b>Details</b>
        *
        * This is an overloaded function to send the FileIO.dataSet to the printDataSet(DataSet) function
        * 
        * \param none -<b>void</b> - this method takes no parameters
        * 
        * \return none -<b>void</b> - this method returns nothing        
        * 
        * \see printDataSet(DataSet), printTable()
        */
        public static List<string> PrintDataSet()
        {
            return PrintDataSet(GetDataSet());
        }

        /**
        * \brief <b>Brief Description</b> - FileIO <b><i>class method</i></b> - This function prints a given DataTable to the console
        * \details <b>Details</b>
        *
        * This prints the DataTable with column name headers
        * 
        * \param printTable -<b>DataTable</b> - The DataTable to print
        * 
        * \return none -<b>void</b> - this method returns nothing        
        * 
        * <exception cref="System.ArgumentNullException">Thrown if nothing the printTable is null. Must confirm not null as null values cannot have a .Rows value.</exception>
        * 
        * \see PrintDataSet(), PrintDataSet(DataSet)
        */
        public static void PrintTable(DataTable printTable, List<string> ls)
        {
            if (printTable != null)
            {
                Logging.Log("FileIO", "PrintTable", string.Format("Printing Table {0}. {1} Rows, {2} Columns:", printTable.TableName, printTable.Rows.Count, printTable.Columns.Count));
                foreach (DataColumn dataColumn in printTable.Columns) { ls.Add(string.Format("    {0}", dataColumn.ColumnName)); }
                ls.Add("\n");
                foreach (DataRow dataRow in printTable.Rows)
                {
                    foreach (DataColumn dataColumn in printTable.Columns) { ls.Add("    " + ((String)dataRow[dataColumn, DataRowVersion.Current]).PadLeft(dataColumn.ColumnName.Length)); }
                    ls.Add("\n");
                }
            }
            else
            {
                Logging.Log("FileIO", "PrintTable", "printTable is null");
            }
        }

        /**
        * \brief <b>Brief Description</b> - FileIO <b><i>class method</i></b> - This function converts a DataTable to a Dictionary<string, string[]>
        * \details <b>Details</b>
        *
        * This function takes a DataTable and converts it into a dictionary. The format being the first value (typically ID)
        * is the Dictionary Key and all of the values in the row are stored in a string array as the value in the dictionary.
        * 
        * \param dataTable -<b>DataTable</b> - The DataTable to be converted to a dictionary
        * 
        * \return <b>Dictionary<string, string[]></b> - The resulting dictionary containing all rows from the given DataTable
        * 
        * <exception cref="System.ArgumentNullException">Thrown if nothing the dataTable is null. Must confirm not null as null values cannot have a .Rows value.</exception>
        * 
        * \see ConvertRowToStringArray(DataRow)
        */
        public static Dictionary<string, string[]> ConvertTableToDictionary(DataTable dataTable)
        {
            if (dataTable != null)
            {
                Logging.Log("FileIO", "ConvertTableToDictionary", string.Format("Converting Table {0} to Dictionary<string, string[]>", dataTable.TableName));
                Dictionary<string, string[]> dDataTable = new Dictionary<string, string[]>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    if (!dDataTable.ContainsKey(dataRow[0].ToString()))
                    {
                        dDataTable.Add(dataRow[0].ToString(), ConvertRowToStringArray(dataRow));
                    }
                    else
                    {
                        Logging.Log("FileIO", "ConvertTableToDictionary", "Duplicate record with ID " + dataRow[0]);
                        Logging.Log("FileIO", "ConvertTableToDictionary", ConvertRowToStringArray(dataRow));
                        Logging.Log("FileIO", "ConvertTableToDictionary", dDataTable[dataRow[0].ToString()]);
                    }
                }
                return dDataTable;
            }
            Logging.Log("FileIO", "ConvertTableToDictionary", "dataTable is null");
            return new Dictionary<string, string[]>();
        }

        /**
        * \brief <b>Brief Description</b> - FileIO <b><i>class method</i></b> - This function converts a DataRow to a string[]
        * \details <b>Details</b>
        *
        * This function takes a DataRow and converts it into a string[]. This is done in column order set by the DataRow
        * 
        * \param dataRow -<b>DataRow</b> - The DataRow to be converted to a string[]
        * 
        * \return <b>string[]</b> - The resulting dictionary containing all rows from the given DataTable
        * 
        * <exception cref="System.ArgumentNullException">Thrown if nothing the dataRow is null. Must confirm not null as null values cannot have a .ItemArray value.</exception>
        * 
        * \see ConvertTableToDictionary(DataTable)
        */
        public static string[] ConvertRowToStringArray(DataRow dataRow)
        {
            string[] s = dataRow != null ? Array.ConvertAll(dataRow.ItemArray, x => x.ToString()) : null;
            int i = 0;
            if (s != null) { foreach (string str in s) { if (str == "") s[i] = ""; i++; } }            
            return s;
        }

        /**
        * \brief <b>Brief Description</b> - FileIO <b><i>class method</i></b> - This function converts a Dictionary<string, string[]> to a DataTable
        * \details <b>Details</b>
        *
        * This function takes a Dictionary and converts the values into a DataTable in the format of the given TableNames enum
        * 
        * \param dDataTable -<b>Dictionary<string, string[]></b> - The DataRow to be converted to a string[]
        * \param tableName -<b>TableNames</b> - The enum of the table layout to be emulated
        * 
        * \return <b>DataTable</b> - The resulting DataTable containing all records from the given Dictionary
        * 
        * <exception cref="System.ArgumentNullException">Thrown if nothing the dDataTable is null. Must confirm not null as null values cannot have a .Keys value.</exception>
        * 
        * \see ConvertTableToDictionary(DataTable)
        */
        public static DataTable ConvertDictionaryToDataTable(Dictionary<string, string[]> dDataTable, TableNames tableName)
        {
            if (dDataTable != null)
            {
                Logging.Log("FileIO", "ConvertDictionaryToDataTable", string.Format("Converting Dictionary to a Table with the format of {0}", dTableNames[tableName]));
                DataTable dataTable = GetDataTableStructure(tableName);
                foreach (string key in dDataTable.Keys) { dataTable.Rows.Add(dDataTable[key]); }
                return dataTable;
            }
            Logging.Log("FileIO", "ConvertDictionaryToDataTable", "dDataTable is null");
            return new DataTable();
        }

        /**
        * \brief <b>Brief Description</b> - FileIO <b><i>class method</i></b> - This function parses a line from the Master Billing Code file
        * \details <b>Details</b>
        *
        * This function takes a line from the Master file of Billing Codes and conerts it into a KeyValuePair
        * containing the billingCode as the key and the portioned string as the string array value of the KeyValuePair
        * 
        * \param line -<b>string</b> - the string to be parsed
        * 
        * \return <b>KeyValuePair<string, string[]></b> - The Billing code key, all seperated values for the Value of the KeyValuePair
        * 
        * \see ParseBillingOutputString(string), ParseGovernmentResponseString(string)
        */
        private static KeyValuePair<string, string[]> ParseMasterBillingCodeString(string line)
        {
            // A665 20120401 00000913500
            string key = null;
            string[] value = { null };
            if (line.Length == 23)
            {
                key = line.Substring(0, 4);
                Regex d = new Regex("[A-Za-z][0-9]{3}");
                string codeString = line.Substring(0, 4);
                string dateString = line.Substring(4, 8);
                string costString = line.Substring(12, 10);
                DateTime dt = new DateTime();
                if (!d.IsMatch(codeString) || !DateTime.TryParseExact(dateString, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt) || !Int32.TryParse(costString, out int i))
                {
                    Logging.Log("FileIO", "ParseMasterBillingCodeString", "Error Value: " + value);
                    return new KeyValuePair<string, string[]>(null, null);
                }
                value = new string[] { codeString, dateString, costString};
                Logging.Log("FileIO", "ParseMasterBillingCodeString", value);                
            }
            return new KeyValuePair<string, string[]>(key, value);
        }

        /**
        * \brief <b>Brief Description</b> - FileIO <b><i>class method</i></b> - This function parses a line from the Billing Output file
        * \details <b>Details</b>
        *
        * This function takes a line from the Billing output file and conerts it into a KeyValuePair
        * containing an identifiable string as the key and the portioned string as the string array value of the KeyValuePair
        * 
        * \param line -<b>string</b> - the string to be parsed
        * 
        * \return <b>KeyValuePair<string, string[]></b> - The smallest identifiable string Key and the portioned string array Value
        * 
        * \see ParseBillingOutputString(string), ParseGovernmentResponseString(string)
        */
        private static KeyValuePair<string, string[]> ParseBillingOutputString(string line)
        {
            // 20171120 1234567890KV F A6650 0000913500
            string key = line.Length == 34 ? line.Substring(0, 24) : null;
            string[] value = { line.Substring(0, 8), line.Substring(8, 10), line.Substring(18, 1), line.Substring(18, 1), line.Substring(19, 4), line.Substring(23, 10) };
            Logging.Log("FileIO", "ParseBillingOutputString", value);
            return new KeyValuePair<string, string[]>(key,value);
        }

        /**
        * \brief <b>Brief Description</b> - FileIO <b><i>class method</i></b> - This function parses a line from the Government Billing Response file
        * \details <b>Details</b>
        *
        * This function takes a line from the Government Billing Response file and conerts it into a KeyValuePair
        * containing an identifiable string as the key and the portioned string as the string array value of the KeyValuePair
        * 
        * \param line -<b>string</b> - the string to be parsed
        * 
        * \return <b>KeyValuePair<string, string[]></b> - The smallest identifiable string Key and the portioned string array Value
        * 
        * \see ParseBillingOutputString(string), ParseMasterBillingCodeString(string)
        */
        private static KeyValuePair<string, string[]> ParseGovernmentResponseString(string line)
        {
            // 20171120 1234567890KV F A665 00000913500 PAID
            string key = null;
            string[] value = { null };
            if (line.Length == 40)
            {
                key = line.Substring(0, 25);
                Regex billCode = new Regex("[A-Za-z][0-9]{3}");
                Regex gender = new Regex("[MFX]");
                Regex hcn = new Regex("[0-9]{10}[a-zA-Z]{2}");
                List<string> br = new List<string> { "PAID", "DECL", "FHCV", "CMOH" };                
                string dateString = line.Substring(0, 8);
                string hcnString = line.Substring(8, 12);
                string genderstring = line.Substring(20, 1);
                string codeString = line.Substring(21, 4);
                string costString = line.Substring(25, 11);
                string govResponseString = line.Substring(36, 4);
                DateTime dt = new DateTime();
                if (!br.Contains(govResponseString) || !hcn.IsMatch(hcnString) || !gender.IsMatch(genderstring) || !billCode.IsMatch(codeString) || !DateTime.TryParseExact(dateString, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt) || !Int32.TryParse(costString, out int i))
                {
                    return new KeyValuePair<string, string[]>(null, null);
                }
                value = new string[] { dateString, hcnString, genderstring, codeString, costString, govResponseString };
                Logging.Log("FileIO", "ParseMasterBillingCodeString", value);
            }
            return new KeyValuePair<string, string[]>(key, value);
        }

        /**
        * \brief <b>Brief Description</b> - FileIO <b><i>class method</i></b> - This function merges a database table with an input file
        * \details <b>Details</b>
        *
        * This function takes parses a file from the disk into information to be stored in a DataTable, and merges
        * the information, careful not to add duplicate data objects, as confirmed by key
        * 
        * \param baseTable -<b>DataTable</b> - The table to merge with the file with
        * \param filePath -<b>string</b> - The path to the file containing the information to merge with the DataTable
        * \param FileInputFormat -<b>fileInputFormat</b> - The type of parsing to be done to each line of the file
        * 
        * \return <b>DataTable</b> - The resulting merged DataTable
        * 
        * <exception cref="ArgumentNullException">Thrown if nothing the baseTable or file string are null. Must confirm not null as null values cannot have a .TableName and .Rows value.</exception>
        * <exception cref="FileNotFoundException">Thrown if the file given is not found. Confirmed before MergeTebleWithFile is used that filePath is a file that exists</exception>
        * <exception cref="UnauthorizedAccessException">Thrown if the file given is inaccessable. Simply do not merge.</exception>
        * 
        * \see ParseMasterBillingCodeString(string), ParseBillingOutputString(string), ParseMasterBillingCodeString(string)
        */
        private static DataTable MergeTableWithFile(DataTable baseTable, string filePath, FileInputFormat fileInputFormat)
        {
            string[] masterList = File.ReadAllLines(filePath);
            Logging.Log(string.Format("{0} strings to be merged to the {1} table", masterList.Length, baseTable.TableName));
            Dictionary<string, string[]> dCurrentBaseTable = ConvertTableToDictionary(baseTable);
            foreach (string inputLine in masterList)
            {
                try
                {
                    KeyValuePair<string, string[]> kvpRecord = (KeyValuePair<string, string[]>)dStringParse[fileInputFormat].DynamicInvoke(inputLine);
                    if (kvpRecord.Key != null)
                    {
                        if (!dCurrentBaseTable.ContainsKey(kvpRecord.Key)) { baseTable.Rows.Add(kvpRecord.Value); }
                        else { Logging.Log("FileIO", "MergeTableWithFile", string.Format("Database already contains record with ID '{0}'", kvpRecord.Key)); }
                    }
                    else { Logging.Log("FileIO", "MergeTableWithFile", string.Format("Invalid string '{0}'", inputLine)); }
                }
                catch (Exception e) { Logging.Log(e, "FileIO", "MergeTableWithFile", inputLine); }
            }
            return baseTable;
        }

        /**
        * \brief <b>Brief Description</b> - FileIO <b><i>class method</i></b> - This function parses the lines of a file and returns a datatable of the elements in the file.
        * \details <b>Details</b>
        *
        * This function takes parses a file from the disk into information to be stored in a Dictionary, returning said dictionary.
        * 
        * \param filePath -<b>string</b> - The path to the file containing the information to merge with the DataTable
        * \param FileInputFormat -<b>fileInputFormat</b> - The type of parsing to be done to each line of the file
        * 
        * \return <b>DataTable</b> - The resulting merged DataTable
        * 
        * <exception cref="ArgumentNullException">Thrown if nothing the baseTable or file string are null. Must confirm not null as null values cannot have a .TableName and .Rows value.</exception>
        * <exception cref="FileNotFoundException">Thrown if the file given is not found. Confirmed before MergeTebleWithFile is used that filePath is a file that exists</exception>
        * <exception cref="UnauthorizedAccessException">Thrown if the file given is inaccessable. Simply do not merge.</exception>
        * 
        * \see ParseMasterBillingCodeString(string), ParseBillingOutputString(string), ParseMasterBillingCodeString(string)
        */
        public static Dictionary<string, string[]> GetTableFromFile(string filePath, FileInputFormat fileInputFormat)
        {
            if (File.Exists(filePath))
            {
                string[] masterList = File.ReadAllLines(filePath);
                Logging.Log(string.Format("{0} strings to be converted", masterList.Length));
                Dictionary<string, string[]> dCurrentBaseTable = new Dictionary<string, string[]>();
                foreach (string inputLine in masterList)
                {
                    try
                    {
                        KeyValuePair<string, string[]> kvpRecord = (KeyValuePair<string, string[]>)dStringParse[fileInputFormat].DynamicInvoke(inputLine);
                        if (kvpRecord.Key != null)
                        {
                            if (!dCurrentBaseTable.ContainsKey(kvpRecord.Key)) { dCurrentBaseTable.Add(kvpRecord.Key, kvpRecord.Value); }
                            else { Logging.Log("FileIO", "GetTableFromFile", string.Format("Dictionary already contains record with ID '{0}'", kvpRecord.Key)); }
                        }
                        else { Logging.Log("FileIO", "GetTableFromFile", string.Format("Invalid string '{0}'", inputLine)); }
                    }
                    catch (Exception e) { Logging.Log(e, "FileIO", "GetTableFromFile", inputLine); }
                }
                return dCurrentBaseTable;
            }
            else
            {
                return new Dictionary<string, string[]>();
            }
        }

        /**
        * \brief <b>Brief Description</b> - FileIO <b><i>class method</i></b> - This function updates the master list of billing codes
        * \details <b>Details</b>
        *
        * This function updates the table containing the billing codes, confirming with the master list file
        * 
        * \param masterBillingCodeFilePath -<b>string</b> - The path to the Master Billing Code file
        * 
        * \return none -<b>void</b> - this method returns nothing        
        * 
        * \see MergeTableWithFile(DataTable, string, FileInputFormat), ParseMasterBillingCodeString(string)
        */
        public static void UpdateBillingCodesFromFile(string masterBillingCodeFilePath = "masterFile.txt")
        {
            SetDataTable(GetDataTableStructure(TableNames.BillingCodes), TableNames.BillingCodes);
            Logging.Log("FileIO", "UpdateBillingCodesFromFile", string.Format("Attempting to update billing codes using the file {0}", masterBillingCodeFilePath));
            if (File.Exists(masterBillingCodeFilePath)) SetDataTable(MergeTableWithFile(GetDataTable(TableNames.BillingCodes), masterBillingCodeFilePath, FileInputFormat.MasterBillingCode), TableNames.BillingCodes);
            else Logging.Log("FileIO", "UpdateBillingCodesFromFile", string.Format("File {0} does not exist. Failed to update billing codes", masterBillingCodeFilePath));
        }

        /**
        * \brief <b>Brief Description</b> - FileIO <b><i>class method</i></b> - This function saves the database
        * \details <b>Details</b>
        *
        * This function saves the current DataSet to the hard database file
        * 
        * \return <b>bool</b> - If the save was successful or not
        */
        private static bool SaveDatabase(DataSet dataSet)
        {
            bool saveSuccessful = true;
            try
            {
                // writing the xml file
                dataSet.WriteXml(string.Format(databaseFullPathFormat, databasePath, databaseName), XmlWriteMode.WriteSchema);


                // sending all the table data to the SQL server database.
                string[] databaseTables = new string[] { "tblPatients", "tblSchedules", "tblBillingCodes", "tblAppointments", "tblAppointmentBillingRecords", "tblUsers" };

                for (int i = 0; i < dataSet.Tables.Count; i++)
                {
                    // using SQL bulk copy to copy entire table to the sql server
                    using (var bulkCopy = new SqlBulkCopy(DatabaseConnectionString, SqlBulkCopyOptions.KeepIdentity))
                    {

                        foreach (DataColumn col in dataSet.Tables[i].Columns)
                        {
                            bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName); // mapping all columns with sql tabel.
                        }

                        bulkCopy.BulkCopyTimeout = 600;
                        bulkCopy.DestinationTableName = databaseTables[i];
                        bulkCopy.WriteToServer(dataSet.Tables[i]);          // writing data to the sql server database.
                    }
                }

            }
            catch (Exception e)
            {
                saveSuccessful = false;
                Logging.Log(e, "FileIO", "SaveDatabase", "Unable to save Database");
            }
            return saveSuccessful;
        }

        /**
        * \brief <b>Brief Description</b> - FileIO <b><i>class method</i></b> - This function gets the next available row ID
        * \details <b>Details</b>
        *
        * This function generates the next available RowID
        * 
        * \param tableName <b>TableNames</b> - the table to have the ID generated from
        * 
        * \return <b>int</b> - the int representation of the given ID
        * 
        * <exception cref="IndexOutOfRangeException">Thrown if dataSet is null. Must confirm not null and contains all tables.</exception>
        * 
        * \see GenerateTableIDString(TableNames)
        */
        public static int GenerateTableID(TableNames tableName)
        {
            int newID = GetDataSet().Tables[dTableNames[tableName]].Rows.Count;
            Logging.Log("FileIO", "GenerateTableID", string.Format("table {0} generated next ID: {1}", dTableNames[tableName], newID.ToString()));
            return newID;
        }

        /**
        * \brief <b>Brief Description</b> - FileIO <b><i>class method</i></b> - This function gets the next available row ID
        * \details <b>Details</b>
        *
        * This function generates the next available ID as a string
        * 
        * \param tableName <b>TableNames</b> - the table to have the ID generated from
        * 
        * \return <b>string</b> - the string representation of the given ID
        * 
        * \see GenerateTableID(TableNames)
        */
        public static string GenerateTableIDString(TableNames tableName) { return IntToRecordID(GenerateTableID(tableName)); }

        /**
        * \brief <b>Brief Description</b> - FileIO <b><i>class method</i></b> - This function gets the next available row ID
        * \details <b>Details</b>
        *
        * This function converts an integer to the set ID string format
        * 
        * \param value <b>int</b> - Ihe integer to format to an ID string
        * 
        * \return <b>string</b> - the string representation of the given ID
        * 
        * \see GenerateTableIDString(TableNames), GetTableRecord(int, TableNames)
        */
        private static string IntToRecordID(int value) { return value.ToString(); }

        /**
        * \brief <b>Brief Description</b> - FileIO <b><i>class method</i></b> - This function returns a copy of a desired table
        * \details <b>Details</b>
        *
        * This function returns a copy of the desired table for manipulations in other libraries without fear of permanently damaging data
        * 
        * \param tableName <b>TableNames</b> - the table to return a copy of
        * 
        * \return <b>DataTable</b> - The DataTable copy to be manipulated by the other libraries
        * 
        * \see GetDataTableStructure(TableNames)
        */
        public static DataTable GetDataTable(TableNames tableName)             { return GetDataSet().Tables[dTableNames[tableName]].Copy(); }

        /**
        * \brief <b>Brief Description</b> - FileIO <b><i>class method</i></b> - This function returns an empty clone of a desired table
        * \details <b>Details</b>
        *
        * This function returns a clone of the desired table used 
        * 
        * \param tableName <b>TableNames</b> - the table to return an empty clone of
        * 
        * \return <b>DataTable</b> - The DataTable empty clone to be manipulated by the other libraries
        * 
        * \see GetDataTable(TableNames)
        */
        public static DataTable GetDataTableStructure(TableNames tableName) { return GetDataSet().Tables[dTableNames[tableName]].Clone(); }

        /**
        * \brief <b>Brief Description</b> - FileIO <b><i>class method</i></b> - This function returns a record given a int ID from any table in the database
        * \details <b>Details</b>
        *
        * This function searches for a single record with the given ID. if the specified ID is not in the table, then the function returns null.
        * 
        * \param recordID <b>int</b> - the record to search for in the database as an int
        * \param tableName <b>TableNames</b> - the table to search for the record
        * 
        * \return <b>string[]</b> - the string[] of the values representing the record, null if record not found.
        * 
        * \see GetTableRecord(string, string, TableNames, bool = false), GetTableRecord(string, TableNames)
        */
        public static string[] GetTableRecord(int recordID, TableNames tableName)
        {
            return GetTableRecord(IntToRecordID(recordID), tableName);
        }

        /**
        * \brief <b>Brief Description</b> - FileIO <b><i>class method</i></b> - This function returns a record given a string ID from any table in the database
        * \details <b>Details</b>
        *
        * This function searches for a single record with the given ID. if the specified ID is not in the table, then the function returns null.
        * 
        * \param recordID <b>string</b> - the record to search for in the database
        * \param tableName <b>TableNames</b> - the table to search for the record
        * 
        * \return <b>string[]</b> - the string[] of the values representing the record, null if record not found.
        * 
        * \see GetTableRecord(string, string, TableNames, bool = false), GetTableRecord(int, TableNames)
        */
        public static string[] GetTableRecord(string recordID, TableNames tableName)
        {
            Dictionary<string, string[]> dRecord = ConvertTableToDictionary(GetDataTable(tableName));
            if (dRecord != null && recordID != null)
            {
                return dRecord.ContainsKey(recordID) ? dRecord[recordID] : null;
            }
            return null;
        }

        /**
        * \brief <b>Brief Description</b> - FileIO <b><i>class method</i></b> - This searched for a record in a table given a column name (key) and value or partial value. 
        * \details <b>Details</b>
        *
        * This function allows for the search of specific records in a data table given both a key and value (column name and value). The boolean value partialMatch allows for
        * values which may contain the value query but does not exactly match, hence a partial match of the value. The returning dictionary is all values which match
        * the search criteria. If the value is not found then the return dictionary will not contain any records.
        * 
        * \param value <b>string</b> - The value to search for given the column
        * \param key <b>string</b> - The name of the column to search within
        * \param tableName <b>TableNames</b> - The table to search through
        * \param partialMatch <b>bool = false</b> - whether or not to include partial matches, default to false to only include exact matches.
        * 
        * \return <b>List<string[]></b> - The list of records matching the search criteria.
        * 
        * \see GetTableRecord(string, TableNames)
        */
        public static List<string[]> GetTableRecord(string value, string key, TableNames tableName, bool partialMatch = false)
        {
            List<string[]> dReturn = new List<string[]>();
            if (value != null && key != null)
            {
                DataTable searchTable = GetDataTable(tableName);
                if (searchTable.Columns.Contains(key))
                {
                    foreach (DataRow dr in searchTable.Rows)
                    {
                        if ((string)dr[key] == value || (partialMatch && ((String)dr[key]).Contains(value)))
                        {
                            dReturn.Add(ConvertRowToStringArray(dr));
                        }
                    }
                }
            }
            return dReturn;
        }

        /**
        * \brief <b>Brief Description</b> - FileIO <b><i>class method</i></b> - This function adds a single new record into the desired database
        * \details <b>Details</b>
        *
        * This function adds a single record in the form of a DataRow to the DataTable in the main DataSet and saves the change
        * 
        * \param dataRow <b>DataRow</b> - The DataRow to add to the desired table
        * \param tableName <b>TableNames</b> - the table to add the DataRow to
        * \param save <b>bool</b> - boolean representing if the database should be added at the end of the add. Defaults to true.
        * 
        * \return <b>bool</b> - Returns if the save was successful or true if save is false
        * 
        * \see AddRecordToDataTable(string[], TableNames, bool)
        */
        public static bool AddRecordToDataTable(DataRow dataRow, TableNames tableName, bool save = true)
        {
            return dataRow != null ? AddRecordToDataTable(ConvertRowToStringArray(dataRow), tableName, save) : false;
        }

        /**
        * \brief <b>Brief Description</b> - FileIO <b><i>class method</i></b> - This function adds a single new record into the desired database
        * \details <b>Details</b>
        *
        * This function adds a single record in the form of a string array to the DataTable in the main DataSet and saves the change. In the case that
        * a record has an ID of -1, the function generates a new ID for the row based on the number of values in the table.
        * 
        * \param dataRow <b>string[]</b> - The string array to add to the desired table
        * \param tableName <b>TableNames</b> - the table to add the DataRow to
        * \param save <b>bool</b> - boolean representing if the database should be added at the end of the add. Defaults to true.
        * 
        * \return <b>bool</b> - Returns if the save was successful or true if save is false
        * 
        * <exception cref="ArgumentNullException">Thrown if nothing dataRow is null, nothing to index, assumes not null. Confirm not null upon entry.</exception>
        * <exception cref="IndexOutOfRangeException">Thrown if dataRow is an empty string[] as it accesses the first element for the key. Will confirm number of elements before using.</exception>
        * 
        * \see AddRecordToDataTable(DataRow, TableNames, bool)
        */
        public static bool AddRecordToDataTable(string[] dataRow, TableNames tableName, bool save = true)
        {
            try
            {
                DataSet dataSet = GetDataSet();
                if (dataRow != null)
                {
                    Dictionary<string, string[]> dCurrentTableRecords = ConvertTableToDictionary(dataSet.Tables[dTableNames[tableName]]);
                    if (dataRow[0] == "-1")
                    {
                        dataRow[0] = GenerateTableIDString(tableName);
                        dataSet.Tables[dTableNames[tableName]].Rows.Add(dataRow);
                    }
                    else if (!dCurrentTableRecords.ContainsKey(dataRow[0])) { dataSet.Tables[dTableNames[tableName]].Rows.Add(dataRow); }
                    else
                    {
                        Logging.Log("FileIO", "AddRecordToDataTable", string.Format("Record with ID {0} already exists in table {1}", dataRow[0], dTableNames[tableName]));
                        return false;
                    }
                    Logging.Log("FileIO", "AddRecordToDataTable", string.Format("Adding record with ID {0} to table {1}", dataRow[0], dTableNames[tableName]));
                    return save ? SaveDatabase(dataSet) : true;
                }
            }
            catch (Exception e)
            {
                Logging.Log(e, "FileIO", "AddRecordToDataTable", "Likely the db is behind in version number");
            }
            return false;
        }

        /**
        * \brief <b>Brief Description</b> - FileIO <b><i>class method</i></b> - This function updates a single record from a table
        * \details <b>Details</b>
        *
        * This function updates a single record in the form of a string array from the DataTable in the main DataSet and saves the change. In the case that
        * an ID doesn't exist, the function returns false.
        * 
        * \param dataRow <b>DataRow</b> - The DataRow to add to the desired table
        * \param tableName <b>TableNames</b> - the table to add the DataRow to
        * \param save <b>bool</b> - boolean representing if the database should be added at the end of the add. Defaults to true.
        * 
        * \return <b>bool</b> - Returns false if the ID is not in the table or the save was unsuccessful.
        * 
        * \see public bool UpdateRecordFromTable(string[], TableNames, bool = true, DataRow = null)
        */
        public static bool UpdateRecordFromTable(DataRow dataRow, TableNames tableName, bool save = true)
        {
            return dataRow != null ? UpdateRecordFromTable(ConvertRowToStringArray(dataRow), tableName, save, dataRow) : false;
        }

        /**
        * \brief <b>Brief Description</b> - FileIO <b><i>class method</i></b> - This function updates a single record from a table
        * \details <b>Details</b>
        *
        * This function updates a single record in the form of a string array from the DataTable in the main DataSet and saves the change. In the case that
        * an ID doesn't exist, the function returns false.
        * 
        * \param dataRow <b>string[]</b> - The string array to add to the desired table
        * \param tableName <b>TableNames</b> - the table to add the DataRow to
        * \param save <b>bool</b> - boolean representing if the database should be added at the end of the add. Defaults to true.
        * \param dRow <b>DataRow</b> - if the original given was a DataRow, then pass it through for an easy removal from the DataTable
        * 
        * \return <b>bool</b> - Returns false if the ID is not in the table or the save was unsuccessful.
        * 
        * <exception cref="ArgumentNullException">Thrown if dataRow is null, nothing to index, assumes not null. Confirm not null upon entry.</exception>
        * <exception cref="IndexOutOfRangeException">Thrown if dataRow is an empty string[] as it accesses the first element for the key. Will confirm number of elements before using.</exception>
        * 
        * \see UpdateRecordFromTable(DataRow, TableNames, bool = true)
        */
        public static bool UpdateRecordFromTable(string[] dataRow, TableNames tableName, bool save = true, DataRow dRow = null)
        {
            DataSet dataSet = GetDataSet();
            if (dataRow != null)
            {
                Dictionary<string, string[]> dCurrentTableRecords = ConvertTableToDictionary(dataSet.Tables[dTableNames[tableName]]);
                if (dCurrentTableRecords.ContainsKey(dataRow[0]))
                {
                    if (dRow == null)
                    {
                        foreach (DataRow dr in dataSet.Tables[dTableNames[tableName]].Rows)
                        {
                            if ((string)dr[0] == dataRow[0])
                            {
                                dataSet.Tables[dTableNames[tableName]].Rows.Remove(dr);
                                break;
                            }
                        }
                    }
                    else { dataSet.Tables[dTableNames[tableName]].Rows.Remove(dRow); }
                    dataSet.Tables[dTableNames[tableName]].Rows.Add(dataRow);
                }
                else
                {
                    Logging.Log("FileIO", "UpdateRecordFromTable", string.Format("Record with ID {0} does not exist in table {1}. Not updating.", dataRow[0], dTableNames[tableName]));
                    return false;
                }
                Logging.Log("FileIO", "UpdateRecordFromTable", string.Format("Updating record with ID {0} in table {1}", dataRow[0], dTableNames[tableName]));
                return save ? SaveDatabase(dataSet) : true;
            }
            return false;
        }

        /**
        * \brief <b>Brief Description</b> - FileIO <b><i>class method</i></b> - This function merges a DataTable into the current DataSet
        * \details <b>Details</b>
        *
        * This function merges a given DataTable with the stored DataSet's version of the same dataTable
        * 
        * \param newDataTable <b>DataTable</b> - The DataTable containing the new information to be merged
        * \param tableName <b>TableNames</b> - The name of the table to be merged with
        * 
        * \return <b>bool</b> - Returns if the merge was sucessfully saved
        * 
        * \see MergeDataTable(Dictionary<string, string[]>, TableNames)
        */
        public static void MergeDataTable(DataTable newDataTable, TableNames tableName)
        {
            MergeDataTable(ConvertTableToDictionary(newDataTable), tableName);
        }

        /**
        * \brief <b>Brief Description</b> - FileIO <b><i>class method</i></b> - This function merges a DataTable into the current DataSet
        * \details <b>Details</b>
        *
        * This function merges a given DataTable with the stored DataSet's version of the same dataTable
        * 
        * \param newDataTable <b>Dictionary<string, string[]></b> - The dictionary containing the new information to be merged
        * \param tableName <b>TableNames</b> - The name of the table to be merged with
        * 
        * \return <b>bool</b> - Returns if the merge was sucessfully saved
        * 
        * <exception cref="ArgumentNullException">Thrown if newDataTable is null, nothing to index, assumes not null. Confirm not null upon entry.</exception>
        * 
        * \see MergeDataTable(DataTable, TableNames)
        */
        public static void MergeDataTable(Dictionary<string, string[]> newDataTable, TableNames tableName)
        {
            if (newDataTable != null)
            {
                int recordsAdded = 0;
                foreach (string recordKey in newDataTable.Keys) { if (AddRecordToDataTable(newDataTable[recordKey], tableName)) recordsAdded++; }
                Logging.Log("FileIO", "MergeDataTable", string.Format("Added {0} records to table {1}", recordsAdded, dTableNames[tableName]));
            }
        }

        /**
        * \brief <b>Brief Description</b> - FileIO <b><i>class method</i></b> - This function sets the Table to the given values
        * \details <b>Details</b>
        *
        * This function clears the current DataSet's DataTable and replaces it entirely with the given DataTable
        * 
        * \param newDataTable <b>DataTable</b> - The DataTable to replace the current DataTable with
        * \param tableName <b>TableNames</b> - The name of the table to be replaced
        * 
        * \return <b>bool</b> - Returns if the set was sucessfully saved
        * 
        * \see SetDataTable(Dictionary<string, string[]>, TableNames)
        */
        public static bool SetDataTable(DataTable newDataTable, TableNames tableName)
        {
            DataSet dataSet = GetDataSet();
            if (newDataTable != null)
            {
                Logging.Log("FileIO", "SetDataTable", string.Format("Replacing a DataTable containing {0} records with a Datatable containing {1}", dataSet.Tables[dTableNames[tableName]].Rows.Count, newDataTable.Rows.Count));
                dataSet.Tables[dTableNames[tableName]].Clear();
                dataSet.Tables[dTableNames[tableName]].Merge(newDataTable);
                return SaveDatabase(dataSet);
            }
            return false;
        }

        /**
        * \brief <b>Brief Description</b> - FileIO <b><i>class method</i></b> - This function sets the Table to the given values
        * \details <b>Details</b>
        *
        * This function clears the current DataSet's DataTable and replaces it entirely with the given Dictionary
        * 
        * \param newDataTable <b>Dictionary<string, string[]></b> - The dictionary representing a DataTable to replace the current DataTable with
        * \param tableName <b>TableNames</b> - The name of the table to be replaced
        * 
        * \return <b>bool</b> - Returns if the set was sucessfully saved
        * 
        * \see SetDataTable(DataTable, TableNames)
        */
        public static bool SetDataTable(Dictionary<string, string[]> newDataTable, TableNames tableName)
        {
            return SetDataTable(ConvertDictionaryToDataTable(newDataTable, tableName), tableName);
        }

        /**
        * \brief <b>Brief Description</b> - FileIO <b><i>class method</i></b> - This function outputs a dictionary to a new created file
        * \details <b>Details</b>
        *
        * This function takes a dictionary of values and prints it to a file with the name given to the function
        *        
        * \param filePath <b>string</b> - the output file name
        * \param recordsToAdd <b>Dictionary<string, object></b> - The dictionary representing a DataTable to replace the current DataTable with
        * 
        * \return <b>bool</b> - Returns if the new file was sucessfully saved
        */
        public static bool SaveToFile<T>(string fp, Dictionary<string, T> recordsToAdd)
        {
            if (fp != null && recordsToAdd != null)
            {
                List<string> dToString = new List<string>();
                foreach (string s in recordsToAdd.Keys) { dToString.Add(string.Format("{0} : {1}", s, recordsToAdd[s].ToString())); }
                return SaveToFile(fp, dToString);
            }
            return false;
        }

        /**
        * \brief <b>Brief Description</b> - FileIO <b><i>class method</i></b> - This function outputs a dictionary to a new created file
        * \details <b>Details</b>
        *
        * This function takes a dictionary of values and prints it to a file with the name given to the function
        *        
        * \param filePath <b>string</b> - the output file name
        * \param recordsToAdd <b>List<string></b> - The List of strings to print to the file
        * 
        * \return <b>bool</b> - Returns if the new file was sucessfully saved
        */
        public static bool SaveToFile(string fp, List<string> recordsToAdd)
        {
            string filePath = fp + ".txt";
            int i = 2;
            while (File.Exists(filePath)) { filePath = string.Format("{0}{1}.txt", fp, i++); }
            if (filePath != null && recordsToAdd != null && !File.Exists(filePath))
            {
                try
                {
                    StreamWriter sw = new StreamWriter(File.Open(filePath, FileMode.CreateNew));
                    foreach (string Title in recordsToAdd) { sw.WriteLine(Title); }
                    sw.Close();
                    Logging.Log("FileIO", "SaveToFile", string.Format("Successfully printed {0} items to '{1}'.", recordsToAdd.Count, filePath));
                    return true;
                }
                catch (Exception e)
                {
                    Logging.Log(e, "FileIO", "SaveToFile", "Unable to generate new file.");
                }
            }
            return false;
        }
    }
}