CREATE TABLE [dbo].[CharacterClassification]
(
	[CharacterClassificationId] TINYINT NOT NULL IDENTITY(1,1),
	[Classification] VARCHAR(50) NOT NULL,
    CONSTRAINT [PK_CharacterClassification] PRIMARY KEY ([CharacterClassificationId])
)
