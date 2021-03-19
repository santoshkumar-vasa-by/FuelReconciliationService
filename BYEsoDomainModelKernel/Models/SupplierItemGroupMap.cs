using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Mapping;
using RP.DatabaseMetadata;
using RP.DomainModelKernel.Common.Types;

namespace BYEsoDomainModelKernel.Models
{
  public partial class SupplierItemGroup
  {
    public static class MapExpressions
    {
      public static readonly Expression<Func<SupplierItemGroup, IEnumerable<SupplierItemGroupOverride>>>
        Overrides = x => x._Overrides;
    }
  }

  public class SupplierItemGroupMap : ClassMap<SupplierItemGroup>
  {
    public SupplierItemGroupMap()
    {
      var tableMeta = WaveDatabaseMetadata.Supplier_Item_Category;
      this.Table(tableMeta);
      BatchSize(100);

      Id(x => x.ID).Column(tableMeta.Supplier_item_category_id)
                   .GeneratedBy.Custom<TicketGenerator>();

      Map(x => x.Name).Column(tableMeta.Name);
      Map(x => x.IsException).Column(tableMeta.Exception_flag).CustomType<LcaseYesNo>();
      Map(x => x.MaxOrderAmount).Column(tableMeta.Max_order_amt);

      References(x => x.Supplier)
        .Column(tableMeta.Supplier_id)
        .Not.Update()
        .Cascade.None();

      HasMany(SupplierItemGroup.MapExpressions.Overrides)
        .KeyColumn(tableMeta.Supplier_item_category_id)
        .BatchSize(100)
        .Cascade.All();
    }
  }
}
