CREATE VIEW [dbo].[vwResourcesName]
AS
	SELECT [MovieId],
		0 AS [SerieId],
		0 AS [EpisodeId],
		[SynopsisResource],
		[TitleResource]
	FROM [Movie] WITH (NOLOCK)
	
	UNION
	
	SELECT 0 AS [MovieId],
		[SerieId],
		0 AS [EpisodeId],
		[SynopsisResource],
		[TitleResource]
	FROM [Serie] WITH (NOLOCK)

	UNION

	SELECT 0 AS [MovieId],
		0 AS [SerieId],
		[EpisodeId],
		[SynopsisResource],
		[TitleResource]
	FROM [Episode] WITH (NOLOCK)
