using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Transactions;
using BLToolkit.Data;
using Common.Ioc;
using CommonTools;

namespace Factory
{
    public class DalFactoryRepository : IDalFactoryRepository
    {
        private const string ContextKey = "_DbManagerContainer";
        public ConcurrentDictionary<DbConnectionType, DbManager> DbManagerContainer
        {
            get
            {
                var result = (ConcurrentDictionary<DbConnectionType, DbManager>)CallContext.GetData(ContextKey);
                if (result == null)
                {
                    result = new ConcurrentDictionary<DbConnectionType, DbManager>();
                    CallContext.SetData(ContextKey, result);
                }
                return result;
            }

        }


        private List<DbManager> ManagerList
        {
            get
            {
                var result = (List<DbManager>)CallContext.GetData(ContextKey + "temp");
                if (result == null)
                {
                    result = new List<DbManager>();
                    CallContext.SetData(ContextKey + "temp", result);
                }
                return result;
            }

        }

        private DbManager GetDbManager(DbConnectionType dbConnectionType)
        {
            DbManager result = (DbManager)CallContext.GetData("SingleCase" + dbConnectionType);
            if (result == null)
            {
                result = DataProvider.GetDbManager(dbConnectionType);
                CallContext.SetData("SingleCase" + dbConnectionType, result);
                ManagerList.Add(result);
            }
            if (result.Connection.State != ConnectionState.Open)
                result.Connection.Open();
            return result;

        }

        public void Dispose()
        {
            foreach (DbManager dbManager in ManagerList)
            {
                dbManager.Connection.Dispose();
            }
            ManagerList.Clear();
            DbManagerContainer.Clear();
            foreach (var name in Enum.GetNames(typeof(DbConnectionType)))
            {
                CallContext.SetData("SingleCase" + name, null);

            }
        }

        public T CreateInstance<T>(DbConnectionType dbConnectionType = default(DbConnectionType))
        {
            var obj = NinjectContainer.Get<T>();
            DbManager dbManager = GetDbManager(dbConnectionType);

            DynamicMethodMemberAccessor.GeteMemberAccessor().SetValue(obj, "DbManager", dbManager);

            return obj;
        }


        public void BeginTran(IDalFactoryRepository factoryProxy)
        {
            if (factoryProxy == null) return;
            if (DbManagerContainer == null) return;
            //if (DataProvider.IsLocalTran)
            //{
            if (DbManagerContainer.Count == 0)
            {
                var conStrKey = DataProvider.ConnString.Keys.FirstOrDefault();
                DbConnectionType conType;
                if (!Enum.TryParse(conStrKey, true, out conType)) return;
                var dbManager = GetDbManager(conType);
                dbManager.BeginTransaction();
                DbManagerContainer.GetOrAdd(conType, dbManager);
            }
            else DbManagerContainer.Values.First().BeginTransaction();
            //}
            //else
            //{
            //    if (CurrentDistributionTran == null)
            //    {
            //        CurrentDistributionTran = new TransactionScope();
            //    }
            //}
        }

        public void Commit(IDalFactoryRepository factoryProxy)
        {
            if (factoryProxy == null) return;
            if (DbManagerContainer == null) return;
            //if (DataProvider.IsLocalTran)
            //{
            DbManagerContainer.Values.First().CommitTransaction();

            //}
            //else
            //{
            //    if (CurrentDistributionTran != null)
            //    {
            //        LogHelper.Instance.Info("Complete()执行" + this.GetType().Assembly);
            //        CurrentDistributionTran.Complete();
            //    }
            //}
        }

        public void RollBack(IDalFactoryRepository factoryProxy)
        {
            if (factoryProxy == null) return;
            if (DbManagerContainer == null) return;
            if (DbManagerContainer.Values.Any())
            {
                DbManagerContainer.Values.First().RollbackTransaction();
            }
        }


        public TransactionScope CurrentDistributionTran { get; set; }

    }
}
