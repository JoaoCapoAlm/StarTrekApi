CREATE VIEW [dbo].[vwResourcesPlaces]
AS
	SELECT [NameResource] AS [Resource]
	FROM [Place]

	UNION
	
	SELECT [Type] AS [Resource]
	FROM [PlaceType]

	UNION

	SELECT [QuadrantResource] AS [Resource]
	FROM [Quadrant]
