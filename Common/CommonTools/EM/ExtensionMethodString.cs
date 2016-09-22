using System;

namespace CommonTools.EM
{
    public static partial class ExtensionMethod
    {
        #region string 扩展

        /// <summary>
        /// 验证为空
        /// </summary>
        /// <param name="str"></param>
        /// <returns>True空</returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return StringHelper.StringHelper.IsNullOrEmpty(str);
        }

        /// <summary>
        /// 验证非空
        /// </summary>
        /// <param name="str"></param>
        /// <returns>True非空</returns>
        public static bool IsNotNullAndEmpty(this string str)
        {
            return StringHelper.StringHelper.IsNotNullAndEmpty(str);
        }

        /// <summary>
        /// 是否为int类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsInteger(this string str)
        {
            return StringHelper.StringHelper.IsInteger(str);
        }

        /// <summary>
        /// 是否为手机号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsCellPhone(this string str)
        {
            return StringHelper.StringHelper.IsCellPhone(str);
        }

        /// <summary>
        /// 是否固定电话
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsPhone(this string str)
        {
            return StringHelper.StringHelper.IsTelephone(str);
        }
        
        /// <summary>
        /// 是否Email
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsEmail(this string str)
        {
            return StringHelper.StringHelper.IsEmail(str);
        }

        /// <summary>
        /// 是否为身份证号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsIdCard(this string str)
        {
            return StringHelper.StringHelper.IsIdCard(str);
        }
        #endregion

        #region 转换

        /// <summary>
        /// 转换成int类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ToInteger(this string str)
        {
            return int.Parse(str);
        }

        /// <summary>
        /// 转换成double类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double ToNumber(this string str)
        {
            return double.Parse(str);
        }

        /// <summary>
        /// 转换成DateTime类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string str)
        {
            return DateTime.Parse(str);
        }
        #endregion

        #region 拼音

        /// <summary>
        /// 得到汉语的拼音
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToChineseSpell(this string str)
        {
            return StringHelper.StringHelper.GetChineseSpell(str);
        }

        #endregion

    }
}
