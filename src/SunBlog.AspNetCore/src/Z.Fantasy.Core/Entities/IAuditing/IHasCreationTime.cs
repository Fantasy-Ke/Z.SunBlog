using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.Fantasy.Core.Entities.IAuditing
{
    public interface IHasCreationTime
    {
        /// <summary>
        /// Creation time.
        /// </summary>
        DateTime? CreationTime { get; }
    }
}
