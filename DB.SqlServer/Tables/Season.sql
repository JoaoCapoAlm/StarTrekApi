CREATE TABLE [dbo].[Season]
(
	[SeasonId] SMALLINT NOT NULL IDENTITY(1,1),
	[SerieId] SMALLINT NOT NULL, 
    [Number] TINYINT NOT NULL, 
    CONSTRAINT [PK_Season] PRIMARY KEY ([SeasonId]),
	CONSTRAINT [FK_Season_Serie] FOREIGN KEY ([SerieId]) REFERENCES [Serie] ([SerieId])
)
