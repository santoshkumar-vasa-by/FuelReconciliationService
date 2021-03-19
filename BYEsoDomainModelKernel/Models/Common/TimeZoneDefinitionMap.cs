using FluentNHibernate.Mapping;
using RP.DatabaseMetadata;

namespace BYEsoDomainModelKernel.Models.Common
{
  public class TimeZoneDefinitionMap : ClassMap<TimeZoneDefinition>
  {
    public TimeZoneDefinitionMap()
    {
      var tableMeta = WaveDatabaseMetadata.Time_Zone_Defn;
      BatchSize(100);

      this.Table(tableMeta);

      Id(x => x.Id).Column(tableMeta.Time_zone_defn_id).GeneratedBy.Custom<TicketGenerator>();

      Map(x => x.Name).Column(tableMeta.Name);
    }
  }
}
