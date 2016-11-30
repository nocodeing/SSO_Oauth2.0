using System;
using Common.Base;
using CommonTools;
using Model;
using OauthService.IBussiness;
using Master.IRepository;
using System.Threading.Tasks;

namespace OauthService.Bussiness
{
    public class RefreshTokenBussiness : BussinessBase, IRefreshTokenBussiness
    {
        public async Task<ReturnResult> Set(RefreshTokens entity)
        {
            using (FactoryProxy)
            {
                FactoryRepository.BeginTran(FactoryProxy);
                var flag = FactoryRepository.CreateInstance<IRefreshTokensRepository>().Insert(entity) == 1;
                if (!flag)
                    FactoryRepository.RollBack(FactoryProxy);
                else
                    FactoryRepository.Commit(FactoryProxy);

                return new ReturnResult() { Flag = true, Message = "成功" };
            }
        }

        public async Task<RefreshTokens> Get(string id)
        {
            using (FactoryProxy)
            {
                return FactoryRepository.CreateInstance<IRefreshTokensRepository>().Find<RefreshTokens>(id);
            }
        }
    }
}
