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
 * @interface CategoryPageOutput
 */
export interface CategoryPageOutput {

    /**
     * 栏目ID
     *
     * @type {string}
     * @memberof CategoryPageOutput
     */
    id?: string;

    /**
     * 栏目名称
     *
     * @type {string}
     * @memberof CategoryPageOutput
     */
    name?: string | null;

    /**
     * 父级id
     *
     * @type {string}
     * @memberof CategoryPageOutput
     */
    parentId?: string | null;

    /**
     * 封面图
     *
     * @type {string}
     * @memberof CategoryPageOutput
     */
    cover?: string | null;

    /**
     * @type {AvailabilityStatus}
     * @memberof CategoryPageOutput
     */
    status?: AvailabilityStatus;

    /**
     * 排序值（值越小越靠前）
     *
     * @type {number}
     * @memberof CategoryPageOutput
     */
    sort?: number;

    /**
     * 备注
     *
     * @type {string}
     * @memberof CategoryPageOutput
     */
    remark?: string | null;

    /**
     * 创建时间
     *
     * @type {Date}
     * @memberof CategoryPageOutput
     */
    creationTime?: Date | null;

    /**
     * 子栏目
     *
     * @type {Array<CategoryPageOutput>}
     * @memberof CategoryPageOutput
     */
    children?: Array<CategoryPageOutput> | null;
}
