using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RideRate.Migrations
{
    /// <inheritdoc />
    public partial class addedIsActiveFlaToTokenFamily : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TokenFamily",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TokenFamily");
        }
    }
}
