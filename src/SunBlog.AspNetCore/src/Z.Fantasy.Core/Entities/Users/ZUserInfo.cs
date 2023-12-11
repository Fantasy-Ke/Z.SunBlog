using System.ComponentModel.DataAnnotations;
using Z.Fantasy.Core.Entities.Auditing;
using Z.Fantasy.Core.Entities.Enum;

namespace Z.Fantasy.Core.Entities.Users
{
    public class ZUserInfo : FullAuditedEntity<string>
    {

        /// <summary>
        /// 昵称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; }

        /// <summary>
        /// 组织机构id
        /// </summary>
        public string OrgId { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public Gender Gender { get; set; }


        /// <summary>
        /// 头像
        /// </summary>
        [MaxLength(256)]
        public string Avatar { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [MaxLength(16)]
        public string Mobile { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [MaxLength(64)]
        public string Email { get; set; }
        /// <summary>
        /// 可用状态
        /// </summary>
        public AvailabilityStatus Status { get; set; }

        /// <summary>
        /// 最后一次登录IP地址
        /// </summary>
        [MaxLength(64)]
        public string LastLoginIp { get; set; }

        /// <summary>
        /// 最后一次登录位置
        /// </summary>
        public string LastLoginAddress { get; set; }
        /// <summary>
        /// 账号锁定过期时间
        /// </summary>
        public DateTime? LockExpired { get; set; }

        public ZUserInfo()
        {

        }

        public ZUserInfo(string id) : base(id)
        {
        }
    }
}
