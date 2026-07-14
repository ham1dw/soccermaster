using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace soccer.Migrations
{
    /// <inheritdoc />
    public partial class position : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Position",
                table: "Standings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HomeTeam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HomeResult = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HomeScore = table.Column<int>(type: "int", nullable: false),
                    AwayScore = table.Column<int>(type: "int", nullable: false),
                    AwayTeam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AwayResult = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HomePlayer1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HomePlayer2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HomePlayer3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HomePlayer4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AwayPlayer1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AwayPlayer2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AwayPlayer3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AwayPlayer4 = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.AlterColumn<int>(
                name: "Position",
                table: "Standings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
