/**
*  \file SupportTests.cs
*  \project INFO2180 - EMS System Term Project
*  \author The Char Stars - Alex Kozak
*  \date 2018-11-16
*  \brief The unit tests for the Support Library
*  
*  The functions in this file are used to test the functions in the Support library.
*/

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EMS_Library;
using System.Data;
using System.IO;
using System.Collections.Generic;

/** 
* \namespace EMS_Test_Support
*
* \brief <b>Brief Description</b> - This namespace holds the testing functions and classes for the Support Library
*
* \author <i>The Char Stars - Alex Kozak</i>
*/
namespace EMS_Test_Support
{
    /** 
     * \class LoggingTest
     *
     * \brief <b>Brief Description</b> - The class containing the test functions for the Logging class
     * 
     * The tests include Stress, Functional, Exception, and boundary conditions
     *
     * \author <i>The Char Stars - Alex Kozak</i>
     */
    [TestClass]
    public class LoggingTest
    {
        #region Functional Tests
        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Functional")]
        public void Functional_LogSingle()
        {
            try
            {
                Logging.Log("This is a test message");
            }
            catch (Exception)
            {
                Assert.Fail();
            }            
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Functional")]
        public void Functional_LogClassMethod()
        {
            try
            {
                Logging.Log("Class", "Method");
                Logging.Log("Class", "Method", "ExtraInfo");
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Functional")]
        public void Functional_LogException()
        {
            try
            {
                Logging.Log(new Exception("ExceptionMessage"), "Class", "Method");
                Logging.Log(new Exception("ExceptionMessage"), "Class", "Method", "ExtraInfo");
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }
        #endregion

        #region Null Tests
        [TestMethod]
        [Owner("Alex")]
        [TestCategory("NullArgument")]
        public void Null_LogSingle()
        {
            try
            {
                Logging.Log(null);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("NullArgument")]
        public void Null_LogClassMethod()
        {
            try
            {
                Logging.Log(null, null);
                Logging.Log(null, null, (string)null);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("NullArgument")]
        public void Null_LogException()
        {
            try
            {
                Logging.Log(null, null, (string[])null);
                Logging.Log(null, null, null, null);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }
        #endregion

        #region Stress Tests
        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Stress")]
        public void Stress_LogSingle()
        {
            try
            {
                for (int i = 0; i < 100; i++)
                {
                    Logging.Log("This is a test string");
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Stress")]
        public void Stress_LogClassMethod()
        {
            try
            {
                for (int i = 0; i < 100; i++)
                {
                    Logging.Log("TestClass", "TestMethod");
                    Logging.Log("TestClass", "TestMethod", "TestExtraInfo");
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Stress")]
        public void Stress_LogException()
        {
            try
            {
                for (int i = 0; i < 100; i++)
                {
                    Logging.Log(new Exception("TestException"), "TestClass", "TestMethod");
                    Logging.Log(new Exception("TestException"), "TestClass", "TestMethod", "TestExtraInfo");
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }
        #endregion
    }

    /** 
     * \class FileIOTest
     *
     * \brief <b>Brief Description</b> - The class containing the test functions for the FileIO class
     * 
     * The tests include Stress, Functional, Exception, and boundary conditions
     *
     * \author <i>The Char Stars - Alex Kozak</i>
     */
    [TestClass]
    public class FileIOTest
    {
        DataTable dt = new DataTable();

        #region Functional Tests
        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Functional")]
        public void Functional_GetTables()
        {
            FileIO.GenerateEmptyDataset().WriteXml(FileIO.GetFullPath(), XmlWriteMode.WriteSchema);
            try
            {
                foreach (FileIO.TableNames t in Enum.GetValues(typeof(FileIO.TableNames)))
                {
                    FileIO.GetDataTable(t);
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Functional")]
        public void Functional_GetTableStructures()
        {
            FileIO.GenerateEmptyDataset().WriteXml(FileIO.GetFullPath(), XmlWriteMode.WriteSchema);
            try
            {
                foreach (FileIO.TableNames t in Enum.GetValues(typeof(FileIO.TableNames)))
                {
                    FileIO.GetDataTableStructure(t);
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Functional")]        
        public void Functional_PrintingTables()
        {
            FileIO.GenerateEmptyDataset().WriteXml(FileIO.GetFullPath(), XmlWriteMode.WriteSchema);
            try
            {
                FileIO.PrintDataSet();
                foreach (FileIO.TableNames t in Enum.GetValues(typeof(FileIO.TableNames)))
                {
                    List<string> ls = new List<string>();
                    FileIO.PrintTable(FileIO.GetDataTable(t), ls);
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Functional")]
        public void Functional_SetDataTable()
        {
            FileIO.GenerateEmptyDataset().WriteXml(FileIO.GetFullPath(), XmlWriteMode.WriteSchema);
            try
            {
                int currRows = 0;

                foreach (FileIO.TableNames t in Enum.GetValues(typeof(FileIO.TableNames)))
                {
                    dt = FileIO.GetDataTable(t);
                    currRows = dt.Rows.Count;
                    dt.Rows.Add(new string[] { "Test", "Test", "Test" });
                    FileIO.SetDataTable(dt, t);
                    dt = FileIO.GetDataTable(t);
                    if (currRows + 1 != dt.Rows.Count) { throw new Exception("Not saved"); }
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Functional")]
        public void Functional_AddDataTable()
        {
            FileIO.GenerateEmptyDataset().WriteXml(FileIO.GetFullPath(), XmlWriteMode.WriteSchema);
            try
            {
                int currRows = 0;

                foreach (FileIO.TableNames t in Enum.GetValues(typeof(FileIO.TableNames)))
                {
                    currRows = FileIO.GenerateTableID(t);
                    FileIO.AddRecordToDataTable(new string[] { "FileIOAddDataTable", "FileIOAddDataTable", "FileIOAddDataTable" }, t);
                    if (currRows + 1 != FileIO.GenerateTableID(t)) { throw new Exception("Not saved"); }

                    currRows = FileIO.GenerateTableID(t);
                    FileIO.AddRecordToDataTable(new string[] { "FileIOAddDataTable", "FileIOAddDataTable", "FileIOAddDataTable" }, t);
                    if (currRows != FileIO.GenerateTableID(t)) { throw new Exception("Added duplicate"); }
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Functional")]
        public void Functional_MergeDataTable()
        {
            FileIO.GenerateEmptyDataset().WriteXml(FileIO.GetFullPath(), XmlWriteMode.WriteSchema);
            try
            {
                int currRows = 0;

                foreach (FileIO.TableNames t in Enum.GetValues(typeof(FileIO.TableNames)))
                {
                    dt = FileIO.GetDataTable(t);
                    dt.Rows.Add(new string[] { "FileIOMergeDataTable", "FileIOMergeDataTable", "FileIOMergeDataTable" });
                    FileIO.AddRecordToDataTable(new string[] { "FileIOMergeDataTable", "FileIOMergeDataTable", "FileIOMergeDataTable" }, t);
                    currRows = FileIO.GenerateTableID(t);
                    FileIO.MergeDataTable(dt, t);
                    if (currRows != FileIO.GenerateTableID(t)) { throw new Exception("Not saved"); }
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
            FileIO.GenerateEmptyDataset().WriteXml(FileIO.GetFullPath(), XmlWriteMode.WriteSchema);
        }
        #endregion

        #region Null Tests

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("NullArgument")]
        public void Null_PrintDataset()
        {
            try { FileIO.PrintDataSet(null); }
            catch (Exception) { Assert.Fail(); }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("NullArgument")]
        public void Null_PrintTable()
        {
            try { FileIO.PrintTable(null, null); }
            catch (Exception) { Assert.Fail(); }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("NullArgument")]
        public void Null_ToDictionary()
        {
            try { FileIO.ConvertTableToDictionary(null); }
            catch (Exception) { Assert.Fail(); }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("NullArgument")]
        public void Null_ToDataTable()
        {
            try { FileIO.ConvertDictionaryToDataTable(null, FileIO.TableNames.Patients); }
            catch (Exception) { Assert.Fail(); }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("NullArgument")]
        public void Null_UpdateBillingCodes()
        {
            try { FileIO.UpdateBillingCodesFromFile(null); }
            catch (Exception) { Assert.Fail(); }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("NullArgument")]
        public void Null_GetTableRecord()
        {
            try
            {
                FileIO.GetTableRecord((string)null, FileIO.TableNames.Patients);
                FileIO.GetTableRecord(null, null, FileIO.TableNames.Patients);
            }
            catch (Exception) { Assert.Fail(); }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("NullArgument")]
        public void Null_AddRecordToTable()
        {
            try
            {
                FileIO.AddRecordToDataTable((DataRow)null, FileIO.TableNames.Patients);
                FileIO.AddRecordToDataTable((string[])null, FileIO.TableNames.Patients);
            }
            catch (Exception) { Assert.Fail(); }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("NullArgument")]
        public void Null_UpdateRecordFromTable()
        {
            try
            {
                FileIO.UpdateRecordFromTable((DataRow)null, FileIO.TableNames.Patients);
                FileIO.UpdateRecordFromTable((string[])null, FileIO.TableNames.Patients);
            }
            catch (Exception) { Assert.Fail(); }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("NullArgument")]
        public void Null_MergeDataTable()
        {
            try
            {
                FileIO.MergeDataTable((DataTable)null, FileIO.TableNames.Patients);
                FileIO.MergeDataTable((Dictionary<string, string[]>)null, FileIO.TableNames.Patients);
            }
            catch (Exception) { Assert.Fail(); }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("NullArgument")]
        public void Null_SetDataTable()
        {
            try
            {
                FileIO.SetDataTable((DataTable)null, FileIO.TableNames.Patients);
                FileIO.SetDataTable((Dictionary<string, string[]>)null, FileIO.TableNames.Patients);
            }
            catch (Exception) { Assert.Fail(); }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("NullArgument")]
        public void Null_SaveToFile()
        {
            try
            {
                FileIO.SaveToFile(null, (Dictionary<string, object>)null);
                FileIO.SaveToFile(null, (List<string>)null);
            }
            catch (Exception) { Assert.Fail(); }
        }

        #endregion

        #region Stress Tests

        #endregion
    }
}
