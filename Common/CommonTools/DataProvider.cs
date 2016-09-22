using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using BLToolkit.Data;

namespace CommonTools
{
    public abstract class DataProvider
    {
        private static Dictionary<string, string> _conStrs;

        static DataProvider()
        {
            Init();
        }

        public static bool IsLocalTran
        {
            get
            {
                var conStrs = ConfigurationManager.ConnectionStrings;
                return conStrs.Count == 2;
            }
        }

        private static void Init()
        {
            var conArray = typeof(DbConnectionType).GetEnumNames();
            if (conArray.Length > 0)
            {
                _conStrs = new Dictionary<string, string>();
                for (var i = 0; i < conArray.Length; i++)
                {
                    if (ConfigurationManager.ConnectionStrings[conArray[i]] != null)
                    {
                        _conStrs.Add(conArray[i], ConfigurationManager.ConnectionStrings[conArray[i]].ConnectionString);
                    }
                }
            }
        }

        public static Dictionary<string, string> ConnString
        {
            set { _conStrs = value; }
            get
            {
                if (_conStrs == null)
                {
                    Init();
                }
                return _conStrs;
            }
        }

        public static DbManager GetDbManager(DbConnectionType dbConnectionType)
        {
            DbManager manager;
            if (dbConnectionType == default(DbConnectionType))
            {
                manager = new DbManager(new SqlConnection
                {
                    ConnectionString = ConnString.Values.FirstOrDefault() ?? ""
                });
            }
            else
            {
                manager = new DbManager(new SqlConnection
                {
                    ConnectionString = ConnString[dbConnectionType.ToString()]
                });
            }
            return manager;
        }

        public static int Total = 0;
        public static DbManager SetConnection(DbManager manager, DbConnectionType dbConnectionType)
        {
            if (manager == null) throw new ArgumentNullException("manager");

            if (dbConnectionType == default(DbConnectionType))
            {
                manager.Connection = new SqlConnection
                {
                    ConnectionString = ConnString.Values.FirstOrDefault() ?? ""
                };
            }
            manager.Connection = new SqlConnection
            {
                ConnectionString = ConnString[dbConnectionType.ToString()]
            };
            return manager;
        }
    }
}