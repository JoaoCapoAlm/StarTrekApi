CREATE TABLE [dbo].[Place]
(
	[PlaceId] SMALLINT NOT NULL IDENTITY(1, 1),
	[Name] VARBINARY(50) NOT NULL,
    [QuadrantId] TINYINT NOT NULL,
    [PlaceTypeId] TINYINT NOT NULL,
    CONSTRAINT [PK_Place] PRIMARY KEY ([PlaceId]),
	CONSTRAINT [FK_Place_Quadrant] FOREIGN KEY ([QuadrantId]) REFERENCES [Quadrant] ([QuadrantId]),
	CONSTRAINT [FK_Place_PlaceType] FOREIGN KEY ([PlaceTypeId]) REFERENCES [PlaceType] ([PlaceTypeId])
)
