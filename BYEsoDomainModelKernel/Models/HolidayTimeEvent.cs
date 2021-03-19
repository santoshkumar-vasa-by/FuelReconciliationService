namespace BYEsoDomainModelKernel.Models
{
  public class HolidayTimeEvent : TimeEvent
  {
    public HolidayTimeEvent()
    {
      RecurrenceCode = null;
      EventTypeCode = TimeEventTypeCode.Holiday;
    }

    //public override IEnumerable<DateTime> GetOccurrencesForSiteWithinDateRange(Site site, DateTime start, DateTime end,
    //  ICalendarLocatorService calendarLocator)
    //{
    //  var dates = new List<DateTime>();
    //  if (Start != null && (Start.Value >= start && Start.Value <= end))
    //  {
    //    dates.Add(Start.Value);
    //  }

    //  return dates;
    //}

    //public override DateTime? GetFirstOccurrenceForSiteAfterDate(Site site, DateTime date, ICalendarLocatorService calendarLocator)
    //{
    //  return (Start != null && (Start.Value > date)) ? Start.Value : (DateTime?)null;
    //}

    //public override DateTime? GetFirstOccurrenceForSiteOnOrAfterDate(Site site, DateTime date, ICalendarLocatorService calendarLocator)
    //{
    //  return (Start != null && (Start.Value >= date)) ? Start.Value : (DateTime?)null;
    //}
  }
}
