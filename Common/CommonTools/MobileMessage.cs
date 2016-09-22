using System;
using System.Globalization;

namespace CommonTools
{
    //移动信息帮助类
    public class MobileMessage
    {
        private static readonly Random Rand = new Random((int)DateTime.Now.Ticks);
        private const string Min = "1111111111111111111111111111111111111";
        private const string Max = "9999999999999999999999999999999999999";

        public static string GetCheckCode(int length = 6)
        {
            return
                Rand.Next(Convert.ToInt32(Min.Substring(0, length)), Convert.ToInt32(Max.Substring(0, length)))
                    .ToString(CultureInfo.InvariantCulture);
        }
    }
}
