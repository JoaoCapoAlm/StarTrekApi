IF(NOT EXISTS(SELECT TOP 1 1 FROM [dbo].[Quadrant]))
BEGIN
	SET IDENTITY_INSERT [dbo].[Quadrant] ON;
	
	INSERT INTO [dbo].[Quadrant] ([QuadrantId], [QuadrantResource])
	VALUES (1, 'QuadrantAlphaResource'),
		(2, 'QuadrantBetaResource'),
		(3, 'QuadrantGammaResource'),
		(4, 'QuadrantDeltaResource');

	SET IDENTITY_INSERT [dbo].[Quadrant] OFF;
END