using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Z.SunBlog.Core.CustomConfigModule;

namespace Z.SunBlog.EntityFrameworkCore.EntityFrameworkCore.Seed.SeedData
{
    public class DefaultCustomconfigBuilde
    {
        private readonly SunBlogDbContext _context;
        public DefaultCustomconfigBuilde(SunBlogDbContext dbContext)
        {
            _context = dbContext;
        }


        public void Create()
        {
            CreateDefaultCustomconfig();
        }

        private void CreateDefaultCustomconfig()
        {
            var defaultCustomconfig = _context.CustomConfig.IgnoreQueryFilters().ToList(); 
            if (defaultCustomconfig == null || defaultCustomconfig.Count == 0)
            {
                var jsonFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Configs{Path.DirectorySeparatorChar}InitData{Path.DirectorySeparatorChar}Customconfig.txt");
                var pageFilterJson = File.ReadAllText(jsonFileName);
                var customConfig = JsonConvert.DeserializeObject<List<CustomConfig>>(pageFilterJson);
                if (customConfig != null && customConfig.Count > 0)
                {
                    _context.CustomConfig.AddRange(customConfig);
                    _context.SaveChanges();
                }

            }
        }

    }
}
