using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.Fantasy.Core
{
    public static class ZConfigBase
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public const string DefaultUserId = "3ff7dec047a19e5b72c567aeb1c36027";

        /// <summary>
        /// 角色id
        /// </summary>
        public const string DefaultRoleId = "3b020c731abaaf458a099d387ab266ef";

        /// <summary>
        /// 部门id
        /// </summary>
        public const string DefaultOrgId = "584da8a5b21fc52791481420bdbc1491";

        /// <summary>
        /// 用户角色id
        /// </summary>
        public static readonly Guid DefaultUserRoleId = Guid.Parse("3c38e905-c77b-c0e9-38b5-137e9707f33f");
        /// <summary>
        /// "Admin".
        /// </summary>
        public const string DefaultUserName = "Admin";

        /// <summary>
        /// 组织名称
        /// </summary>
        public const string DefaultOrgName = "超级公司";

        /// <summary>
        /// 默认角色密码
        /// </summary>
        public const string DefaultPassWord = "b8446d3e7b3605007b9f2cbdddf28928";

        /// <summary>
        /// "Admin".
        /// </summary>
        public const string DefaultRoleCode = "Admin";

        /// <summary>
        /// "Admin".
        /// </summary>
        public const string DefaultRoleName = "管理员";
    }
}
