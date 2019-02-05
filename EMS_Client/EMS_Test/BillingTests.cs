using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EMS_Library;

namespace EMS_Test_Billing
{
    /** 
     * \class BillingTest
     *
     * \brief <b>Brief Description</b> - The class containing the test functions for the Billing class
     * 
     * The tests include Null, Functional, Exception, and boundary conditions
     *
     * \author <i>The Char Stars - Alex Kozak</i>
     */
    [TestClass]
    public class BillingTests
    {
        Billing b = new Billing();

        [TestMethod]
        [Owner("Attila")]
        [TestCategory("Functional")]
        public void Functional_AddNewRecord()
        {
            try
            {
                b.AddNewRecord("1234567", "123456789", "a003");
            }
            catch(Exception e)
            {
                Assert.Fail(e.Message);
            } 
        }

        [TestMethod]
        [Owner("Attila")]
        [TestCategory("Null")]
        public void Null_AddNewRecord()
        {
            try
            {
                b.AddNewRecord(null, null, null);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        [Owner("Attila")]
        [TestCategory("Null")]
        public void Exception_GenerateMonthlyBillingFile()
        {
            Scheduling tmpSchedule = new Scheduling();
            Demographics tmpDemo = new Demographics();
            try
            {
                b.GenerateMonthlyBillingFile(tmpSchedule, tmpDemo, 2018, 13);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        [Owner("Attila")]
        [TestCategory("Boundary")]
        public void Boundary_GenerateMonthlyBillingFile()
        {
            Scheduling tmpSchedule = new Scheduling();
            Demographics tmpDemo = new Demographics();
            try
            {
                b.GenerateMonthlyBillingFile(tmpSchedule, tmpDemo, Int32.MaxValue, Int32.MaxValue);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        [Owner("Attila")]
        [TestCategory("Functional")]
        public void Functional_GenerateMonthlyBillingFile()
        {
            Scheduling tmpSchedule = new Scheduling();
            Demographics tmpDemo = new Demographics();
            try
            {
                b.GenerateMonthlyBillingFile(tmpSchedule, tmpDemo, 2018, 12);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        [Owner("Attila")]
        [TestCategory("Functional")]
        public void Functional_FlagAppointment()
        {
            Scheduling tmpSchedule = new Scheduling();

            try
            {
                b.FlagAppointment(tmpSchedule, 0, 1);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        [Owner("Attila")]
        [TestCategory("Boundary")]
        public void Boundary_FlagAppointment()
        {
            Scheduling tmpSchedule = new Scheduling();

            try
            {
                b.FlagAppointment(tmpSchedule, Int32.MaxValue, Int32.MaxValue);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        [Owner("Attila")]
        [TestCategory("Functional")]
        public void Functional_ReconcileMonthlyBilling()
        {
            try
            {
                b.ReconcileMonthlyBilling("201812govFile.txt");
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        [Owner("Attila")]
        [TestCategory("Functional")]
        public void Functional_SaveApptBillingRecords()
        {
            try
            {
                b.SaveApptBillingRecords();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        [Owner("Attila")]
        [TestCategory("Functional")]
        public void Functional_UpdateRecord()
        {
            try
            {
                b.UpdateRecord("0", "0", "0", "a003");
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        [Owner("Attila")]
        [TestCategory("Null")]
        public void Null_FlagAppointment()
        {
            Scheduling tmpSchedule = new Scheduling();
            try
            {
                b.FlagAppointment(null, 0, 0);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        [Owner("Attila")]
        [TestCategory("Null")]
        public void Null_UpdateRecord()
        {
            Scheduling tmpSchedule = new Scheduling();
            try
            {
                b.UpdateRecord(null, null, null, null);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        [Owner("Attila")]
        [TestCategory("Null")]
        public void Functional_ApptBillRecord_Constructor()
        {
            string[] testString = { "1", "1", "1", "a003" };
            ApptBillRecord a = new ApptBillRecord(testString);

            string[] checkString = a.ToStringArray();

            CollectionAssert.AreEqual(testString, checkString);

        }
    }
}

