CREATE VIEW [dbo].[vwImdb]
AS
	SELECT [ImdbId]
	FROM [dbo].[Serie]

	UNION
	
	SELECT [ImdbId]
	FROM [dbo].[Movie]
