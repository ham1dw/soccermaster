using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace soccer.Migrations
{
    /// <inheritdoc />
    public partial class teams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TeamLogoURl",
                table: "NextMatches",
                newName: "Team2LogoUrl");

            migrationBuilder.AddColumn<string>(
                name: "Team1LogoUrl",
                table: "NextMatches",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Team1LogoUrl",
                table: "NextMatches");

            migrationBuilder.RenameColumn(
                name: "Team2LogoUrl",
                table: "NextMatches",
                newName: "TeamLogoURl");
        }
    }
}
