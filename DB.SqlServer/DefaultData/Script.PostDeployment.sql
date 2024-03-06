/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
SET NOCOUNT ON;

:r .\Country.sql
:r .\Languages.sql
:r .\Timelines.sql
:r .\Movies.sql
:r .\Series.sql
:r .\Seasons.sql
:r .\Episodes.sql
--:r .\Roles.sql
:r .\Quadrants.sql
:r .\PlaceTypes.sql