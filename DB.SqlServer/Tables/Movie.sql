CREATE TABLE [dbo].[Movie]
(
	[MovieId] SMALLINT NOT NULL IDENTITY,
	[OriginalName] VARCHAR(200) NULL, 
    [SynopsisResource] VARCHAR(25) NULL, 
    [OriginalLanguageId] SMALLINT NOT NULL,
    [ReleaseDate] DATE NOT NULL, 
    [Time] SMALLINT NOT NULL, 
    [TimelineId] TINYINT NOT NULL, 
    [ImdbId] VARCHAR(9) NULL,
    [TmdbId] INT NOT NULL,
    [DateSyncTmdb] DATETIME NULL,
    CONSTRAINT [PK_Movie] PRIMARY KEY ([MovieId]),
    CONSTRAINT [FK_Movie_LanguageId] FOREIGN KEY ([OriginalLanguageId]) REFERENCES [Language] ([LanguageId]),
    CONSTRAINT [FK_Movie_TimelineId] FOREIGN KEY ([TimelineId]) REFERENCES [Timeline] ([TimelineId]),
    CONSTRAINT [UQ_Movie_TmdbId] UNIQUE ([TmdbId])
)
