using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Igrocom.Migrations
{
    /// <inheritdoc />
    public partial class Create11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GameFiles",
                table: "GameFiles");

            migrationBuilder.DropIndex(
                name: "IX_GameFiles_GameId",
                table: "GameFiles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameFiles",
                table: "GameFiles",
                columns: new[] { "GameId", "Id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GameFiles",
                table: "GameFiles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameFiles",
                table: "GameFiles",
                columns: new[] { "Id", "GameId" });

            migrationBuilder.CreateIndex(
                name: "IX_GameFiles_GameId",
                table: "GameFiles",
                column: "GameId");
        }
    }
}
