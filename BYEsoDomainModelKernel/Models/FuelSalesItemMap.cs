using FluentNHibernate.Mapping;
using RP.DatabaseMetadata;

namespace BYEsoDomainModelKernel.Models
{
  public class FuelSalesItemMap : ClassMap<FuelSalesItem>
  {
    public FuelSalesItemMap()
    {
      var tableMeta = WhDatabaseMetadata.Fuel_sales_item;
      this.Table(tableMeta);
      this.UseWarehouseSchema();
      BatchSize(100);

      Id(x => x.FuelSalesItemID).Column(tableMeta.Fuel_sales_item_id).GeneratedBy.Assigned();

      Map(x => x.GradeId).Column(tableMeta.Grade_id);
      Map(x => x.PricingTierId).Column(tableMeta.Pricing_tier_id);
      Map(x => x.ServiceModeId).Column(tableMeta.Service_mode_id);
      Map(x => x.FuelSalesItemName).Column(tableMeta.Name);
      Map(x => x.BlendItem1Id).Column(tableMeta.Blend_item_1_id);
      Map(x => x.BlendItem1Percentage).Column(tableMeta.Blend_item_1_percentage);
      Map(x => x.BlendItem2Id).Column(tableMeta.Blend_item_2_id);
      Map(x => x.BlendItem2Percentage).Column(tableMeta.Blend_item_2_percentage);
    }
  }
}
