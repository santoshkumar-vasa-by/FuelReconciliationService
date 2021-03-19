using FluentNHibernate.Mapping;
using RP.DatabaseMetadata;

namespace BYEsoDomainModelKernel.Models.Common
{
  public class TimeZoneDefinitionDeaMap : ClassMap<TimeZoneDefinitionDea>
  {
    public TimeZoneDefinitionDeaMap()
    {
      var tableMeta = WaveDatabaseMetadata.Time_Zone_Defn_Time_Zone_DEA;
      BatchSize(100);

      this.Table(tableMeta);

      CompositeId()
       .KeyReference(x => x.TimeZoneDefinition, tableMeta.Time_zone_defn_id)
       .KeyProperty(x => x.StartDate, tableMeta.Start_date);

      References(x => x.TimeZone)
        .Column(tableMeta.Time_zone_id)
        .Not.Update()
        .Not.Insert()
        .Cascade.None();

      References(x => x.TimeZoneDefinition)
        .Column(tableMeta.Time_zone_defn_id)
        .Not.Insert()
        .Not.Update()
        .Cascade.None();
    }
  }
}
