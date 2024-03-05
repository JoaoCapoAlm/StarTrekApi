CREATE VIEW [dbo].[vwResourcesName]
AS
	SELECT [MovieId] AS [Id],
		[SynopsisResource] AS [Resource]
	FROM [dbo].[Movie] WITH (NOLOCK)
	
	UNION

	SELECT [MovieId] AS [Id],
		[TitleResource] AS [Resource]
	FROM [dbo].[Movie] WITH (NOLOCK)

	UNION
	
	SELECT [SerieId] AS [Id],
		[TitleResource] AS [Resource]
	FROM [dbo].[Serie] WITH (NOLOCK)

	UNION
	
	SELECT [SerieId] AS [Id],
		[SynopsisResource] AS [Resource]
	FROM [dbo].[Serie] WITH (NOLOCK)

	UNION

	SELECT [EpisodeId] AS [Id],
		[TitleResource] AS [Resource]
	FROM [dbo].[Episode] WITH (NOLOCK)

	UNION

	SELECT [EpisodeId] AS [Id],
		[SynopsisResource] AS [Resource]
	FROM [dbo].[Episode] WITH (NOLOCK)
