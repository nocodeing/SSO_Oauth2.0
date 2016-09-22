
using System.Text.RegularExpressions;

namespace CommonTools
{
    public static class RegexHelp
    {
        /// <summary>
        /// 发帖及回复时把属于第五大街的a标签加上跳转链接
        /// </summary>
        /// <param name="reply"></param>
        /// <returns></returns>
        public static string OutReplaceReply(this string reply)
        {
            if (string.IsNullOrWhiteSpace(reply))
            {
                return reply;
            }
            const string pattern = @"((https):(\/\/|\\\\)(((?!img1)\w)+[.]){1}(5tha.com)(((\/[\~]*|\\[\~]*)(\w)+)|[.](\w)+)*(((([?](\w)+){1}[=]*))*((\w)+){1}([\&](\w)+[\=](\w)+)*)*)";
            Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
            return  r.Replace(reply, new MatchEvaluator(OutPutThreadMatch));

        }

        static string OutPutThreadMatch(Match match)
        {
            return "<a href='" + match.Value + "' target='_blank'>" + match.Value + "</a>";
        }
    }
}
