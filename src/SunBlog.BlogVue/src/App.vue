<template>
  <v-app :theme="theme">
    <!-- 导航栏 -->
    <TopNavBar />
    <!-- 侧边导航栏 -->
    <SideNavBar />
    <v-main>
      <router-view :key="key" />
    </v-main>
    <!-- 脚部 -->
    <Footer v-if="useRoute().name !== 'message'" />
    <!-- 返回顶部 -->
    <BackTop />
  </v-app>
</template>

<script setup lang="ts">
import { useRoute } from "vue-router";
import { storeToRefs } from "pinia";
import TopNavBar from "./components/layout/TopNavBar.vue";
import SideNavBar from "./components/layout/SideNavBar.vue";
import Footer from "./components/layout/Footer.vue";
import BackTop from "./components/BackTop.vue";
import { useThemeSettingStore } from "./stores/themeSetting";
import { computed, onMounted } from "vue";
import { ElNotification } from "element-plus";
import signalR from "./utils/signalRService";
import { useAuth } from "./stores/auth";
const { theme } = storeToRefs(useThemeSettingStore());
const route = useRoute();
const authStore = useAuth();
const key = computed(() => {
  return route.fullPath + Math.random();
});

onMounted(async () => {
  signalR.close();
  signalR.start();
});
</script>
