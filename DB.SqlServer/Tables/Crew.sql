CREATE TABLE [dbo].[Crew]
(
	[CrewId] INT NOT NULL IDENTITY(1,1),
    [Name] VARCHAR(250) NOT NULL,
    [BirthDate] DATE NOT NULL,
    [DeathDate] DATE NULL,
    [CountryId] SMALLINT NULL, 
    [ImdbId] VARCHAR(9) NULL, 
    CONSTRAINT [PK_Crew] PRIMARY KEY ([CrewId]),
    CONSTRAINT [FK_Crew_CountryId] FOREIGN KEY ([CountryId]) REFERENCES [Country] ([CountryId])
)
