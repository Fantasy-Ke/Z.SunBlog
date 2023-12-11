using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Fantasy.Core.Entities.Auditing;

namespace Z.SunBlog.Core.PraiseModule;

public class Praise:FullAuditedEntity<Guid>
{
    /// <summary>
    /// 用户ID 
    /// </summary>
    public string AccountId { get; set; }

    /// <summary>
    /// 点赞对象ID
    /// </summary>
    public Guid ObjectId { get; set; }
}
