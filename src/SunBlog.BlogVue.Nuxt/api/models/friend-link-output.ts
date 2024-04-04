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
 * @interface FriendLinkOutput
 */
export interface FriendLinkOutput {

    /**
     * 友链ID
     *
     * @type {string}
     * @memberof FriendLinkOutput
     */
    id?: string;

    /**
     * 友链
     *
     * @type {string}
     * @memberof FriendLinkOutput
     */
    link?: string | null;

    /**
     * logo
     *
     * @type {string}
     * @memberof FriendLinkOutput
     */
    logo?: string | null;

    /**
     * 站点名称
     *
     * @type {string}
     * @memberof FriendLinkOutput
     */
    siteName?: string | null;

    /**
     * 网站描述
     *
     * @type {string}
     * @memberof FriendLinkOutput
     */
    remark?: string | null;
}