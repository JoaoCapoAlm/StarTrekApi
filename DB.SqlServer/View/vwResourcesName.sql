CREATE VIEW [dbo].[vwResourcesName]
AS
	SELECT 'm' + CAST([MovieId] AS varchar) AS [Id],
		[SynopsisResource],
		[TitleResource]
	FROM [Movie] WITH (NOLOCK)
	
	UNION
	
	SELECT 's' + CAST([SerieId] AS varchar) AS [Id],
		[SynopsisResource],
		[TitleResource]
	FROM [Serie] WITH (NOLOCK)

	UNION

	SELECT 'e'+ CAST([EpisodeId] AS varchar) AS [Id],
		[SynopsisResource],
		[TitleResource]
	FROM [Episode] WITH (NOLOCK)
