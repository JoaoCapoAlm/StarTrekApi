IF(NOT EXISTS(SELECT TOP 1 1 FROM Serie))
BEGIN
SET IDENTITY_INSERT Serie ON;
INSERT INTO Serie (SerieId, OriginalName, [OriginalLanguageId], [TimelineId], ImdbId, [SynopsisResource], Abbreviation, TmdbId)
VALUES (1, 'Star Trek: The Original Series', 136, 1, 'tt0060028', 'OriginalSeries', 'TOS', 253)
	--(2, 'Star Trek: The Animated Series', 136, 1, 'tt0069637', 'AnimatedSeries', 'TAS', 1992),
	--(3, 'Star Trek: The Next Generation', 136, 1, 'tt0092455', 'NextGeneration', 'TNG', 655),
	--(4, 'Star Trek: Deep Space Nine', 136, 1, 'tt0106145', 'DeepSpaceNine', 'DS9', 580),
	--(5, 'Star Trek: Voyager', 136, 1, 'tt0112178', 'Voyager', 'VOY', 1855),
	--(6, 'Star Trek: Enterprise', 136, 1, 'tt0244365', 'Enterprise', 'ENT', 314),
	--(7, 'Star Trek: Discovery', 136, 1, 'tt5171438', 'Discovery', 'DSC', 67198),
	--(8, 'Star Trek: Lower Decks', 136, 1, 'tt9184820', 'LowerDecks', 'LDS', 85948),
	--(9, 'Star Trek: Picard', 136, 1, 'tt8806524', 'Picard', 'PIC', 85949),
	--(10,'Star Trek: Prodigy', 136, 1, 'tt9795876', 'Prodigy', 'PRO', 106393),
	--(11,'Star Trek: Strange New Worlds', 136, 1, 'tt12327578', 'StrangeNewWorlds', 'SNW', 103516);
SET IDENTITY_INSERT Serie OFF;
END