using FluentNHibernate.Mapping;
using RP.DatabaseMetadata;

namespace BYEsoDomainModelKernel.Models
{
  public class PostedSalesItemMap : ClassMap<PostedSalesItem>
  {
    public PostedSalesItemMap()
    {
      var tableMeta = WhDatabaseMetadata.Sales_item;
      this.Table(tableMeta);
      this.UseWarehouseSchema();

      Id(x => x.SalesItemID).Column(tableMeta.Sales_item_id).GeneratedBy.Assigned();
      
      Map(x => x.ItemName).Column(tableMeta.Item_name);

      References(x => x.ItemHierarchy)
        .Column(tableMeta.Item_hierarchy_id);

      References(x => x.Item)
      .Column(tableMeta.Item_id);

    }

  }
}
