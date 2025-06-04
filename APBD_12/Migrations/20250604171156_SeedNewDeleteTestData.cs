using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APBD_12.Migrations
{
    /// <inheritdoc />
    public partial class SeedNewDeleteTestData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Client",
                columns: new[] { "IdClient", "Email", "FirstName", "LastName", "Pesel", "Telephone" },
                values: new object[] { 10, "delete@example.com", "Delete", "test", "29020254321", "897654321" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Client",
                keyColumn: "IdClient",
                keyValue: 10);
        }
    }
}
