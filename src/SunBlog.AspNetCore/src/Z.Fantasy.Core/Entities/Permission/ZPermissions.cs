namespace Z.Fantasy.Core.Entities.Permission
{
    public class ZPermissions : PermissionsBase
    {
        public ZPermissions() { }
        public ZPermissions(string name, string code, string parentcode, bool group, bool page, bool button)
        {
            Name = name;
            Code = code;
            ParentCode = parentcode;
            Group = group;
            Page = page;
            Button = button;
        }
    }
}
