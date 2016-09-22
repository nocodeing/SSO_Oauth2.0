using System.Linq;
using Ninject;
using Ninject.Parameters;

namespace Common.Ioc
{
    /// <summary>
    /// 依赖注入容器
    /// </summary>
    public class NinjectContainer
    {
        private static IKernel _kernel;

        static NinjectContainer()
        {
            _kernel = new StandardKernel(new IocModule());
        }

        public static T Get<T>()
        {
            var module = _kernel.GetModules().ToList()[0] as IocModule;
            if (module != null)
            {
                module.LoadType(typeof(T));
            }
            return _kernel.Get<T>();
        }

        public static T Get<T>(string propertyName, object value)
        {
            var module = _kernel.GetModules().ToList()[0] as IocModule;
            if (module != null)
            {
                module.LoadType(typeof(T));
            }
            return _kernel.Get<T>(new PropertyValue(propertyName, value));
        }
    }
}