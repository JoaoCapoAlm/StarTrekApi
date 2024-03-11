CREATE VIEW [dbo].[vwFullSerie]
AS
	SELECT [s].[SerieId],
		[OriginalName],
        [s].[TitleResource] AS [SerieTitleResource],
        [s].[SynopsisResource] AS [SerieSynopsisResource],
        [Abbreviation],
        [s].[ImdbId] AS [ImdbSerie],
        [s].TimelineId,
        [t].[Name] AS [TimelineName],
        [l].[LanguageId],
        [l].[CodeISO],
        [l].[ResourceName] AS [LanguageResourceName],
        [se].[SeasonId],
        [se].[Number] AS [SeasonNumber],
        [e].[EpisodeId],
        [e].[Number] AS [EpisodeNumber],
        [e].[RealeaseDate],
        [e].[StardateFrom],
        [e].[StardateTo],
        [e].[SynopsisResource] AS [EpisodeSynopsisResource],
        [e].[TitleResource] AS [EpisodeTitleResource],
        [e].[Time],
        [e].[ImdbId] AS [ImdbEpisode]
	FROM [dbo].[Serie] s WITH (NOLOCK)
    INNER JOIN [dbo].[Timeline] t WITH (NOLOCK) ON [t].[TimelineId] = [s].[TimelineId]
    INNER JOIN [dbo].[Language] l WITH (NOLOCK) ON [l].[LanguageId] = [s].[OriginalLanguageId]
    LEFT JOIN [dbo].[Season] se WITH (NOLOCK) ON [se].[SerieId] = [s].[SerieId]
    LEFT JOIN [dbo].[Episode] e WITH (NOLOCK) ON [e].[SeasonId] = [se].[SeasonId]
