using System;
using System.Collections.Generic;
using System.Text;

namespace ByEsoDomainInfraStructure.Cache.Interfaces
{
  public interface ICacheService : IAsyncDisposable
  {
    void Connect();

    bool SetValue(string key, object value);

    string GetValue(string key);

    bool Haskey(string key);
  }
}
