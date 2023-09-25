using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TempusFujit.Migrations
{
    /// <inheritdoc />
    public partial class CategoryCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "TimeEntries",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimeEntries_CategoryId",
                table: "TimeEntries",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeEntries_Categories_CategoryId",
                table: "TimeEntries",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeEntries_Categories_CategoryId",
                table: "TimeEntries");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_TimeEntries_CategoryId",
                table: "TimeEntries");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "TimeEntries");
        }
    }
}
