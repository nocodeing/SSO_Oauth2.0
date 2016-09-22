using System.Web.Mvc;
using Model;

namespace CommonTools.WebHelper
{
    public static class PageResult
    {
        public static JsonResult Messages(bool state, string msg, object data = null)
        {
            return new JsonResult
            {
                Data = new { State = state, Msg = msg, PageData = data }
            };
        }


        public static JsonResult Messages(ResponseBase response)
        {
            return new JsonResult { Data = new { State = response.IsSuccess, Msg = response.Message } };
        }

        public static JsonResult Messages(bool state, string msg,int  code, object data = null)
        {
            return new JsonResult
            {
                Data = new { State = state, Msg = msg,Code=code, PageData = data }
            };
        }
    }
}