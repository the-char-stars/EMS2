/**
*  \file EMS database Script
*  \project INFO2180 - EMS System Term Project
*  \author The Char Stars - alex , attila, div, tudor
*  \date 2019-04-04
*  \brief Patient class definition and functions
*  
*  
*/




/*creating the database called EMS2 in to SQL server*/
CREATE DATABASE EMS2;

USE EMS2;		/*here we tell SSMS that from here we everything we create or delete, will be from the EMS2 database*/


/* creating the patient table which will hold the patients profile details*/
Create table tblPatients (
	PatientID int IDENTITY(0,1) PRIMARY KEY,
	FirstName varchar(50),
	LastName varchar(50),
	HCN varchar(12),
	MInitial varchar(1),
	DateOfBirth date,
	Sex varchar(1),
	HeadOfHouse varchar(12),
	AddressLine1 varchar(50),
	AddressLine2 varchar(50),
	City varchar(10),
	Province varchar(2),
	Postal_Code varchar(6),
	PhoneNum int
);

/* here script is creating the Appointment table to hold appointment information*/
create table tblAppointments(
	AppointmentID int IDENTITY(0,1) PRIMARY KEY,
	PatientID int NOT NULL FOREIGN KEY REFERENCES tblPatients(PatientID),
	DependantID int FOREIGN KEY REFERENCES tblPatients(PatientID),
	Appointment_Notes varchar(50)
);

/* creating the schedual table where EMS will store it all the Appointment wich has been */
create table tblSchedules(
	AppointmentID int NOT NULL FOREIGN KEY REFERENCES tblAppointments(AppointmentID),
	Appointment_Date date NOT NULL,
	Appointment_TimeSlot int NOT NULL
);


/* creating billing codes table which will contain all the billing records which EMS has generated*/ 
create table tblBillingCodes(
	Billing_code varchar(25) NOT NULL PRIMARY KEY,
	Effective_Date date,
	cost decimal
);


/*creating the appointment billing record which will contain infromation about which billing records is from which appointment*/
create table tblAppointmentBillingRecords(
	AppointmentID int NOT NULL FOREIGN KEY REFERENCES tblAppointments(AppointmentID),
	PatientID int NOT NULL FOREIGN KEY REFERENCES tblPatients(PatientID),
	BillingCode varchar(25) NOT NULL FOREIGN KEY REFERENCES tblBillingCodes(Billing_code),
);

/*creating user information which will contains the user login infromation*/
create table tblUsers(
	UserID int IDENTITY(0,1) PRIMARY KEY,
	UserName varchar(25),
	User_Password varchar(256),
	accessLevel int	
);
