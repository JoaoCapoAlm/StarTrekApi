﻿IF(NOT EXISTS(SELECT TOP 1 1 FROM Serie))
BEGIN
SET IDENTITY_INSERT Serie ON;
INSERT INTO Serie (SerieId, OriginalName, [OriginalLanguageId], [TimelineId], ImdbId, [SynopsisResource], Abbreviation)
VALUES (1, 'Star Trek: The Original Series', 137, 1, 'tt0060028', 'OriginalSeries', 'TOS'),
	(2, 'Star Trek: The Animated Series', 137, 1, 'tt0069637', 'AnimatedSeries', 'TAS'),
	(3, 'Star Trek: The Next Generation', 137, 1, 'tt0092455', 'NextGeneration', 'TNG'),
	(4, 'Star Trek: Deep Space Nine', 137, 1, 'tt0106145', 'DeepSpaceNine', 'DS9'),
	(5, 'Star Trek: Voyager', 137, 1, 'tt0112178', 'Voyager', 'VOY'),
	(6, 'Star Trek: Enterprise', 137, 1, 'tt0244365', 'Enterprise', 'ENT'),
	(7, 'Star Trek: Discovery', 137, 1, 'tt5171438', 'Discovery', 'DSC'),
	(8, 'Star Trek: Lower Decks', 137, 1, 'tt9184820', 'LowerDecks', 'LDS'),
	(9, 'Star Trek: Picard', 137, 1, 'tt8806524', 'Picard', 'PIC'),
	(10,'Star Trek: Prodigy', 137, 1, 'tt9795876', 'Prodigy', 'PRO'),
	(11,'Star Trek: Strange New Worlds', 137, 1, 'tt12327578', 'StrangeNewWorlds', 'SNW');
SET IDENTITY_INSERT Serie OFF;
END