using System;

namespace BYEsoDomainModelKernel.Models.Common
{
  public class TimeZoneYear : BaseAuditEntity
  {
    public virtual TimeZone TimeZone { get; set; }
    public virtual int YearID { get; set; }
    public virtual DateTime DstStartDate { get; set; }
    public virtual DateTime DstEndDate { get; set; }

    protected internal TimeZoneYear()
    {
    }

    protected internal TimeZoneYear(TimeZone timeZone, int yearID, DateTime dstStart, DateTime dstEnd)
    {
      TimeZone = timeZone;
      YearID = yearID;
      DstStartDate = dstStart;
      DstEndDate = dstEnd;
    }

    public override bool Equals(object other)
    {
      var tzy = other as TimeZoneYear;
      if (other == null || tzy == null)
      {
        return false;
      }
      return TimeZone == tzy.TimeZone && YearID == tzy.YearID;
    }

    public override int GetHashCode()
    {
      return TimeZone.GetHashCode() ^ YearID;
    }
  }
}
