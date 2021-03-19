using ByEsoDomainInfraStructure.Cache;
using System;
using System.Collections.Generic;
using System.Text;
using ByEsoDomainInfraStructure.Cache.Interfaces;
using Newtonsoft.Json;

namespace ByEsoDomainInfraStructure.Cache
{
  public static class EsoCacheManager
  {
    private static ICacheService _cacheService;

    static EsoCacheManager()
    {
      //Logger.Init(typeof(WfmCacheManager));
    }

    public static void Connect(ICacheService cacheService)
    {
      //Logger.Debug("Connection Started..................................................");
      //Stopwatch sw = new Stopwatch();
      //sw.Start();
      _cacheService = cacheService;
      _cacheService.Connect();
      //sw.Stop();
      //Logger.Debug("Connection Time: " + sw.ElapsedMilliseconds.ToString() + " ms");
    }

    public static bool SetValue(string key, IUserData value)
    {
      //Stopwatch sw = new Stopwatch();
      //sw.Start();
      var isSet = _cacheService.SetValue(key, JsonConvert.SerializeObject(value));
      //sw.Stop();
      //Logger.Debug("SetValue Time: " + sw.ElapsedMilliseconds.ToString() + " ms");
      return isSet;
    }

    public static T GetValue<T>(string key)
    {
      //Stopwatch sw = new Stopwatch();
      //sw.Start();
      var dataString = JsonConvert.DeserializeObject(_cacheService.GetValue(key)).ToString();
      var data = JsonConvert.DeserializeObject<T>(dataString);
      //sw.Stop();
      //Logger.Debug("GetValue Time: " + sw.ElapsedMilliseconds.ToString() + " ms");
      return data;
    }

    public static bool Haskey(string key)
    {
      //Stopwatch sw = new Stopwatch();
      //sw.Start();
      var hasKey = _cacheService.Haskey(key);
      //sw.Stop();
      //Logger.Debug("Haskey Time: " + sw.ElapsedMilliseconds.ToString() + " ms");
      return hasKey;
    }

    public static void Dispose()
    {
      if (_cacheService != null)
      {
        //Stopwatch sw = new Stopwatch();
        //sw.Start();
        // Fire and Forget
        _cacheService.DisposeAsync();
        //sw.Stop();
        //Logger.Debug("Disposing Time: " + sw.ElapsedMilliseconds.ToString() + " ms");
        //Logger.Debug("Connection Disposed..................................................");
      }
    }
  }
}
