using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace soccer.Migrations
{
    /// <inheritdoc />
    public partial class logos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Team1LogoUrl",
                table: "Matches",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Team2LogoUrl",
                table: "Matches",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Team1LogoUrl",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "Team2LogoUrl",
                table: "Matches");
        }
    }
}
