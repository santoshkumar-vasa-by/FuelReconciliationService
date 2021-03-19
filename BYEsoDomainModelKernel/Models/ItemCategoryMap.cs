using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Mapping;
using RP.DatabaseMetadata;
using RP.DomainModelKernel.Common.Types;

namespace BYEsoDomainModelKernel.Models
{
  public partial class ItemCategory
  {
    public static class MapExpressions
    {
      public static readonly Expression<Func<ItemCategory, IEnumerable<ItemCategory>>> _Children =
        x => x._Children;
      //public static readonly Expression<Func<ItemCategory, IEnumerable<ItemCategorySiteOverride>>> _ItemCategorySiteOverrides =
      //  x => x._ItemCategorySiteOverrides;
    }
  }

  public class ItemCategoryMap : ClassMap<ItemCategory>
  {
    public ItemCategoryMap()
    {
      var tableMeta = WaveDatabaseMetadata.Item_Hierarchy;
      this.Table(tableMeta);
      this.BatchSize(100);

      Id(x => x.ID).Column(tableMeta.Item_hierarchy_id)
        .GeneratedBy.Custom<TicketGenerator>();
      Map(x => x.Name).Column(tableMeta.Name);
      Map(x => x.Description).Column(tableMeta.Description);

      Map(x => x.IsReclamationItemCategory).Column(tableMeta.Reclamation_flag)
        .CustomType<LcaseYesNo>();

      Map(x => x.IsFuelItemCategory).Column(tableMeta.Fuel_flag)
      .CustomType<LcaseYesNo>();

      Map(x => x.UseInvoiceCostOnDeliveriesItemCategory).Column(tableMeta.Use_invoice_cost_on_deliveries_flag)
        .CustomType<LcaseYesNo>();

      Map(x => x.IsPurged).Column(tableMeta.Purge_flag).CustomType<LcaseYesNo>();

      //Map(x => x.DefaultValuationMethod).Column(tableMeta.Valuation_method_code)
      //  .CustomType(typeof(EnumCharType<ValuationMethod>));

      Map(x => x.OwnerID).Column(tableMeta.Cdm_owner_id);
      Map(x => x.RestrictBUEditFlag).Column(tableMeta.Restrict_bu_edit_flag).CustomType<LcaseYesNo>();

      References(x => x.Parent)
        .Column(tableMeta.Parent_item_hierarchy_id)
        .Cascade.None();

      //References(x => x.Level)
      //  .Column(tableMeta.Item_hierarchy_level_id)
      //  .Cascade.None();

      HasMany(ItemCategory.MapExpressions._Children)
        .KeyColumn(tableMeta.Parent_item_hierarchy_id)
        .BatchSize(100)
        .Cascade.All();
      
      //HasMany(ItemCategory.MapExpressions._ItemCategorySiteOverrides)
      //  .KeyColumn(tableMeta.Item_hierarchy_id)
      //  .BatchSize(100)
      //  .Cascade.All();
    }
  }
}