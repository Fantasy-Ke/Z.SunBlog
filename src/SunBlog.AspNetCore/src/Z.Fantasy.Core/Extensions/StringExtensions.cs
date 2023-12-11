using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Z.Fantasy.Core.Extensions;

public static class StringExtensions
{
    public static bool IsNullEmpty(this string source)
    {
        return string.IsNullOrEmpty(source);
    }
    /// <summary>
    /// 字符串是否为null或空“”或空白字符串“ ”    
    /// </summary>       
    /// <returns></returns>
    public static bool IsNullWhiteSpace(this string str)
    {
        return string.IsNullOrWhiteSpace(str);
    }


    /// <summary>
    /// 删除为string类型有匹配的最后字符串
    /// </summary>
    /// <param name="str"></param>
    /// <param name="postFixes"></param>
    /// <returns></returns>
    public static string RemoveFix(this string str, params string[] postFixes)
    {
        if (str == null)
        {
            return null;
        }

        if (string.IsNullOrEmpty(str))
        {
            return string.Empty;
        }

        if (postFixes is null)
        {
            return str;
        }

        foreach (string text in postFixes)
        {
            if (str.EndsWith(text))
            {
                return str.Left(str.Length - text.Length);
            }
        }

        return str;
    }


    /// <summary>
    /// 从字符串的开头获取指定长度的子字符串
    /// </summary>
    /// <param name="str"></param>
    /// <param name="len"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static string Left(this string str, int len)
    {
        if (str == null)
        {
            throw new ArgumentNullException(nameof(str));
        }

        if (str.Length < len)
        {
            throw new ArgumentException("Len参数不能大于给定字符串的长度!");
        }

        return str.Substring(0, len);
    }
    /// <summary>
    /// 从字符串的结尾处获取指定长度的子字符串
    /// </summary>
    /// <param name="str"></param>
    /// <param name="len"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static string Right(this string str, int len)
    {
        if (str == null)
        {
            throw new ArgumentNullException(nameof(str));
        }

        if (str.Length < len)
        {
            throw new ArgumentException("Len参数不能大于给定字符串的长度!");
        }

        return str.Substring(str.Length - len, len);
    }
    /// <summary>
    /// 如果字符串超过最大长度，则从字符串的开头获取该字符串的子字符串。
    /// 如果字符串被截断，它会在字符串末尾添加一个“…”后缀。
    /// 返回的字符串不能超过maxLength。
    /// </summary>
    /// <param name="str">字符串</param>
    /// <param name="maxLength">最大长度</param>
    /// <returns></returns>
    public static string TruncateWithPostfix(this string str, int maxLength)
    {
        return str.TruncateWithPostfix(maxLength, "...");
    }

    /// <summary>
    /// 给超出指定长度的字符串进行截断，追加后缀
    /// </summary>
    /// <param name="str">字符串</param>
    /// <param name="maxLength">指定的最大长度</param>
    /// <param name="postfix">后缀</param>
    /// <returns></returns>
    public static string TruncateWithPostfix(this string str, int maxLength, string postfix)
    {
        if (str == null)
        {
            return null;
        }

        if (string.IsNullOrEmpty(str) || maxLength == 0)
        {
            return string.Empty;
        }

        if (str.Length <= maxLength)
        {
            return str;
        }

        if (maxLength <= postfix.Length)
        {
            return postfix.Left(maxLength);
        }

        return str.Left(maxLength - postfix.Length) + postfix;
    }
    /// <summary>
    /// 驼峰命名转换 例：HelloWorld => hello-world
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string CamelCaseToLowerLine(this string str)
    {
        var strCamelCase = JsonNamingPolicy.CamelCase.ConvertName(str);
        var strTarget = new StringBuilder();

        for (var j = 0; j < strCamelCase.Length; j++)  //strItem是原始字符串
        {
            var temp = strCamelCase[j].ToString();
            if (Regex.IsMatch(temp, "[A-Z]"))
            {
                temp = "-" + temp.ToLower();
            }
            strTarget.Append(temp);
        }

        return strTarget.ToString();
    }

    /// <summary>
    /// char类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="strList"></param>
    /// <param name="separator"></param>
    /// <returns></returns>
    public static string JoinAsString<T>(this IEnumerable<T> strList, char separator)
    {
        return string.Join(separator, strList);
    }

    /// <summary>
    /// string类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="strList"></param>
    /// <param name="separator"></param>
    /// <returns></returns>
    public static string JoinAsString<T>(this IEnumerable<T> strList, string separator)
    {
        return string.Join(separator, strList);
    }


    public static string NotNullString(this object obj) => obj != null ? obj.ToString()!.Trim() : string.Empty;
}
