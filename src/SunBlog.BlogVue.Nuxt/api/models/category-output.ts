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
 * @interface CategoryOutput
 */
export interface CategoryOutput {

    /**
     * 栏目ID
     *
     * @type {string}
     * @memberof CategoryOutput
     */
    id?: string;

    /**
     * 父级ID
     *
     * @type {string}
     * @memberof CategoryOutput
     */
    parentId?: string | null;

    /**
     * 排序
     *
     * @type {number}
     * @memberof CategoryOutput
     */
    sort?: number;

    /**
     * 栏目名称
     *
     * @type {string}
     * @memberof CategoryOutput
     */
    name?: string | null;

    /**
     * 文章条数
     *
     * @type {number}
     * @memberof CategoryOutput
     */
    total?: number;
}
