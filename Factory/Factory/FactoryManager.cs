using Common.Ioc;

namespace Factory
{
    public class FactoryManager
    {
        public static T BuilderInstance<T>() where T : class
        {
            return NinjectContainer.Get<T>();
        }
    }
}
