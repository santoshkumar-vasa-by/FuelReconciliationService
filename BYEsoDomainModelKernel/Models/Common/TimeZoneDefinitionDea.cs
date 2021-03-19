using System;

namespace BYEsoDomainModelKernel.Models.Common
{
  public class TimeZoneDefinitionDea : BaseAuditEntity
  {
    public virtual TimeZoneDefinition TimeZoneDefinition { get; set; }
    public virtual DateTime StartDate { get; set; }
    public virtual TimeZone TimeZone { get; set; }

    protected internal TimeZoneDefinitionDea()
    {

    }

    public TimeZoneDefinitionDea(TimeZoneDefinition timeZoneDefinition, DateTime startDate, 
      TimeZone timeZone) : this()
    {
      TimeZoneDefinition = timeZoneDefinition;
      StartDate = startDate;
      TimeZone = timeZone;
    }

    public override bool Equals(object obj)
    {
      var tzdd = obj as TimeZoneDefinitionDea;
      if (tzdd == null)
      {
        return false;
      }
      return TimeZoneDefinition == tzdd.TimeZoneDefinition && StartDate == tzdd.StartDate;
    }

    public override int GetHashCode()
    {
      return TimeZoneDefinition.GetHashCode() ^ StartDate.GetHashCode();
    }
  }
}
