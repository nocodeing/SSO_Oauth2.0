using System;
using System.Web;
using System.Web.Caching;
namespace Buffer
{
    public class BufferHelp
    {
        private static readonly Cache BufferPool = HttpRuntime.Cache;

        public static void Add(string key, object obj, int minutes = 60)
        {
            BufferPool.Insert(key, obj, null, DateTime.Now.AddMinutes(minutes), Cache.NoSlidingExpiration);
        }

        public static T Get<T>(string key)
        {
            return (T)BufferPool[key];
        } 

        public static void Remove(string key)
        {
            BufferPool.Remove(key);
        }

    }
}
