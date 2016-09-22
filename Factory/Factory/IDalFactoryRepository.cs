using System.Collections.Concurrent;
using System.Transactions;
using BLToolkit.Data;
using CommonTools;

namespace Factory
{
    public interface IDalFactoryRepository : System.IDisposable
    {
        T CreateInstance<T>(DbConnectionType dbConnectionType = default(DbConnectionType));
        ConcurrentDictionary<DbConnectionType, DbManager> DbManagerContainer { get; }
        TransactionScope CurrentDistributionTran { set; get; }
        void RollBack(IDalFactoryRepository factoryProxy);
        void BeginTran(IDalFactoryRepository factoryProxy);
        void Commit(IDalFactoryRepository factoryProxy);
    }
}
