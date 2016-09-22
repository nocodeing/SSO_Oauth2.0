using System;
using System.Collections.Generic;
using Model.Const;
using ServiceStack.Redis;
using ServiceStack.Text;

namespace Redis.DB
{
    public class SimpleRedis
    {
        private readonly RedisClient _redisClient;

        /// <summary>
        /// 初始化Redis
        /// </summary>
        public SimpleRedis(string serverIp)
        {
            if (_redisClient != null) return;
            switch (serverIp)
            {
                case "203.171.233.12":
                    _redisClient = new RedisClient(RedisConst.HostProduct, RedisConst.Port, RedisConst.Password) { Db = 0 };
                    return;
                case "203.171.233.5":
                    _redisClient = new RedisClient(RedisConst.HostProduct, RedisConst.Port, RedisConst.Password) { Db = 1 };
                    break;
                case "192.168.2.79":
                    _redisClient = new RedisClient(RedisConst.HostSandbox, RedisConst.Port, RedisConst.Password) { Db = 0 };
                    return;
                case "192.168.2.89":
                    _redisClient = new RedisClient(RedisConst.HostSandbox, RedisConst.Port, RedisConst.Password) { Db = 1 };
                    break;
                default:
                    _redisClient = new RedisClient(RedisConst.HostSandbox, RedisConst.Port, RedisConst.Password) { Db = 2 };
                    break;
            }
        }

        public bool Set<T>(string key, T value)
        {
            return _redisClient.Set(key, value);
        }

        public bool Set<T>(string key, T value, DateTime expiresAt)
        {
            return _redisClient.Set(key, value, expiresAt);
        }

        public bool Set<T>(string key, T value, TimeSpan expiresIn)
        {
            return _redisClient.Set(key, value, expiresIn);
        }

        public void SetAll(Dictionary<string, string> map)
        {
            _redisClient.SetAll(map);
        }

        public void SetAll<T>(IDictionary<string, T> values)
        {
            _redisClient.SetAll(values);
        }

        public void SetAll(IEnumerable<string> keys, IEnumerable<string> values)
        {
            _redisClient.SetAll(keys, values);
        }
        public T Get<T>(string key)
        {
            return _redisClient.Get<T>(key);
        }

        public byte[] Get(string key)
        {
            return _redisClient.Get(key);
        }

        public IDictionary<string, T> GetAll<T>(IEnumerable<string> keys)
        {
            return _redisClient.GetAll<T>(keys);
        }

        public List<T> GetValues<T>(List<string> keys)
        {
            return _redisClient.GetValues<T>(keys);
        }

        public bool Remove(string key)
        {
            return _redisClient.Remove(key);
        }

        public void RemoveAll(IEnumerable<string> keys)
        {
            _redisClient.RemoveAll(keys);
        }

        public bool RemoveEntry(params string[] keys)
        {
            return _redisClient.RemoveEntry(keys);
        }

        public bool Replace<T>(string key, T value)
        {
            return _redisClient.Replace<T>(key, value);
        }

        public bool Replace<T>(string key, T value, DateTime expiresAt)
        {
            return _redisClient.Replace<T>(key, value, expiresAt);
        }

        public bool Replace<T>(string key, T value, TimeSpan expiresIn)
        {
            return _redisClient.Replace<T>(key, value, expiresIn);
        }
        public void EnqueueItemOnList(string listId, string value)
        {
            _redisClient.EnqueueItemOnList(listId, value);
        }
        public string DequeueItemFromList(string listId)
        {
            return _redisClient.DequeueItemFromList(listId);
        }

        public double GetListCount(string listId)
        {
            return _redisClient.GetListCount(listId);
        }

        public bool SetEntryInHash(string hashId, string key, string value)
        {
            return _redisClient.SetEntryInHash(hashId, key, value);

        }

        public string GetValueFromHash(string hashId, string key)
        {
            return _redisClient.GetValueFromHash(hashId, key);
        }

        public bool SetEntryInHash<T>(string hashId, string key, T value)
        {
            return _redisClient.SetEntryInHash(hashId, key, JsonSerializer.SerializeToString(value));
        }

        public T GetValueFromHash<T>(string hashId, string key)
        {
            return JsonSerializer.DeserializeFromString<T>(_redisClient.GetValueFromHash(hashId, key));
        }
    }
}
