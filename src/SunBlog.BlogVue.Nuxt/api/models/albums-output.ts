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
 * @interface AlbumsOutput
 */
export interface AlbumsOutput {

    /**
     * 相册ID
     *
     * @type {string}
     * @memberof AlbumsOutput
     */
    id?: string;

    /**
     * 相册名称
     *
     * @type {string}
     * @memberof AlbumsOutput
     */
    name?: string | null;

    /**
     * 相册封面
     *
     * @type {string}
     * @memberof AlbumsOutput
     */
    cover?: string | null;

    /**
     * 相册描述
     *
     * @type {string}
     * @memberof AlbumsOutput
     */
    remark?: string | null;

    /**
     * 创建时间
     *
     * @type {Date}
     * @memberof AlbumsOutput
     */
    createdTime?: Date | null;
}
