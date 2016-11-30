using Common.Base;
using CommonTools;
using OauthService.Common.ViewModel;

namespace OauthService.IBussiness
{
    public interface IUsersBussiness : IBussinessBase
    {
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="model">用户数据</param>
        /// <returns></returns>
        ReturnResult Register(UsersModel model);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        UsersModel GetModel(string userName, string password);
    }
}
