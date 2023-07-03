using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsWebsite.API.Migrations
{
    public partial class addisdeletedcolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "News",
                table: "News",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "News",
                table: "Authors",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "News",
                table: "News");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "News",
                table: "Authors");
        }
    }
}
