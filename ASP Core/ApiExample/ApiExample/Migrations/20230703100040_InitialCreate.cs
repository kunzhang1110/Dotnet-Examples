using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiExample.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    _id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    Title = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Viewed = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x._id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    _id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(225)", unicode: false, maxLength: 225, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x._id);
                });

            migrationBuilder.CreateTable(
                name: "ArticleTag",
                columns: table => new
                {
                    _id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticleID = table.Column<int>(type: "int", nullable: true),
                    TagID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleTag", x => x._id);
                    table.ForeignKey(
                        name: "FK__ArticleTa__Artic__5629CD9C",
                        column: x => x.ArticleID,
                        principalTable: "Articles",
                        principalColumn: "_id");
                    table.ForeignKey(
                        name: "FK__ArticleTa__TagID__571DF1D5",
                        column: x => x.TagID,
                        principalTable: "Tags",
                        principalColumn: "_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTag_ArticleID",
                table: "ArticleTag",
                column: "ArticleID");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTag_TagID",
                table: "ArticleTag",
                column: "TagID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleTag");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Tags");
        }
    }
}
