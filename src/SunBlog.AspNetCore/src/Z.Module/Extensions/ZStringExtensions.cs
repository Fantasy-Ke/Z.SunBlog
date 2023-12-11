using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Z.Module.Extensions
{
    public static class ZStringExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string Right(this string str, int len)
        {
            if (str.Length < len)
            {
                throw new ArgumentException("len的长度不能大于字符串长度！");
            }

            return str.Substring(str.Length - len, len);
        }
    }
}
