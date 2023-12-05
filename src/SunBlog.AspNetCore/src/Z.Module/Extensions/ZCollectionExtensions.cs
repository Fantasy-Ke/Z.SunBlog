using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.Module.Extensions
{
    public static class ZCollectionExtensions
    {

        
        public static bool AddIfNotContains<T>(this ICollection<T> source, T item)
        {
            //Check.NotNull(source, nameof(source));

            if (source.Contains(item))
            {
                return false;
            }

            source.Add(item);
            return true;
        }
    }
}
