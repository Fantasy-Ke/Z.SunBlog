using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.Fantasy.Core.RedisModule.CacheHelper
{
    public enum CacheType
    {
        /// <summary>
        /// 使用内存缓存(不支持分布式)
        /// </summary>
        Memory,

        /// <summary>
        /// 使用Redis缓存(支持分布式)
        /// </summary>
        Redis
    }
}
