using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Z.SunBlog.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class ExceptionLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ZExceptionLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    RequestUri = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_general_ci"),
                    ClientIP = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_general_ci"),
                    Message = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_general_ci"),
                    Source = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_general_ci"),
                    StackTrace = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_general_ci"),
                    Type = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_general_ci"),
                    OperatorId = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_general_ci"),
                    OperatorName = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_general_ci"),
                    UserAgent = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_general_ci"),
                    UserOS = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_general_ci"),
                    CreatorId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZExceptionLog", x => x.Id);
                })
                .Annotation("Relational:Collation", "utf8mb4_general_ci");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ZExceptionLog");
        }
    }
}
