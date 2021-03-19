using FluentNHibernate.Mapping;
using RP.DatabaseMetadata;

namespace BYEsoDomainModelKernel.Models
{
  public class SupplierItemGroupOverrideMap : ClassMap<SupplierItemGroupOverride>
  {
    public SupplierItemGroupOverrideMap()
    {
      var tableMeta = WaveDatabaseMetadata.Supplier_Item_Category_Override;
      this.Table(tableMeta);
      BatchSize(100);

      CompositeId()
        .KeyReference(x => x.SupplierItemGroup, tableMeta.Supplier_item_category_id)
        .KeyReference(x => x.OrganizationalHierarchy, tableMeta.Org_hierarchy_id);

      Map(x => x.MaxOrderAmount).Column(tableMeta.Max_order_amt);
    }
  }
}
