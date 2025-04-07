using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Moosic.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrltoMusic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Genre",
                table: "MusicItems",
                newName: "ImageUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "MusicItems",
                newName: "Genre");
        }
    }
}
