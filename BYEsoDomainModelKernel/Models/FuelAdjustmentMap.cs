using FluentNHibernate.Mapping;
using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using NHibernate.Type;
using WaveDatabaseMetadata = RP.DatabaseMetadata.WaveDatabaseMetadata;


namespace BYEsoDomainModelKernel.Models
{
  public partial class FuelAdjustment
  {
    public static class MapExpressions
    {
      public static readonly Expression<Func<FuelAdjustment, IEnumerable<FuelAdjustmentLineItem>>>
        LineItems = x => x.LineItems;
    }
  }

  public class FuelAdjustmentMap : ClassMap<FuelAdjustment>
  {
    public FuelAdjustmentMap()
    {
      var tableMeta = WaveDatabaseMetadata.Fuel_Adjustment;
      this.Table(tableMeta);
      this.BatchSize(100);

      Id(x => x.ID).Column(tableMeta.Fuel_adjustment_id)
        .GeneratedBy.Custom<TicketGenerator>();

      References(x => x.Site)
        .Column(tableMeta.Business_unit_id)
        .Not.Update();

      References(x => x.FuelDelivery).Column(tableMeta.Fuel_delivery_id)
        .Not.Update()
        .Cascade.None();

      Map(x => x.BusinessDate).Column(tableMeta.Business_date);
      Map(x => x.AdjustmentDate).Column(tableMeta.Adjustment_timestamp);
      Map(x => x.Status).Column(tableMeta.Status_code)
        .CustomType<EnumCharType<FuelAdjustmentStatus>>();
      Map(x => x.FuelAdjustmentType).Column(tableMeta.Adjustment_type_code)
        .CustomType<EnumCharType<FuelAdjustmentType>>();
      Map(x => x.Remarks).Column(tableMeta.Remarks);
      Map(x => x.IsLocked).Column(tableMeta.Locked_flag)
        .CustomType<EnumCharType<FuelAdjustmentLockedFlagStatus>>();
      HasMany(FuelAdjustment.MapExpressions.LineItems)
       .KeyColumn(tableMeta.Fuel_adjustment_id)
       .BatchSize(100)
       .Inverse()
       .Cascade.AllDeleteOrphan();
    }
  }
}