using System;
using System.Collections.Generic;
using System.Text;

namespace ByEsoDomainInfraStructure.Cache
{
  public static class CacheConfiguration
  {
    //private static readonly Logger _Logger = new Logger(typeof(CacheConfiguration));
    public static int CacheOperationExceptionsAllowedBeforeBreaking { get; private set; }
    public static int CacheOperationRetryAttempts { get; private set; }
    public static int CacheCircuitBreakerDuration { get; private set; }
    public static TimeSpan CacheExpiryInHours { get; private set; }
    public static int MaxPoolSize { get; private set; }
    public static int MedianFirstRetryDelayInMilliseconds { get; private set; }
    public static int CacheConnectionRetryAttempts { get; private set; }
    public static int CacheDeltaBackoffInMilliSeconds { get; private set; }
    public static int CacheMaxDeltaBackOffInMilliseconds { get; private set; }
    public static int CacheConnectionTimeoutInMilliSeconds { get; private set; }

    public static void SetCacheOperationRetryAttemptsValue(string configValue)
    {
      CacheOperationRetryAttempts = GetConfigValue(2, nameof(SetCacheOperationRetryAttemptsValue), configValue.ToInt32);
    }

    public static void SetExceptionsAllowedBeforeBreakingValue(string configValue)
    {
      CacheOperationExceptionsAllowedBeforeBreaking = GetConfigValue(2, nameof(SetExceptionsAllowedBeforeBreakingValue), configValue.ToInt32);
    }

    public static void SetCacheConnectionMaxPoolSizeValue(string configValue)
    {
      MaxPoolSize = GetConfigValue(2, nameof(SetCacheConnectionMaxPoolSizeValue), configValue.ToInt32);
    }
    public static void SetCacheConnectionTimeoutValue(string configValue)
    {
      CacheConnectionTimeoutInMilliSeconds = GetConfigValue(2, nameof(SetCacheConnectionTimeoutValue), configValue.ToInt32);
    }

    public static void SetDurationOfCircuitBreakValue(string configValue)
    {
      CacheCircuitBreakerDuration = GetConfigValue(1, nameof(SetDurationOfCircuitBreakValue), configValue.ToInt32);
    }

    public static bool GetCacheEnabledValue(string configValue)
    {
      return GetConfigValue(false, nameof(GetCacheEnabledValue), configValue.ToTrueOrFalse, false);
    }

    public static void SetCacheExpiryInHoursValue(string configValue)
    {
      CacheExpiryInHours = GetConfigValue(TimeSpan.FromHours(1), nameof(SetCacheExpiryInHoursValue), configValue.ToTimeSpanHours);
    }

    public static void SetMedianFirstRetryDelayValue(string configValue)
    {
      MedianFirstRetryDelayInMilliseconds = GetConfigValue(500, nameof(SetMedianFirstRetryDelayValue),
        configValue.ToInt32);
    }

    public static void SetCacheConnectionRetryAttemptsValue(string configValue)
    {
      CacheConnectionRetryAttempts = GetConfigValue(3, nameof(SetCacheConnectionRetryAttemptsValue), configValue.ToInt32);
    }

    public static void SetCacheDeltaBackoffInMilliSecondsValue(string configValue)
    {
      CacheDeltaBackoffInMilliSeconds = GetConfigValue(10, nameof(SetCacheDeltaBackoffInMilliSecondsValue),
        configValue.ToInt32);
    }

    public static void SetCacheMaxDeltaBackOffInMillisecondsValue(string configValue)
    {
      CacheMaxDeltaBackOffInMilliseconds = GetConfigValue(500, nameof(SetCacheMaxDeltaBackOffInMillisecondsValue),
        configValue.ToInt32);
    }

    internal static T GetConfigValue<T>(T defaultValue, string callingMethod, Func<T> func,
      bool logError = true)
    {
      try
      {
        return func();
      }
      catch (Exception e)
      {
        return LogErrorAndReturnDefaultValue(defaultValue, callingMethod, logError,
          CacheConstants.ConfigInvalidWarnMessage + e.Message);
      }
    }

    private static T LogErrorAndReturnDefaultValue<T>(T defaultValue, string callingMethod, bool logError,
      string errorMessage)
    {
      if (logError)
        string.Format(errorMessage, callingMethod,
         defaultValue.ToString());
        return defaultValue;
    }
  }
}
