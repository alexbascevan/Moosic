using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Moosic.Migrations
{
    /// <inheritdoc />
    public partial class ExtraMusicDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "popularity",
                table: "MusicItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "releaseDate",
                table: "MusicItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "totalTracks",
                table: "MusicItems",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "popularity",
                table: "MusicItems");

            migrationBuilder.DropColumn(
                name: "releaseDate",
                table: "MusicItems");

            migrationBuilder.DropColumn(
                name: "totalTracks",
                table: "MusicItems");
        }
    }
}
