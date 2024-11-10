using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Igrocom.Migrations
{
    /// <inheritdoc />
    public partial class GameUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Peculiarities",
                table: "Game",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Review",
                table: "Game",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Peculiarities",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "Review",
                table: "Game");
        }
    }
}
