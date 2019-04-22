CREATE PROCEDURE CheckUserLogin (@UserName VARCHAR(512),@UserPassword VARCHAR(512))
AS
BEGIN
	IF EXISTS (SELECT 1 FROM [dbo].[tblUsers] WHERE [dbo].[tblUsers].[UserName]=@UserName AND [dbo].[tblUsers].[Password]=@UserPassword)
		BEGIN
			RETURN 1
		END
	ELSE
		BEGIN
			RETURN -1
		END
	RETURN -1
END