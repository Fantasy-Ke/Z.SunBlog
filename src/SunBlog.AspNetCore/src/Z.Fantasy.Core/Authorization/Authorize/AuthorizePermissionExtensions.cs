namespace Z.Fantasy.Core.Authorization.Authorize
{
    public static class AuthorizePermissionExtensions
    {
        public static SystemPermission AddGroup(this SystemPermission permission, string name, string code)
        {
            if (!permission.IsGroup)
            {
                ThrowAuthorizeationError.ThrowAuthorizeationErro("this permission is not group,can not add group");
            }
            var _permission = new SystemPermission
            {
                Name = name,
                Code = code,
                ParentCode = permission.Code,
                IsGroup = true,
                Page = false,
                Button = false
            };
            if (permission.Childrens == null)
            {
                permission.Childrens = new List<SystemPermission>();
            }
            permission.Childrens.Add(_permission);
            return _permission;
        }

        public static SystemPermission AddChild(this SystemPermission permission, string name, string code)
        {
            if (!permission.IsGroup)
            {
                ThrowAuthorizeationError.ThrowAuthorizeationErro("this permission is not group,can not add group");
            }
            var child = new SystemPermission()
            {
                Name = name,
                Code = code,
                ParentCode = permission.Code,
                IsGroup = false,
                Page = true,
                Button = false
            };
            if (permission.Childrens == null)
            {
                permission.Childrens = new List<SystemPermission>();
            }
            permission.Childrens.Add(child);
            return child;
        }

        public static void AddPermissin(this SystemPermission permission, string name, string code)
        {
            if (!permission.Page)
            {
                ThrowAuthorizeationError.ThrowAuthorizeationErro("this permission is not group,can not add group");
            }
            var _permission = new SystemPermission()
            {
                Name = name,
                ParentCode = permission.Code,
                Code = code,
                IsGroup = false,
                Page = false,
                Button = true
            };
            if (permission.Childrens == null)
            {
                permission.Childrens = new List<SystemPermission>();
            }
            permission.Childrens.Add(_permission);
        }
    }
}
