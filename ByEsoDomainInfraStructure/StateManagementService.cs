using System;
using System.Threading.Tasks;
using ByEsoDomainInfraStructure.Cache;
using ByEsoDomainInfraStructure.Cache.Interfaces;
using ByEsoDomainInfraStructure.UnitOfWork;
using Newtonsoft.Json;

namespace ByEsoDomainInfraStructure
{
  public interface IStateManagementService
  {
    ISessionData GetSessionData(string siteId);
  }

  public class StateManagementService : IStateManagementService
  {
    public const string CacheConnectionKey = "CacheConnection";
    public const string CacheMaxPoolSizeKey = "CacheConnectionMaxPoolSize";
    public const string IsCacheEnabledKey = "UseCache";

    private readonly ICacheService _CacheService;

    public StateManagementService(ICacheService cacheService)
    {
      _CacheService = cacheService;
    }

    public ISessionData GetSessionData(string siteId)
    {
      var sessionDataStr = _CacheService.GetValue(siteId);
      return string.IsNullOrWhiteSpace(sessionDataStr) ? default(ISessionData) : JsonConvert.DeserializeObject<SessionData>(sessionDataStr);
    }
    
    //public static void Connect(string connectionString, int maxPoolSize, bool isCacheEnabled)
    //{
    //  var cacheService = new RedisCacheService(connectionString, maxPoolSize);
    //  EsoCacheManager.Connect(cacheService);
    //}

    //public async Task Initialize(int siteId)
    //{
    //  // check if the cache has the data for key 
    //  if (KeyExists(Personae.FuelReconciliation, $"{siteId}"))
    //  {

    //    return;
    //  }
    //}
    // get the site details from DB
    // serialize it and save it in cache

    
    //}

    //public bool KeyExists(Personae persona, string sub)
    //{
    //  var key = GetCacheKey(persona, sub);

    //  if (EsoCacheManager.Haskey(key))
    //  {
    //    var data = EsoCacheManager.GetValue<UserData>(key);
    //    _CurrentState = GetSessionData(data);
    //    // Since key is found with this sited
    //    _CurrentState.CurrentHierarchyId = GetSiteIdFromRequest();
    //    return true;
    //  }

    //  return false;
    //}

    //public void AddToCache(Personae persona, string sub, SessionData sessionData)
    //{
    //  try
    //  {
    //    var key = GetCacheKey(persona, sub);
    //    _CurrentState = sessionData;

    //    if (persona == Personae.FuelReconciliation)
    //    {
    //      UnitOfWorkFactory.Start(_CurrentState);
    //    }

    //    var data = new UserData()
    //    {
    //      DefaultLangId = _CurrentState.DefaultLanguageId,
    //      UserId = _CurrentState.UserId,
    //      SecurityGroups = _CurrentState.SecurityGroups,
    //      SecurityAccessId = _CurrentState.SecurityAccessId
    //    };
    //    EsoCacheManager.SetValue(key, data);
    //  }
    //  catch
    //  {
    //    throw;
    //  }
    //  finally
    //  {
    //    UnitOfWorkFactory.Dispose();
    //  }

    //}

    //public static void Dispose()
    //{
    //  EsoCacheManager.Dispose();
    //}

    //public SessionData GetCurrentState()
    //{
    //  return _CurrentState;
      
    //}

    //private static string GetCacheKey(Personae persona, string sub)
    //{
    //  if (persona == Personae.None)
    //    return string.Empty;

    //  if (persona != Personae.EsoSmApi)
    //  {
    //    var siteId = GetSiteIdFromRequest();

    //    if (siteId != 0)
    //    {
    //      return string.Format("{0}.{1}.{2}", persona, sub, siteId);
    //    }

    //    return string.Empty;
    //  }
    //  else
    //  {
    //    return string.Format("{0}.{1}", persona, sub);
    //  }
    //}

    //private static int GetSiteIdFromRequest()
    //{
    //  var siteId = 0;

    //  //var siteIdValue = HttpContextHelper.Current.GetQueryString("siteId");

    //  //if (!string.IsNullOrEmpty(siteIdValue))
    //  //{
    //  //  siteId = Convert.ToInt32(siteIdValue, CultureInfo.InvariantCulture);
    //  //}

    //  return siteId;
    //}

    //private static SessionData GetSessionData(IUserData data)
    //{
    //  return new SessionData()
    //  {
    //    UserId = data.UserId,
    //    DefaultLanguageId = data.DefaultLangId,
    //    SecurityGroups = data.SecurityGroups,
    //    SecurityAccessId = data.SecurityAccessId
    //  };
    //}
  }

  public enum Personae
  {
    None,
    FuelReconciliation,
    EsoSmApi
  }

}
