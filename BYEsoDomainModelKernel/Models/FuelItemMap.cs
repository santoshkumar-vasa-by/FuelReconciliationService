using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Mapping;
using WaveDatabaseMetadata = RP.DatabaseMetadata.WaveDatabaseMetadata;
using RP.DomainModelKernel.Common;

namespace BYEsoDomainModelKernel.Models
{
  public partial class FuelItem
  {
    public static class MapExpressions
    {
      public static readonly Expression<Func<FuelItem, IEnumerable<FuelBlendItemPercentage>>>
        _BlendedPercentages = x => x._BlendedPercentages;
    }
  }

  public class FuelItemMap : SubclassMap<FuelItem>
  {
    public FuelItemMap()
    {
      DiscriminatorValue(EnumHelper.ToString<ItemType>(ItemType.Fuel));

      var itemTableMeta = WaveDatabaseMetadata.Item;
      this.Table(itemTableMeta);
      this.BatchSize(100);

      var fuelBlendPercentageMetaData = WaveDatabaseMetadata.Fuel_Blend_Percentage;
      HasMany(FuelItem.MapExpressions._BlendedPercentages)
       .KeyColumn(fuelBlendPercentageMetaData.Fuel_blended_item_id)
       .BatchSize(100)
       .Inverse()
       .Cascade.AllDeleteOrphan();
    }
  }
}