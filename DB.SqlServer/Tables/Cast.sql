CREATE TABLE [dbo].[Cast]
(
	[CastId] INT NOT NULL IDENTITY(1,1),
    [Name] VARCHAR(250) NOT NULL,
    [BirthDate] DATE NOT NULL,
    [DeathDate] DATE NULL,
    [CountryId] SMALLINT NULL, 
    CONSTRAINT [PK_Cast] PRIMARY KEY ([CastId]),
    CONSTRAINT [FK_Cast_CountryId] FOREIGN KEY ([CountryId]) REFERENCES [Country] ([CountryId])
)
