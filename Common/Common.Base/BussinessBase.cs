using CommonTools;
using Factory;

namespace Common.Base
{
    public abstract class BussinessBase : IBussinessBase
    {
        private IDalFactoryRepository _dalFactory;

        protected IBussinessFactory BFactory
        {
            get { return BussinessFactory.GetBussinessFactory(); }
        }

        public IDalFactoryRepository FactoryRepository
        {
            set
            {
                _dalFactory = value;
                FactoryProxy = value;
            }
            get
            {
                return _dalFactory;
            }
        }
        protected IDalFactoryRepository FactoryProxy;
        public void JoinTransactioin(IDalFactoryRepository transactionRepository)
        {

            foreach (var tranKv in FactoryRepository.DbManagerContainer)
            {
                transactionRepository.DbManagerContainer.TryAdd(tranKv.Key, tranKv.Value);
            }
            if (transactionRepository.CurrentDistributionTran == null)
            {
                transactionRepository.CurrentDistributionTran = FactoryRepository.CurrentDistributionTran;
            }

            //FactoryRepository.Dispose();//源链接关闭
            FactoryRepository = transactionRepository;
            FactoryProxy = null;
        }
    }
}
    