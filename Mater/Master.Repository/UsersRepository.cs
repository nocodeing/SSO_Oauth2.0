using System;
using Common.Base;
using Master.IRepository;
using Model;
using System.Data;
using System.Data.SqlClient;

namespace Master.Repository
{
    public class UsersRepository : DataBase, IUsersRepository
    {
        public Users Find(string userName, string password)
        {
            SqlText = string.Format("SELECT {0} FROM {1} WHERE UserName=@userName and PasswordHash=@password", Fields, TableName);
            return DbManager.SetCommand(SqlText, new IDbDataParameter[]
            {
                    new SqlParameter("@userName", userName),
                    new SqlParameter("@password", password)
            }).ExecuteObject<Users>();
        }
    }
}
