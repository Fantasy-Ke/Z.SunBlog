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
import { CreationType } from './creation-type';
 /**
 * 
 *
 * @export
 * @interface ArticlePageOutput
 */
export interface ArticlePageOutput {

    /**
     * 文章ID
     *
     * @type {string}
     * @memberof ArticlePageOutput
     */
    id?: string;

    /**
     * 标题
     *
     * @type {string}
     * @memberof ArticlePageOutput
     */
    title?: string | null;

    /**
     * @type {AvailabilityStatus}
     * @memberof ArticlePageOutput
     */
    status?: AvailabilityStatus;

    /**
     * 排序
     *
     * @type {number}
     * @memberof ArticlePageOutput
     */
    sort?: number;

    /**
     * 封面
     *
     * @type {string}
     * @memberof ArticlePageOutput
     */
    cover?: string | null;

    /**
     * 是否置顶
     *
     * @type {boolean}
     * @memberof ArticlePageOutput
     */
    isTop?: boolean;

    /**
     * 创建时间
     *
     * @type {Date}
     * @memberof ArticlePageOutput
     */
    createdTime?: Date;

    /**
     * @type {CreationType}
     * @memberof ArticlePageOutput
     */
    creationType?: CreationType;

    /**
     * 发布时间
     *
     * @type {Date}
     * @memberof ArticlePageOutput
     */
    publishTime?: Date;

    /**
     * 浏览次数
     *
     * @type {number}
     * @memberof ArticlePageOutput
     */
    views?: number;

    /**
     * 栏目名称
     *
     * @type {string}
     * @memberof ArticlePageOutput
     */
    categoryName?: string | null;
}
