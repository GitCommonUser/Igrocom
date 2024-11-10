using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Igrocom.Migrations
{
    /// <inheritdoc />
    public partial class GameUpdate4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Content");

            migrationBuilder.RenameColumn(
                name: "Genre",
                table: "Content",
                newName: "ImageMime");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Content",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Content",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Preface",
                table: "Content",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "Content",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "MediaFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    File = table.Column<byte[]>(type: "bytea", nullable: true),
                    FileMime = table.Column<string>(type: "text", nullable: true),
                    ContentId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaFiles_Content_ContentId",
                        column: x => x.ContentId,
                        principalTable: "Content",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_ContentId",
                table: "MediaFiles",
                column: "ContentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MediaFiles");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Content");

            migrationBuilder.DropColumn(
                name: "Preface",
                table: "Content");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "Content");

            migrationBuilder.RenameColumn(
                name: "ImageMime",
                table: "Content",
                newName: "Genre");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Content",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Content",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
