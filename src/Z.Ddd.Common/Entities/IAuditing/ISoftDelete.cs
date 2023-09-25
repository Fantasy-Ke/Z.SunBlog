﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.Ddd.Common.Entities.IAuditing;

public interface ISoftDelete
{
    /// <summary>
    /// 软删除状态
    /// </summary>
    bool? IsDeleted { get; }
}
