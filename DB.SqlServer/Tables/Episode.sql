CREATE TABLE [dbo].[Episode]
(
	[EpisodeId] INT NOT NULL IDENTITY(1,1),
	[SeasonId] SMALLINT NOT NULL, 
    [RealeaseDate] DATE NULL, 
    [TitleResource] VARCHAR(40) NOT NULL, 
    [SynopsisResource] VARCHAR(48) NOT NULL, 
    [Time] TINYINT NULL, 
    [Number] TINYINT NOT NULL, 
    [StardateFrom] FLOAT NULL, 
    [StardateTo] FLOAT NULL, 
    [ImdbId] VARCHAR(9) NULL, 
    CONSTRAINT [PK_Episode] PRIMARY KEY ([EpisodeId]),
    CONSTRAINT [FK_Episode_Season] FOREIGN KEY ([SeasonId]) REFERENCES [Season] ([SeasonId])
)
