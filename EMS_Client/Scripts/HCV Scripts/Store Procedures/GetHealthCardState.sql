CREATE PROCEDURE GetHealthCardState
	@healthCardNumber VARCHAR(10),
	@healthCardVersion VARCHAR(2),
	@postalCode VARCHAR(10)
AS
BEGIN
	IF EXISTS(SELECT 1 FROM tblHealthCards WHERE HealthCardNumber=@healthCardNumber AND HealthCardVersion=@healthCardVersion)
		BEGIN
			RETURN 1
		END

	IF EXISTS(SELECT 1 FROM tblHealthCards WHERE HealthCardNumber=@healthCardNumber AND HealthCardVersion!=@healthCardVersion)
		BEGIN
			RETURN 2
		END
	
	RETURN 3
END