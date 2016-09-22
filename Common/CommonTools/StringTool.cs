using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using CommonTools.EM;

namespace CommonTools
{
    public static class StringTool
    {
        /// <summary>
        /// 隐藏手机中间数字
        /// </summary>
        /// <param name="cellphone"></param>
        /// <returns></returns>
        public static string PhoneHide(this string cellphone)
        {
            return Regex.Replace(cellphone, @"(?im)(\d{3})(\d{4})(\d{4})", "$1****$3");
        }

        /// <summary>
        /// 是否存在特殊字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool ExistSpecial(this string str)
        {
            Regex reg = new Regex(@"^[A-Za-z0-9]+$"); //只能输入字母或数字
            return reg.Match(str).Success;
        }

        /// <summary>
        /// 过滤不安全的字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FilteSqlStr(this string str)
        {
            str = str.Replace("'", "");
            str = str.Replace("\"", "");
            str = str.Replace("&", "&amp");
            str = str.Replace("<", "&lt");
            str = str.Replace(">", "&gt");

            str = str.Replace("delete", "");
            str = str.Replace("update", "");
            str = str.Replace("insert", "");

            return str;
        }
        public static string FilterSql(this string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            s = s.Trim().ToLower();
            s = s.Replace("=", "");
            s = s.Replace("'", "");
            s = s.Replace(";", "");
            s = s.Replace(" or ", "");
            s = s.Replace("select", "");
            s = s.Replace("update", "");
            s = s.Replace("insert", "");
            s = s.Replace("delete", "");
            s = s.Replace("declare", "");
            s = s.Replace("exec", "");
            s = s.Replace("drop", "");
            s = s.Replace("create", "");
            s = s.Replace("%", "");
            s = s.Replace("--", "");
            return s;
        }

        /// <summary>
        /// 隐藏Email中间字符
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static string EmailHide(this string email)
        {
            return Regex.Replace(email, @"(?<=^.{3}).*(?=.{4}$)", @"***");
        }

        public static List<Guid> ToGuids(this string idList, char splitChar = ',')
        {
            if (String.IsNullOrEmpty(idList)) return null;
            var ids = idList.Split(new[] { splitChar }, StringSplitOptions.RemoveEmptyEntries);
            if (ids.Length == 0) return null;
            return ids.Select(Guid.Parse).ToList();
        }

        public static string ReplaceHtml(this string htmlString)
        {
            if (htmlString.IsNullOrEmpty()) return null;
            var result = Regex.Replace(htmlString.Trim(), "<.*?>", "",
                RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            result = Regex.Replace(result, "&[a-z]+;", "", RegexOptions.IgnoreCase);
            return result;
        }

        public static string SubAnyString(this string src, int len, char fillChar = ' ')
        {
            src = src.PadRight(src.Length > len ? src.Length : len, fillChar);
            return src.Substring(0, len);
        }

        //获取含有汉字的固定长度字符串，转换成汉字长度

        public static string SubAnyChineseString(this string src, int len, char fillChar = ' ')
        {
            var chinesesLength = src.Count(c => c > 1000);
            if (chinesesLength > len)
            {
                return src.Substring(0, len);
            }
            src = src.PadRight(len * 2, fillChar);
            return src.Substring(0, len * 2 - chinesesLength);
        }

        public static string HtmlDecode(this string escapeString)
        {
            return escapeString == null ? null : HttpUtility.HtmlDecode(escapeString);
        }

        public static string HtmlEncode(this string str)
        {
            return str == null ? null : HttpUtility.HtmlEncode(str);
        }

        public static string UrlDecode(this string escapeString)
        {
            return escapeString == null ? null : HttpUtility.UrlDecode(escapeString);
        }

        public static string UrlEncode(this string str)
        {
            return str == null ? null : HttpUtility.HtmlEncode(str);
        }

        public static string UriEscape(this string str)
        {
            return str == null ? null : Uri.EscapeDataString(str);
        }

        public static string UriUnEscape(this string escapeString)
        {
            return escapeString == null ? null : Uri.UnescapeDataString(escapeString);
        }

        public static T GetEnum<T>(this string enumStr)
        {
            return (T)Enum.Parse(typeof(T), enumStr);
        }

        public static int GetEnumIndex(this string enumStr, Type enumType)
        {
            var em = (Enum)Enum.Parse(enumType, enumStr);
            return Convert.ToInt16(em);
        }

        /// <summary>
        /// 是否为NULL
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>为NULL返回空字符串</returns>
        public static string IsNull(this string str)
        {
            return string.IsNullOrEmpty(str) ? "" : str;
        }

        /// <summary>
        /// 取得HTML中所有图片的 URL。
        /// </summary>
        /// <param name="sHtmlText">HTML代码</param>
        /// <returns>图片的URL列表</returns>
        public static string[] GetHtmlImageUrlList(this string sHtmlText)
        {
            if (sHtmlText.IsNullOrEmpty()) return null;
            // 定义正则表达式用来匹配 img 标签
            Regex regImg =
                new Regex(
                    @"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>",
                    RegexOptions.IgnoreCase);

            // 搜索匹配的字符串
            MatchCollection matches = regImg.Matches(sHtmlText);

            int i = 0;
            string[] sUrlList = new string[matches.Count];

            // 取得匹配项列表
            foreach (Match match in matches)
                sUrlList[i++] = match.Groups["imgUrl"].Value;

            return sUrlList;
        }


        /// <summary>
        /// 生成随机数的种子
        /// </summary>
        /// <returns></returns>
        private static int GetNewSeed()
        {
            byte[] rndBytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(rndBytes);
            return BitConverter.ToInt32(rndBytes, 0);
        }

        /// <summary>
        /// 生成固定位随机数（字符串，含数字）
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetRandomString(int length)
        {
            string s = "123456789abcdefghijklmnpqrstuvwxyzABCDEFGHIJKLMNPQRSTUVWXYZ";
            string reValue = string.Empty;
            Random rnd = new Random(GetNewSeed());
            while (reValue.Length < length)
            {
                string s1 = s[rnd.Next(0, s.Length)].ToString();
                if (reValue.IndexOf(s1) == -1) reValue += s1;
            }
            return reValue;
        }

        /// <summary>
        /// 生成固定位随机数（字符串，不含数字，不含敏感组合）
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetRandomStr(int length)
        {
            string s = "acefghijklmnpqrstuvwxyzACEFGHIJKLMNPQRSTUVWXYZ";
            string reValue = string.Empty;
            Random rnd = new Random(GetNewSeed());
            while (reValue.Length < length)
            {
                string s1 = s[rnd.Next(0, s.Length)].ToString();
                if (reValue.IndexOf(s1) == -1) reValue += s1;
            }
            return reValue;
        }

        /// <summary>
        /// 四舍五不入
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static decimal GetRoundString(decimal amount)
        {
            return decimal.Parse(amount.ToString().Substring(0, amount.ToString().IndexOf('.') + 3));
        }

    }
}