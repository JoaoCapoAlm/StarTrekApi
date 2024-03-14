CREATE TABLE [dbo].[Serie]
(
	[SerieId] SMALLINT NOT NULL IDENTITY(1, 1),
	[OriginalName] VARCHAR(80) NOT NULL,
    [OriginalLanguageId] SMALLINT NOT NULL,
    [TimelineId] TINYINT NOT NULL DEFAULT ((1)),
    [TitleResource] VARCHAR(30) NOT NULL,
    [SynopsisResource] VARCHAR(30) NOT NULL,
    [Abbreviation] VARCHAR(3) NOT NULL,
    [ImdbId] VARCHAR(11) NULL,
    [TmdbId] INT NOT NULL,
    [DateSyncTmdb] DATETIME NULL, 
    CONSTRAINT [PK_Serie] PRIMARY KEY ([SerieId]),
    CONSTRAINT [FK_Serie_LanguageId] FOREIGN KEY ([OriginalLanguageId]) REFERENCES [Language] ([LanguageId]),
    CONSTRAINT [FK_Serie_TimelineId] FOREIGN KEY ([TimelineId]) REFERENCES [Timeline] ([TimelineId]),
    CONSTRAINT [UQ_Serie_TmdbId] UNIQUE ([TmdbId])
)
