IF(NOT EXISTS(SELECT TOP 1 1 FROM Episode))
BEGIN
	INSERT INTO Episode (SeasonId, Number, RealeaseDate, TitleResource, SynopsisResource, [Time], ImdbId)
	VALUES (1, 0, '1988-10-04', 'TheCage', 'TheCageSynopsis', 63, 'tt0059753'),
		(1, 1, '1966-09-08', 'TheManTrap', 'TheManTrapSynopsis', 50, 'tt0708469'),
		(1, 2, '1966-09-15', 'CharlieX', 'CharlieXSynopsis', 50, 'tt0708424')
END