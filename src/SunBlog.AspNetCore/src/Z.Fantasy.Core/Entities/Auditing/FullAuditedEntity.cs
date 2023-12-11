using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Fantasy.Core.Entities.IAuditing;

namespace Z.Fantasy.Core.Entities.Auditing;

public abstract class FullAuditedEntity : CreationAuditedEntity, IDeletionAuditedObject
{
    [MaxLength(32)]
    public virtual string DeleterId { get; set; }

    public virtual DateTime? DeletionTime { get; set; }

    public virtual bool? IsDeleted { get; set; }
}


public abstract class FullAuditedEntity<Tkey> : CreationAuditedEntity<Tkey>, IDeletionAuditedObject
{

    public FullAuditedEntity()
    {

    }
    public FullAuditedEntity(Tkey id)
    {
        Id = id;
    }
    [MaxLength(32)]
    public virtual string DeleterId { get; set; }

    public virtual DateTime? DeletionTime { get; set; }

    public virtual bool? IsDeleted { get; set; }

}