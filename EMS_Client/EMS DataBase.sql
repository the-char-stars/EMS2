/**
*  \file EMS database Script
*  \project INFO2180 - EMS System Term Project
*  \author The Char Stars - alex , attila, div, tudor
*  \date 2019-04-04
*  \brief Patient class definition and functions
*/


/*creating the database called EMS2 in to SQL server*/
CREATE DATABASE EMS2;

USE EMS2;		/*here we tell SSMS that from here we everything we create or delete, will be from the EMS2 database*/


/* creating the patient table which will hold the patients profile details*/
Create table tblPatients (
	PatientID int PRIMARY KEY,
	FirstName varchar(64),
	LastName varchar(64),
	HCN varchar(12),
	MInitial varchar(1),
	DateOfBirth date,
	Gender varchar(1),
	HeadOfHouse varchar(12),
	AddressLine1 varchar(64),
	AddressLine2 varchar(64),
	City varchar(64),
	Province varchar(2),
	PhoneNum varchar(10),
	PostalCode varchar(10)
);

/* here script is creating the Appointment table to hold appointment information*/
create table tblAppointments(
	AppointmentID int PRIMARY KEY,
	PatientID int FOREIGN KEY REFERENCES tblPatients(PatientID),
	DependantID int FOREIGN KEY REFERENCES tblPatients(PatientID),
	recallFlag int,
	AppointmentNotes varchar(512)
);

/* creating the schedule table where EMS will store it all the Appointment which has been */
create table tblSchedules(
	AppointmentID int FOREIGN KEY REFERENCES tblAppointments(AppointmentID),
	AppointmentDate date,
	AppointmentTimeSlot int
);


/* creating billing codes table which will contain all the billing records which EMS has generated*/ 
create table tblBillingCodes(
	BillingCode varchar(25) PRIMARY KEY,
	Effective_Date date,
	Cost decimal
);


/*creating the appointment billing record which will contain information about which billing records is from which appointment*/
create table tblAppointmentBillingRecords(
	AppointmentBillingRecordsID int PRIMARY KEY,
	AppointmentID int FOREIGN KEY REFERENCES tblAppointments(AppointmentID),
	PatientID int FOREIGN KEY REFERENCES tblPatients(PatientID),
	BillingCode varchar(25) FOREIGN KEY REFERENCES tblBillingCodes(BillingCode),
);


/*creating user information which will contains the user login information*/
create table tblUsers(
	UserID int PRIMARY KEY,
	UserName varchar(256),
	Password varchar(256),
	AccessLevel int	
);
