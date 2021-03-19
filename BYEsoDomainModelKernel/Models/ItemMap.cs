using System;
using FluentNHibernate.Mapping;
using WaveDatabaseMetadata = RP.DatabaseMetadata.WaveDatabaseMetadata;
using RP.DomainModelKernel.Common.Types;

namespace BYEsoDomainModelKernel.Models
{
  public partial class Item
  {
    public static class MapExpressions<T> where T : Item
    {
      //public static readonly Expression<Func<T, IEnumerable<ItemUnitOfMeasureConversion>>>
      //  _UnitOfMeasureConversions = x => x._UnitOfMeasureConversions;
      //public static readonly Expression<Func<T, IEnumerable<SalesInfo>>>
      //  _SalesInfo = x => x._SalesInfo;
    }
  }

  public class ItemMap : ClassMap<Item>
  {
    public ItemMap()
    {
      var itemTableMeta = WaveDatabaseMetadata.Item;
      this.Table(itemTableMeta);
      this.BatchSize(200);    

      Id(x => x.ID).Column(itemTableMeta.Item_id)
        .GeneratedBy.Assigned();
      Map(x => x.Name).Column(itemTableMeta.Name);
      Map(x => x.Description).Column(itemTableMeta.Description);
      Map(x => x.IsTracked).Column(itemTableMeta.Track_flag).CustomType<LcaseYesNo>();
      Map(x => x.ExternalID).Column(itemTableMeta.Xref_code);
      Map(x => x.IsRecipe).Column(itemTableMeta.Recipe_flag).CustomType<LcaseYesNo>();
      //Map(x => x.Type).Column(itemTableMeta.Item_type_code)
      //  .CustomType<EnumCharType<ItemType>>();
      Map(x => x.IsRecalled).Column(itemTableMeta.Recall_flag).CustomType<LcaseYesNo>();
      Map(x => x.IsPurged).Column(itemTableMeta.Purge_flag).CustomType<LcaseYesNo>();
      Map(x => x.ShelfLabelQuantity).Column(itemTableMeta.Shelf_label_quantity);

      References(x => x.Category)
        .Column<ItemCategory>(itemTableMeta.Item_hierarchy_id)
        .Cascade.None();
      //References(x => x.UnitOfMeasureClass)
      //  .Column<UnitOfMeasureClass>(itemTableMeta.Unit_of_measure_class_id)
      //  .Cascade.None();

      var itemDAListMetadata = WaveDatabaseMetadata.Item_DA_Effective_Date_List;
      HasMany(x => x.ItemDataAccessorAssignments)
        .KeyColumn<ItemDataAccessorAssignment>(itemDAListMetadata.Item_id)
        .BatchSize(100)
        .Inverse()
        .Cascade.AllDeleteOrphan();

      //var itemLanguageTranslationMetadata = WaveDatabaseMetadata.Item_Alternate_Lang;
      //HasMany(x => x.ItemLanguageTranslations)
      //  .KeyColumn<ItemLanguageTranslation>(itemLanguageTranslationMetadata.Item_id)
      //  .BatchSize(100)
      //  .Inverse()
      //  .Cascade.AllDeleteOrphan();

      //var itemUOMConversionMetadata = WaveDatabaseMetadata.Item_UOM_Conversion;
      //HasMany(Item.MapExpressions<Item>._UnitOfMeasureConversions)
      //  .KeyColumn(itemUOMConversionMetadata.Item_id)
      //  .BatchSize(1000)
      //  .Inverse()
      //  .Cascade.AllDeleteOrphan();

      //var retailItemMetadata = WaveDatabaseMetadata.Retail_Item;
      //HasMany(Item.MapExpressions<Item>._SalesInfo)
      //  .KeyColumn(retailItemMetadata.Retail_item_id)
      //  .BatchSize(1000)
      //  .Inverse()
      //  .Cascade.AllDeleteOrphan();

      //var itemReviewStepItemOrgHierarchyTableMeta = WaveDatabaseMetadata.Item_Review_Step_Item_Org_Hierarchy_List;
      //HasMany(x => x.ItemReviewStepItemOrgHierarchyList)
      //  .KeyColumn<ItemReviewStepItemOrgHierarchy>(itemReviewStepItemOrgHierarchyTableMeta.Item_id)
      //  .BatchSize(100)
      //  .Inverse()
      //  .Cascade.AllDeleteOrphan();

      //References(x => x.SalesInfo)
      //  .Column<SalesInfo>(itemTableMeta.Item_id)
      //  .Not.Insert()
      //  .Not.Update()
      //  .Access.NoOp();

      string discriminatorFormula = @"(
        CASE WHEN item_type_code = 'f' THEN
          'f'
        WHEN item_type_code = 't' OR item_type_code = 'x' THEN
          'l'
        WHEN recipe_flag = 'y' THEN
          'r'
        ELSE
          'i'
        END)";

      Where(String.Format("{0} IN ('{1}','{2}','{3}','{4}')", itemTableMeta.Item_type_code.ColumnName,
        (char)ItemType.Fuel, (char)ItemType.InventoryItem, (char)ItemType.InstantLotteryTicket, (char)ItemType.MachineLotteryTicket));

      DiscriminateSubClassesOnColumn(itemTableMeta.Item_type_code.ColumnName, "i")
        .Formula(discriminatorFormula);
    }
  }
}