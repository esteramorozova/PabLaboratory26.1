using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdminName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
                columns: new[] { "FirstName", "FullName", "LastName" },
                values: new object[] { "Adam", "Adam Kowalko", "Kowalko" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
                columns: new[] { "FirstName", "FullName", "LastName" },
                values: new object[] { "Jan", "Jan Kowalski", "Kowalski" });
        }
    }
}
