CREATE TABLE [dbo].[Quadrant]
(
	[QuadrantId] TINYINT NOT NULL IDENTITY(1,1),
	[QuadrantResource] VARCHAR(40) NOT NULL, 
    CONSTRAINT [PK_Quadrant] PRIMARY KEY ([QuadrantId])
)
