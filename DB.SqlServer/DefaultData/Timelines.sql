IF(NOT EXISTS(SELECT TOP 1 1 FROM Timeline))
BEGIN
	SET IDENTITY_INSERT Timeline ON;

	INSERT INTO Timeline (TimelineId, [Name])
	VALUES (1, 'Prime'), (2, 'Kelvin');

	SET IDENTITY_INSERT Timeline OFF;
END