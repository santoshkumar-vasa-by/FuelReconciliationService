using System;
using System.Collections.Concurrent;
using System.Linq;
using StackExchange.Redis;

namespace ByEsoDomainInfraStructure.Cache
{
  public class RedisConnection
  {
    private static string _connectionString;
    private static int _maxPoolSize;
    public const string CacheConnectionKey = "CacheConnection";
    public const string CacheMaxPoolSizeKey = "CacheConnectionMaxPoolSize";
    public const string IsCacheEnabledKey = "UseCache";

    private static readonly Lazy<ConnectionMultiplexer> _lazyConnection = new Lazy<ConnectionMultiplexer>(
      () => ConnectionMultiplexer.Connect(_connectionString));

    private static ConcurrentBag<Lazy<ConnectionMultiplexer>> _connections;
    public static ConnectionMultiplexer Multiplexer => _lazyConnection.Value;
    public static bool IsCacheEnabled { get; private set; }

    public static void InitializeRedisConnection(string azureRedisCacheconnectionString, bool isCacheEnabled,
      int maxPoolSize = 0)
    {
      IsCacheEnabled = isCacheEnabled;
      if (IsCacheEnabled)
      {
        if (string.IsNullOrWhiteSpace(azureRedisCacheconnectionString))
          throw new ArgumentNullException(nameof(azureRedisCacheconnectionString));

        _connectionString = azureRedisCacheconnectionString;
        _maxPoolSize = maxPoolSize;
        Initialize();
      }
    }

    // public ConnectionMultiplexer ConnectionMultiplexer => Multiplexer;
    public static IConnectionMultiplexer GetMultiplexer()
    {
      // TODO: Set connection time outs and handle timed out connections
      Lazy<ConnectionMultiplexer> connectionMultiplexer;

      var loadedConnections = _connections.Where(lazy => lazy.IsValueCreated);

      // TODO: Add therad sync logic
      if (loadedConnections.Count() == _connections.Count)
        // Round robin logic to get least loaded connection
        connectionMultiplexer = _connections.OrderBy(x => x.Value.GetCounters().TotalOutstanding).First();
      else
        // TODO: Optimize based on max load setting
        connectionMultiplexer = _connections.First(lazy => !lazy.IsValueCreated);

      return connectionMultiplexer.Value;
    }

    public static void Dispose()
    {
      var activeConnections = _connections.Where(lazy => lazy.IsValueCreated).ToList();
      activeConnections.ForEach(c => c.Value.Close());
      //Multiplexer.Dispose();
    }

    private static void Initialize()
    {
      _connections = new ConcurrentBag<Lazy<ConnectionMultiplexer>>();

      for (var i = 0; i < _maxPoolSize; i++)
        _connections.Add(new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(_connectionString)));
    }
  }
}
