IF(NOT EXISTS(SELECT TOP 1 1 FROM Movie))
BEGIN
	INSERT INTO Movie (OriginalName, OriginalLanguageId, ReleaseDate, SynopsisResource, [Time], ImdbId, TimelineId)
	VALUES ('Star Trek: The Motion Picture', 137, '1980-01-18', 'MotionPicture', 130, '0079945', 1),
		('Star Trek II: The Wrath of Khan', 137, '1982-09-10', 'WrathOfKhan', 113, '0084726', 1),
		('Star Trek III: The search for Spock', 137, '1984-10-05', 'SearchForSpock', 105, '0088170', 1),
		('Star Trek IV : The Voyage Home', 137, '1987-05-07', 'VoyageHome', 119, '0088170', 1),
		('Star Trek V: The Final Frontier', 137, '1989-06-09', 'FinalFrontier', 107, '0098382', 1),
		('Star Trek VI: The Undiscovered Country', 137, '1991-12-06', 'UndiscoveredCountry', 110, '0102975', 1),
		('Star Trek: First Contact', 137, '1997-02-21', 'FirstContact', 112, '0117731', 1),
		('Star Trek: Insurrection', 137, '1998-12-11', 'Insurrection', 103, '0120844', 1),
		('Star Trek: Nemesis', 137, '2002-12-13', 'Nemesis', 117, '0253754', 1),
		('Star Trek', 137, '2009-05-08', 'StarTrek2013', 128, '0796366', 2),
		('Star Trek Into Darkness', 137, '2013-05-16', 'IntoDarkness', 132, '1408101', 2),
		('Star Trek Beyond', 137, '2016-07-22', 'Beyond', 122, '2660888', 2)
END