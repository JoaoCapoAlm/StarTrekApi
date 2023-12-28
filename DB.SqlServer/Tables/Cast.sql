CREATE TABLE [dbo].[Cast]
(
	[CastId] UNIQUEIDENTIFIER NOT NULL,
    [Name] VARCHAR(250) NOT NULL,
    [BirthDate] DATE NULL,
    [DeathDate] DATE NULL,
    [CountryId] SMALLINT NULL, 
    CONSTRAINT [PK_Cast] PRIMARY KEY ([CastId]),
    CONSTRAINT [FK_Cast_CountryId] FOREIGN KEY ([CountryId]) REFERENCES [Country] ([IdCountry])
)
