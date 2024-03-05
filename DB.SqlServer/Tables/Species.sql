CREATE TABLE [dbo].[Species]
(
	[SpeciesId] SMALLINT NOT NULL IDENTITY(1,1),
	[Name] VARCHAR(50) NOT NULL,
    [PlanetId] SMALLINT NOT NULL,
    CONSTRAINT [PK_Species] PRIMARY KEY ([SpeciesId]),
	CONSTRAINT [FK_Species_Planet] FOREIGN KEY ([PlanetId]) REFERENCES [Place] ([PlaceId])
)
