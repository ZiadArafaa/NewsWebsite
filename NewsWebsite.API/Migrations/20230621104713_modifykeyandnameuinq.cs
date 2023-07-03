using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsWebsite.API.Migrations
{
    public partial class modifykeyandnameuinq : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_News_Authors_Author",
                schema: "News",
                table: "News");

            migrationBuilder.RenameColumn(
                name: "Author",
                schema: "News",
                table: "News",
                newName: "AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_News_Author",
                schema: "News",
                table: "News",
                newName: "IX_News_AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_Name",
                schema: "News",
                table: "Authors",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_News_Authors_AuthorId",
                schema: "News",
                table: "News",
                column: "AuthorId",
                principalSchema: "News",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_News_Authors_AuthorId",
                schema: "News",
                table: "News");

            migrationBuilder.DropIndex(
                name: "IX_Authors_Name",
                schema: "News",
                table: "Authors");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                schema: "News",
                table: "News",
                newName: "Author");

            migrationBuilder.RenameIndex(
                name: "IX_News_AuthorId",
                schema: "News",
                table: "News",
                newName: "IX_News_Author");

            migrationBuilder.AddForeignKey(
                name: "FK_News_Authors_Author",
                schema: "News",
                table: "News",
                column: "Author",
                principalSchema: "News",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
