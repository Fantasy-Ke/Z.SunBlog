using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Fantasy.Core.Entities.Enum;

namespace Z.SunBlog.Core.MenuModule.Dtos
{
    public class MenuCreateDto
    {
        /// <summary>
        /// 路由名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 路由地址
        /// </summary>
        public string? Path { get; set; }
        /// <summary>
        /// 组件
        /// </summary>
        public string? Component { get; set; }

        /// <summary>
        /// 路由扩展信息
        /// </summary>
        public RouterMetaDto Meta { get; set; }

        /// <summary>
        /// 子菜单
        /// </summary>
        public List<MenuCreateDto> Children { get; set; } = new();
    }

    public class RouterMetaDto
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// 外链
        /// </summary>
        public string? IsLink { get; set; }
        /// <summary>
        /// 是否隐藏
        /// </summary>
        public bool IsHide { get; set; }
        /// <summary>
        /// 是否缓存
        /// </summary>
        public bool IsKeepAlive { get; set; }
        /// <summary>
        /// 是否固定
        /// </summary>
        public bool IsAffix { get; set; }

        /// <summary>
        /// 菜单
        /// </summary>
        public string? Icon { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public MenuType Type { get; set; }
    }
}
