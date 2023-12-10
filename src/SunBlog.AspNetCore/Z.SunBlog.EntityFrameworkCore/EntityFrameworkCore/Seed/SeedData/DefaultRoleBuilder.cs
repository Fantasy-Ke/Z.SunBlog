using Microsoft.EntityFrameworkCore;
using Z.Ddd.Common;
using Z.Ddd.Common.Entities.Roles;
using Z.Ddd.Common.Entities.Users;

namespace Z.SunBlog.EntityFrameworkCore.EntityFrameworkCore.Seed.SeedData
{
    public class DefaultRoleBuilder
    {
        private readonly SunBlogDbContext _context;
        public DefaultRoleBuilder(SunBlogDbContext dbContext)
        {
            _context = dbContext;
        }


        public void Create()
        {
            CreateDefaultRole();
            CreateDefaultUserRole();
        }

        private void CreateDefaultRole()
        {
            // 创建 Default 用户。

            var defaultRole = _context.ZRoles.IgnoreQueryFilters()
                .FirstOrDefault(t => t.Name == ZConfigBase.DefaultRoleName);
            if (defaultRole == null)
            {
                defaultRole = new ZRoleInfo
                {
                    Id = ZConfigBase.DefaultRoleId,
                    Name = ZConfigBase.DefaultRoleName,
                    Code = ZConfigBase.DefaultRoleCode,
                    IsDeleted = false,
                };

                _context.ZRoles.Add(defaultRole);
                _context.SaveChanges();
            }
        }

        private void CreateDefaultUserRole()
        {
            // 创建 Default 用户。

            var defaultRole = _context.ZUserRoles.IgnoreQueryFilters()
                .FirstOrDefault(t => t.Id == ZConfigBase.DefaultUserRoleId);
            if (defaultRole == null)
            {
                defaultRole = new ZUserRole
                {
                    Id = ZConfigBase.DefaultUserRoleId,
                    UserId = ZConfigBase.DefaultUserId,
                    RoleId = ZConfigBase.DefaultRoleId,
                    IsDeleted = false,
                };

                _context.ZUserRoles.Add(defaultRole);
                _context.SaveChanges();
            }
        }
    }
}
