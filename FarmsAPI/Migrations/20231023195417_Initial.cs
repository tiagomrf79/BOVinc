using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FarmsAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Farms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Region = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Farms", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Farms",
                columns: new[] { "Id", "Address", "City", "Country", "DateCreated", "DateUpdated", "Name", "Region" },
                values: new object[,]
                {
                    { 1, "Avenida Diogo Leite 70", "Porto", "Portugal", new DateTime(2023, 10, 7, 15, 57, 36, 0, DateTimeKind.Unspecified), new DateTime(2023, 10, 11, 17, 51, 22, 0, DateTimeKind.Unspecified), "Jorge Oliveira de Pacheco", "Porto" },
                    { 2, "Estrada Monumental 316", "Funchal", "Portugal", new DateTime(2023, 4, 3, 17, 32, 38, 0, DateTimeKind.Unspecified), new DateTime(2023, 4, 9, 15, 34, 34, 0, DateTimeKind.Unspecified), "Eduarda Clara Moreira", "Madeira" },
                    { 3, "Avenida Calouste Gulbenkian 22B", "Coimbra", "Portugal", new DateTime(2023, 7, 15, 16, 42, 14, 0, DateTimeKind.Unspecified), null, "Nuno Abreu Nascimento", "Coimbra" },
                    { 4, "Largo Senhora-A-Branca 144", "Braga", "Portugal", new DateTime(2023, 8, 28, 15, 34, 34, 0, DateTimeKind.Unspecified), null, "André Raúl Cardoso", "Braga" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Farms");
        }
    }
}
