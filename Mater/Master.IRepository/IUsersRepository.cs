using Common.Base;
using Model;

namespace Master.IRepository
{
    public interface IUsersRepository : IDataBase
    {
        /// <summary>
        /// 获取用户实体
        /// </summary>
        /// <returns></returns>
        Users Find(string userName, string password);
    }
}
