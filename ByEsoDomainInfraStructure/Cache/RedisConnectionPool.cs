using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace ByEsoDomainInfraStructure.Cache
{
  public class RedisConnectionPool : IAsyncDisposable, IDisposable
  {
    private static ConcurrentBag<Lazy<ConnectionMultiplexer>> _connections;
    private readonly string _connectionString;
    private readonly int _maxPoolSize;

    public RedisConnectionPool(string connectionString, int maxPoolSize)
    {
      _connectionString = connectionString;
      _maxPoolSize = maxPoolSize;
      Initialize();
    }

    public IConnectionMultiplexer GetConnection()
    {
      // TODO: Set connection time outs and handle timed out connections
      Lazy<ConnectionMultiplexer> connectionMultiplexer;

      var loadedConnections = _connections.Where(lazy => lazy.IsValueCreated);

      // TODO: Add therad sync logic
      if (loadedConnections.Count() == _connections.Count)
      {
        // Round robin logic to get least loaded connection
        connectionMultiplexer = _connections.OrderBy(x => x.Value.GetCounters().TotalOutstanding).First();
      }
      else
      {
        // TODO: Optimize based on max load setting
        connectionMultiplexer = _connections.First(lazy => !lazy.IsValueCreated);
      }

      return connectionMultiplexer.Value;
    }

    public async ValueTask DisposeAsync()
    {
      var activeConnections = _connections.Where(lazy => lazy.IsValueCreated).ToList();
      var tasks = activeConnections.Select(c => c.Value.CloseAsync()).ToArray();

      await Task.WhenAll(tasks);

      // Suppress finalization.
      GC.SuppressFinalize(this);
    }

    // For finalizer
    public void Dispose()
    {
      var activeConnections = _connections.Where(lazy => lazy.IsValueCreated).ToList();
      activeConnections.ForEach(c => c.Value.Close());

      // Suppress finalization.
      GC.SuppressFinalize(this);
    }

    // Finalizer
    ~RedisConnectionPool()
    {
      Dispose();
    }

    private void Initialize()
    {
      _connections = new ConcurrentBag<Lazy<ConnectionMultiplexer>>();

      for (int i = 0; i < _maxPoolSize; i++)
      {
        _connections.Add(new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(_connectionString)));
      }
    }
  }
}
