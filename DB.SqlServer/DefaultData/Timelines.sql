IF(NOT EXISTS(SELECT TOP 1 1 FROM [dbo].[Timeline]))
BEGIN
	SET IDENTITY_INSERT [dbo].[Timeline] ON;

	INSERT INTO [dbo].[Timeline] ([TimelineId], [Name])
	VALUES (1, 'Prime'), (2, 'Kelvin');

	SET IDENTITY_INSERT [dbo].[Timeline] OFF;
END