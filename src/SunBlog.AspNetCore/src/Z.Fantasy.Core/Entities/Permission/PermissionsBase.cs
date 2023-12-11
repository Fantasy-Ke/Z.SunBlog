using System.ComponentModel.DataAnnotations;
using Z.Fantasy.Core.Entities.Auditing;

namespace Z.Fantasy.Core.Entities.Permission
{
    public class PermissionsBase : CreationAuditedEntity<string>
    {
        public string ParentCode { get; set; }

        [MaxLength(40)]
        public string Name { get; set; }
        public string Code { get; set; }

        public bool Group { get; set; }
        public bool Page { get; set; }

        public bool Button { get; set; }
    }
}
