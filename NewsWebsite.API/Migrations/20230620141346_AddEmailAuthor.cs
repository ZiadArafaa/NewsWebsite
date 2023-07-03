using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsWebsite.API.Migrations
{
    public partial class AddEmailAuthor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "News",
                table: "Authors",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_Email",
                schema: "News",
                table: "Authors",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Authors_Email",
                schema: "News",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "News",
                table: "Authors");
        }
    }
}
