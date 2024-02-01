CREATE TABLE [dbo].[CrewRole]
(
	[CrewRoleId] INT NOT NULL,
	[CrewId] INT NOT NULL,
	[RoleId] TINYINT NOT NULL,
    [EpisodeId] INT NULL,
    [MovieId] SMALLINT NULL,
    CONSTRAINT PK_CrewRole PRIMARY KEY ([CrewRoleId]),
	CONSTRAINT FK_CrewRole_Crew FOREIGN KEY ([CrewId]) REFERENCES [Crew] ([CrewId]),
	CONSTRAINT FK_CrewRole_Role FOREIGN KEY ([RoleId]) REFERENCES [Role] ([RoleId]),
	CONSTRAINT FK_CrewRole_Episode FOREIGN KEY ([EpisodeId]) REFERENCES [Episode] ([EpisodeId]),
	CONSTRAINT FK_CrewRole_Movie FOREIGN KEY ([MovieId]) REFERENCES [Movie] ([MovieId])
)
