CREATE VIEW [dbo].[vwResourcesName]
AS
	SELECT DISTINCT [SynopsisResource], null AS [TitleResource]
	FROM [Movie]
	
	UNION
	
	SELECT DISTINCT [SynopsisResource], null AS [TitleResource]
	FROM [Serie]

	UNION

	SELECT DISTINCT [SynopsisResource], [TitleResource]
	FROM [Episode]

