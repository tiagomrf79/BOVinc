using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Animal.API.Migrations
{
    /// <inheritdoc />
    public partial class AddPkToAnimalStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "AnimalStatus",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AnimalStatus",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "AnimalStatus",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnimalStatus",
                table: "AnimalStatus",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AnimalStatus",
                table: "AnimalStatus");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AnimalStatus");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AnimalStatus");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                table: "AnimalStatus");
        }
    }
}
