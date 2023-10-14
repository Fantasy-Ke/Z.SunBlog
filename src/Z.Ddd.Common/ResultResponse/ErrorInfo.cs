﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.Ddd.Common.ResultResponse
{
    public class ErrorInfo
    {
        public object? Message { get; set; }

        public ErrorInfo(string error)
        {
            Message = error;
        }

        public ErrorInfo() { }
    }
}
