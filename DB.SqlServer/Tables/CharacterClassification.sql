CREATE TABLE [dbo].[CharacterClassification]
(
	[CharacterClassificationId] INT NOT NULL IDENTITY(1,1),
	[Classification] VARCHAR(50) NOT NULL,
    CONSTRAINT [PK_CharacterClassification] PRIMARY KEY ([CharacterClassificationId])
)
