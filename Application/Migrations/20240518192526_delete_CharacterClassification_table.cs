using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Migrations
{
    /// <inheritdoc />
    public partial class delete_CharacterClassification_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Character_Classification",
                table: "Character");

            migrationBuilder.DropTable(
                name: "CharacterClassification");

            migrationBuilder.DropIndex(
                name: "IX_Character_ClassificationId",
                table: "Character");

            migrationBuilder.DropColumn(
                name: "ClassificationId",
                table: "Character");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "ClassificationId",
                table: "Character",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateTable(
                name: "CharacterClassification",
                columns: table => new
                {
                    CharacterClassificationId = table.Column<byte>(type: "tinyint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Classification = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterClassification", x => x.CharacterClassificationId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Character_ClassificationId",
                table: "Character",
                column: "ClassificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Character_Classification",
                table: "Character",
                column: "ClassificationId",
                principalTable: "CharacterClassification",
                principalColumn: "CharacterClassificationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
