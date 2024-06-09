using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RideRate.Migrations
{
    /// <inheritdoc />
    public partial class verificationTokenAsIntArr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerificationToken",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "VerificationTokenJson",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerificationTokenJson",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "VerificationToken",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }
    }
}
