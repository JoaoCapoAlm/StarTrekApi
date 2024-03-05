IF(NOT EXISTS(SELECT TOP 1 1 FROM [dbo].[Quadrant]))
BEGIN
	SET IDENTITY_INSERT [dbo].[Quadrant] ON;
	
	INSERT INTO [dbo].[Quadrant] ([QuadrantId], [QuadrantResource])
	VALUES (1, 'AlphaResource'),
		(2, 'BetaResource'),
		(3, 'GamaResource'),
		(4, 'DeltaResource');

	SET IDENTITY_INSERT [dbo].[Quadrant] OFF;
END