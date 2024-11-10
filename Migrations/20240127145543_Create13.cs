using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Igrocom.Migrations
{
    /// <inheritdoc />
    public partial class Create13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameFiles_Game_GameId1",
                table: "GameFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameFiles",
                table: "GameFiles");

            migrationBuilder.DropIndex(
                name: "IX_GameFiles_GameId1",
                table: "GameFiles");

            migrationBuilder.DropColumn(
                name: "GameId1",
                table: "GameFiles");

            migrationBuilder.AlterColumn<int>(
                name: "GameId",
                table: "GameFiles",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameFiles",
                table: "GameFiles",
                columns: new[] { "Id", "GameId" });

            migrationBuilder.CreateIndex(
                name: "IX_GameFiles_GameId",
                table: "GameFiles",
                column: "GameId");

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

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameFiles",
                table: "GameFiles");

            migrationBuilder.DropIndex(
                name: "IX_GameFiles_GameId",
                table: "GameFiles");

            migrationBuilder.AlterColumn<int>(
                name: "GameId",
                table: "GameFiles",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "GameId1",
                table: "GameFiles",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameFiles",
                table: "GameFiles",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameFiles_GameId1",
                table: "GameFiles",
                column: "GameId1");

            migrationBuilder.AddForeignKey(
                name: "FK_GameFiles_Game_GameId1",
                table: "GameFiles",
                column: "GameId1",
                principalTable: "Game",
                principalColumn: "Id");
        }
    }
}
