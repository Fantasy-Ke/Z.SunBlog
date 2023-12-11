using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.Fantasy.Core.ResultResponse
{
    public abstract class ZResponseBase
    {

        public bool Success { get; set; }

        public ErrorInfo Error { get; set; }

        public bool UnAuthorizedRequest { get; set; }

        public int StatusCode { get; set; }

        public object Extras { get; set; }
    }
}
