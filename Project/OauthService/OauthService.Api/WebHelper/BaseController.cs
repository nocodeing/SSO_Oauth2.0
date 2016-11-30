using Factory;
using System.Web.Http;

namespace OauthService.Api.WebHelper
{
    public class BaseApiController : ApiController
    {
        protected IBussinessFactory BFactory
        {
            get { return BussinessFactory.GetBussinessFactory(); }
        }

    }
}