using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddTableAlbumSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AlbumSets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumSets", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlbumSets_Name",
                table: "AlbumSets",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlbumSets");
        }
    }
}
