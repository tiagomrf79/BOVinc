using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Animal.API.Migrations
{
    /// <inheritdoc />
    public partial class AddStaticData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BreedingStatus",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BreedingStatus",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "BreedingStatus",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Catalog",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Catalog",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Catalog",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Catalog",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "MilkingStatus",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MilkingStatus",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Purpose",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Purpose",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Purpose",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Purpose",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Purpose",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Sex",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Sex",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
