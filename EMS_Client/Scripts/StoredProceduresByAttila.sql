
CREATE PROCEDURE ChangePassword (@userID INTEGER, @password NVARCHAR(50))
AS
BEGIN
	UPDATE tblUsers
	SET [Password] = @password
	WHERE UserID = @userID

	IF @password = (SELECT [Password] FROM tblUsers WHERE UserID = @userID) 
		RETURN 0
	ELSE
		RETURN 1;
END

EXEC ChangePassword 10, abc
