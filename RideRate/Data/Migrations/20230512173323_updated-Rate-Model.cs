using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RideRate.Migrations
{
    /// <inheritdoc />
    public partial class updatedRateModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Rate",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Rate",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Rate");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Rate");
        }
    }
}
