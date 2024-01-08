CREATE TABLE [dbo].[Movie]
(
	[MovieId] SMALLINT NOT NULL IDENTITY,
	[OriginalName] VARCHAR(200) NULL, 
    [SynopsisResource] VARCHAR(25) NULL, 
    [OriginalLanguageId] SMALLINT NOT NULL,
    [ReleaseDate] DATETIME NOT NULL, 
    [Time] SMALLINT NOT NULL, 
    [ImdbId] VARCHAR(7) NULL, 
    [TimelineId] TINYINT NOT NULL, 
    CONSTRAINT [PK_Movie] PRIMARY KEY ([MovieId]),
    CONSTRAINT [FK_Movie_LanguageId] FOREIGN KEY ([OriginalLanguageId]) REFERENCES [Language] ([LanguageId]),
    CONSTRAINT [FK_Movie_TimelineId] FOREIGN KEY ([TimelineId]) REFERENCES [Timeline] ([TimelineId])
)
