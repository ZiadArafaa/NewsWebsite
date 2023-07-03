using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsWebsite.API.Migrations
{
    public partial class addpublicId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PublicId",
                schema: "News",
                table: "News",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicId",
                schema: "News",
                table: "News");
        }
    }
}
