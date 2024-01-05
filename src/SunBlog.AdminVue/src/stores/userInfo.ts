import { defineStore } from 'pinia';
// import Cookies from 'js-cookie';
// import { Session } from '@/utils/storage';
import { computed, inject } from 'vue';
import { AuthsServiceProxy, UsersServiceProxy, ZFantasyToken } from '@/shared/service-proxies';
import apiHttpClient from '@/utils/http';
import { StoreKey, useUserStore } from './user';
import { log } from 'console';
const _usersCService = new UsersServiceProxy(inject('$baseurl'), apiHttpClient as any);
const _authService = new AuthsServiceProxy(inject('$baseurl'), apiHttpClient as any);

/**
 * 用户信息
 * @methods setUserInfos 设置用户信息
 */
export const useUserInfo = defineStore(StoreKey.UserInfo, () => {
  const userStore = useUserStore();
  const userInfoState = computed(() => userStore);

  //用户信息
  const userInfo = computed(() => userStore.userInfo);

  const setToken = (zToken: ZFantasyToken) => {
    userStore.setToken({ zToken: zToken });
  };

  const clearToken = async () => {
    await _authService.zSignOut(userStore?.zToken?.accessToken);
    userStore.$reset();
  };
  /**
   * 获取当前用户基本信息
   */
  const getUserInfo = async () => {
    const { result } = await _usersCService.currentUserInfo();
    if (result.userName) {
      userStore.setUserInfo(result!);
      return;
    }
    await clearToken();
  };
  /**
   * 获取当前用户基本信息
   */

  return { userInfoState, userInfo, getUserInfo, setToken, clearToken };
});
