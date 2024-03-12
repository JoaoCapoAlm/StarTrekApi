CREATE TABLE [dbo].[Character]
(
	[CharacterId] INT NOT NULL IDENTITY(1,1),
	[Name] VARCHAR(100) NOT NULL, 
    [DateBirth] DATE NULL, 
    [DateDeath] DATE NULL, 
    [SpeciesId] SMALLINT NOT NULL, 
    [ClassificationId] INT NOT NULL, 
    CONSTRAINT [PK_Character] PRIMARY KEY ([CharacterId]),
    CONSTRAINT [FK_Character_Species] FOREIGN KEY ([SpeciesId]) REFERENCES [dbo].[Species] ([SpeciesId]),
    CONSTRAINT [FK_Character_Classification] FOREIGN KEY ([ClassificationId]) REFERENCES [dbo].[CharacterClassification] ([CharacterClassificationId])
)
