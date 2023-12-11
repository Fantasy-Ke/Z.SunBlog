using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.Fantasy.Core
{
    public class JwtSettings
    {
        /// <summary>
        /// 发行者
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 受众
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// secretKey值
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// 访问令牌过期时间
        /// </summary>
        public int AccessTokenExpirationMinutes { get; set; }

        /// <summary>
        /// cokkie过期时间
        /// </summary>
        public int CokkieExpirationMinutes { get; set; }

        /// <summary>
        /// 刷新令牌过期时间
        /// </summary>
        public int RefreshTokenExpirationMinutes { get; set; }
    }
}
