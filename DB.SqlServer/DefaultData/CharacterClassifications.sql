IF(NOT EXISTS(SELECT TOP 1 1 FROM [dbo].[CharacterClassification]))
BEGIN
	SET IDENTITY_INSERT [dbo].[CharacterClassification] ON;
	INSERT INTO [dbo].[CharacterClassification]([CharacterClassificationId], [Classification])
	VALUES (1, 'Humanoid'),
		(2, 'Andoid')
	SET IDENTITY_INSERT [dbo].[CharacterClassification] OFF;
END