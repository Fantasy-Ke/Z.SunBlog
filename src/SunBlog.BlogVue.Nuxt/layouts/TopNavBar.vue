<template>
  <v-app-bar
    :class="vm.navClass"
    flat
    :height="60"
    :style="{ transform: `translateY(${vm.top}px)` }"
  >
    <!-- 手机端导航栏 -->
    <div class="d-md-none nav-mobile-container">
      <client-only>
        <h1 style="font-size: 18px; font-weight: bold">
          <a href="/">
            {{ blogSetting.siteName }}
          </a>
        </h1>
      </client-only>
      <div style="margin-left: auto">
        <a @click="searchModelHandel">
          <!-- <i class="iconfont iconsousuo"  /> -->
          <v-icon size="small">mdi mdi-magnify</v-icon>
        </a>
        <a style="margin-left: 10px; font-size: 20px" @click="drawerHandle">
          <!-- <i class="iconfont iconhanbao" /> -->
          <v-icon size="small">mdi mdi-menu</v-icon>
        </a>
      </div>
    </div>
    <!-- 电脑导航栏 -->
    <div class="d-md-block d-none nav-container">
      <client-only>
        <h1 class="float-left blog-title">
          <nuxt-link to="/">
            {{ blogSetting.siteName }}
          </nuxt-link>
        </h1>
      </client-only>
      <div class="float-right nav-title">
        <div class="menus-item">
          <a class="menu-btn" @click="searchModelHandel">
            <!-- <i class="iconfont iconsousuo" />  -->
            <v-icon size="small">mdi mdi-magnify</v-icon>
            搜索
          </a>
        </div>
        <div class="menus-item">
          <nuxt-link class="menu-btn" href="/">
            <!-- <i class="iconfont iconzhuye" />  -->
            <v-icon size="small">mdi mdi-home</v-icon>
            首页
          </nuxt-link>
        </div>
        <div class="menus-item">
          <a class="menu-btn">
            <!-- <i class="iconfont iconfaxian" />  -->
            <v-icon size="small">mdi mdi-apple-safari</v-icon>
            发现
            <!-- <i class="iconfont iconxiangxia2 expand" /> -->
            <v-icon>mdi mdi-chevron-down</v-icon>
          </a>
          <ul class="menus-submenu">
            <li>
              <nuxt-link to="/archives">
                <!-- <i class="iconfont iconguidang" />  -->
                <v-icon size="small">mdi mdi-text-box</v-icon>
                归档
              </nuxt-link>
            </li>
            <li>
              <nuxt-link to="/categories">
                <!-- <i class="iconfont iconfenlei" />  -->
                <v-icon size="small">mdi mdi-view-grid</v-icon>
                分类
              </nuxt-link>
            </li>
            <li>
              <nuxt-link to="/tags">
                <!-- <i class="iconfont iconbiaoqian" />  -->
                <v-icon size="small">mdi mdi-tag-multiple</v-icon>
                标签
              </nuxt-link>
            </li>
          </ul>
        </div>
        <div class="menus-item">
          <a class="menu-btn">
            <!-- <i class="iconfont iconqita" />  -->
            <v-icon size="small">mdi mdi-clover</v-icon>
            娱乐
            <!-- <i class="iconfont iconxiangxia2 expand" /> -->
            <v-icon>mdi mdi-chevron-down</v-icon>
          </a>
          <ul class="menus-submenu">
            <li>
              <nuxt-link to="/albums">
                <!-- <i class="iconfont iconxiangce1" /> -->
                <v-icon size="small">mdi mdi-image-size-select-actual</v-icon>
                模块封面
              </nuxt-link>
            </li>
            <li>
              <nuxt-link to="/talks">
                <!-- <i class="iconfont iconpinglun" />  -->
                <v-icon size="small">mdi mdi-message-processing</v-icon>
                说说
              </nuxt-link>
            </li>
          </ul>
        </div>
        <div class="menus-item">
          <a class="menu-btn">
            <!-- <i class="iconfont iconqita" />  -->
            <v-icon size="small">mdi mdi-silverware-variant</v-icon>
            更多
            <!-- <i class="iconfont iconxiangxia2 expand" /> -->
            <v-icon>mdi mdi-chevron-down</v-icon>
          </a>
          <ul class="menus-submenu">
            <li>
              <nuxt-link class="menu-btn" to="/about">
                <!-- <i class="iconfont iconzhifeiji" />  -->
                <v-icon size="small" style="transform: rotate(-45deg); margin-bottom: 3px; margin-right: 0px"
                  >mdi mdi-send-variant</v-icon
                >
                关于
              </nuxt-link>
            </li>
            <li>
              <nuxt-link to="/links">
                <!-- <i class="iconfont iconlianjie" />  -->
                <v-icon size="small">mdi mdi-camera-iris</v-icon>
                友链
              </nuxt-link>
            </li>
            <li>
              <nuxt-link class="menu-btn" to="/message">
                <!-- <i class="iconfont iconpinglunzu" />  -->
                <v-icon size="small">mdi mdi-message-bulleted</v-icon>
                留言
              </nuxt-link>
            </li>
            <li>
              <nuxt-link class="menu-btn" to="/donation">
                <v-icon size="small">mdi mdi-heart</v-icon>
                打赏
              </nuxt-link>
            </li>
          </ul>
        </div>
        <client-only>
          <div class="menus-item">
            <!-- <a class="menu-btn"> <i class="iconfont icondenglu" /> 登录 </a> -->
            <div v-if="!authStore.info">
              <a class="menu-btn">
                <!-- <i class="iconfont iconqita" />  -->
                <v-icon size="small">mdi mdi-login</v-icon>
                登录
                <!-- <i class="iconfont iconxiangxia2 expand" /> -->
                <v-icon>mdi mdi-chevron-down</v-icon>
              </a>
              <ul class="menus-submenu">
                <li>
                  <a v-if="!authStore.info" @click="handleLogin('gitee')" class="menu-btn">
                    <!-- <i class="iconfont iconqq" />  -->
                    <v-icon size="small">mdi mdi-gitlab</v-icon>
                    Gitee
                  </a>
                </li>
                <li>
                  <a v-if="!authStore.info" @click="handleLogin('github')" class="menu-btn">
                    <!-- <i class="iconfont iconqq" />  -->
                    <v-icon size="small">mdi mdi-github</v-icon>
                    Github
                  </a>
                </li>
              </ul>
            </div>

            <template v-else>
              <img
                class="user-avatar"
                :src="info?.avatar!"
                height="30"
                width="30"
              />
              <ul class="menus-submenu">
                <li>
                  <nuxt-link to="/user">
                    <!-- <i class="iconfont icongerenzhongxin" />  -->
                    <v-icon size="small">mdi mdi-account-circle</v-icon>
                    个人中心
                  </nuxt-link>
                </li>
                <li>
                  <a @click="handleLoginOut">
                    <!-- <i class="iconfont icontuichu" />  -->
                    <v-icon size="small">mdi mdi-logout</v-icon>
                    退出</a
                  >
                </li>
              </ul>
            </template>
          </div>
        </client-only>
      </div>
    </div>
  </v-app-bar>
  <search-model v-if="vm.isShow" v-model:isShow="vm.isShow"></search-model>
</template>

<script setup lang="ts">
import { storeToRefs } from "pinia";
import { onMounted, watch, reactive, defineAsyncComponent } from "vue";
import OAuthApi from "~/api/OAuthApi";
const SearchModel = defineAsyncComponent(() => import("~/components/SearchModel.vue"));
const authStore = useAuth();
const appStore = useApp();
const { blogSetting } = storeToRefs(appStore);
const vm = reactive({
  scrollTop: 0,
  navClass: "nav",
  isShow: false,
  isLogin: false,
  top: 0, //控制头部菜单栏显示隐藏
});
const store = useDrawerSettingStore();
const { drawer } = storeToRefs(store);
const { info } = storeToRefs(authStore);
watch(
  () => vm.scrollTop,
  (n, o) => {
    vm.top = n > o && vm.scrollTop > 60 ? -60 : 0;
  }
);

const scroll = (): void => {
  vm.scrollTop =
    window.scrollY ||
    document.documentElement.scrollTop ||
    document.body.scrollTop;
  vm.navClass = vm.scrollTop > 60 ? "nav-fixed" : "nav";
};
const searchModelHandel = () => {
  vm.isShow = true;
};

// 跳转登录
const handleLogin = async (type: string) => {
  const { data } = await OAuthApi.get(type);
  location.href = data!.value?.result!;
};
const handleLoginOut = () => {
  authStore.logout();
};
onMounted(() => {
  window.addEventListener("scroll", scroll);
});

const drawerHandle = (): void => {
  store.setDrawer(!drawer.value);
};
</script>
<style scoped lang="scss">
i {
  margin-right: 4px;
}
ul {
  list-style: none;
}
.nav {
  overflow: inherit;
  background: rgba(0, 0, 0, 0) !important;
  a {
    color: #eee !important;
  }
  .menu-btn {
    text-shadow: 0.05rem 0.05rem 0.1rem rgba(0, 0, 0, 0.3);
  }
  .blog-title a {
    text-shadow: 0.1rem 0.1rem 0.2rem rgba(0, 0, 0, 0.15);
  }
}
.v-theme--light.nav-fixed {
  background: rgba(255, 255, 255, 0.8) !important;
  box-shadow: 0 5px 6px -5px rgba(133, 133, 133, 0.6);
}
.v-theme--dark.nav-fixed {
  background: rgba(18, 18, 18, 0.8) !important;
}
.v-theme--dark.nav-fixed a {
  color: rgba(255, 255, 255, 0.8) !important;
}
.v-theme--light.nav-fixed a {
  color: #4c4948 !important;
}
.nav-fixed {
  overflow: inherit;
  .menus-item a,
  .blog-title a {
    text-shadow: none;
  }
}
.nav-container {
  font-size: 14px;
  width: 100%;
  height: 100%;
}
.nav-mobile-container {
  width: 100%;
  display: flex;
  align-items: center;
}
.blog-title,
.nav-title {
  display: flex;
  align-items: center;
  height: 100%;
}
.blog-title a {
  font-size: 18px;
  font-weight: bold;
}
.menus-item {
  position: relative;
  display: inline-block;
  margin: 0 0 0 0.875rem;
}
.menus-item a {
  transition: all 0.2s;
}
.nav-fixed .menu-btn:hover {
  color: #0099CC !important;
}
.menu-btn:hover:after {
  width: 100%;
}
.menus-item a:after {
  position: absolute;
  bottom: -5px;
  left: 0;
  z-index: -1;
  width: 0;
  height: 3px;
  background-color: #80c8f8;
  content: "";
  transition: all 0.3s ease-in-out;
}
.user-avatar {
  cursor: pointer;
  border-radius: 50%;
}
.menus-item:hover .menus-submenu {
  display: block;
}
.menus-submenu {
  position: absolute;
  display: none;
  right: 0;
  width: max-content;
  margin-top: 8px;
  box-shadow: 0 5px 20px -4px rgba(0, 0, 0, 0.5);
  background-color: #fff;
  animation: submenu 0.3s 0.1s ease both;
}
.menus-submenu:before {
  position: absolute;
  top: -8px;
  left: 0;
  width: 100%;
  height: 20px;
  content: "";
}
.menus-submenu a {
  line-height: 2;
  color: #4c4948 !important;
  text-shadow: none;
  display: block;
  padding: 6px 14px;
}
.menus-submenu a:hover {
  background: #4ab1f4;
}
@keyframes submenu {
  0% {
    opacity: 0;
    filter: alpha(opacity=0);
    transform: translateY(10px);
  }
  100% {
    opacity: 1;
    filter: none;
    transform: translateY(0);
  }
}
</style>
