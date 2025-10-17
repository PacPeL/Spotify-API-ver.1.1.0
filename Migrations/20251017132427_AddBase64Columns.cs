using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spotify_API.Migrations
{
    /// <inheritdoc />
    public partial class AddBase64Columns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CoverImageUrl",
                table: "Songs",
                newName: "ImageBase64");

            migrationBuilder.RenameColumn(
                name: "AudioUrl",
                table: "Songs",
                newName: "AudioBase64");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageBase64",
                table: "Songs",
                newName: "CoverImageUrl");

            migrationBuilder.RenameColumn(
                name: "AudioBase64",
                table: "Songs",
                newName: "AudioUrl");
        }
    }
}
