using System.Linq;
using System.Web.Http;
using System.Net.Http.Formatting;
using Newtonsoft.Json.Serialization;

namespace OauthService.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();//使用CamelCase(驼峰)命名法序列化
        }
    }
}
