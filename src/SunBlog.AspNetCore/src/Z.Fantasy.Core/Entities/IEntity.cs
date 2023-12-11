using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.Fantasy.Core.Entities
{

    /// <summary>
    /// 万物实体的接口
    /// </summary>
    public interface IEntity
    {
    }

    /// <summary>
    /// 万物实体主键的接口
    /// </summary>
    public interface IEntity<Tkey> : IEntity
    {
        Tkey Id { get; }
    }
}
