using FluentNHibernate.Mapping;
using RP.DatabaseMetadata;

namespace BYEsoDomainModelKernel.Models.Common
{
  public class TimeZoneYearMap : ClassMap<TimeZoneYear>
  {
    public TimeZoneYearMap()
    {
      var tableMeta = WaveDatabaseMetadata.Time_Zone_Year;

      this.Table(tableMeta);
      CompositeId()
        .KeyReference(x => x.TimeZone, tableMeta.Time_zone_id)
        .KeyProperty(x => x.YearID, tableMeta.Year_id);

      Map(x => x.DstStartDate).Column(tableMeta.Dst_start_date);
      Map(x => x.DstEndDate).Column(tableMeta.Dst_end_date);
    }
  }
}
