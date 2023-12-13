import { AuthsServiceProxy, OAuthsServiceProxy } from "@/shared/service-proxies";
import { defineStore } from "pinia";
import { computed, inject } from "vue";
import apiHttpClient from "../utils/api-http-client";
import { StoreKey, useUserStore } from "./user";
import signalR from "@/utils/signalRService";
import { useToast } from "./toast";
const _oAuthCService = new OAuthsServiceProxy(inject("$baseurl"), apiHttpClient as any);
const _authService = new AuthsServiceProxy(inject("$baseurl"), apiHttpClient as any);

export const useAuth = defineStore(StoreKey.Auth, () => {
  const userStore = useUserStore();
  /**
   * 登录
   * @param code 登录码
   * @returns
   */
  const login = async (code: string) => {
    const { result, success } = await _oAuthCService.login(code);
    if (success) {
      userStore.setToken({ zToken: result });
      await getUserInfo();
    }
    return result;
  };

  /**
   * 退出登录
   */
  const logout = () => {
    _authService.zSignOut(userStore.zToken.accessToken).then((res) => {
      const toast = useToast();
      userStore.clearToken();
      signalR.close();
      signalR.start();
      toast.success("退出登录成功！");
    });
  };

  /**
   * 退出登录
   */
  const clearToken = async () => {
    await _authService.zSignOut(userStore.zToken?.accessToken);
    userStore.clearToken();
  };

  /**
   * 获取用户信息
   */
  const getUserInfo = async () => {
    const { result } = await _oAuthCService.userInfo();
    if (result.id) {
      userStore.setUserInfo(result);
    }
    return result;
  };

  const info = computed(() => {
    return userStore.userInfo;
  });

  const token = computed(() => {
    return userStore.zToken;
  });

  return { login, logout, getUserInfo, clearToken, info, token };
});
