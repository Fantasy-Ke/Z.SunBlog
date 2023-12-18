using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.EventBus.Attributes;
using Z.Fantasy.Core.Entities.EntityLog;

namespace Z.Fantasy.Application.Handlers
{
    [EventDiscriptor("RequestLog", 1000, false)]
    public class RequestLogDto: ZRequestLog
    {
    }
}
