import { defineStore } from 'pinia';

/**
 * 布局配置
 * 修复：https://gitee.com/lyt-top/vue-next-admin/issues/I567R1，感谢@lanbao123
 * 2020.05.28 by lyt 优化。开发时配置不生效问题
 * 修改配置时：
 * 1、需要每次都清理 `window.localStorage` 浏览器永久缓存
 * 2、或者点击布局配置最底部 `一键恢复默认` 按钮即可看到效果
 */
export const useThemeConfig = defineStore('themeConfig', {
	state: (): ThemeConfigState => ({
		themeConfig: {
			isDrawer: false,
			primary: '#009688',
			isIsDark: false,
			topBar: '#FFFFFF',
			topBarColor: '#212121',
			isTopBarColorGradual: false,
			menuBar: '#FFFFFF',
			menuBarColor: '#3A3A3A',
			menuBarActiveColor: 'rgba(0, 0, 0, 0.2)',
			isMenuBarColorGradual: false,
			columnsMenuBar: '#545c64',
			columnsMenuBarColor: '#e6e6e6',
			isColumnsMenuBarColorGradual: false,
			isColumnsMenuHoverPreload: false,
			isCollapse: false,
			isUniqueOpened: true,
			isFixedHeader: false,
			isFixedHeaderChange: false,
			isClassicSplitMenu: false,
			isLockScreen: false,
			lockScreenTime: 30,
			isShowLogo: true,
			isShowLogoChange: false,
			isBreadcrumb: true,
			isTagsview: true,
			isBreadcrumbIcon: true,
			isTagsviewIcon: false,
			isCacheTagsView: false,
			isSortableTagsView: true,
			isShareTagsView: false,
			isFooter: false,
			isGrayscale: false,
			isInvert: false,
			isWartermark: false,
			wartermarkText: 'Fantasy-Ke',
			tagsStyle: 'tags-style-five',
			animation: 'slide-right',
			columnsAsideStyle: 'columns-round',
			columnsAsideLayout: 'columns-vertical',
			layout: 'defaults',
			isRequestRoutes: true,
			globalTitle: 'SunBlog Admin',
			globalViceTitle: 'SunBlog Admin',
			globalViceTitleMsg: '专注、免费、开源、维护、解疑',
			globalI18n: 'zh-cn',
			globalComponentSize: 'default',
		},
	}),
	actions: {
		setThemeConfig(data: ThemeConfigState) {
			this.themeConfig = data.themeConfig;
		},
	},
});
