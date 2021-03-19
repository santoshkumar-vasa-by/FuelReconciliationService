using FluentNHibernate.Mapping;
using RP.DatabaseMetadata;
using RP.DomainModelKernel.Common.Types;

namespace BYEsoDomainModelKernel.Models.Common
{
  public class TimeZoneMap : ClassMap<TimeZone>
  {
    public TimeZoneMap()
    {
      var tableMeta = WaveDatabaseMetadata.Time_Zone;
      BatchSize(100);

      this.Table(tableMeta);
      Id(x => x.ID).Column(tableMeta.Time_zone_id).GeneratedBy.Custom<TicketGenerator>();

      Map(x => x.OffsetValue).Column(tableMeta.Gmt_offset_value);
      Map(x => x.TimeZoneName).Column(tableMeta.Ms_name);
      Map(x => x.ParticipatesInDst).Column(tableMeta.Participates_in_dst_flag).CustomType<LcaseYesNo>();

      HasMany(x => x.TimeZoneYearList)
        .KeyColumn(tableMeta.Time_zone_id)
        .Cascade.All();

      HasMany(x => x.TimeZoneDefinitionDeaList)
        .KeyColumn(tableMeta.Time_zone_id)
        .Cascade.None();
    }
  }
}
