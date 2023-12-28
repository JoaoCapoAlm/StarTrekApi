﻿CREATE TABLE [dbo].[Country]
(
	[IdCountry] SMALLINT NOT NULL IDENTITY(1,1),
    [ResourceName] VARCHAR(100) NOT NULL,
	[ISO] CHAR(2) NOT NULL,
    CONSTRAINT [PK_Country] PRIMARY KEY ([IdCountry]),
	CONSTRAINT [UQ_Country_ISO] UNIQUE ([ISO])
)
