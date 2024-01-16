IF(NOT EXISTS (SELECT TOP 1 1 FROM Season))
BEGIN
	SET IDENTITY_INSERT Season ON;
	INSERT INTO Season (SeasonId, SerieId, Number)
	VALUES (1, 1, 1), (2, 1, 2), (3, 1, 3), -- The Original Series
		(4, 2, 1), (5, 2, 2), -- The Animated Series
		(6, 3, 1), (7, 3, 2), (8, 3, 3), (9, 3, 4), (10, 3, 5), (11, 3, 6), (12, 3, 7) -- The Next Generation
	SET IDENTITY_INSERT Season OFF;
END