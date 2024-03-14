using System.Text;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Migrations
{
    /// <inheritdoc />
    public partial class AddViews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var script = new StringBuilder("CREATE VIEW [dbo].[vwImdb] AS ");
            script.Append("SELECT [ImdbId] ");
            script.Append("FROM [dbo].[Serie] WITH(NOLOCK) ");
            script.Append("UNION ");
            script.Append("SELECT [ImdbId] ");
            script.Append("FROM [dbo].[Movie] WITH(NOLOCK)");
            
            migrationBuilder.Sql(script.ToString());
            script.Clear();

            script.Append("CREATE VIEW [dbo].[vwResourcesPlaces] AS ");
            script.Append("SELECT [NameResource] AS [Resource] ");
            script.Append("FROM [Place] WITH(NOLOCK) ");
            script.Append("UNION ");
            script.Append("SELECT [Type] AS [Resource] ");
            script.Append("FROM [PlaceType] WITH (NOLOCK) ");
            script.Append("UNION ");
            script.Append("SELECT [QuadrantResource] AS [Resource] ");
            script.Append("FROM [Quadrant] WITH (NOLOCK)");

            migrationBuilder.Sql(script.ToString());
            script.Clear();

            script.Append("CREATE VIEW [dbo].[vwResourcesTitleSynopsis] AS ");
            script.Append("SELECT [MovieId] AS [Id], [SynopsisResource] AS [Resource] ");
            script.Append("FROM [dbo].[Movie] WITH (NOLOCK) ");
            script.Append("UNION ");
            script.Append("SELECT [MovieId] AS [Id], [TitleResource] AS [Resource] ");
            script.Append("FROM [dbo].[Movie] WITH (NOLOCK) ");
            script.Append("UNION ");
            script.Append("SELECT [SerieId] AS [Id], [TitleResource] AS [Resource]");
            script.Append("FROM [dbo].[Serie] WITH (NOLOCK) ");
            script.Append("UNION ");
            script.Append("SELECT [SerieId] AS [Id], [SynopsisResource] AS [Resource] ");
            script.Append("FROM [dbo].[Serie] WITH (NOLOCK)");
            script.Append("UNION ");
            script.Append("SELECT [EpisodeId] AS [Id], [TitleResource] AS [Resource] ");
            script.Append("FROM [dbo].[Episode] WITH (NOLOCK) ");
            script.Append("UNION ");
            script.Append("SELECT [EpisodeId] AS [Id], [SynopsisResource] AS [Resource] ");
            script.Append("FROM [dbo].[Episode] WITH (NOLOCK)");

            migrationBuilder.Sql(script.ToString());
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE VIEW [dbo].[vwImdb]");
            migrationBuilder.Sql("DELETE VIEW [dbo].[vwResourcesPlaces]");
            migrationBuilder.Sql("DELETE VIEW [dbo].[vwResourcesTitleSynopsis]");
        }
    }
}
