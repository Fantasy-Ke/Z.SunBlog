using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.Fantasy.Core.Helper
{
    public static class CacheHelper
    {
        public static readonly string InterceptedKey = "InterceptedKey";

        /// <summary>
        /// 字典缓存
        /// </summary>
        public static readonly ConcurrentDictionary<string, IEnumerable<Type>> InterceptedDict = new();
    }
}
