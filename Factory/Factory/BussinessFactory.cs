using Common.Ioc;

namespace Factory
{
    public class BussinessFactory : IBussinessFactory
    {
        private BussinessFactory()
        {
        }

        public static IBussinessFactory GetBussinessFactory()
        {
            return new BussinessFactory();
        }

        public virtual T CreateBussiness<T>()
        {
            var obj = NinjectContainer.Get<T>("FactoryRepository", new DalFactoryRepository());
            return obj;

        }
    }
}
