using System.Web.Mvc;

namespace CommonTools.WebHelper
{
    public class RequestFilterAttribute : ActionFilterAttribute
    {
        private readonly bool _isFilterAjax;

        public RequestFilterAttribute(bool isFilterAjax=true)
        {
            _isFilterAjax = isFilterAjax;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (_isFilterAjax && !filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = PageResult.Messages(false, "无效的请求!");
            }
        }
    }
}