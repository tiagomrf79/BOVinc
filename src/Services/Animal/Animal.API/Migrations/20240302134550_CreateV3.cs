using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Animal.API.Migrations
{
    /// <inheritdoc />
    public partial class CreateV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Breed",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GestationLength = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Breed", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BreedingStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    BreedingStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BreedingStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Catalog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Catalog = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catalog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MilkingStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    MilkingStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MilkingStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Purpose",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Purpose = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purpose", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sex",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Sex = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sex", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Animal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    RegistrationId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: true),
                    DamId = table.Column<int>(type: "int", nullable: true),
                    SireId = table.Column<int>(type: "int", nullable: true),
                    SexId = table.Column<int>(type: "int", nullable: false),
                    BreedId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    PurposeId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CatalogId = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Animal_Animal_DamId",
                        column: x => x.DamId,
                        principalTable: "Animal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Animal_Animal_SireId",
                        column: x => x.SireId,
                        principalTable: "Animal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Animal_Breed_BreedId",
                        column: x => x.BreedId,
                        principalTable: "Breed",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Animal_Catalog_CatalogId",
                        column: x => x.CatalogId,
                        principalTable: "Catalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Animal_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Animal_Purpose_PurposeId",
                        column: x => x.PurposeId,
                        principalTable: "Purpose",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Animal_Sex_SexId",
                        column: x => x.SexId,
                        principalTable: "Sex",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AnimalStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    AnimalId = table.Column<int>(type: "int", nullable: false),
                    CurrentGroupName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateLeftHerd = table.Column<DateTime>(type: "date", nullable: true),
                    ReasonLeftHerd = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MilkingStatusId = table.Column<int>(type: "int", nullable: true),
                    LastCalvingDate = table.Column<DateTime>(type: "date", nullable: true),
                    SheduledDryDate = table.Column<DateTime>(type: "date", nullable: true),
                    LastDryDate = table.Column<DateTime>(type: "date", nullable: true),
                    BreedingStatusId = table.Column<int>(type: "int", nullable: true),
                    LastHeatDate = table.Column<DateTime>(type: "date", nullable: true),
                    ExpectedHeatDate = table.Column<DateTime>(type: "date", nullable: true),
                    LastBreedingDate = table.Column<DateTime>(type: "date", nullable: true),
                    LastBreedingBull = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DueDateForCalving = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimalStatus_Animal_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimalStatus_BreedingStatus_BreedingStatusId",
                        column: x => x.BreedingStatusId,
                        principalTable: "BreedingStatus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AnimalStatus_MilkingStatus_MilkingStatusId",
                        column: x => x.MilkingStatusId,
                        principalTable: "MilkingStatus",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Lactation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    LactationNumber = table.Column<int>(type: "int", nullable: false),
                    CalvingDate = table.Column<DateTime>(type: "date", nullable: false),
                    EndDate = table.Column<DateTime>(type: "date", nullable: true),
                    FarmAnimalId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lactation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lactation_Animal_FarmAnimalId",
                        column: x => x.FarmAnimalId,
                        principalTable: "Animal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "BreedingStatus",
                columns: new[] { "Id", "BreedingStatus" },
                values: new object[,]
                {
                    { 1, "Open" },
                    { 2, "Bred" },
                    { 3, "Confirmed" }
                });

            migrationBuilder.InsertData(
                table: "Catalog",
                columns: new[] { "Id", "Catalog" },
                values: new object[,]
                {
                    { 1, "Initial inventory" },
                    { 2, "Historic record" },
                    { 3, "Calving" },
                    { 4, "Transfer" }
                });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "Category" },
                values: new object[,]
                {
                    { 1, "Calf" },
                    { 2, "Heifer" },
                    { 3, "Milking Cow" },
                    { 4, "Dry Cow" },
                    { 5, "Bull" },
                    { 6, "Steer" }
                });

            migrationBuilder.InsertData(
                table: "MilkingStatus",
                columns: new[] { "Id", "MilkingStatus" },
                values: new object[,]
                {
                    { 1, "Milking" },
                    { 2, "Dry" }
                });

            migrationBuilder.InsertData(
                table: "Purpose",
                columns: new[] { "Id", "Purpose" },
                values: new object[,]
                {
                    { 1, "Breeding" },
                    { 2, "Milk" },
                    { 3, "Meat" },
                    { 4, "To cull" },
                    { 5, "To sell" }
                });

            migrationBuilder.InsertData(
                table: "Sex",
                columns: new[] { "Id", "Sex" },
                values: new object[,]
                {
                    { 1, "Female" },
                    { 2, "Male" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Animal_BreedId",
                table: "Animal",
                column: "BreedId");

            migrationBuilder.CreateIndex(
                name: "IX_Animal_CatalogId",
                table: "Animal",
                column: "CatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_Animal_CategoryId",
                table: "Animal",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Animal_DamId",
                table: "Animal",
                column: "DamId");

            migrationBuilder.CreateIndex(
                name: "IX_Animal_PurposeId",
                table: "Animal",
                column: "PurposeId");

            migrationBuilder.CreateIndex(
                name: "IX_Animal_SexId",
                table: "Animal",
                column: "SexId");

            migrationBuilder.CreateIndex(
                name: "IX_Animal_SireId",
                table: "Animal",
                column: "SireId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalStatus_AnimalId",
                table: "AnimalStatus",
                column: "AnimalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnimalStatus_BreedingStatusId",
                table: "AnimalStatus",
                column: "BreedingStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalStatus_MilkingStatusId",
                table: "AnimalStatus",
                column: "MilkingStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Lactation_FarmAnimalId",
                table: "Lactation",
                column: "FarmAnimalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimalStatus");

            migrationBuilder.DropTable(
                name: "Lactation");

            migrationBuilder.DropTable(
                name: "BreedingStatus");

            migrationBuilder.DropTable(
                name: "MilkingStatus");

            migrationBuilder.DropTable(
                name: "Animal");

            migrationBuilder.DropTable(
                name: "Breed");

            migrationBuilder.DropTable(
                name: "Catalog");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Purpose");

            migrationBuilder.DropTable(
                name: "Sex");
        }
    }
}
