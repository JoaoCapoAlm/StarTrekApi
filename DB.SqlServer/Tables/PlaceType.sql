CREATE TABLE [dbo].[PlaceType]
(
	[PlaceTypeId] TINYINT NOT NULL IDENTITY(1,1),
	[Type] VARCHAR(30) NOT NULL, 
    CONSTRAINT [PK_PlaceType] PRIMARY KEY ([PlaceTypeId])
)
