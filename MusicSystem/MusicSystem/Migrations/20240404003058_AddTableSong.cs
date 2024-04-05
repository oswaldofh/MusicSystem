using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddTableSong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SongSets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AlbumSetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SongSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SongSets_AlbumSets_AlbumSetId",
                        column: x => x.AlbumSetId,
                        principalTable: "AlbumSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SongSets_AlbumSetId",
                table: "SongSets",
                column: "AlbumSetId");

            migrationBuilder.CreateIndex(
                name: "IX_SongSets_Name_AlbumSetId",
                table: "SongSets",
                columns: new[] { "Name", "AlbumSetId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SongSets");
        }
    }
}
