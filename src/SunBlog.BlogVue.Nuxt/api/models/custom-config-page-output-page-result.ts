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

import type { CustomConfigPageOutput } from './custom-config-page-output';
 /**
 * 
 *
 * @export
 * @interface CustomConfigPageOutputPageResult
 */
export interface CustomConfigPageOutputPageResult {

    /**
     * @type {number}
     * @memberof CustomConfigPageOutputPageResult
     */
    pageNo?: number;

    /**
     * @type {number}
     * @memberof CustomConfigPageOutputPageResult
     */
    pageSize?: number;

    /**
     * @type {number}
     * @memberof CustomConfigPageOutputPageResult
     */
    pages?: number;

    /**
     * @type {number}
     * @memberof CustomConfigPageOutputPageResult
     */
    total?: number;

    /**
     * @type {Array<CustomConfigPageOutput>}
     * @memberof CustomConfigPageOutputPageResult
     */
    rows?: Array<CustomConfigPageOutput> | null;
}
