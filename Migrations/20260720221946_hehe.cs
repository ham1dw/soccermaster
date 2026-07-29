using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace soccer.Migrations
{
    /// <inheritdoc />
    public partial class hehe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Goals",
                table: "Players",
                newName: "WeeklyGoals");

            migrationBuilder.RenameColumn(
                name: "Assists",
                table: "Players",
                newName: "WeeklyAssists");

            migrationBuilder.AddColumn<int>(
                name: "TotalAssists",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalGoals",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalAssists",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "TotalGoals",
                table: "Players");

            migrationBuilder.RenameColumn(
                name: "WeeklyGoals",
                table: "Players",
                newName: "Goals");

            migrationBuilder.RenameColumn(
                name: "WeeklyAssists",
                table: "Players",
                newName: "Assists");
        }
    }
}
