using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Ddd.Common.Exceptions;

namespace Z.Ddd.Common.Helper;
public static class App
{
    /// <summary>
    /// 存储根服务，可能为空
    /// </summary>
    public static IServiceProvider RootServices => AppSettings.RootServices;

    /// <summary>
    /// 获取请求上下文
    /// </summary>
    public static HttpContext HttpContext => CatchOrDefault(() => RootServices.GetRequiredService<IHttpContextAccessor>()?.HttpContext);

    /// <summary>
    /// 处理获取对象异常问题
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="action">获取对象委托</param>
    /// <param name="defaultValue">默认值</param>
    /// <returns>T</returns>
    private static T CatchOrDefault<T>(Func<T> action, T defaultValue = null)
        where T : class
    {
        try
        {
            return action();
        }
        catch
        {
            return defaultValue ?? null;
        }
    }

    /// <summary>
    /// 获取Ip所属详细地理位置
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static IpInfoDto GetGeolocation(this HttpContext context)
    {
        try
        {
            string ip = context.GetRemoteIp();
            //获取ip信息
            return GetGeolocation(ip);
        }
        catch (Exception e)
        {
            throw new UserFriendlyException(e.Message);
        }
    }

    /// <summary>
    /// 获取ip详细信息
    /// </summary>
    /// <param name="ip"></param>
    /// <returns></returns>
    public static IpInfoDto GetGeolocation(string ip)
    {
        if (string.IsNullOrWhiteSpace(ip))
        {
            return default;
        }
        try
        {
            var httpclient = new HttpClient();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //获取ip信息
            string json = httpclient.GetStringAsync($"http://whois.pconline.com.cn/ipJson.jsp?ip={ip}&json=true").GetAwaiter().GetResult();
            //string json = Encoding.UTF8.GetString(bytes);          
            return JsonConvert.DeserializeObject<IpInfoDto>(json) ?? new IpInfoDto();
        }
        catch
        {
            return new IpInfoDto();
        }
    }

    /// <summary>
    /// 获取ip
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static string GetRemoteIp(this HttpContext context)
    {
        string ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(ip))
        {
            ip = context.GetRemoteIpAddressToIPv4();
        }

        return ip;
    }

    /// <summary>
    /// 获取远程 IPv4地址
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static string GetRemoteIpAddressToIPv4(this HttpContext context)
    {
        return context.Connection.RemoteIpAddress?.MapToIPv6()?.ToString();
    }

    public class IpInfoDto
    {
        /// <summary>
        /// ip
        /// </summary>
        [JsonProperty("ip")]
        public string Ip { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        [JsonProperty("pro")]
        public string Province { get; set; }

        /// <summary>
        /// 省编码
        /// </summary>
        [JsonProperty("proCode")]
        public string ProCode { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        [JsonProperty("city")]
        public string City { get; set; }

        /// <summary>
        /// 城市编码
        /// </summary>
        [JsonProperty("cityCode")]
        public string CityCode { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        [JsonProperty("region")]
        public string Region { get; set; }

        /// <summary>
        /// 区编码
        /// </summary>
        [JsonProperty("regionCode")]
        public string RegionCode { get; set; }

        /// <summary>
        /// IP归属地
        /// </summary>
        [JsonProperty("addr")]
        public string Address { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("regionNames")]
        public string RegionNames { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        [JsonProperty("err")]
        public string Error { get; set; }
    }
}
