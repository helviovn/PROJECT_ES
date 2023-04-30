using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PROJECTES.Data.Migrations
{
    /// <inheritdoc />
    public partial class Movie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                            name: "Movie",
                            columns: table => new
                            {
                                Id = table.Column<int>(nullable: false)
                                    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                                Title = table.Column<string>(maxLength: 256, nullable: false),
                                Description = table.Column<string>(nullable: true),
                                Diretor = table.Column<string>(nullable: true),
                                Actor = table.Column<string>(nullable: true),
                                Genre = table.Column<string>(nullable: true),
                                Code = table.Column<string>(nullable: true),
                                Image = table.Column<string>(nullable: true),
                                Date = table.Column<string>(nullable: true)
                            },
                            constraints: table =>
                            {
                                table.PrimaryKey("PK_Movie", x => x.Id);
                            });

            migrationBuilder.CreateIndex(
                name: "MovieIndex",
                table: "Movie",
                column: "Title",
                unique: true,
                filter: "[Title] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
            name: "Movie");
        }
    }
}
