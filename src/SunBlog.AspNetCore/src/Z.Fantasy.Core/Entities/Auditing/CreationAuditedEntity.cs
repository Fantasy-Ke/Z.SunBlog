using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Fantasy.Core.Entities.IAuditing;

namespace Z.Fantasy.Core.Entities.Auditing
{
    [Serializable]
    public abstract class CreationAuditedEntity : Entity, IHasCreationTime, IMayHaveCreator
    {
        /// <summary>
        /// 创建用户
        /// </summary>
        [MaxLength(32)]
        public virtual string CreatorId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime? CreationTime { get; set; }

    }

    [Serializable]
    public abstract class CreationAuditedEntity<Tkey> : Entity<Tkey>, IHasCreationTime, IMayHaveCreator
    {
        protected CreationAuditedEntity()
        {
        }

        protected CreationAuditedEntity(Tkey id)
        {
            Id = id;
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        [MaxLength(32)]
        public virtual string CreatorId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime? CreationTime { get; set; }





    }
}
