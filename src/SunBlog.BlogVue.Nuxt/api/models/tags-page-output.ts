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
 * @interface TagsPageOutput
 */
export interface TagsPageOutput {

    /**
     * 标签ID
     *
     * @type {string}
     * @memberof TagsPageOutput
     */
    id?: string;

    /**
     * 标签名称
     *
     * @type {string}
     * @memberof TagsPageOutput
     */
    name?: string | null;

    /**
     * @type {AvailabilityStatus}
     * @memberof TagsPageOutput
     */
    status?: AvailabilityStatus;

    /**
     * 排序
     *
     * @type {number}
     * @memberof TagsPageOutput
     */
    sort?: number;

    /**
     * 标签封面
     *
     * @type {string}
     * @memberof TagsPageOutput
     */
    cover?: string | null;

    /**
     * 标签图标
     *
     * @type {string}
     * @memberof TagsPageOutput
     */
    icon?: string | null;

    /**
     * 标签颜色
     *
     * @type {string}
     * @memberof TagsPageOutput
     */
    color?: string | null;

    /**
     * 创建时间
     *
     * @type {Date}
     * @memberof TagsPageOutput
     */
    createdTime?: Date | null;
}
