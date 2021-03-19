using FluentNHibernate.Mapping;
using WaveDatabaseMetadata = RP.DatabaseMetadata.WaveDatabaseMetadata;
using RP.DomainModelKernel.Common.Types;

namespace BYEsoDomainModelKernel.Models
{
  public class FuelTankMap : ClassMap<FuelTank>
  {
    public FuelTankMap()
    {
      var tableMeta = WaveDatabaseMetadata.Fuel_Tank;
      this.Table(tableMeta);
      this.BatchSize(100);

      Id(x => x.ID).Column(tableMeta.Fuel_tank_id)
        .GeneratedBy.Custom<TicketGenerator>();

      References(x => x.Site)
        .Column(tableMeta.Business_unit_id)
        .Not.Update();

      References(x => x.FuelItem)
        .Column(tableMeta.Fuel_inventory_item_id);

      Map(x => x.IsActive).Column(tableMeta.Active_flag).CustomType<LcaseYesNo>();

      string discriminatorFormula = @"(
        CASE WHEN EXISTS (  select 1
                            from fuel_manifold_tank fmt
                            where fmt.manifold_fuel_tank_id = fuel_tank_id) THEN
          'M'
        ELSE
          'P'
        END)";

      DiscriminateSubClassesOnColumn(tableMeta.Fuel_tank_id.ColumnName)
        .Formula(discriminatorFormula);
    }
  }
}