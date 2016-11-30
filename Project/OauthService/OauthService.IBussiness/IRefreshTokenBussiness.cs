using Common.Base;
using CommonTools;
using Model;
using System.Threading.Tasks;

namespace OauthService.IBussiness
{
    public interface IRefreshTokenBussiness : IBussinessBase
    {
        /// <summary>
        /// 设置刷新票据
        /// </summary>
        /// <param name="entity">刷新实体</param>
        /// <returns></returns>
        Task<ReturnResult> Set(RefreshTokens entity);

        /// <summary>
        /// 获取票据实体
        /// </summary>
        /// <param name="id">票据id</param>
        /// <returns></returns>
        Task<RefreshTokens> Get(string id);
    }
}
