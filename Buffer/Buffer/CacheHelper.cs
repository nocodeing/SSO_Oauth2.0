using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;
using System.Web.Caching;
using BLToolkit.Data;

namespace Buffer
{
    public  class CacheHelper
    {
        private static readonly Cache WebCache = HttpRuntime.Cache;
        private readonly string _sqlText;
        private static readonly Dictionary<string, CacheHelper> SqlList =new  Dictionary<string, CacheHelper>();  
        private CacheHelper(string sqlText)
        {

            _sqlText = sqlText;
        }

        public static CacheHelper CreateCacheHelper(string sqlText)
        {
            if (!SqlList.ContainsKey(sqlText.ToLower()))
            {
                SqlList.Add(sqlText.ToLower(),new CacheHelper(sqlText));
            }
            return SqlList[sqlText.ToLower()];
        }

        //public static void Regist(string connection)
        //{
        //    SqlDependency.Start(connection);
        //}
        //public static void Stop(string connection)
        //{
        //    SqlDependency.Stop(connection);
        //}
        public  IList<T> GetCacheData<T>(DbManager db)
        {
            if (WebCache[_sqlText] == null)
            {
                db.SetCommand(_sqlText);
                var dep = new SqlDependency(db.SelectCommand as SqlCommand);
                dep.OnChange += dep_OnChange;
                var list = db.ExecuteList<T>();
                WebCache[_sqlText] = list;
                return list;
            }
            return (IList<T>)WebCache[_sqlText];
        }

        private  void dep_OnChange(object sender, SqlNotificationEventArgs e)
        {
            WebCache.Remove(_sqlText);
        }
        
    }
}
