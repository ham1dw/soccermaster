using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace soccer.Migrations
{
    /// <inheritdoc />
    public partial class league : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "League",
                table: "NextMatches",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TeamLogoURl",
                table: "NextMatches",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "League",
                table: "NextMatches");

            migrationBuilder.DropColumn(
                name: "TeamLogoURl",
                table: "NextMatches");
        }
    }
}
