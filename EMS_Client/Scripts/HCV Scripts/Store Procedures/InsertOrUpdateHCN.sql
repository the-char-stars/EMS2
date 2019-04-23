CREATE PROCEDURE [dbo].[InsertOrUpdateHCN]
	@healthCardNumber VARCHAR(10),
	@healthCardVersion VARCHAR(2),
	@postalCode VARCHAR(10)
AS
BEGIN

	IF EXISTS(SELECT 1 FROM tblHealthCards WHERE HealthCardNumber=@healthCardNumber AND HealthCardVersion=@healthCardVersion)
		BEGIN
			UPDATE tblHealthCards
			SET PostalCode = @postalCode
			WHERE HealthCardNumber=@healthCardNumber AND HealthCardVersion=@healthCardVersion;
			RETURN 1;
		END
	ELSE
		BEGIN
			INSERT INTO tblHealthCards(HealthCardNumber, HealthCardVersion, PostalCode)
			VALUES (@healthCardNumber, @healthCardVersion, @postalCode);
			RETURN 2;
		END
		RETURN -1;
END