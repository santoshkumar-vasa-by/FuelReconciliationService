using System;
using System.Globalization;

namespace BYEsoDomainModelKernel.Models
{
  public class TimeEvent : BaseAuditEntity
  {
    #region Constants
    public static readonly string First = TimeEventRecurrenceWeekOfMonth.First.ToString().ToUpper(CultureInfo.InvariantCulture);
    public static readonly string Second = TimeEventRecurrenceWeekOfMonth.Second.ToString().ToUpper(CultureInfo.InvariantCulture);
    public static readonly string Third = TimeEventRecurrenceWeekOfMonth.Third.ToString().ToUpper(CultureInfo.InvariantCulture);
    public static readonly string Fourth = TimeEventRecurrenceWeekOfMonth.Fourth.ToString().ToUpper(CultureInfo.InvariantCulture);
    public static readonly string Last = TimeEventRecurrenceWeekOfMonth.Last.ToString().ToUpper(CultureInfo.InvariantCulture);

    public static readonly string Monthly = TimeEventRecurrenceCode.Monthly.ToString().ToUpper(CultureInfo.InvariantCulture);
    public static readonly string Daily = TimeEventRecurrenceCode.Daily.ToString().ToUpper(CultureInfo.InvariantCulture);
    public static readonly string Weekly = TimeEventRecurrenceCode.Weekly.ToString().ToUpper(CultureInfo.InvariantCulture);
    public static readonly string Custom = TimeEventRecurrenceCode.Custom.ToString().ToUpper(CultureInfo.InvariantCulture);

    public static readonly string FullDay = ForecastedBufferHours.FullDay.ToString().ToUpper(CultureInfo.InvariantCulture);
    public static readonly string HalfDay = ForecastedBufferHours.HalfDay.ToString().ToUpper(CultureInfo.InvariantCulture);
    public static readonly string None = ForecastedBufferHours.None.ToString().ToUpper(CultureInfo.InvariantCulture);

    public static readonly string Monday = DayOfWeek.Monday.ToString().ToUpper(CultureInfo.InvariantCulture);
    public static readonly string Tuesday = DayOfWeek.Tuesday.ToString().ToUpper(CultureInfo.InvariantCulture);
    public static readonly string Wednesday = DayOfWeek.Wednesday.ToString().ToUpper(CultureInfo.InvariantCulture);
    public static readonly string Thursday = DayOfWeek.Thursday.ToString().ToUpper(CultureInfo.InvariantCulture);
    public static readonly string Friday = DayOfWeek.Friday.ToString().ToUpper(CultureInfo.InvariantCulture);
    public static readonly string Saturday = DayOfWeek.Saturday.ToString().ToUpper(CultureInfo.InvariantCulture);
    public static readonly string Sunday = DayOfWeek.Sunday.ToString().ToUpper(CultureInfo.InvariantCulture);
    #endregion

    public virtual int ID { get; set; }
    public virtual DateTime? Start { get; set; }
    public virtual string Name { get; set; }
    public virtual RecurringFlag? RecurringFlag { get; set; }
    public virtual TimeEventRecurrenceThresholdScheduleCode? TimeRecurrenceCode { get; set; }
    public virtual TimeEventRecurrenceCode? RecurrenceCode { get; set; }
    public virtual int? RecurrenceFrequency { get; set; }
    public virtual TimeEventRecurrenceWeekOfMonth? RecurrenceWeekOfTheMonth { get; set; }
    public virtual int? DayOfMonth { get; set; }
    public virtual TimeSpan? StartTime { get; set; }
    public virtual TimeEventTypeCode EventTypeCode { get; set; }
    //public virtual IEnumerable<DateTime> GetOccurrencesForSiteWithinDateRange(Site site, DateTime start, DateTime end,
    //  ICalendarLocatorService calendarLocator)
    //{
    //  return new List<DateTime>();
    //}

    //public virtual DateTime? GetFirstOccurrenceForSiteAfterDate(Site site, DateTime date, ICalendarLocatorService calendarLocator)
    //{
    //  return null;
    //}

    //public virtual DateTime? GetFirstOccurrenceForSiteOnOrAfterDate(Site site, DateTime date, ICalendarLocatorService calendarLocator)
    //{
    //  return null;
    //}

    public override bool Equals(object other)
    {
      var ot = other as TimeEvent;
      if (ot == null)
      {
        return false;
      }
      return ot.ID == ID;
    }

    public override int GetHashCode()
    {
      return ID.GetHashCode();
    }
  }

  public enum TimeEventRecurrenceCode
  {
    DayOfWeek = 'a',
    Custom = 'c',
    Daily = 'd',
    Monthly = 'm',
    UnknownO = 'o',
    Weekly = 'w',
    YearlyMaybe = 'y',
    FinancialPeriod = 'f',
    Holiday = 'h'
  }
  public enum TimeEventRecurrenceThresholdScheduleCode
  {
    TimeRecurrence = 't',
    TimeRecurrenceOnce = 'o',
    TimeRecurrenceHour = 'h',
    TimeRecurrenceMinute = 'm'
  }

  public enum TimeEventTypeCode
  {
    Order = 'o',
    Delivery = 'd',
    Holiday = 'h'
  }

  public enum TimeEventRecurrenceWeekOfMonth
  {
    First = '1',
    Second = '2',
    Third = '3',
    Fourth = '4',
    Last = '5'
  }
  public enum RecurringFlag
  {
    Yes = 'y',
    No = 'n'
  }

  public enum ForecastedBufferHours
  {
    None = 0,
    HalfDay = 12,
    FullDay = 24
  }

}
