using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;
using Factory;
using OauthService.IBussiness;
using System.Security.Claims;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Linq;

namespace OauthService.Api.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        /// <summary>
        /// 认证用户(第三方应用程序)，目前认为所有第三方应用都是合法的
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// 授权资源访问证书
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var user = BussinessFactory.GetBussinessFactory().CreateBussiness<IUsersBussiness>().GetModel(context.UserName, context.Password);
            if (user == null)
            {
                context.SetError("授权无效", "用户名或密码错误.");
                return;
            }
            //声明身份标识属性
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
            identity.AddClaim(new Claim("sub", context.UserName));

            var props = new AuthenticationProperties(new Dictionary<string, string>
            {
                {
                    "as:client_id", context.ClientId ?? string.Empty
                },
                {
                    "userName", context.UserName
                }
            });
            //生成票据
            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);
        }

        /// <summary>
        /// 把Context中的属性加入到token中
        /// </summary>
        /// <param name="context">票据上下文</param>
        /// <returns></returns>
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// 刷新票据
        /// </summary>
        /// <param name="context">认证刷新票据上下文</param>
        /// <returns></returns>
        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            //实例化身份认证类型
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);

            var newClaim = newIdentity.Claims.Where(c => c.Type == "newClaim").FirstOrDefault();
            if (newClaim != null)
            {
                newIdentity.RemoveClaim(newClaim);
            }
            newIdentity.AddClaim(new Claim("newClaim", "newValue"));
            //生成新票据
            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }

    }
}