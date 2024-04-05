using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicSystem.Migrations
{
    /// <inheritdoc />
    public partial class StoreProcedureAlbumSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE GetAlbumSets
                AS
                BEGIN
                    SELECT *
                    FROM AlbumSets
                END");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE InsertAlbumSet
                (
                    @Name VARCHAR(100)
                )
                AS
                BEGIN
                    INSERT INTO AlbumSets (Name)
                    VALUES (@Name)
                END");

            migrationBuilder.Sql(@"CREATE PROCEDURE GetAlbumSet
                (
                    @Id INT
                )
                AS
                BEGIN
                    SELECT *
                    FROM AlbumSets
                    WHERE Id = @Id
                END");

            migrationBuilder.Sql(@"CREATE PROCEDURE UpdateAlbumSet
                (
                    @Id INT,
                    @Name VARCHAR(100)
                )
                AS
                BEGIN
                    UPDATE AlbumSets
                    SET Name = @Name
                    WHERE Id = @Id
                END");

            migrationBuilder.Sql(@"CREATE PROCEDURE DeleteAlbumSet
                (
                    @Id INT
                )
                AS
                BEGIN
                    DELETE FROM AlbumSets
                    WHERE Id = @Id
                END");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE DeleteAlbumSet");
            migrationBuilder.Sql("DROP PROCEDURE UpdateAlbumSet");
            migrationBuilder.Sql("DROP PROCEDURE GetAlbumSet");
            migrationBuilder.Sql("DROP PROCEDURE GetAlbumSets");
        }
    }
}
