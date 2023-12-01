import { defineStore } from "pinia";
import { reactive, computed, inject } from "vue";
import { randomNumber } from "@/utils";
import apiHttpClient from "../utils/api-http-client";
import {
  ArticleCsServiceProxy,
  ArticleReportOutput,
  BlogSetting,
  BloggerInfo,
  OAuthsServiceProxy,
} from "@/shared/service-proxies";

const _articleCService = new ArticleCsServiceProxy(inject("$baseurl"), apiHttpClient as any);
const _oAuthCService = new OAuthsServiceProxy(inject("$baseurl"), apiHttpClient as any);

export const useApp = defineStore("app", () => {
  const app = reactive({
    covers: {
      home: [] as string[],
      about: [] as string[],
      donation: [] as string[],
      archives: [] as string[],
      category: [] as string[],
      tag: [] as string[],
      album: [] as string[],
      talk: [] as string[],
      message: [] as string[],
      user: [] as string[],
      link: [] as string[],
      tagList: [] as string[],
      categories: [] as string[],
    },
    info: {} as BloggerInfo,
    isInit: false,
    blogSetting: {} as BlogSetting,
    report: {
      articleCount: 0,
      tagCount: 0,
      categoryCount: 0,
    } as ArticleReportOutput,
  });

  /**
   * 初始化博客基本信息
   */
  const init = async () => {
    if (app.isInit) {
      return;
    }
    app.isInit = true;
    await _oAuthCService.info().then((res) => {
      var data = res.result;
      const covers = data!.covers!;
      app.info =
        data!.info ??
        ({
          nikeName: "Fantasy-Ke",
          motto: "凡是过往，皆为序章",
          qq: "2246080525",
          avatar: "/cover/login.jpg",
        } as unknown as BloggerInfo);
      app.blogSetting =
        data!.site ??
        ({
          siteName: "Fantasy-Ke",
          motto: "凡是过往，皆为序章",
          isAllowComments: true,
          isAllowMessage: true,
          runTime: new Date("2023/06/01"),
          copyright: "©2023 By Fantasy-Ke",
          description: "Fantasy-Ke的博客",
          filing: "鄂ICP备没有号-2",
          favicon: "favicon.ico",
          keyword: "Fantasy-Ke的博客",
          visitorNumbers: 0,
        } as any);
      app.covers.home = covers.Home ?? ["/cover/default.jpg"];
      app.covers.about = covers.About ?? ["/cover/about.jpg"];
      app.covers.donation = covers.Donation ?? ["/cover/about.jpg"];
      app.covers.archives = covers.Archives ?? ["/cover/archives.jpg"];
      app.covers.category = covers.Category ?? ["/cover/category.jpg"];
      app.covers.tag = covers.Tag ?? ["/cover/tag.png"];
      app.covers.album = covers.Album ?? ["/cover/album.jpg"];
      app.covers.talk = covers.Talk ?? ["/cover/talk.jpg"];
      app.covers.message = covers.Message ?? ["/cover/message.png"];
      app.covers.user = covers.User ?? ["/cover/user.jpg"];
      app.covers.link = covers.Link ?? ["/cover/default.jpg"];
      app.covers.tagList = covers.TagList ?? ["/cover/default.jpg"];
      app.covers.categories = covers.Categories ?? ["/cover/default.jpg"];

      console.log(app);
    });
  };
  /**
   * 首页cover
   */
  const homeCover = () => {
    const arr = app.covers.home;
    return arr[randomNumber(0, arr.length - 1)];
  };
  /**
   * 关于
   */
  const aboutCover = () => {
    const arr = app.covers.about;
    return arr[randomNumber(0, arr.length - 1)];
  };
  /**
   * 归档
   */
  const archivesCover = () => {
    const arr = app.covers.archives;
    return arr[randomNumber(0, arr.length - 1)];
  };
  /**
   * 分类
   */
  const categoryCover = () => {
    const arr = app.covers.category;
    return arr[randomNumber(0, arr.length - 1)];
  };
  /**
   * 标签
   */
  const tagCover = () => {
    const arr = app.covers.tag;
    return arr[randomNumber(0, arr.length - 1)];
  };
  /**
   * 打赏
   */
  const donationCover = () => {
    const arr = app.covers.donation;
    return arr[randomNumber(0, arr.length - 1)];
  };

  /**
   * 相册
   */
  const albumCover = () => {
    const arr = app.covers.album;
    return arr[randomNumber(0, arr.length - 1)];
  };
  /**
   * 说说
   * @returns
   */
  const talkCover = () => {
    const arr = app.covers.talk;
    return arr[randomNumber(0, arr.length - 1)];
  };
  /**
   * 留言
   */
  const messageCover = () => {
    const arr = app.covers.message;
    return arr[randomNumber(0, arr.length - 1)];
  };
  /**
   * 个人中心
   */
  const userCover = () => {
    const arr = app.covers.user;
    return arr[randomNumber(0, arr.length - 1)];
  };
  /**
   * 友链
   */
  const linkCover = () => {
    const arr = app.covers.link;
    return arr[randomNumber(0, arr.length - 1)];
  };
  /**
   * 标签列表封面
   * @returns
   */
  const tagListCover = () => {
    const arr = app.covers.tagList;
    return arr[randomNumber(0, arr.length - 1)];
  };
  /**
   * 分类列表封面
   */
  const categoriesCover = () => {
    const arr = app.covers.categories;
    return arr[randomNumber(0, arr.length - 1)];
  };

  /**
   * 是否已初始化
   */
  const isInit = computed(() => {
    return app.isInit;
  });

  const info = computed(() => {
    return app.info;
  });

  const blogSetting = computed(() => {
    return app.blogSetting;
  });

  const report = computed(() => {
    return app.report;
  });

  const getSiteReport = async () => {
    await _articleCService.reportStatistics().then((res) => {
      const data = res.result;
      if (res.success) {
        app.report = data;
      }
    });
  };

  return {
    init,
    homeCover,
    aboutCover,
    archivesCover,
    categoryCover,
    tagCover,
    albumCover,
    talkCover,
    messageCover,
    userCover,
    linkCover,
    tagListCover,
    categoriesCover,
    getSiteReport,
    donationCover,
    isInit,
    info,
    blogSetting,
    report,
  };
});
