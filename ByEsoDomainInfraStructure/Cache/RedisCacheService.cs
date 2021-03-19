using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ByEsoDomainInfraStructure.Cache.Interfaces;
using Newtonsoft.Json;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Wrap;
using StackExchange.Redis;

namespace ByEsoDomainInfraStructure.Cache
{
  public class RedisCacheService : ICacheService
  {
    private RedisConnectionPool _connectionPool;
    private readonly string _connectionString;
    private readonly int _maxPoolSize;

    private IDatabase _cache
    {
      get { return _connectionPool.GetConnection().GetDatabase(); }
    }

    public RedisCacheService(string connectionString, int maxPoolSize)
    {
      _connectionString = connectionString;
      _maxPoolSize = maxPoolSize;
    }

    public void Connect()
    {
      _connectionPool = new RedisConnectionPool(_connectionString, _maxPoolSize);
    }

    public bool SetValue(string key, object value)
    {
      return _cache.StringSet(key, JsonConvert.SerializeObject(value));
    }

    public string GetValue(string key)
    {
      return _cache.StringGet(key).ToString();
    }

    public bool Haskey(string key)
    {
      return _cache.KeyExists(key);
    }

    public async ValueTask DisposeAsync()
    {
      await _connectionPool.DisposeAsync();
    }

    public ISessionData GetSessionData()
    {
      throw new NotImplementedException();
    }
  }
}
