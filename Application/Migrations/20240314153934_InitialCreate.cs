using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CharacterClassification",
                columns: table => new
                {
                    CharacterClassificationId = table.Column<byte>(type: "tinyint", nullable: false),
                    Classification = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterClassification", x => x.CharacterClassificationId);
                });

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    CountryId = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResourceName = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    LanguageId = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResourceName = table.Column<string>(type: "varchar(50)", nullable: false),
                    CodeISO = table.Column<string>(type: "varchar(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("FK_Language", x => x.LanguageId);
                });

            migrationBuilder.CreateTable(
                name: "PlaceType",
                columns: table => new
                {
                    PlaceTypeId = table.Column<byte>(type: "tinyint", nullable: false),
                    Type = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaceType", x => x.PlaceTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Quadrant",
                columns: table => new
                {
                    QuadrantId = table.Column<byte>(type: "tinyint", nullable: false),
                    QuadrantResource = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quadrant", x => x.QuadrantId);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleId = table.Column<byte>(type: "tinyint", nullable: false),
                    RoleResource = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Timeline",
                columns: table => new
                {
                    TimelineId = table.Column<byte>(type: "tinyint", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timeline", x => x.TimelineId);
                });

            migrationBuilder.CreateTable(
                name: "Crew",
                columns: table => new
                {
                    CrewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CountryId = table.Column<short>(type: "smallint", nullable: true),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    DeathDate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Crew", x => x.CrewId);
                    table.ForeignKey(
                        name: "FK_Crew_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onUpdate: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Place",
                columns: table => new
                {
                    PlaceId = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameResource = table.Column<string>(type: "varchar(50)", nullable: false),
                    QuadrantId = table.Column<byte>(type: "tinyint", nullable: false),
                    PlaceTypeId = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Place", x => x.PlaceId);
                    table.ForeignKey(
                        name: "FK_Place_PlaceType",
                        column: x => x.PlaceTypeId,
                        principalTable: "PlaceType",
                        principalColumn: "PlaceTypeId",
                        onUpdate: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Place_Quadrant",
                        column: x => x.QuadrantId,
                        principalTable: "Quadrant",
                        principalColumn: "QuadrantId",
                        onUpdate: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Movie",
                columns: table => new
                {
                    MovieId = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OriginalName = table.Column<string>(type: "varchar(150)", nullable: false),
                    TitleResource = table.Column<string>(type: "varchar(50)", nullable: false),
                    SynopsisResource = table.Column<string>(type: "varchar(50)", nullable: false),
                    OriginalLanguageId = table.Column<short>(type: "smallint", nullable: false),
                    ReleaseDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Time = table.Column<short>(type: "smallint", nullable: false),
                    ImdbId = table.Column<string>(type: "varchar(10)", nullable: true),
                    TimelineId = table.Column<byte>(type: "tinyint", nullable: false),
                    TmdbId = table.Column<int>(type: "int", nullable: false),
                    DateSyncTmdb = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movie", x => x.MovieId);
                    table.ForeignKey(
                        name: "FK_Movie_LanguageId",
                        column: x => x.OriginalLanguageId,
                        principalTable: "Language",
                        principalColumn: "LanguageId",
                        onUpdate: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Movie_TimelineId",
                        column: x => x.TimelineId,
                        principalTable: "Timeline",
                        principalColumn: "TimelineId",
                        onUpdate: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Serie",
                columns: table => new
                {
                    SerieId = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OriginalName = table.Column<string>(type: "varchar(150)", nullable: false),
                    OriginalLanguageId = table.Column<short>(type: "smallint", nullable: false),
                    TimelineId = table.Column<byte>(type: "tinyint", nullable: false),
                    ImdbId = table.Column<string>(type: "varchar(10)", nullable: false),
                    Abbreviation = table.Column<string>(type: "varchar(3)", nullable: false),
                    SynopsisResource = table.Column<string>(type: "varchar(50)", nullable: false),
                    TitleResource = table.Column<string>(type: "varchar(50)", nullable: false),
                    TmdbId = table.Column<int>(type: "int", nullable: false),
                    DateSyncTmdb = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Serie", x => x.SerieId);
                    table.ForeignKey(
                        name: "FK_Serie_LanguageId",
                        column: x => x.OriginalLanguageId,
                        principalTable: "Language",
                        principalColumn: "LanguageId",
                        onUpdate: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Serie_TimelineId",
                        column: x => x.TimelineId,
                        principalTable: "Timeline",
                        principalColumn: "TimelineId",
                        onUpdate: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CrewRole",
                columns: table => new
                {
                    CrewRoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CrewId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrewRole", x => x.CrewRoleId);
                    table.ForeignKey(
                        name: "FK_CrewRole_Crew",
                        column: x => x.CrewId,
                        principalTable: "Crew",
                        principalColumn: "CrewId",
                        onUpdate: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CrewRole_Role",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onUpdate: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Species",
                columns: table => new
                {
                    SpeciesId = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResourceName = table.Column<string>(type: "varchar(50)", nullable: false),
                    PlanetId = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Species", x => x.SpeciesId);
                    table.ForeignKey(
                        name: "FK_Species_Planet",
                        column: x => x.PlanetId,
                        principalTable: "Place",
                        principalColumn: "PlaceId",
                        onUpdate: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Season",
                columns: table => new
                {
                    SeasonId = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SerieId = table.Column<short>(type: "smallint", nullable: false),
                    Number = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Season", x => x.SeasonId);
                    table.ForeignKey(
                        name: "FK_Season_Serie",
                        column: x => x.SerieId,
                        principalTable: "Serie",
                        principalColumn: "SerieId",
                        onUpdate: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Character",
                columns: table => new
                {
                    CharacterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    SpeciesId = table.Column<short>(type: "smallint", nullable: false),
                    ClassificationId = table.Column<byte>(type: "tinyint", nullable: false),
                    Name = table.Column<string>(type: "varchar(150)", nullable: false),
                    DeathDate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Character", x => x.CharacterId);
                    table.ForeignKey(
                        name: "FK_Character_Classification",
                        column: x => x.ClassificationId,
                        principalTable: "CharacterClassification",
                        principalColumn: "CharacterClassificationId",
                        onUpdate: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Character_Species",
                        column: x => x.SpeciesId,
                        principalTable: "Species",
                        principalColumn: "SpeciesId",
                        onUpdate: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Episode",
                columns: table => new
                {
                    EpisodeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeasonId = table.Column<short>(type: "smallint", nullable: false),
                    RealeaseDate = table.Column<DateOnly>(type: "date", nullable: true),
                    TitleResource = table.Column<string>(type: "varchar(50)", nullable: false),
                    SynopsisResource = table.Column<string>(type: "varchar(50)", nullable: false),
                    Time = table.Column<byte>(type: "tinyint", nullable: true),
                    Number = table.Column<byte>(type: "tinyint", nullable: false),
                    StardateFrom = table.Column<float>(type: "real", nullable: true),
                    StardateTo = table.Column<float>(type: "real", nullable: true),
                    ImdbId = table.Column<string>(type: "varchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Episode", x => x.EpisodeId);
                    table.ForeignKey(
                        name: "FK_Episode_Season",
                        column: x => x.SeasonId,
                        principalTable: "Season",
                        principalColumn: "SeasonId",
                        onUpdate: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Character_ClassificationId",
                table: "Character",
                column: "ClassificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Character_SpeciesId",
                table: "Character",
                column: "SpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_Crew_CountryId",
                table: "Crew",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_CrewRole_CrewId",
                table: "CrewRole",
                column: "CrewId");

            migrationBuilder.CreateIndex(
                name: "IX_CrewRole_RoleId",
                table: "CrewRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Episode_SeasonId",
                table: "Episode",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Movie_OriginalLanguageId",
                table: "Movie",
                column: "OriginalLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Movie_TimelineId",
                table: "Movie",
                column: "TimelineId");

            migrationBuilder.CreateIndex(
                name: "IX_Place_PlaceTypeId",
                table: "Place",
                column: "PlaceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Place_QuadrantId",
                table: "Place",
                column: "QuadrantId");

            migrationBuilder.CreateIndex(
                name: "IX_Season_SerieId",
                table: "Season",
                column: "SerieId");

            migrationBuilder.CreateIndex(
                name: "IX_Serie_OriginalLanguageId",
                table: "Serie",
                column: "OriginalLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Serie_TimelineId",
                table: "Serie",
                column: "TimelineId");

            migrationBuilder.CreateIndex(
                name: "IX_Species_PlanetId",
                table: "Species",
                column: "PlanetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Character");

            migrationBuilder.DropTable(
                name: "CrewRole");

            migrationBuilder.DropTable(
                name: "Episode");

            migrationBuilder.DropTable(
                name: "Movie");

            migrationBuilder.DropTable(
                name: "CharacterClassification");

            migrationBuilder.DropTable(
                name: "Species");

            migrationBuilder.DropTable(
                name: "Crew");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Season");

            migrationBuilder.DropTable(
                name: "Place");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropTable(
                name: "Serie");

            migrationBuilder.DropTable(
                name: "PlaceType");

            migrationBuilder.DropTable(
                name: "Quadrant");

            migrationBuilder.DropTable(
                name: "Language");

            migrationBuilder.DropTable(
                name: "Timeline");
        }
    }
}
