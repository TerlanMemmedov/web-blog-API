using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace web_blog.Migrations
{
    public partial class Fourth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ReportedArticles",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_ReportedArticles_ArticleId",
                table: "ReportedArticles",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportedArticles_UserId",
                table: "ReportedArticles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedArticles_Articles_ArticleId",
                table: "ReportedArticles",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedArticles_AspNetUsers_UserId",
                table: "ReportedArticles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportedArticles_Articles_ArticleId",
                table: "ReportedArticles");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportedArticles_AspNetUsers_UserId",
                table: "ReportedArticles");

            migrationBuilder.DropIndex(
                name: "IX_ReportedArticles_ArticleId",
                table: "ReportedArticles");

            migrationBuilder.DropIndex(
                name: "IX_ReportedArticles_UserId",
                table: "ReportedArticles");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ReportedArticles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
