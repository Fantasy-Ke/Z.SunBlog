using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [MaxLength(256)]
        public string RequestUri { get; set; }
        /// <summary>
        /// 客户端IP 
        /// </summary>
        [MaxLength(50)]
        public string ClientIP { get; set; }
        /// <summary>
        /// 异常信息 
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string Message { get; set; }
        /// <summary>
        /// 异常来源 
        /// </summary>
        [MaxLength(256)]
        public string Source { get; set; }
        /// <summary>
        /// 异常堆栈信息 
        /// </summary>
        [MaxLength(int.MaxValue)]
        public string StackTrace { get; set; }
        /// <summary>
        /// 异常类型 
        /// </summary>
        [MaxLength(256)]
        public string Type { get; set; }
        /// <summary>
        /// 操作人id 
        /// </summary>
        [MaxLength(50)]
        public string OperatorId { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        [MaxLength(50)]
        public string OperatorName { get; set; }

        /// <summary>
        /// 用户代理（主要指浏览器）
        /// </summary>
        [MaxLength(50)]
        public string UserAgent { get; set; }
        /// <summary>
        /// 操作系统
        /// </summary>
        [MaxLength(50)]
        public string UserOS { get; set; }
    }
}
