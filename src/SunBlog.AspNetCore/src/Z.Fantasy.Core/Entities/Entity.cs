using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Fantasy.Core.Entities.KeyGenerator;

namespace Z.Fantasy.Core.Entities
{
    /// <summary>
    /// 自定义实体无主键实体
    /// </summary>
    [Serializable]
    public abstract class Entity : IEntity
    {

        protected Entity(Guid id)
        {
            Id = id;
        }

        [MaxLength(36)]
        public Guid Id { get; }

        protected Entity()
        {
        }
        public override string ToString()
        {
            return $"[Entity: {GetType().Name}] Keys = {string.Join(",", GetKeys())}";
        }

        public abstract object[] GetKeys();
    }

    [Serializable]
    public abstract class Entity<Tkey>
        : IEntity<Tkey>
    {
        protected Entity()
        {
        }
        /// <summary>
        /// ID
        /// </summary>
        [MaxLength(36)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Tkey Id { get; set; }

        protected Entity(Tkey id) => Id = id;

        public override string ToString()
        {
            return $"[Entity:{GetType().Name}] Id = {Id}";
        }
    }
}
