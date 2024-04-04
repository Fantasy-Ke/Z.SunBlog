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
 * @interface ArticlePageQueryInput
 */
export interface ArticlePageQueryInput {

    /**
     * @type {number}
     * @memberof ArticlePageQueryInput
     */
    pageNo?: number;

    /**
     * @type {number}
     * @memberof ArticlePageQueryInput
     */
    pageSize?: number;

    /**
     * 标题
     *
     * @type {string}
     * @memberof ArticlePageQueryInput
     */
    title?: string | null;

    /**
     * 栏目ID
     *
     * @type {string}
     * @memberof ArticlePageQueryInput
     */
    categoryId?: string | null;
}