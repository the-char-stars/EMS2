using System;
using System.Collections.Generic;
using EMS_Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EMS_Test_Scheduling
{
    [TestClass]
    public class DayTest
    {
        Day day = new Day();

        #region Functional Tests
        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Functional")]
        public void Functional_AddAppointment()
        {
            try
            {
                if (!day.AddAppointment(new Appointment(100, 10, 1), 0)) throw new Exception();
                if (!IsCorrectPatientID(100, 0)) throw new Exception();
            }
            catch (Exception e)
            {
                Logging.Log(e, "DayTests", "Functional_AddAppointment");
                Assert.Fail();
            }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Functional")]
        public void Functional_GetAppointments()
        {
            Day d = new Day();
            try
            {
                d.GetAppointments();
            }
            catch (Exception e)
            {
                Logging.Log(e, "DayTests", "Functional_GetAppointments");
                Assert.Fail();
            }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Functional")]
        public void Functional_DeleteAppointment()
        {
            try
            {
                day.DeleteAppointment(0);
                if (!IsCorrectPatientID(-1, 0)) throw new Exception();
            }
            catch (Exception e)
            {
                Logging.Log(e, "DayTests", "Functional_DeleteAppointment");
                Assert.Fail();
            }
        }

        public bool IsCorrectPatientID(int patientID, int index)
        {
            return day.GetAppointments()[index].PatientID == patientID;
        }
        #endregion

        #region Null Tests
        [TestMethod]
        [Owner("Alex")]
        [TestCategory("NullArgument")]
        public void Null_Constructor()
        {
            try
            {
                Day d = new Day(DayOfWeek.Sunday, null);
            }
            catch (Exception e)
            {
                Logging.Log(e, "DayTests", "Null_Constructor");
                Assert.Fail();
            }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("NullArgument")]
        public void Null_AddAppointment()
        {
            try
            {
                day.AddAppointment(null, 0);
            }
            catch (Exception e)
            {
                Logging.Log(e, "DayTests", "Null_AddAppointment");
                Assert.Fail();
            }
        }
        #endregion

        #region Exception Tests
        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Exception")]
        public void Exception_AddAppointment()
        {
            try
            {
                if (day.AddAppointment(new Appointment(100, 10, 1), 10)) throw new Exception();
                if (IsCorrectPatientID(100, 0)) throw new Exception();
            }
            catch (Exception e)
            {
                Logging.Log(e, "DayTests", "Functional_AddAppointment");
                Assert.Fail();
            }
        }


        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Functional")]
        public void Exception_DeleteAppointment()
        {
            try
            {
                day.DeleteAppointment(100);
            }
            catch (Exception e)
            {
                Logging.Log(e, "DayTests", "Functional_DeleteAppointment");
                Assert.Fail();
            }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Functional")]
        public void Exception_UpdateAppointment()
        {
            try
            {
                day.UpdateAppointment(new Appointment(), 100);
            }
            catch (Exception e)
            {
                Logging.Log(e, "DayTests", "Functional_DeleteAppointment");
                Assert.Fail();
            }
        }
        #endregion
    }

    [TestClass]
    public class WeekTest
    {
        Scheduling scheduling = new Scheduling();
        Week week = new Week(DateTime.Now);

        #region Functional Tests
        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Functional")]
        public void Functional_GetAllAvailableDays()
        {
            try { week.GetAllAvailableDays(); }
            catch (Exception) { Assert.Fail(); }
        }
        #endregion

        #region Null Tests
        [TestMethod]
        [Owner("Alex")]
        [TestCategory("NullArgument")]
        public void Null_Constructor()
        {
            try
            {
                Week d = new Week(new DateTime());
                d = new Week(null);
            }
            catch (Exception) { Assert.Fail(); }
        }
        #endregion
    }

    [TestClass]
    public class AppointmentTest
    {
        #region Functional Tests
        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Functional")]
        [DataRow(new string[] { "0", "1", "1", "1" }, 0, 1, 1, 1)]
        [DataRow(new string[] { "0", "0", "0", "0" }, 0, 0, 0, 0)]
        [DataRow(new string[] { "0", "200", "13", "0" }, 0, 200, 13, 0)]
        public void Functional_Constructor(string[] apptInfo, int appointmentID, int patientID, int dependantID, int recallFlag)
        {
            Appointment a = new Appointment(apptInfo);
            Assert.AreEqual(a.AppointmentID, appointmentID);
            Assert.AreEqual(a.PatientID, patientID);
            Assert.AreEqual(a.DependantID, dependantID);
            Assert.AreEqual(a.RecallFlag, recallFlag);
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Functional")]
        [DataRow(new string[] { "0", "1", "1", "1" }, 0, 1, 1, 1)]
        [DataRow(new string[] { "0", "0", "0", "0" }, 0, 0, 0, 0)]
        [DataRow(new string[] { "0", "200", "13", "0" }, 0, 200, 13, 0)]
        public void Functional_ToStringArray(string[] apptInfo, int appointmentID, int patientID, int dependantID, int recallFlag)
        {
            Appointment a = new Appointment(appointmentID, patientID, dependantID, recallFlag);
            string[] s = a.ToStringArray();
            for (int i = 0; i < apptInfo.Length; i++)
            {
                Assert.AreEqual(apptInfo[i], s[i]);
            }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Functional")]
        [DataRow(0, 1, 1, 1)]
        [DataRow(0, 0, 0, 0)]
        [DataRow(0, 200, 13, 0)]
        public void Functional_UpdateAppointment(int appointmentID, int patientID, int dependantID, int recallFlag)
        {
            Appointment a = new Appointment();
            a.UpdateAppointment(patientID, dependantID, recallFlag);
            Assert.AreEqual(a.PatientID, patientID);
            Assert.AreEqual(a.DependantID, dependantID);
            Assert.AreEqual(a.RecallFlag, recallFlag);            
        }
        #endregion

        #region Exception Tests
        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Exception")]
        [DataRow(new string[] { "0", "1", "1", "1" }, 0, 1, 1, 1)]
        [DataRow(new string[] { "0", "0", "0", "0" }, 0, 0, 0, 0)]
        [DataRow(new string[] { "0", "200", "13", "0" }, 0, 200, 13, 0)]
        public void Exception_ToStringArray(string[] apptInfo, int appointmentID, int patientID, int dependantID, int recallFlag)
        {
            Appointment a = new Appointment(appointmentID, patientID, dependantID, recallFlag);
            string[] s = a.ToStringArray();
            for (int i = 0; i < apptInfo.Length; i++)
            {
                Assert.AreEqual(apptInfo[i], s[i]);
            }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Exception")]
        [DataRow(0, 1, 1, 1)]
        [DataRow(0, 0, 0, 0)]
        [DataRow(0, 200, 13, 0)]
        public void Exception_UpdateAppointment(int appointmentID, int patientID, int dependantID, int recallFlag)
        {
            Appointment a = new Appointment();
            a.UpdateAppointment(patientID, dependantID, recallFlag);
            Assert.AreEqual(a.PatientID, patientID);
            Assert.AreEqual(a.DependantID, dependantID);
            Assert.AreEqual(a.RecallFlag, recallFlag);
        }
        #endregion

        #region Null Tests
        [TestMethod]
        [Owner("Alex")]
        [TestCategory("NullArgument")]
        public void Null_Constructor()
        {
            try { Appointment a = new Appointment(null); }
            catch (Exception) { Assert.Fail(); }
        }
        #endregion
    }

    [TestClass]
    public class SchedulingTest
    {
        Scheduling s = new Scheduling();

        #region Functional Tests
        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Functional")]
        public void Functional_GetAllAppointmentsForPatient()
        {
            try { s.GetAllAppointmentsForPatient(0); }
            catch (Exception) { Assert.Fail(); }            
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Functional")]
        public void Functional_GetAppointmentsByMonth()
        {
            try { s.GetAppointmentsByMonth(DateTime.Now); }
            catch (Exception e) { Assert.Fail(e.Message); }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Functional")]
        public void Functional_GetDateByAppointmentID()
        {
            try { s.GetDateByAppointmentID(0); }
            catch (Exception e) { Assert.Fail(e.Message); }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Functional")]
        public void Functional_GetDateFromDay()
        {
            try { s.GetDateFromDay(new Day()); }
            catch (Exception e) { Assert.Fail(e.Message); }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Functional")]
        public void Functional_GetDayByAppointmentID()
        {
            try { s.GetDayByAppointmentID(0); }
            catch (Exception e) { Assert.Fail(e.Message); }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Functional")]
        public void Functional_GetFlaggedAppointments()
        {
            try { s.GetFlaggedAppointments(); }
            catch (Exception e) { Assert.Fail(e.Message); }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Functional")]
        public void Functional_GetScheduleByDay()
        {
            try { s.GetScheduleByDay(DateTime.Now); }
            catch (Exception e) { Assert.Fail(e.Message); }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Functional")]
        public void Functional_GetScheduleByMonth()
        {
            try { s.GetDaysByMonth(DateTime.Now); }
            catch (Exception e) { Assert.Fail(e.Message); }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Functional")]
        public void Functional_GetScheduleByWeek()
        {
            try { s.GetScheduleByWeek(DateTime.Now); }
            catch (Exception e) { Assert.Fail(e.Message); }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Functional")]
        public void Functional_GetScheduledAppointmentByAppointmentID()
        {
            try { s.GetScheduledAppointmentByAppointmentID(0); }
            catch (Exception e) { Assert.Fail(e.Message); }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Functional")]
        public void Functional_ScheduleAppointment()
        {
            try { s.ScheduleAppointment(new Appointment(), DateTime.Now, 0); }
            catch (Exception e) { Assert.Fail(e.Message); }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Functional")]
        public void Functional_SearchForAppointment()
        {
            try { s.SearchForAppointment(0); }
            catch (Exception e) { Assert.Fail(e.Message); }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Functional")]
        public void Functional_UpdateAppointmentDate()
        {
            try { s.UpdateAppointmentDate(DateTime.Now, 0, 0); }
            catch (Exception e) { Assert.Fail(e.Message); }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Functional")]
        public void Functional_UpdateAppointmentInfoR()
        {
            try { s.UpdateAppointmentInfo(0, 3); }
            catch (Exception e) { Assert.Fail(e.Message); }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Functional")]
        public void Functional_UpdateAppointmentInfoA()
        {
            try { s.UpdateAppointmentInfo(0, 0, -1, 1); }
            catch (Exception e) { Assert.Fail(e.Message); }
        }
        #endregion

        #region Exception Tests
        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Exception")]
        [DataRow( -1, 0)]
        [DataRow( -123, 0)]
        public void Exception_GetAllAppointmentsForPatient(int patientID, int count)
        {
            try
            {
                List<Appointment> la = s.GetAllAppointmentsForPatient(patientID);
                Assert.AreEqual(count, la.Count);
            }
            catch (Exception e) { Assert.Fail(e.Message); }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Exception")]
        public void Exception_GetDateByAppointmentID()
        {
            try { s.GetDateByAppointmentID(-100); }
            catch (Exception e) { Assert.Fail(e.Message); }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Exception")]
        public void Exception_GetDayByAppointmentID()
        {
            try { s.GetDayByAppointmentID(-100); }
            catch (Exception e) { Assert.Fail(e.Message); }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("Exception")]
        public void Exception_GetScheduledAppointmentByAppointmentID()
        {
            try { s.GetScheduledAppointmentByAppointmentID(-100); }
            catch (Exception e) { Assert.Fail(e.Message); }
        }
        #endregion

        #region Null Tests
        [TestMethod]
        [Owner("Alex")]
        [TestCategory("NullArgument")]
        public void Null_GetDateFromDay()
        {
            try { s.GetDateFromDay(null); }
            catch (Exception e) { Assert.Fail(e.Message); }
        }

        [TestMethod]
        [Owner("Alex")]
        [TestCategory("NullArgument")]
        public void Null_ScheduleAppointment()
        {
            try { s.ScheduleAppointment(null, DateTime.Now, 0); }
            catch (Exception e) { Assert.Fail(e.Message); }
        }
        #endregion
    }
}
