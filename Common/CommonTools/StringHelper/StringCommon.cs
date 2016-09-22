
namespace CommonTools.StringHelper
{
    internal static class StringCommon
    {
        /// <summary>
        /// 邮箱正则表达式
        /// </summary>
        public const string RegEmail = @"^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";

        /// <summary>
        /// 固话号正则表达式
        /// </summary>
        public const string RegTelephone = "^(0[0-9]{2,3}/\\-)?([2-9][0-9]{6,7})+(/\\-[0-9]{1,4})?$";

        /// <summary>   
        /// 手机号正则表达式
        /// </summary>
        public const string RegCellphone = @"^(13[0-9]|14[5|7]|15[0|1|2|3|5|6|7|8|9]|18[0-9]|17[0-9])\d{8}$";

        /// <summary>
        /// ip地址表达式
        /// </summary>
        public const string RegIp = @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$";
        /// <summary>
        /// 匹配img标签里图片表达式
        /// </summary>
        public const string RegImg = "src=[\'\"]?([^\'\"]*)[\'\"]?";
    }
}
