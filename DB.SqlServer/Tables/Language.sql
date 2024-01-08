CREATE TABLE [dbo].[Language]
(
	[LanguageId] SMALLINT NOT NULL IDENTITY(1,1),
    [ResourceName] VARCHAR(50) NOT NULL, 
    [CodeISO] NCHAR(6) NULL,
    CONSTRAINT [FK_Language] PRIMARY KEY ([LanguageId])
)
