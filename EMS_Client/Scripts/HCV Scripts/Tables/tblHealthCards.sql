CREATE TABLE [dbo].[tblHealthCards] (
    [HealthCardNumber]  VARCHAR (10) NOT NULL,
    [HealthCardVersion] VARCHAR (2)  NOT NULL,
    [PostalCode]        VARCHAR (10) NOT NULL,
    PRIMARY KEY CLUSTERED ([HealthCardNumber] ASC, [HealthCardVersion] ASC)
);

