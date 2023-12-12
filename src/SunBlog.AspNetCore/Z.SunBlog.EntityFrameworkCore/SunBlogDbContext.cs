using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using Z.EntityFrameworkCore;
using Z.EntityFrameworkCore.Options;
using Z.SunBlog.Core.AlbumsModule;
using Z.SunBlog.Core.ArticleCategoryModule;
using Z.SunBlog.Core.ArticleModule;
using Z.SunBlog.Core.ArticleTagModule;
using Z.SunBlog.Core.AuthAccountModule;
using Z.SunBlog.Core.CategoriesModule;
using Z.SunBlog.Core.CommentsModule;
using Z.SunBlog.Core.CustomConfigModule;
using Z.SunBlog.Core.FriendLinkModule;
using Z.SunBlog.Core.MenuModule;
using Z.SunBlog.Core.PicturesModule;
using Z.SunBlog.Core.PraiseModule;
using Z.SunBlog.Core.TagModule;
using Z.SunBlog.Core.TalksModule;
using Z.SunBlog.EntityFrameworkCore.ConfigureExtensions;

namespace Z.SunBlog.EntityFrameworkCore
{
    public class SunBlogDbContext : ZDbContext<SunBlogDbContext>
    {
        public SunBlogDbContext(ZDbContextOptions<SunBlogDbContext> options) : base(options)
        {
            //options.EnableSoftDelete = true;
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureModel();
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging(true);
            optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddSerilog()));
            base.OnConfiguring(optionsBuilder);
        }

        public virtual DbSet<Article> Article { get; set; }
        public virtual DbSet<ArticleTag> ArticleTag { get; set; }
        public virtual DbSet<ArticleCategory> ArticleCategory { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Tags> Tags { get; set; }
        public virtual DbSet<Pictures> Pictures { get; set; }
        public virtual DbSet<Praise> Praise { get; set; }
        public virtual DbSet<Albums> Albums { get; set; }
        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<Talks> Talks { get; set; }
        public virtual DbSet<AuthAccount> AuthAccount { get; set; }
        public virtual DbSet<FriendLink> FriendLink { get; set; }
        public virtual DbSet<CustomConfigItem> CustomConfigItem { get; set; }
        public virtual DbSet<CustomConfig> CustomConfig { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<ZRoleMenu> RoleMenu { get; set; }


    }
}
