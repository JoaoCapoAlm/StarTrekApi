IF(NOT EXISTS(SELECT TOP 1 1 FROM Movie))
BEGIN
	INSERT INTO Movie (OriginalName, OriginalLanguageId, ReleaseDate, SynopsisResource, [Time], ImdbId, TimelineId)
	VALUES ('Star Trek: The Motion Picture', 136, '1980-01-18', 'MotionPicture', 130, 'tt0079945', 1),
		('Star Trek II: The Wrath of Khan', 136, '1982-09-10', 'WrathOfKhan', 113, 'tt0084726', 1),
		('Star Trek III: The search for Spock', 136, '1984-10-05', 'SearchForSpock', 105, 'tt0088170', 1),
		('Star Trek IV : The Voyage Home', 136, '1987-05-07', 'VoyageHome', 119, 'tt0088170', 1),
		('Star Trek V: The Final Frontier', 136, '1989-06-09', 'FinalFrontier', 107, 'tt0098382', 1),
		('Star Trek VI: The Undiscovered Country', 136, '1991-12-06', 'UndiscoveredCountry', 110, 'tt0102975', 1),
		('Star Trek: First Contact', 136, '1997-02-21', 'FirstContact', 112, 'tt0117731', 1),
		('Star Trek: Insurrection', 136, '1998-12-11', 'Insurrection', 103, 'tt0120844', 1),
		('Star Trek: Nemesis', 136, '2002-12-13', 'Nemesis', 117, 'tt0253754', 1),
		('Star Trek', 136, '2009-05-08', 'StarTrek2013', 128, 'tt0796366', 2),
		('Star Trek Into Darkness', 136, '2013-05-16', 'IntoDarkness', 132, 'tt1408101', 2),
		('Star Trek Beyond', 136, '2016-07-22', 'Beyond', 122, 'tt2660888', 2)
END