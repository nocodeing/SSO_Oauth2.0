using System;
using System.Linq;
using System.Web.Mvc;

namespace CommonTools.WebHelper
{
    public class AntiSqlInjectAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var actionParameters = filterContext.ActionDescriptor.GetParameters();
            foreach (var p in actionParameters)
            {
                if (p.ParameterType == typeof(string))
                {
                    if (filterContext.ActionParameters[p.ParameterName] != null)
                    {
                        filterContext.ActionParameters[p.ParameterName] = filterContext.ActionParameters[p.ParameterName].ToString().FilterSql();
                    }
                    continue;
                }
                Type temp = null;
                if (filterContext.ActionParameters[p.ParameterName] != null)
                {
                    temp = filterContext.ActionParameters[p.ParameterName].GetType();
                }
                if (temp == null || temp.GetProperties().Count() == 1) continue;
                foreach (var pi in temp.GetProperties())
                {
                    object value = pi.GetValue(filterContext.ActionParameters[p.ParameterName], null);
                    if (value is string)
                    {
                        value = value.ToString().FilterSql();
                        pi.SetValue(filterContext.ActionParameters[p.ParameterName], value);
                    }
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }
    }
}
