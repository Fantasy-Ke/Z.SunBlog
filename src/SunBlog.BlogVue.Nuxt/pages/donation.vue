<template>
    <div class="about-banner banner" :style="cover">
      <h1 class="banner-title">赞助</h1>
    </div>
    <!-- 关于我内容 -->
    <v-card class="blog-container">
      <div class="about-content markdown-body" v-html="info.donation" />
    </v-card>
  </template>
  
  <script setup lang="ts">
  import { computed } from "vue";
  import { storeToRefs } from "pinia";
  const appStore = useApp();
  const { info } = storeToRefs(appStore);
  const cover = computed(() => {
    return "background: url(" + appStore.donationCover() + ") center center / cover no-repeat";
  });
  </script>
  
  <style scoped>
  .about-banner {
    /* background: url(https://static.talkxj.com/config/2a56d15dd742ff8ac238a512d9a472a1.jpg) center center / cover
      no-repeat; */
    background-color: #0099CC;
  }
  .about-content {
    word-break: break-word;
    line-height: 1.8;
    font-size: 14px;
  }
  .about-content {
    margin-top: 20px;
  }
  </style>
  