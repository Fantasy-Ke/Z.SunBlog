﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Fantasy.Core.Entities.Auditing;

namespace Z.Fantasy.Core.Entities.EntityLog
{
    public class ZExceptionLog : CreationAuditedEntity<Guid>
    {
        /// <summary>
        /// 请求URI 
        /// </summary>
        public string RequestUri { get; set; }
        /// <summary>
        /// 客户端IP 
        /// </summary>
        public string ClientIP { get; set; }
        /// <summary>
        /// 异常信息 
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 异常来源 
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// 异常堆栈信息 
        /// </summary>
        public string StackTrace { get; set; }
        /// <summary>
        /// 异常类型 
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 操作人id 
        /// </summary>
        public string OperatorId { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string OperatorName { get; set; }

        /// <summary>
        /// 用户代理（主要指浏览器）
        /// </summary>
        public string UserAgent { get; set; }
        /// <summary>
        /// 操作系统
        /// </summary>
        public string UserOS { get; set; }
    }
}
