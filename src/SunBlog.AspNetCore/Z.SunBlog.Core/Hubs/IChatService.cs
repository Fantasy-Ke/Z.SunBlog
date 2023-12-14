using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.SunBlog.Core.Hubs
{
    public interface IChatService
    {
        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task ReceiveMessage(object context);
    }
}
