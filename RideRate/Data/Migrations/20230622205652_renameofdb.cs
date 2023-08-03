using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RideRate.Migrations
{
    /// <inheritdoc />
    public partial class renameofdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RefreshToken",
                table: "RefreshToken");

            migrationBuilder.RenameTable(
                name: "RefreshToken",
                newName: "TokenFamily");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TokenFamily",
                table: "TokenFamily",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TokenFamily",
                table: "TokenFamily");

            migrationBuilder.RenameTable(
                name: "TokenFamily",
                newName: "RefreshToken");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RefreshToken",
                table: "RefreshToken",
                column: "Id");
        }
    }
}
