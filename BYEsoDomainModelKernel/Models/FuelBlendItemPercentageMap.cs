using FluentNHibernate.Mapping;
using WaveDatabaseMetadata = RP.DatabaseMetadata.WaveDatabaseMetadata;

namespace BYEsoDomainModelKernel.Models
{
  public class FuelBlendedItemPercentagesMap : ClassMap<FuelBlendItemPercentage>
  {
    public FuelBlendedItemPercentagesMap()
    {
      var tableMeta = WaveDatabaseMetadata.Fuel_Blend_Percentage;
      this.Table(tableMeta);

      CompositeId()
        .KeyReference(x => x.FuelBlendedItem, tableMeta.Fuel_blended_item_id)
        .KeyReference(x => x.FuelItem, tableMeta.Fuel_inventory_item_id);

      References(x => x.FuelBlendedItem).Column(tableMeta.Fuel_blended_item_id)
        .Not.Update()
        .Not.Insert()
        .Cascade.None();

      //References(x => x.FuelItem).Column(tableMeta.Fuel_inventory_item_id)
      //  .Not.Update()
      //  .Not.Insert()
      //  .Cascade.None();

      var columnNameToGetAroundIncorrectMetaDataName = "fuel_blend_percentage";
      Map(x => x.FuelBlendPercentage).Column(columnNameToGetAroundIncorrectMetaDataName);
    }
  }
}