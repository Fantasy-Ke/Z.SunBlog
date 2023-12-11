using Microsoft.EntityFrameworkCore;
using Z.Fantasy.Core;
using Z.Fantasy.Core.Entities.Enum;
using Z.Fantasy.Core.Entities.Organizations;

namespace Z.SunBlog.EntityFrameworkCore.EntityFrameworkCore.Seed.SeedData
{
    public class DefaultOrganizationBuilder
    {
        private readonly SunBlogDbContext _context;
        public DefaultOrganizationBuilder(SunBlogDbContext dbContext)
        {
            _context = dbContext;
        }


        public void Create()
        {
            CreateDefaultOrganization();
        }

        private void CreateDefaultOrganization()
        {
            // 创建 Default 用户。

            var defaultOrg = _context.ZOrganizations.IgnoreQueryFilters()
                .FirstOrDefault(t => t.Id == ZConfigBase.DefaultOrgId);
            
            if (defaultOrg == null)
            {
                defaultOrg = new ZOrganization
                {
                    Id = ZConfigBase.DefaultOrgId,
                    Name = ZConfigBase.DefaultOrgName,
                    Code = "Admin",
                    Status = AvailabilityStatus.Enable,
                    Sort = 1,
                    IsDeleted = false,
                };

                _context.ZOrganizations.AddRange(defaultOrg);
                _context.SaveChanges();
            }
        }
    }
}
