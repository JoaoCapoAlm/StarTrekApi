IF(NOT EXISTS(SELECT TOP 1 1 FROM [Role]))
BEGIN
	SET IDENTITY_INSERT [Role] ON;
	INSERT INTO [Role](RoleId, RoleResource)
	VALUES (1, 'Creator'),
		(2, 'Direcotr'),
		(3, 'Writer'),
		(4, 'Producer'),
		(5, 'Actor')
	SET IDENTITY_INSERT [Role] OFF;
END