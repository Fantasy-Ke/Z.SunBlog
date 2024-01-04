using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Z.SunBlog.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class updatefile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FileType",
                table: "ZFileInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FileTypeString",
                table: "ZFileInfo",
                type: "longtext",
                nullable: true,
                collation: "utf8mb4_general_ci");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileType",
                table: "ZFileInfo");

            migrationBuilder.DropColumn(
                name: "FileTypeString",
                table: "ZFileInfo");
        }
    }
}
