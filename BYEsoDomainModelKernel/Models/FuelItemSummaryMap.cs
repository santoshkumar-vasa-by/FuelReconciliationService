using FluentNHibernate.Mapping;
using WhDatabaseMetadata = RP.DatabaseMetadata.WhDatabaseMetadata;

namespace BYEsoDomainModelKernel.Models
{
  public class FuelItemSummaryMap: ClassMap<FuelItemSummary>
  {
    public FuelItemSummaryMap()
    {
      var tableMeta = WhDatabaseMetadata.F_pcs_fuel_item_summary_bu_day;
      this.Table(tableMeta);
      this.UseWarehouseSchema();

      CompositeId()
        .KeyReference(x => x.Site, tableMeta.Bu_id)
        .KeyReference(x => x.FuelItem, tableMeta.Item_id)
        .KeyProperty(x => x.BusinessDate, tableMeta.Business_date);


      References(x => x.Site).Column(tableMeta.Bu_id)
        .Not.Update()
        .Not.Insert()
        .Cascade.None();

      References(x => x.FuelItem).Column(tableMeta.Item_id)
        .Not.Update()
        .Not.Insert()
        .Cascade.None();

      References(x => x.ItemCategory).Column(tableMeta.Item_hierarchy_id)
        .Not.Update()
        .Cascade.None();
      
      Map(x => x.OpeningWac).Column(tableMeta.Opening_unit_weighted_average_cost).Scale(10);
      Map(x => x.ClosingWac).Column(tableMeta.Closing_unit_weighted_average_cost).Scale(10);
      Map(x => x.Sales).Column(tableMeta.Sales_qty).Scale(6);
      Map(x => x.EndOnHandQuantity).Column(tableMeta.End_onhand_qty).Scale(6);
      Map(x => x.ItemVarianceQuantity).Column(tableMeta.Fuel_item_variance_qty).Scale(6);
      Map(x => x.PumpTestVolumeReturned).Column(tableMeta.Pump_test_volume_returned);
      Map(x => x.PumpTestVolumeNotReturned).Column(tableMeta.Pump_test_volume_non_returned);
    }
  }
}
