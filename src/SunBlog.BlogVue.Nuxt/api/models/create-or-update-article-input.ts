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
 * @interface CreateOrUpdateArticleInput
 */
export interface CreateOrUpdateArticleInput {

    /**
     * 标题
     *
     * @type {string}
     * @memberof CreateOrUpdateArticleInput
     */
    title: string;

    /**
     * 概要
     *
     * @type {string}
     * @memberof CreateOrUpdateArticleInput
     */
    summary: string;

    /**
     * 封面图
     *
     * @type {string}
     * @memberof CreateOrUpdateArticleInput
     */
    cover: string;

    /**
     * 是否置顶
     *
     * @type {boolean}
     * @memberof CreateOrUpdateArticleInput
     */
    isTop?: boolean;

    /**
     * 作者
     *
     * @type {string}
     * @memberof CreateOrUpdateArticleInput
     */
    author: string;

    /**
     * 原文地址
     *
     * @type {string}
     * @memberof CreateOrUpdateArticleInput
     */
    link?: string | null;

    /**
     * @type {CreationType}
     * @memberof CreateOrUpdateArticleInput
     */
    creationType?: CreationType;

    /**
     * 文章正文（Html或markdown）
     *
     * @type {string}
     * @memberof CreateOrUpdateArticleInput
     */
    content: string;

    /**
     * 文章正文是否为html代码
     *
     * @type {boolean}
     * @memberof CreateOrUpdateArticleInput
     */
    isHtml?: boolean;

    /**
     * 发布时间
     *
     * @type {Date}
     * @memberof CreateOrUpdateArticleInput
     */
    publishTime?: Date;

    /**
     * @type {AvailabilityStatus}
     * @memberof CreateOrUpdateArticleInput
     */
    status?: AvailabilityStatus;

    /**
     * 排序值（值越小越靠前）
     *
     * @type {number}
     * @memberof CreateOrUpdateArticleInput
     */
    sort: number;

    /**
     * 是否允许评论
     *
     * @type {boolean}
     * @memberof CreateOrUpdateArticleInput
     */
    isAllowComments?: boolean;

    /**
     * 过期时间（过期后文章不显示）
     *
     * @type {Date}
     * @memberof CreateOrUpdateArticleInput
     */
    expiredTime?: Date | null;

    /**
     * 标签
     *
     * @type {Array<string>}
     * @memberof CreateOrUpdateArticleInput
     */
    tags: Array<string>;

    /**
     * 栏目ID
     *
     * @type {string}
     * @memberof CreateOrUpdateArticleInput
     */
    categoryId: string;

    /**
     * 文章ID
     *
     * @type {string}
     * @memberof CreateOrUpdateArticleInput
     */
    id?: string | null;
}
