using System;
using System.Collections.Generic;
using System.Text;

namespace ByEsoDomainInfraStructure.Cache
{
  public static class StringExtensions
  {
    public static int ToInt32(this string input)
    {
      if (int.TryParse(input, out var intValue))
        return intValue;

      throw new FormatException($"Input is not correctly formed- method name: {nameof(ToInt32)}");
    }

    public static bool ToTrueOrFalse(this string input)
    {
      if (bool.TryParse(input, out var boolValue))
        return boolValue;

      throw new FormatException($"Input is not correctly formed- method name: {nameof(ToTrueOrFalse)}");
    }

    public static TimeSpan ToTimeSpanHours(this string input)
    {
      if (int.TryParse(input, out var timeInHours))
      {
        var ts = TimeSpan.FromHours(timeInHours);
        return ts;
      }

      throw new FormatException($"Input is not correctly formed- method name: {nameof(ToTimeSpanHours)}");
    }

    public static TimeSpan ToTimeSpanMilliseconds(this string input)
    {
      if (int.TryParse(input, out var timeInMilliseconds))
      {
        var ts = TimeSpan.FromMilliseconds(timeInMilliseconds);
        return ts;
      }

      throw new FormatException($"Input is not correctly formed- method name: {nameof(ToTimeSpanMilliseconds)}");
    }
  }
}
