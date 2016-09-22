using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CommonTools
{
    public static class EnumHelper
    {
        public static Dictionary<object, object> GetNameValueDic(this Type enumType)
        {
            return Enum.GetNames(enumType).ToDictionary<string, object, object>(value => Enum.Format(enumType, Enum.Parse(enumType, value), "d"), value => value);
        }

        public static IEnumerable<string> GetNames(this Enum em)
        {
            return em.GetType().GetEnumNames();
        }

        public static Enum GetEnum(Type em, string enumStr)
        {
            return (Enum)Enum.Parse(em, enumStr);
        }

        public static string ShortToEnum<T>(this short state)
        {
            var targetEnum = (T)Enum.Parse(typeof(T), state.ToString(CultureInfo.InvariantCulture));
            return targetEnum.ToString();
        }

        public static string ShortToEnum<T>(this short? state)
        {
            var targetEnum = (T)Enum.Parse(typeof(T), (state ?? 0).ToString(CultureInfo.InvariantCulture));
            return targetEnum.ToString();
        }
        public static T ToEnum<T>(this short? state)
        {
            var targetEnum = (T)Enum.Parse(typeof(T), (state ?? 0).ToString(CultureInfo.InvariantCulture));
            return targetEnum;
        }
        public static string IntToEnum<T>(this int state)
        {
            var targetEnum = (T)Enum.Parse(typeof(T), state.ToString(CultureInfo.InvariantCulture));
            return targetEnum.ToString();
        }
        public static T ToEnum<T>(this int state)
        {
            var targetEnum = (T)Enum.Parse(typeof(T), state.ToString(CultureInfo.InvariantCulture));
            return targetEnum;
        }

        public static string IntToEnum<T>(this int? state)
        {
            var targetEnum = (T)Enum.Parse(typeof(T), (state ?? 0).ToString(CultureInfo.InvariantCulture));
            return targetEnum.ToString();
        }

        public static string ByteToEnum<T>(this byte? state)
        {
            var targetEnum = (T)Enum.Parse(typeof(T), (state ?? 0).ToString(CultureInfo.InvariantCulture));
            return targetEnum.ToString();
        }

        public static string BoolToEnum<T>(this bool? state)
        {
            return IntToEnum<T>(Convert.ToInt32(state));
        }
    }
}
