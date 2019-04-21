using System;
using System.Collections.Generic;
using EMS_Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EMS_Test_Demographics
{
    /** 
     * \class DemographicsTests
     *
     * \brief <b>Brief Description</b> - THis class contains the all the test methods for the demographics class
     * 
     * The tests include Stress, Functional, Exception, and boundary conditions
     *
     * \author <i>The Char Stars - Alex Kozak</i>
     */
    [TestClass]
    public class DemographicsTests
    {
        
        #region Functional Tests
        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Functional")]
        public void Functional_AddNewPatient()
        {           
            try
            {
                 Demographics d = new Demographics();
              
                Patient p = new Patient(d);
                p.FirstName = "Divyangbhai";
                p.LastName = "Dankhara";
                p.HCN = "1234567890AB";
                p.MInitial = "A";
                p.DateOfBirth = "12111997";
                p.Sex = "M";
               
                d.AddNewPatient(p);
            
                Patient f = d.GetPatientByHCN("1234567890AB");
                if(!(f.HCN == p.HCN))
                {   
                    Assert.Fail();
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }




        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Null")]
        public void Null_AddNewPatient()
        {
            
            try
            {
                Demographics d = new Demographics();
               
                d.AddNewPatient(null);

            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }






        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Functional")]
        public void Create_AddNewPatient()
        {
            
            try
            {
                Demographics d = new Demographics();
                
                Patient DPatient = d.CreateNewPatient();
                 
                if(DPatient == null)
                {
                    Assert.Fail();
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }






        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Functional")]
        public void Functional_GetPatientbyHCN()
        {
            
            try
            {
                Demographics d = new Demographics();
                Patient p = new Patient(d);
                p.FirstName = "Divyangbhai";
                p.LastName = "Dankhara";
                p.HCN = "0987654321AB";
                p.MInitial = "A";
                p.DateOfBirth = "12111997";
                p.Sex = "M";
                p.HeadOfHouse = "1234567890AB";

                d.AddNewPatient(p);
                Patient dp = d.GetPatientByHCN(p.HCN);


                if (!(dp.HCN == p.HCN))
                {
                    Assert.Fail();
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }




        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Null")]
        public void Null_GetPatientbyHCN()
        {
            
            try
            {
                Demographics d = new Demographics();
                Patient p = new Patient(d);
                p.FirstName = "Divyangbhai";
                p.LastName = "Dankhara";
                p.HCN = "0987654321AB";
                p.MInitial = "A";
                p.DateOfBirth = "12111997";
                p.Sex = "M";
                p.HeadOfHouse = "1234567890AB";

                d.AddNewPatient(p);
                Patient dp = d.GetPatientByHCN(null);

                if(!(dp == null))
                {
                    Assert.Fail();
                }

            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }





        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Functional")]
        public void Functional_GetPatientbyName()
        {
            
            try
            {
                Demographics d = new Demographics();
                Patient p = new Patient(d);
                p.FirstName = "Divyangbhai";
                p.LastName = "Dankhara";
                p.HCN = "0987654321AB";
                p.MInitial = "A";
                p.DateOfBirth = "12111997";
                p.Sex = "M";
                p.HeadOfHouse = "1234567890AB";

                d.AddNewPatient(p);
                List<Patient> lP =  d.GetPatientByName(p.FirstName,p.LastName);

                bool contains = false;
                if ((lP != null))
                {
                    foreach(Patient tp in lP)
                    {
                        
                        if((tp.FirstName == p.FirstName) && (tp.LastName == p.LastName))
                        {
                            contains = true;
                        }
                    }

                    if(contains == false)
                    {
                        
                        Assert.Fail();
                    }
                }
                else
                {
                    
                    Assert.Fail();
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }




        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Null")]
        public void Null_GetPatientbyName()
        {
            
            try
            {
                Demographics d = new Demographics();
                List<Patient> lP = d.GetPatientByName(null, null);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }


        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Functional")]
        public void Functional_GetPatientbyID()
        {
            
            try
            {
                Demographics d = new Demographics();

                Patient p = new Patient(d);
                p.FirstName = "alex";
                p.LastName = "koxak";
                p.HCN = "0987654321GF";
                p.MInitial = "A";
                p.DateOfBirth = "12111997";
                p.Sex = "M";
                p.HeadOfHouse = "1234567890AB";

                d.AddNewPatient(p);
                int count = d.GetPatientIDByHCN(p.HCN);
                Patient dp =  d.GetPatientByID(count);

                if(dp != null)
                {
                    if (!(dp.HCN == p.HCN))
                    {
                        Assert.Fail();
                    }

                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }


        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Functional")]
        public void Functional_UpdatePatient()
        {
            // in this test we test that demographics class actually add the patient to database
            try
            {
                Demographics d = new Demographics();

                Patient p = new Patient(d);
                p.FirstName = "Divyangbhai";
                p.LastName = "Dankhara";
                p.HCN = "0987654321AB";
                p.MInitial = "A";
                p.DateOfBirth = "12111997";
                p.Sex = "M";
                p.HeadOfHouse = "1234567890AB";

                d.AddNewPatient(p);
                p.LastName = "dankhara1";

                // here we are updating the patient that we have just added with new last name.
                d.UpdatePatient(p);

                Patient Dp = d.GetPatientByHCN(p.HCN);
                if ( !(Dp.LastName == p.LastName))
                {
                    Assert.Fail();
                }

            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }




        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Null")]
        public void Null_UpdatePatient()
        {    
            try
            {
                Demographics d = new Demographics();
                d.UpdatePatient(null);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }



        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Functional")]
        public void Functional_GetPatientList()
        {
            
            try
            {
                Demographics d = new Demographics();

                Patient p = new Patient(d);
                p.FirstName = "Divyangbhai";
                p.LastName = "Dankhara";
                p.HCN = "0987654321AB";
                p.MInitial = "A";
                p.DateOfBirth = "12111997";
                p.Sex = "M";
                p.HeadOfHouse = "1234567890AB";

                d.AddNewPatient(p);    
                int count = d.dPatientRoster.Count;

                if(count == 0)
                {
                    Assert.Fail();
                }


            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }



        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Functional")]
        public void Functional_GetDependants()
        {
            
            try
            {
                Demographics d = new Demographics();

                Patient p = new Patient(d);
                p.FirstName = "Divyangbhai";
                p.LastName = "Dankhara";
                p.HCN = "1234567890AB";
                p.MInitial = "A";
                p.DateOfBirth = "12111997";
                p.Sex = "M";

                d.AddNewPatient(p);

                List<Patient> dp = d.GetRelations(p);

                if(!(dp == null))
                {
                    foreach(Patient a in dp)
                    {
                        if(!(a.HeadOfHouse == p.HCN))
                        {
                            Assert.Fail();
                        }
                    }
                }

            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }




        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Null")]
        public void Null_GetDependants()
        {
            
            try
            {
                Demographics d = new Demographics();
                
                d.GetRelations(null);

            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }


        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Functional")]
        public void Functional_GetPatientIDByHCN()
        {
            try
            {
                Demographics d = new Demographics();
                int pID =  d.GetPatientIDByHCN("1234567890AB");
                Patient p = d.GetPatientByHCN("1234567890AB");

                if(!(pID == p.PatientID))
                {
                    Assert.Fail();
                }


            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }



        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Null")]
        public void Null_GetPatientIDByHCN()
        {
            try
            {
                Demographics d = new Demographics();
                int  pID= d.GetPatientIDByHCN(null);            
                if(!(pID == -1))
                {
                    Assert.Fail();
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
     * \class PatientTests
     *
     * \brief <b>Brief Description</b> -This class contains the 
     * 
     * The tests include Stress, Functional, Exception, and boundary conditions
     *
     * \author <i>The Char Stars - Alex Kozak</i>
     */
    [TestClass]
    public class PatientTests
    {
        Demographics d = new Demographics();
        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Functional")]
        public void Functional_SetPatientID()
        {
            try
            {
                Patient p = new Patient(d);
                /// setting patient to 0
                p.PatientID = 1;

                if(!(p.PatientID == 1))
                {
                    // check if the patientif is not 0 then failed the test
                    Assert.Fail();
                }
            }
            catch(Exception)
            {
                Assert.Fail();
            }
        }


        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Exception")]
        public void Exception_PatientID()
        {
            try
            {
                Patient p = new Patient(d);
                // try to set patientid -123
                p.PatientID = -123;

                if (p.PatientID == -123)
                {
                    // if patient ID is -100 then fail the test
                    Assert.Fail();
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }



        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Boundary")]
        public void Boundary_PatientID()
        {
            try
            {
                Patient p = new Patient(d);
                p.PatientID = 0;

                if (!(p.PatientID == 0))
                {
                    Assert.Fail();
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }



        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Functional")]
        public void Functional_FirstName()
        {
            try
            {
                Patient p = new Patient(d);

                // setting vaild Fname to the patient
                string testString = "DIV";

                testString = testString.ToUpper();
                p.FirstName = testString;

                if (!(p.FirstName == testString))
                {
                    //if patinet fname is not valid name that we set then fail the test
                    Assert.Fail();
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }




        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Exceptional")]
        public void Exceptional_FirstName()
        {
            try
            {
                Patient p = new Patient(d);

                // setting vaild Fname to the patient
                string testString = "!@#$#@$$$#123";

                testString = testString.ToUpper();
                p.FirstName = testString;

                if (p.FirstName == testString)
                {
                    Assert.Fail();
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }





        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Functional")]
        public void Functional_LastName()
        {
            try
            {
                Patient p = new Patient(d);

                
                string testString = "Dankhara";

                testString = testString.ToUpper();
                p.LastName = testString;

                if (!(p.LastName == testString))
                {
                   
                    Assert.Fail();
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }




        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Exceptional")]
        public void Exceptional_LastName()
        {
            try
            {
                Patient p = new Patient(d);

                
                string testString = "DIv)(&(324";

                testString = testString.ToUpper();
                p.LastName = testString;

                if (p.LastName == testString)
                {
                    
                    Assert.Fail();
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }



        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Functional")]
        public void Functional_MInitial()
        {
            try
            {
                Patient p = new Patient(d);

                
                string testString = "A";

                testString = testString.ToUpper();
                p.MInitial = testString;

                if (!(p.MInitial == testString))
                {
                   
                    Assert.Fail();
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }




        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Exceptional")]
        public void Exceptional_MInitial()
        {
            try
            {
                Patient p = new Patient(d);

                
                string testString = "asd";

                testString = testString.ToUpper();
                p.MInitial = testString;

                if (p.MInitial == testString)
                {
                    
                    Assert.Fail();
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }


        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Functional")]
        public void Functional_HCN()
        {
            try
            {
                Patient p = new Patient(d);

                
                string testString = "1234567890AB";

                testString = testString.ToUpper();
                p.HCN = testString;

                if (!(p.HCN == testString))
                {
                   
                    Assert.Fail();
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }




        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Exceptional")]
        public void Exceptional_HCN()
        {
            try
            {
                Patient p = new Patient(d);

                
                string testString = "1234567890AJASJK";

                testString = testString.ToUpper();
                p.HCN = testString;

                if (p.HCN == testString)
                {
                    
                    Assert.Fail();
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }





        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Functional")]
        public void Functional_DateOfBirth()
        {
            try
            {
                Patient p = new Patient(d);

                
                string testString = "11121997";

                testString = testString.ToUpper();
                p.DateOfBirth = testString;

                if (!(p.DateOfBirth == testString))
                {
                   
                    Assert.Fail();
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }




        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Exceptional")]
        public void Exceptional_DateOfBirth()
        {
            try
            {
                Patient p = new Patient(d);

                
                string testString = "19971112";

                testString = testString.ToUpper();
                p.DateOfBirth = testString;

                if (p.DateOfBirth == testString)
                {
                    
                    Assert.Fail();
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }


        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Functional")]
        public void Functional_Sex()
        {
            try
            {
                Patient p = new Patient(d);

                
                string testString = "X";

                testString = testString.ToUpper();
                p.Sex = testString;

                if (!(p.Sex == testString))
                {
                   
                    Assert.Fail();
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }




        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Exceptional")]
        public void Exceptional_Sex()
        {
            try
            {
                Patient p = new Patient(d);

                
                string testString = "H";

                testString = testString.ToUpper();
                p.Sex = testString;

                if (p.Sex == testString)
                {
                    
                    Assert.Fail();
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }





        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Functional")]
        public void Functional_HeadOfHouse()
        {
            try
            {
                Patient p = new Patient(d);

                
                string testString = "1234567890AB";

                testString = testString.ToUpper();
                p.HeadOfHouse = testString;

                if (!(p.HeadOfHouse == testString))
                {
                   
                    Assert.Fail();
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }




        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Exceptional")]
        public void Exceptional_HeadOfHouse()
        {
            try
            {
                Patient p = new Patient(d);

                
                string testString = "1234567890AJASJK";

                testString = testString.ToUpper();
                p.HeadOfHouse = testString;

                if (p.HeadOfHouse == testString)
                {
                    
                    Assert.Fail();
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }



        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Functional")]
        public void Functional_AddressLine1()
        {
            try
            {
                Patient p = new Patient(d);

                
                string testString = "12 swtraj hansh society";

                testString = testString.ToUpper();
                p.AddressLine1 = testString;

                if (!(p.AddressLine1 == testString))
                {
                   
                    Assert.Fail();
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }




        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Exceptional")]
        public void Exceptional_AddressLine1()
        {
            try
            {
                Patient p = new Patient(d);

                
                string testString = "!@#$%%^%$#$%^%$#";

                testString = testString.ToUpper();
                p.AddressLine1 = testString;

                if (p.AddressLine1 == testString)
                {
                    
                    Assert.Fail();
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }



        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Functional")]
        public void Functional_AddressLine2()
        {
            try
            {
                Patient p = new Patient(d);

                
                string testString = "12 swtraj hansh society";

                testString = testString.ToUpper();
                p.AddressLine2 = testString;

                if (!(p.AddressLine2 == testString))
                {
                   
                    Assert.Fail();
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }




        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Exceptional")]
        public void Exceptional_AddressLine2()
        {
            try
            {
                Patient p = new Patient(d);

                
                string testString = "!@#$%%^%$#$%^%$#";

                testString = testString.ToUpper();
                p.AddressLine2 = testString;

                if (p.AddressLine2 == testString)
                {
                    
                    Assert.Fail();
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }




        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Functional")]
        public void Functional_City()
        {
            try
            {
                Patient p = new Patient(d);

                
                string testString = "Surat";

                testString = testString.ToUpper();
                p.City = testString;

                if (!(p.City == testString))
                {
                   
                    Assert.Fail();
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }




        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Exceptional")]
        public void Exceptional_City()
        {
            try
            {
                Patient p = new Patient(d);

                
                string testString = "!@#$%%^%$#$%^%$#";

                testString = testString.ToUpper();
                p.City = testString;

                if (p.City == testString)
                {
                    Assert.Fail();
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }



        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Functional")]
        public void Functional_Province()
        {
            try
            {
                Patient p = new Patient(d);


                string testString = "ON";

                testString = testString.ToUpper();
                p.Province = testString;

                if (!(p.Province == testString))
                {

                    Assert.Fail();
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }




        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Exceptional")]
        public void Exceptional_Province()
        {
            try
            {
                Patient p = new Patient(d);


                string testString = "YZ";

                testString = testString.ToUpper();
                p.Province = testString;

                if (p.Province == testString)
                {
                    Assert.Fail();
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }




        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Functional")]
        public void Functional_PhoneNumber()
        {
            try
            {
                Patient p = new Patient(d);


                string testString = "9999999999";

                testString = testString.ToUpper();
                p.PhoneNumber = testString;

                if (!(p.PhoneNumber == testString))
                {

                    Assert.Fail();
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }




        [TestMethod]
        [Owner("Divyangbhai")]
        [TestCategory("Exceptional")]
        public void Exceptional_PhoneNumber()
        {
            try
            {
                Patient p = new Patient(d);


                string testString = "asdasdasdfa98027408927345";

                testString = testString.ToUpper();
                p.PhoneNumber = testString;

                if (p.PhoneNumber == testString)
                {
                    Assert.Fail();
                }
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }
    }
}