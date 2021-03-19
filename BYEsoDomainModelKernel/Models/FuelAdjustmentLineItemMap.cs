using FluentNHibernate.Mapping;
using WaveDatabaseMetadata = RP.DatabaseMetadata.WaveDatabaseMetadata;

namespace BYEsoDomainModelKernel.Models
{
  public class FuelAdjustmentLineItemMap : ClassMap<FuelAdjustmentLineItem>
  {
    public FuelAdjustmentLineItemMap()
    {
      var tableMeta = WaveDatabaseMetadata.Fuel_Adjustment_Line_Item;
      this.Table(tableMeta);

      CompositeId()
        .KeyProperty(x => x.LineNumber, tableMeta.Fuel_adjustment_line_item_number)
        .KeyReference(x => x.FuelAdjustment, tableMeta.Fuel_adjustment_id);

      References(x => x.Site).Column(tableMeta.Business_unit_id)
        .Not.Update()
        .Cascade.None();

      References(x => x.FuelAdjustment).Column(tableMeta.Fuel_adjustment_id)
        .Not.Update()
        .Not.Insert()
        .Cascade.None();

      References(x => x.FuelPhysicalTank).Column(tableMeta.Physical_fuel_tank_id)
        .Not.Update()
        .Cascade.None();

      References(x => x.FuelItem).Column(tableMeta.Fuel_inventory_item_id)
        .Not.Update()
        .Cascade.None();

      Map(x => x.Volume).Column(tableMeta.Net_volume);
    }
  }
}