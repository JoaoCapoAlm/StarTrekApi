IF(NOT EXISTS(SELECT TOP 1 1 FROM Serie))
BEGIN
SET IDENTITY_INSERT Serie ON;
INSERT INTO Serie (SerieId, OriginalName, [OriginalLanguageId], [TimelineId], ImdbId, [SynopsisResource], Abbreviation, TmdbId)
VALUES (1, 'Star Trek: The Original Series', 137, 1, 'tt0060028', 'OriginalSeries', 'TOS', 253)
	--(2, 'Star Trek: The Animated Series', 137, 1, 'tt0069637', 'AnimatedSeries', 'TAS', 0),
	--(3, 'Star Trek: The Next Generation', 137, 1, 'tt0092455', 'NextGeneration', 'TNG', 0),
	--(4, 'Star Trek: Deep Space Nine', 137, 1, 'tt0106145', 'DeepSpaceNine', 'DS9', 0),
	--(5, 'Star Trek: Voyager', 137, 1, 'tt0112178', 'Voyager', 'VOY', 0),
	--(6, 'Star Trek: Enterprise', 137, 1, 'tt0244365', 'Enterprise', 'ENT', 0),
	--(7, 'Star Trek: Discovery', 137, 1, 'tt5171438', 'Discovery', 'DSC', 0),
	--(8, 'Star Trek: Lower Decks', 137, 1, 'tt9184820', 'LowerDecks', 'LDS', 0),
	--(9, 'Star Trek: Picard', 137, 1, 'tt8806524', 'Picard', 'PIC', 0),
	--(10,'Star Trek: Prodigy', 137, 1, 'tt9795876', 'Prodigy', 'PRO', 0),
	--(11,'Star Trek: Strange New Worlds', 137, 1, 'tt12327578', 'StrangeNewWorlds', 'SNW', 0);
SET IDENTITY_INSERT Serie OFF;
END