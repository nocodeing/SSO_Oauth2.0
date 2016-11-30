using CommonTools;
using Factory;
using OauthService.Api.WebHelper;
using OauthService.Common.ViewModel;
using OauthService.IBussiness;
using System.Web.Http;

namespace OauthService.Api.Controllers
{

    [Authorize]
    [RoutePrefix("api/User")]
    public class UserController : BaseApiController
    {
        [AllowAnonymous]
        [Route("Register")]
        public ReturnResult Register(UsersModel model)
        {
            var result = BFactory.CreateBussiness<IUsersBussiness>().Register(model);

            return result;
        }

        [HttpGet,Route("hello")]
        public string SayHello()
        {
            return "Hello, World";
        }
    }
}