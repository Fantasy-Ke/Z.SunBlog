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

 /**
 * 
 *
 * @export
 * @interface TalksOutput
 */
export interface TalksOutput {

    /**
     * @type {string}
     * @memberof TalksOutput
     */
    id?: string;

    /**
     * 是否置顶
     *
     * @type {boolean}
     * @memberof TalksOutput
     */
    isTop?: boolean;

    /**
     * 内容
     *
     * @type {string}
     * @memberof TalksOutput
     */
    content?: string | null;

    /**
     * 图片
     *
     * @type {string}
     * @memberof TalksOutput
     */
    images?: string | null;

    /**
     * 是否已点赞
     *
     * @type {boolean}
     * @memberof TalksOutput
     */
    isPraise?: boolean;

    /**
     * 点赞数量
     *
     * @type {number}
     * @memberof TalksOutput
     */
    upvote?: number;

    /**
     * 评论数量
     *
     * @type {number}
     * @memberof TalksOutput
     */
    comments?: number;

    /**
     * 发布时间
     *
     * @type {Date}
     * @memberof TalksOutput
     */
    createdTime?: Date | null;
}
