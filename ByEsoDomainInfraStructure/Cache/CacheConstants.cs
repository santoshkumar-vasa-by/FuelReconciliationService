using System;
using System.Collections.Generic;
using System.Text;

namespace ByEsoDomainInfraStructure.Cache
{
  public class CacheConstants
  {
    public const string CacheConnectivityErrorMessage =
      "Exception occurred while operating on Azure Redis Cache after retrying, in the method : {0}. Falling back to database call. Exception Message: {1} ";
    public static string ConfigInvalidWarnMessage = "Configuration method, {0} returned error. Considering the default value, {1}. Error Message - ";
  }
}
