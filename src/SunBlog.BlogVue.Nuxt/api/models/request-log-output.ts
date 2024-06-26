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
 * @interface RequestLogOutput
 */
export interface RequestLogOutput {

    /**
     * @type {string}
     * @memberof RequestLogOutput
     */
    id?: string;

    /**
     * 请求URI
     *
     * @type {string}
     * @memberof RequestLogOutput
     */
    requestUri?: string | null;

    /**
     * 请求方式
     *
     * @type {string}
     * @memberof RequestLogOutput
     */
    requestType?: string | null;

    /**
     * 请求数据
     *
     * @type {string}
     * @memberof RequestLogOutput
     */
    requestData?: string | null;

    /**
     * 响应数据
     *
     * @type {string}
     * @memberof RequestLogOutput
     */
    responseData?: string | null;

    /**
     * 用户ID
     *
     * @type {string}
     * @memberof RequestLogOutput
     */
    userId?: string | null;

    /**
     * 用户姓名
     *
     * @type {string}
     * @memberof RequestLogOutput
     */
    userName?: string | null;

    /**
     * 访问ip
     *
     * @type {string}
     * @memberof RequestLogOutput
     */
    clientIP?: string | null;

    /**
     * 用户代理（主要指浏览器）
     *
     * @type {string}
     * @memberof RequestLogOutput
     */
    userAgent?: string | null;

    /**
     * 操作系统
     *
     * @type {string}
     * @memberof RequestLogOutput
     */
    userOS?: string | null;

    /**
     * 耗时
     *
     * @type {string}
     * @memberof RequestLogOutput
     */
    spendTime?: string | null;

    /**
     * 创建时间
     *
     * @type {Date}
     * @memberof RequestLogOutput
     */
    creationTime?: Date | null;
}
