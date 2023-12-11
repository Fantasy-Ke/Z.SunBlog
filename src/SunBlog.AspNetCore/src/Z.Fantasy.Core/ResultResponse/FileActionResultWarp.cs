using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.Fantasy.Core.ResultResponse
{
    internal class FileActionResultWarp : IActionResultWarp
    {
        public void Wrap(FilterContext context)
        {
            FileResult result = null;

            switch (context)
            {
                case ResultExecutingContext resultExecutingContext:
                    result = resultExecutingContext.Result as FileStreamResult;
                    break;
            }

            if (result == null)
            {
                throw new ArgumentException("Action Result should be JsonResult!");
            }
        }
    }
}
