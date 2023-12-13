import { defineStore } from 'pinia';
// import Cookies from 'js-cookie';
// import { Session } from '@/utils/storage';
import { computed, inject } from 'vue';
import { UsersServiceProxy, ZFantasyToken } from '@/shared/service-proxies';
import apiHttpClient from '@/utils/http';
import { StoreKey, useUserStore } from './user';
const _usersCService = new UsersServiceProxy(inject('$baseurl'), apiHttpClient as any);

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

	const clearToken = () => {
		userStore.$reset();
	};
	/**
	 * 获取当前用户基本信息
	 */
	const getUserInfo = () => {
		_usersCService.currentUserInfo().then(res=>{
        userStore.setUserInfo(res.result!);
     }).catch(() => {
      clearToken();
    })
		
	};
	/**
	 * 获取当前用户基本信息
	 */

	return { userInfoState, userInfo, getUserInfo, setToken, clearToken };
});
