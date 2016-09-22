using System.Web.Mvc;

namespace CommonTools.WebHelper
{
    public class JsonpResult<T> : ActionResult
    {
        private T Obj { get; set; }
        private string CallbackName { get; set; }

        public JsonpResult(T obj, string callback)
        {
            Obj = obj;
            CallbackName = callback;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var js = new System.Web.Script.Serialization.JavaScriptSerializer();
            var jsonp = CallbackName + "(" + js.Serialize(Obj) + ")";
            context.HttpContext.Response.ContentType = "application/json; charset=utf-8";
            context.HttpContext.Response.Write(jsonp);
        }
    }


}