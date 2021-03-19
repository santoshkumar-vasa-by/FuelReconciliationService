using System;
using System.Collections.Generic;


namespace BYEsoDomainModelKernel.Models.Common
{
  public class TimeZone : BaseAuditEntity
  {
    public virtual int ID { get; set; }
    public virtual decimal OffsetValue { get; set; }
    public virtual string TimeZoneName { get; set; }
    public virtual IList<TimeZoneYear> TimeZoneYearList { get; set; }
    public virtual IList<TimeZoneDefinitionDea> TimeZoneDefinitionDeaList { get; set; }
    public virtual bool ParticipatesInDst { get; set; }

    protected internal TimeZone()
    {
      TimeZoneYearList = new List<TimeZoneYear>();
      TimeZoneDefinitionDeaList = new List<TimeZoneDefinitionDea>();
    }

    public TimeZone(decimal offsetValue, string timeZoneName, bool participatesInDstFlag)
      : this()
    {
      OffsetValue = offsetValue;
      TimeZoneName = timeZoneName;
      ParticipatesInDst = participatesInDstFlag;
    }

    public virtual TimeZoneYear AddTimeZoneYear(int yearID, DateTime dstStart, DateTime dstEnd)
    {
      var newTimeZoneYear = new TimeZoneYear(this, yearID, dstStart, dstEnd);

      TimeZoneYearList.Add(newTimeZoneYear);

      return newTimeZoneYear;
    }

    public virtual TimeZoneDefinitionDea AddTimeZoneDefinitionDea(TimeZoneDefinition timeZoneDefinition, 
      DateTime dstStart)
    {
      var timeZoneDefinitionDea = new TimeZoneDefinitionDea(timeZoneDefinition, dstStart, this);

      TimeZoneDefinitionDeaList.Add(timeZoneDefinitionDea);

      return timeZoneDefinitionDea;
    }
  }
}