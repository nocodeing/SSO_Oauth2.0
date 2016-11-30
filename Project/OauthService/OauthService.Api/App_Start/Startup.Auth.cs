using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using OauthService.Api.Providers;
using OauthService.Api.Models;

namespace OauthService.Api
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        // 有关配置身份验证的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {

            // 针对基于 OAuth 的流配置应用程序
            PublicClientId = "self";
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,//允许客户端使用http协议请求；
                TokenEndpointPath = new PathString("/Token"),//获取证书路径，必需以“/”开头
                //AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                Provider = new SimpleAuthorizationServerProvider(),

                //在生产模式下设 AllowInsecureHttp = false
                RefreshTokenProvider = new SimpleRefreshTokenProvider()
            };

            // 使应用程序可以使用不记名令牌来验证用户身份
            app.UseOAuthBearerTokens(OAuthOptions);

            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
            // 取消注释以下行可允许使用第三方登录提供程序登录
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //    consumerKey: "",
            //    consumerSecret: "");

            //app.UseFacebookAuthentication(
            //    appId: "",
            //    appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }
    }
}
