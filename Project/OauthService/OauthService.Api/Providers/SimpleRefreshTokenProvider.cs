using System;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Infrastructure;
using Model;
using Factory;
using OauthService.IBussiness;
using CommonTools;

namespace OauthService.Api.Providers
{
    public class SimpleRefreshTokenProvider : IAuthenticationTokenProvider
    {
        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var refreshTokenId = DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(100, 999);
            var token = new RefreshTokens()
            {
                Id = refreshTokenId.GetHash(),
                Subject = context.Ticket.Identity.Name,
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
            };
            context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
            context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

            token.ProtectedTicket = context.SerializeTicket();

            var result = await BussinessFactory.GetBussinessFactory().CreateBussiness<IRefreshTokenBussiness>().Set(token);
            if (result.Flag) context.SetToken(refreshTokenId);
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            string hashTokenId = context.Token.GetHash();
            var result = await BussinessFactory.GetBussinessFactory().CreateBussiness<IRefreshTokenBussiness>().Get(hashTokenId);
            if (result != null)
            {
                context.DeserializeTicket(result.ProtectedTicket);
            }
        }
    }
}