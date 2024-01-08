CREATE TABLE [dbo].[Timeline]
(
	[TimelineId] TINYINT NOT NULL IDENTITY(1,1),
    [Name] VARCHAR(50) NOT NULL,
	CONSTRAINT [PK_Timeline] PRIMARY KEY ([TimelineId])
)
