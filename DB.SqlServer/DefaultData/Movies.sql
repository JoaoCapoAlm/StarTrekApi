IF(NOT EXISTS(SELECT TOP 1 1 FROM [dbo].[Movie]))
BEGIN
	INSERT INTO [dbo].[Movie] ([OriginalName], [OriginalLanguageId], [ReleaseDate], [TitleResource], [SynopsisResource], [Time], [ImdbId], [TimelineId], [TmdbId])
	VALUES ('Star Trek: The Motion Picture', 136, '1980-01-18', 'MotionPicture', 'MotionPictureSynopsis', 130, 'tt0079945', 1, 152),
		('Star Trek II: The Wrath of Khan', 136, '1982-09-10', 'WrathOfKhan', 'WrathOfKhanSynopsis', 113, 'tt0084726', 1, 154),
		('Star Trek III: The search for Spock', 136, '1984-10-05', 'SearchForSpock', 'SearchForSpockSynopsis', 105, 'tt0088170', 1, 157),
		('Star Trek IV : The Voyage Home', 136, '1987-05-07', 'VoyageHome', 'VoyageHomeSynopsis', 119, 'tt0088170', 1, 168)
		-- FIX adicionar ID do TMDB
		--('Star Trek V: The Final Frontier', 136, '1989-06-09', 'FinalFrontier', 'FinalFrontierSynopsis', 107, 'tt0098382', 1, null),
		--('Star Trek VI: The Undiscovered Country', 136, '1991-12-06', 'UndiscoveredCountry', 'UndiscoveredCountrySynopsis', 110, 'tt0102975', 1, null),
		--('Star Trek: First Contact', 136, '1997-02-21', 'FirstContact', 'FirstContactSynopsis', 112, 'tt0117731', 1, null),
		--('Star Trek: Insurrection', 136, '1998-12-11', 'Insurrection', 'InsurrectionSynopsis', 103, 'tt0120844', 1, null),
		--('Star Trek: Nemesis', 136, '2002-12-13', 'Nemesis', 'NemesisSynopsis', 117, 'tt0253754', 1, null),
		--('Star Trek', 136, '2009-05-08', 'StarTrek2013', 'StarTrek2013Synopsis', 128, 'tt0796366', 2, null),
		--('Star Trek Into Darkness', 136, '2013-05-16', 'IntoDarkness', 'IntoDarknessSynopsis', 132, 'tt1408101', 2, null),
		--('Star Trek Beyond', 136, '2016-07-22', 'Beyond', 'BeyondSynopsis', 122, 'tt2660888', 2, null)
END