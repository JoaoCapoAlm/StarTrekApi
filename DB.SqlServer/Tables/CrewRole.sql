CREATE TABLE [dbo].[CrewRole]
(
	[CrewRoleId] INT NOT NULL,
	[CrewId] INT NOT NULL,
	[RoleId] TINYINT NOT NULL,
    CONSTRAINT PK_CrewRole PRIMARY KEY ([CrewRoleId]),
	CONSTRAINT FK_CrewRole_Crew FOREIGN KEY ([CrewId]) REFERENCES [Crew] ([CrewId]),
	CONSTRAINT FK_CrewRole_Role FOREIGN KEY ([RoleId]) REFERENCES [Role] ([RoleId])
)
