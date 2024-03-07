IF(NOT EXISTS(SELECT TOP 1 1 FROM [dbo].[PlaceType]))
BEGIN
	SET IDENTITY_INSERT [dbo].[PlaceType] ON;

	INSERT INTO [dbo].[PlaceType] ([PlaceTypeId], [Type])
	VALUES (1, 'Planet'),
		(2, 'SpaceStation'),
		(3, 'City');

	SET IDENTITY_INSERT [dbo].[PlaceType] OFF;
END