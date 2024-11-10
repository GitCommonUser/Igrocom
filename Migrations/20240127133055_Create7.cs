using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Igrocom.Migrations
{
    /// <inheritdoc />
    public partial class Create7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameFiles_Game_GameId",
                table: "GameFiles");

            migrationBuilder.AlterColumn<int>(
                name: "GameId",
                table: "GameFiles",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GameFiles_Game_GameId",
                table: "GameFiles",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameFiles_Game_GameId",
                table: "GameFiles");

            migrationBuilder.AlterColumn<int>(
                name: "GameId",
                table: "GameFiles",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_GameFiles_Game_GameId",
                table: "GameFiles",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "Id");
        }
    }
}
