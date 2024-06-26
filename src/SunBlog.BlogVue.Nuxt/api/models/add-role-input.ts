/* tslint:disable */
/* eslint-disable */
/**
 * SunBlog API
 * Web API for managing By Z.SunBlog
 *
 * OpenAPI spec version: SunBlog API v1
 * 
 *
 * NOTE: This class is auto generated by the swagger code generator program.
 * https://github.com/swagger-api/swagger-codegen.git
 * Do not edit the class manually.
 */

import { AvailabilityStatus } from './availability-status';
 /**
 * 
 *
 * @export
 * @interface AddRoleInput
 */
export interface AddRoleInput {

    /**
     * 角色名称
     *
     * @type {string}
     * @memberof AddRoleInput
     */
    name: string;

    /**
     * @type {AvailabilityStatus}
     * @memberof AddRoleInput
     */
    status?: AvailabilityStatus;

    /**
     * 角色编码
     *
     * @type {string}
     * @memberof AddRoleInput
     */
    code: string;

    /**
     * 排序值
     *
     * @type {number}
     * @memberof AddRoleInput
     */
    sort?: number;

    /**
     * 备注
     *
     * @type {string}
     * @memberof AddRoleInput
     */
    remark?: string | null;

    /**
     * 授权按钮菜单Id
     *
     * @type {Array<string>}
     * @memberof AddRoleInput
     */
    menus: Array<string>;
}
