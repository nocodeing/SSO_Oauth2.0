using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CommonTools.StringHelper
{
    public static partial class StringHelper
    {
        #region 判断

        /// <summary>
        /// 是否是null或空字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// 是否不为空字符串也不是null
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNotNullAndEmpty(string str)
        {
            return !string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// 是否是Integer
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsInteger(string str)
        {
            int i;
            return int.TryParse(str, out i);
        }

        /// <summary>
        /// 是否是double
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDouble(string str)
        {
            double dbl;
            return double.TryParse(str, out dbl);
        }

        /// <summary>
        /// 是否是single
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsSingle(string str)
        {
            float flt;
            return float.TryParse(str, out flt);
        }

        /// <summary>
        /// 是否是ip地址
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIp(string ip)
        {
            return Regex.IsMatch(ip, StringCommon.RegIp);

        }

        /// <summary>
        /// 是否是手机号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsCellPhone(string str)
        {
            var rphone = new Regex(StringCommon.RegCellphone);
            return rphone.IsMatch(str);
        }

        /// <summary>
        /// 是否是固话号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsTelephone(string str)
        {
            var rphone = new Regex(StringCommon.RegTelephone);
            return rphone.IsMatch(str);
        }

        /// <summary>
        /// 是否是邮箱地址
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsEmail(string str)
        {
            var remail = new Regex(StringCommon.RegEmail);
            return remail.IsMatch(str);
        }
        /// <summary>
        /// 是否是中国公民身份证号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsIdCard(string id)
        {
            if (id.Length == 18)
            {
                return IsIdCard18(id);

            }
            else if (id.Length == 15)
            {
                return IsIdCard15(id);
            }
            else
            {
                return false;
            }
        }

        private static bool IsIdCard18(string Id)
        {
            long n = 0;
            if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证
            }
            const string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2), StringComparison.Ordinal) == -1)
            {
                return false;//省份验证
            }
            var birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time;
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            var arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            var wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            var ai = Id.Remove(17).ToCharArray();
            var sum = 0;
            for (var i = 0; i < 17; i++)
            {
                sum += int.Parse(wi[i]) * int.Parse(ai[i].ToString());
            }
            var y = -1;
            Math.DivRem(sum, 11, out y);
            return arrVarifyCode[y] == Id.Substring(17, 1).ToLower();
        }
        private static bool IsIdCard15(string Id)
        {
            long n = 0;
            if (long.TryParse(Id, out n) == false || n < Math.Pow(10, 14))
            {
                return false;//数字验证
            }
            const string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2), StringComparison.Ordinal) == -1)
            {
                return false;//省份验证
            }
            var birth = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time;
            return DateTime.TryParse(birth, out time) != false;
        }

        public static bool CustomRegex(string inputStr, string express)
        {
            var regex = new Regex(express);
            return regex.IsMatch(inputStr);
        }
        /// <summary>
        /// App用来匹配商品详情页面中的图文详情里的图片地址列表
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static List<string> ImageRegex(string inputStr)
        {
            var imgList = new List<string>();
            Regex reg = new Regex(StringCommon.RegImg);
            var matches = reg.Matches(inputStr);
            for (int i = 0; i < matches.Count; i++)
            {
                var groups = matches[i].Groups;
                var value = groups[1].Value;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    value = value.StartsWith("https:", StringComparison.OrdinalIgnoreCase) ? value : "https:" + value;
                    value = value.Trim('"');
                    if (!imgList.Contains(value))
                    {
                        imgList.Add(value);
                    }
                }
            }
            return imgList;
        }

        #endregion
    }
}
