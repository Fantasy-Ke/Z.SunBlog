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
 * @interface CustomConfigPageOutput
 */
export interface CustomConfigPageOutput {

    /**
     * 自定义配置Id
     *
     * @type {string}
     * @memberof CustomConfigPageOutput
     */
    id?: string;

    /**
     * @type {AvailabilityStatus}
     * @memberof CustomConfigPageOutput
     */
    status?: AvailabilityStatus;

    /**
     * 备注
     *
     * @type {string}
     * @memberof CustomConfigPageOutput
     */
    remark?: string | null;

    /**
     * 配置名称
     *
     * @type {string}
     * @memberof CustomConfigPageOutput
     */
    name?: string | null;

    /**
     * 唯一编码
     *
     * @type {string}
     * @memberof CustomConfigPageOutput
     */
    code?: string | null;

    /**
     * 是否是多项配置
     *
     * @type {boolean}
     * @memberof CustomConfigPageOutput
     */
    isMultiple?: boolean;

    /**
     * 是否允许创建实体
     *
     * @type {boolean}
     * @memberof CustomConfigPageOutput
     */
    allowCreationEntity?: boolean;

    /**
     * 配置id
     *
     * @type {Array<string>}
     * @memberof CustomConfigPageOutput
     */
    configItemId?: Array<string> | null;

    /**
     * 是否已生成实体
     *
     * @type {boolean}
     * @memberof CustomConfigPageOutput
     */
    isGenerate?: boolean;

    /**
     * 创建时间
     *
     * @type {Date}
     * @memberof CustomConfigPageOutput
     */
    createdTime?: Date | null;
}