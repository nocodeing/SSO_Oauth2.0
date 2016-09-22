using System;
using System.Collections.Generic;
using Redis.DB;

namespace CommonTools
{
    public class RedisHelper
    {
        private static string _serverIp;

        /// <summary>
        /// 初始化Redis
        /// </summary>
        public RedisHelper()
        {
            _serverIp = RequestHelper.GetServerIp();
        }



        /// <summary>
        /// 插入一条键值对数据
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值[T.对象]</param>
        /// <returns></returns>
        public bool Set<T>(string key, T value)
        {
            return new SimpleRedis(_serverIp).Set(key, value);
        }

        /// <summary>
        /// 插入一条键值对数据
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值[T.对象]</param>
        /// <param name="expiresAt">到期时间</param>
        /// <returns></returns>
        public bool Set<T>(string key, T value, DateTime expiresAt)
        {
            return new SimpleRedis(_serverIp).Set(key, value, expiresAt);
        }

        /// <summary>
        /// 插入一条键值对数据
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值[T.对象]</param>
        /// <param name="expiresIn">多久到期</param>
        /// <returns></returns>
        public bool Set<T>(string key, T value, TimeSpan expiresIn)
        {
            return new SimpleRedis(_serverIp).Set(key, value, expiresIn);
        }

        /// <summary>
        /// 插入集合数据
        /// </summary>
        /// <param name="map"></param>
        public void SetAll(Dictionary<string, string> map)
        {
            new SimpleRedis(_serverIp).SetAll(map);
        }

        /// <summary>
        /// 插入集合数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        public void SetAll<T>(IDictionary<string, T> values)
        {
            new SimpleRedis(_serverIp).SetAll(values);
        }

        /// <summary>
        /// 插入两个键值对的集合,下标相同
        /// </summary>
        /// <param name="keys">键</param>
        /// <param name="values">值</param>
        public void SetAll(IEnumerable<string> keys, IEnumerable<string> values)
        {
            new SimpleRedis(_serverIp).SetAll(keys, values);
        }

        /// <summary>
        /// T.获取一条键值对数据
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            return new SimpleRedis(_serverIp).Get<T>(key);
        }

        /// <summary>
        /// byte[].获取一条键值对数据
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public byte[] Get(string key)
        {
            return new SimpleRedis(_serverIp).Get(key);
        }

        public IDictionary<string, T> GetAll<T>(IEnumerable<string> keys)
        {
            return new SimpleRedis(_serverIp).GetAll<T>(keys);
        }

        public List<T> GetValues<T>(List<string> keys)
        {
            return new SimpleRedis(_serverIp).GetValues<T>(keys);
        }

        /// <summary>
        /// 删除一个键值对
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            return new SimpleRedis(_serverIp).Remove(key);
        }

        public void RemoveAll(IEnumerable<string> keys)
        {
            new SimpleRedis(_serverIp).RemoveAll(keys);
        }

        public bool RemoveEntry(params string[] keys)
        {
            return new SimpleRedis(_serverIp).RemoveEntry(keys);
        }

        /// <summary>
        /// 替换键值对
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public bool Replace<T>(string key, T value)
        {
            return new SimpleRedis(_serverIp).Replace<T>(key, value);
        }

        public bool Replace<T>(string key, T value, DateTime expiresAt)
        {
            return new SimpleRedis(_serverIp).Replace<T>(key, value, expiresAt);
        }

        public bool Replace<T>(string key, T value, TimeSpan expiresIn)
        {
            return new SimpleRedis(_serverIp).Replace<T>(key, value, expiresIn);
        }
        /// <summary>
        /// 向消息队列中集合中添加值
        /// </summary>
        /// <param name="listId">集合名称</param>
        /// <param name="value">值</param>
        public void EnqueueItemOnList(string listId, string value)
        {
            new SimpleRedis(_serverIp).EnqueueItemOnList(listId, value);
        }
        /// <summary>
        /// 从指定的消息队列集合中取出值
        /// </summary>
        /// <param name="listId">集合名称</param>
        /// <returns></returns>
        public string DequeueItemFromList(string listId)
        {
            return new SimpleRedis(_serverIp).DequeueItemFromList(listId);
        }

        public double GetListCount(string listId)
        {
            return new SimpleRedis(_serverIp).GetListCount(listId);
        }

        /// <summary>
        /// 设置一个hash值,如果存在则覆盖,如果不存在则添加
        /// </summary>
        /// <param name="hashId"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>如果key不存在则返回true,如果key存在则返回false</returns>
        public bool SetEntryInHash(string hashId, string key, string value)
        {
            return new SimpleRedis(_serverIp).SetEntryInHash(hashId, key, value);
        }

        /// <summary>
        /// 获取hash里对应key的值
        /// </summary>
        /// <param name="hashId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValueFromHash(string hashId, string key)
        {
            return new SimpleRedis(_serverIp).GetValueFromHash(hashId, key);
        }

        public bool SetEntryInHash<T>(string hashId, string key, T value)
        {
            return new SimpleRedis(_serverIp).SetEntryInHash(hashId, key, value);
        }

        public T GetValueFromHash<T>(string hashId, string key)
        {
            return new SimpleRedis(_serverIp).GetValueFromHash<T>(hashId, key);
        }
    }
}
