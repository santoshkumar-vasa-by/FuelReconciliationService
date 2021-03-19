using FluentNHibernate.Mapping;
using WaveDatabaseMetadata = RP.DatabaseMetadata.WaveDatabaseMetadata;

namespace BYEsoDomainModelKernel.Models
{
  public class CostLevelMap : ClassMap<CostLevel>
  {
    public CostLevelMap()
    {
      var tableMeta = WaveDatabaseMetadata.Merch_Cost_Level;
      this.Table(tableMeta);

      Id(x => x.ID).Column(tableMeta.Merch_cost_level_id)
                   .GeneratedBy.Custom<TicketGenerator>();

      Map(x => x.Name).Column(tableMeta.Name);
      Map(x => x.DefaultRanking).Column(tableMeta.Default_ranking);

      References(x => x.Supplier)
        .Column(tableMeta.Supplier_id)
        .Not.Update()
        .Cascade.None();
    }
  }
}
