using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Fantasy.Core.RedisModule.CacheHelper;

namespace Z.Fantasy.Core.RedisModule
{
    public class CacheOptions
    {
        public CacheType CacheType { get; set; }
        public string RedisEndpoint { get; set; }
    }
}
