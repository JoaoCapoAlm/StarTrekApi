using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Migrations
{
    /// <inheritdoc />
    public partial class update_dates_character : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateBirth",
                table: "Character");

            migrationBuilder.DropColumn(
                name: "DeathDate",
                table: "Character");

            migrationBuilder.AddColumn<byte>(
                name: "DayBirth",
                table: "Character",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "DayDeath",
                table: "Character",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "MonthBirth",
                table: "Character",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "MonthDeath",
                table: "Character",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "YearBirth",
                table: "Character",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "YearDeath",
                table: "Character",
                type: "smallint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayBirth",
                table: "Character");

            migrationBuilder.DropColumn(
                name: "DayDeath",
                table: "Character");

            migrationBuilder.DropColumn(
                name: "MonthBirth",
                table: "Character");

            migrationBuilder.DropColumn(
                name: "MonthDeath",
                table: "Character");

            migrationBuilder.DropColumn(
                name: "YearBirth",
                table: "Character");

            migrationBuilder.DropColumn(
                name: "YearDeath",
                table: "Character");

            migrationBuilder.AddColumn<DateOnly>(
                name: "DateBirth",
                table: "Character",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "DeathDate",
                table: "Character",
                type: "date",
                nullable: true);
        }
    }
}
