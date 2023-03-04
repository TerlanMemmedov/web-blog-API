using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace web_blog.Migrations
{
    public partial class Seventh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportedArticles_Articles_ArticleId",
                table: "ReportedArticles");

            migrationBuilder.AlterColumn<int>(
                name: "ArticleId",
                table: "ReportedArticles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedArticles_Articles_ArticleId",
                table: "ReportedArticles",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportedArticles_Articles_ArticleId",
                table: "ReportedArticles");

            migrationBuilder.AlterColumn<int>(
                name: "ArticleId",
                table: "ReportedArticles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedArticles_Articles_ArticleId",
                table: "ReportedArticles",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
