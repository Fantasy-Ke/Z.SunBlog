using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using Z.Fantasy.Core.Helper;

namespace Z.Fantasy.Core.Extensions;

public static class HttpExtension
{
    /// <summary>
    /// 规范化结果额外数据键
    /// </summary>
    internal static string UnifyResultExtrasKey = "UNIFY_RESULT_EXTRAS";
    /// <summary>
    /// 填充附加信息
    /// </summary>
    /// <param name="extras"></param>
    public static void Fill(object extras)
    {
        var items = App.HttpContext?.Items;
        if (items.ContainsKey(UnifyResultExtrasKey)) items.Remove(UnifyResultExtrasKey);
        items.Add(UnifyResultExtrasKey, extras);
    }

    /// <summary>
    /// 读取附加信息
    /// </summary>
    public static object Take()
    {
        object extras = null;
        App.HttpContext?.Items?.TryGetValue(UnifyResultExtrasKey, out extras);
        return extras;
    }

    public static string GetRequestBody(this HttpRequest request)
    {
        if (!request.Body.CanRead)
        {
            return default;
        }

        if (!request.Body.CanSeek)
        {
            return default;
        }

        if (request.Body.Length < 1)
        {
            return default;
        }

        var bodyStr = "";
        // 启用倒带功能，就可以让 Request.Body 可以再次读取
        request.Body.Seek(0, SeekOrigin.Begin);
        using (StreamReader reader
               = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
        {
            bodyStr = reader.ReadToEnd();
        }

        request.Body.Position = 0;
        return bodyStr;
    }
}