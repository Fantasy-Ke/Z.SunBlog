using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Z.SunBlog.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Albums",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, collation: "utf8mb4_general_ci"),
                    Cover = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false, collation: "utf8mb4_general_ci"),
                    Type = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    Remark = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true, collation: "utf8mb4_general_ci"),
                    IsVisible = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatorId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albums", x => x.Id);
                })
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "Article",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    Title = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false, collation: "utf8mb4_general_ci"),
                    Summary = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false, collation: "utf8mb4_general_ci"),
                    Cover = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false, collation: "utf8mb4_general_ci"),
                    IsTop = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Views = table.Column<int>(type: "int", nullable: false),
                    Author = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, collation: "utf8mb4_general_ci"),
                    Link = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true, collation: "utf8mb4_general_ci"),
                    CreationType = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "longtext", maxLength: 2147483647, nullable: false, collation: "utf8mb4_general_ci"),
                    IsHtml = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PublishTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    IsAllowComments = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ExpiredTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatorId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Article", x => x.Id);
                })
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "ArticleCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    ArticleId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CategoryId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatorId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleCategory", x => x.Id);
                })
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "ArticleTag",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    ArticleId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TagId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatorId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleTag", x => x.Id);
                })
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "AuthAccount",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false, collation: "utf8mb4_general_ci"),
                    OAuthId = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_general_ci"),
                    Type = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_general_ci"),
                    IsBlogger = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Name = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true, collation: "utf8mb4_general_ci"),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Avatar = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true, collation: "utf8mb4_general_ci"),
                    Email = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true, collation: "utf8mb4_general_ci"),
                    UpdatedTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatorId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthAccount", x => x.Id);
                })
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, collation: "utf8mb4_general_ci"),
                    ParentId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Cover = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false, collation: "utf8mb4_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    Remark = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true, collation: "utf8mb4_general_ci"),
                    CreatorId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                })
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    ModuleId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    RootId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ParentId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    AccountId = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_general_ci"),
                    ReplyAccountId = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_general_ci"),
                    Content = table.Column<string>(type: "varchar(1024)", maxLength: 1024, nullable: false, collation: "utf8mb4_general_ci"),
                    IP = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false, collation: "utf8mb4_general_ci"),
                    Geolocation = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false, collation: "utf8mb4_general_ci"),
                    CreatorId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                })
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "CustomConfig",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, collation: "utf8mb4_general_ci"),
                    Code = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, collation: "utf8mb4_general_ci"),
                    IsMultiple = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Json = table.Column<string>(type: "longtext", maxLength: 2147483647, nullable: true, collation: "utf8mb4_general_ci"),
                    AllowCreationEntity = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsGenerate = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Remark = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true, collation: "utf8mb4_general_ci"),
                    CreatorId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomConfig", x => x.Id);
                })
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "CustomConfigItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    ConfigId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Json = table.Column<string>(type: "longtext", maxLength: 2147483647, nullable: false, collation: "utf8mb4_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatorId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomConfigItem", x => x.Id);
                })
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "FriendLink",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    AppUserId = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_general_ci"),
                    SiteName = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, collation: "utf8mb4_general_ci"),
                    Link = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false, collation: "utf8mb4_general_ci"),
                    Logo = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false, collation: "utf8mb4_general_ci"),
                    Url = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true, collation: "utf8mb4_general_ci"),
                    IsIgnoreCheck = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Remark = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true, collation: "utf8mb4_general_ci"),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatorId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendLink", x => x.Id);
                })
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "Menu",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false, collation: "utf8mb4_general_ci"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true, collation: "utf8mb4_general_ci"),
                    ParentId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    RouteName = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    Path = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true, collation: "utf8mb4_general_ci"),
                    Component = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true, collation: "utf8mb4_general_ci"),
                    Redirect = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true, collation: "utf8mb4_general_ci"),
                    Icon = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true, collation: "utf8mb4_general_ci"),
                    IsIframe = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Link = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true, collation: "utf8mb4_general_ci"),
                    IsVisible = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsKeepAlive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsFixed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    Remark = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true, collation: "utf8mb4_general_ci"),
                    CreatorId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu", x => x.Id);
                })
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "Pictures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    AlbumId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Url = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false, collation: "utf8mb4_general_ci"),
                    Remark = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true, collation: "utf8mb4_general_ci"),
                    CreatorId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pictures", x => x.Id);
                })
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "Praise",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    AccountId = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_general_ci"),
                    ObjectId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatorId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Praise", x => x.Id);
                })
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "RoleMenu",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    RoleId = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_general_ci"),
                    MenuId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatorId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleMenu", x => x.Id);
                })
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, collation: "utf8mb4_general_ci"),
                    Cover = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false, collation: "utf8mb4_general_ci"),
                    Color = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true, collation: "utf8mb4_general_ci"),
                    Icon = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    Remark = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_general_ci"),
                    CreatorId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                })
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "Talks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    IsTop = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Content = table.Column<string>(type: "longtext", nullable: false, collation: "utf8mb4_general_ci"),
                    Images = table.Column<string>(type: "varchar(2048)", maxLength: 2048, nullable: true, collation: "utf8mb4_general_ci"),
                    IsAllowComments = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatorId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Talks", x => x.Id);
                })
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "ZAccessLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_general_ci"),
                    RoutePath = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_general_ci"),
                    RouteParams = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_general_ci"),
                    RemoteIp = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true, collation: "utf8mb4_general_ci"),
                    UserAgent = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true, collation: "utf8mb4_general_ci"),
                    Location = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true, collation: "utf8mb4_general_ci"),
                    OsDescription = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true, collation: "utf8mb4_general_ci"),
                    Message = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true, collation: "utf8mb4_general_ci"),
                    LogType = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_general_ci"),
                    CreatorId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZAccessLog", x => x.Id);
                })
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "ZExceptionLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    RequestUri = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true, collation: "utf8mb4_general_ci"),
                    ClientIP = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8mb4_general_ci"),
                    Message = table.Column<string>(type: "longtext", maxLength: 2147483647, nullable: true, collation: "utf8mb4_general_ci"),
                    Source = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true, collation: "utf8mb4_general_ci"),
                    StackTrace = table.Column<string>(type: "longtext", maxLength: 2147483647, nullable: true, collation: "utf8mb4_general_ci"),
                    Type = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true, collation: "utf8mb4_general_ci"),
                    OperatorId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8mb4_general_ci"),
                    OperatorName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8mb4_general_ci"),
                    UserAgent = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8mb4_general_ci"),
                    UserOS = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8mb4_general_ci"),
                    CreatorId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZExceptionLog", x => x.Id);
                })
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "ZFileInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    ParentId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    FileDisplayName = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_general_ci"),
                    FileName = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_general_ci"),
                    FileExt = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_general_ci"),
                    ContentType = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_general_ci"),
                    FilePath = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_general_ci"),
                    FileSize = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_general_ci"),
                    Code = table.Column<string>(type: "varchar(95)", maxLength: 95, nullable: false, collation: "utf8mb4_general_ci"),
                    IsFolder = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatorId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZFileInfo", x => x.Id);
                })
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "ZOperationLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_general_ci"),
                    LogLevel = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true, collation: "utf8mb4_general_ci"),
                    ControllerName = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true, collation: "utf8mb4_general_ci"),
                    ActionName = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true, collation: "utf8mb4_general_ci"),
                    HttpMethod = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true, collation: "utf8mb4_general_ci"),
                    TraceId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true, collation: "utf8mb4_general_ci"),
                    ThreadId = table.Column<int>(type: "int", nullable: false),
                    UserAgent = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true, collation: "utf8mb4_general_ci"),
                    RequestUrl = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true, collation: "utf8mb4_general_ci"),
                    RemoteIp = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true, collation: "utf8mb4_general_ci"),
                    OsDescription = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true, collation: "utf8mb4_general_ci"),
                    Elapsed = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true, collation: "utf8mb4_general_ci"),
                    HttpStatusCode = table.Column<int>(type: "int", nullable: false),
                    Parameter = table.Column<string>(type: "longtext", maxLength: 2147483647, nullable: true, collation: "utf8mb4_general_ci"),
                    Response = table.Column<string>(type: "longtext", maxLength: 2147483647, nullable: true, collation: "utf8mb4_general_ci"),
                    Message = table.Column<string>(type: "longtext", maxLength: 2147483647, nullable: true, collation: "utf8mb4_general_ci"),
                    Exception = table.Column<string>(type: "longtext", maxLength: 2147483647, nullable: true, collation: "utf8mb4_general_ci"),
                    CreatorId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZOperationLog", x => x.Id);
                })
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "ZOrganizations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false, collation: "utf8mb4_general_ci"),
                    ParentId = table.Column<string>(type: "varchar(36)", nullable: true, collation: "utf8mb4_general_ci"),
                    Name = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    Code = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true, collation: "utf8mb4_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    Remark = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true, collation: "utf8mb4_general_ci"),
                    CreatorId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZOrganizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZOrganizations_ZOrganizations_ParentId",
                        column: x => x.ParentId,
                        principalTable: "ZOrganizations",
                        principalColumn: "Id");
                })
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "ZPermissions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false, collation: "utf8mb4_general_ci"),
                    CreatorId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ParentCode = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_general_ci"),
                    Name = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true, collation: "utf8mb4_general_ci"),
                    Code = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_general_ci"),
                    Group = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Page = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Button = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZPermissions", x => x.Id);
                })
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "ZRequestLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    RequestUri = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true, collation: "utf8mb4_general_ci"),
                    RequestType = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8mb4_general_ci"),
                    RequestData = table.Column<string>(type: "varchar(2560)", maxLength: 2560, nullable: true, collation: "utf8mb4_general_ci"),
                    ResponseData = table.Column<string>(type: "longtext", maxLength: 2147483647, nullable: true, collation: "utf8mb4_general_ci"),
                    UserId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8mb4_general_ci"),
                    UserName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8mb4_general_ci"),
                    ClientIP = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8mb4_general_ci"),
                    UserAgent = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8mb4_general_ci"),
                    UserOS = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8mb4_general_ci"),
                    SpendTime = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8mb4_general_ci"),
                    CreatorId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZRequestLog", x => x.Id);
                })
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "ZRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false, collation: "utf8mb4_general_ci"),
                    Name = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    Code = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    Remark = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true, collation: "utf8mb4_general_ci"),
                    CreatorId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZRoles", x => x.Id);
                })
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "ZUserRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", maxLength: 36, nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_general_ci"),
                    RoleId = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_general_ci"),
                    CreatorId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZUserRoles", x => x.Id);
                })
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "ZUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false, collation: "utf8mb4_general_ci"),
                    Name = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true, collation: "utf8mb4_general_ci"),
                    UserName = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true, collation: "utf8mb4_general_ci"),
                    PassWord = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: true, collation: "utf8mb4_general_ci"),
                    OrgId = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_general_ci"),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Avatar = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true, collation: "utf8mb4_general_ci"),
                    Mobile = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true, collation: "utf8mb4_general_ci"),
                    Birthday = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Email = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true, collation: "utf8mb4_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    LastLoginIp = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true, collation: "utf8mb4_general_ci"),
                    LastLoginAddress = table.Column<string>(type: "longtext", nullable: true, collation: "utf8mb4_general_ci"),
                    LockExpired = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatorId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeleterId = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci"),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZUsers", x => x.Id);
                })
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_ZOrganizations_ParentId",
                table: "ZOrganizations",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Albums");

            migrationBuilder.DropTable(
                name: "Article");

            migrationBuilder.DropTable(
                name: "ArticleCategory");

            migrationBuilder.DropTable(
                name: "ArticleTag");

            migrationBuilder.DropTable(
                name: "AuthAccount");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "CustomConfig");

            migrationBuilder.DropTable(
                name: "CustomConfigItem");

            migrationBuilder.DropTable(
                name: "FriendLink");

            migrationBuilder.DropTable(
                name: "Menu");

            migrationBuilder.DropTable(
                name: "Pictures");

            migrationBuilder.DropTable(
                name: "Praise");

            migrationBuilder.DropTable(
                name: "RoleMenu");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Talks");

            migrationBuilder.DropTable(
                name: "ZAccessLog");

            migrationBuilder.DropTable(
                name: "ZExceptionLog");

            migrationBuilder.DropTable(
                name: "ZFileInfo");

            migrationBuilder.DropTable(
                name: "ZOperationLog");

            migrationBuilder.DropTable(
                name: "ZOrganizations");

            migrationBuilder.DropTable(
                name: "ZPermissions");

            migrationBuilder.DropTable(
                name: "ZRequestLog");

            migrationBuilder.DropTable(
                name: "ZRoles");

            migrationBuilder.DropTable(
                name: "ZUserRoles");

            migrationBuilder.DropTable(
                name: "ZUsers");
        }
    }
}
