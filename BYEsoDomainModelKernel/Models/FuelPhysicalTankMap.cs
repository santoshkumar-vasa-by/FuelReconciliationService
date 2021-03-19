using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;
using System.Linq.Expressions;
using WaveDatabaseMetadata = RP.DatabaseMetadata.WaveDatabaseMetadata;

namespace BYEsoDomainModelKernel.Models
{
  public partial class FuelPhysicalTank
  {
    public static class MapExpressions
    {
      public static readonly Expression<Func<FuelPhysicalTank, IEnumerable<FuelManifoldTankPhysicalTankAssignment>>>
         _ManifoldTankAssignments = x => x._ManifoldTankAssignments;
      public static readonly Expression<Func<FuelPhysicalTank, IEnumerable<FuelTankReadingLineItem>>>
        _FuelTankReadingLineItems = x => x.FuelTankReadingLineItemList;
      public static readonly Expression<Func<FuelPhysicalTank, IEnumerable<FuelAdjustmentLineItem>>>
        _FuelAdjustmentLineItems = x => x.FuelAdjustmentLineItemList;
      public static readonly Expression<Func<FuelPhysicalTank, IEnumerable<FuelDeliveryLineItem>>>
        _FuelDeliveryLineItems = x => x.FuelDeliveryLineItemList;

      public static readonly Expression<Func<FuelPhysicalTank, object>> JoinedClientID = x => x._JoinedClientID;
      public static readonly Expression<Func<FuelPhysicalTank, object>> JoinedLastModifiedUserID = x => x._JoinedLastModifiedUserID;
      public static readonly Expression<Func<FuelPhysicalTank, object>> JoinedLastModifiedTimestamp = x => x._JoinedLastModifiedTimestamp;
    }
  }

  public class FuelPhysicalTankMap : SubclassMap<FuelPhysicalTank>
  {
    public FuelPhysicalTankMap()
    {
      DiscriminatorValue("P");

      var tableMeta = WaveDatabaseMetadata.Fuel_Tank;
      this.Table(tableMeta);
      this.BatchSize(100);

      var fuelPhysicalTankTableMeta = WaveDatabaseMetadata.Fuel_Physical_Tank;
      this.Join(fuelPhysicalTankTableMeta, fpt =>
      {
        fpt.KeyColumn(fuelPhysicalTankTableMeta.Physical_fuel_tank_id);
        fpt.Map(x => x.TankNumber).Column(fuelPhysicalTankTableMeta.Tank_number);
        fpt.Map(x => x.WaterThreshold).Column(fuelPhysicalTankTableMeta.Maximum_water_volume);
        fpt.Map(x => x.LowTankThreshold).Column(fuelPhysicalTankTableMeta.Low_tank_threshold);
        fpt.Map(x => x.VarianceVolumeThreshold).Column(fuelPhysicalTankTableMeta.Daily_variance_volume_threshold);
        fpt.Map(x => x.VarianceBySalesThresholdPercent).Column(fuelPhysicalTankTableMeta.Daily_variance_sales_percent_threshold);
        fpt.Map(x => x.IsAutomaticGauge).Column(fuelPhysicalTankTableMeta.Atg_flag).CustomType<LcaseYesNo>();

        fpt.Map(FuelPhysicalTank.MapExpressions.JoinedClientID).Column(fuelPhysicalTankTableMeta.Client_id);
        fpt.Map(FuelPhysicalTank.MapExpressions.JoinedLastModifiedUserID).Column(fuelPhysicalTankTableMeta.Last_modified_user_id);
        fpt.Map(FuelPhysicalTank.MapExpressions.JoinedLastModifiedTimestamp).Column(fuelPhysicalTankTableMeta.Last_modified_timestamp);

        fpt.References(x => x.FuelTankType).Column(fuelPhysicalTankTableMeta.Fuel_tank_type_id);
        fpt.References(x => x.Site).Column(fuelPhysicalTankTableMeta.Business_unit_id);
      });

      var manifoldPhysicalTankMetaData = WaveDatabaseMetadata.Fuel_Manifold_Physical_Tank;
      HasMany(FuelPhysicalTank.MapExpressions._ManifoldTankAssignments)
        .KeyColumn(manifoldPhysicalTankMetaData.Physical_fuel_tank_id)
        .BatchSize(100)
        .Inverse()
        .Cascade.DeleteOrphan();

      HasMany(FuelPhysicalTank.MapExpressions._FuelTankReadingLineItems)
        .KeyColumn(fuelPhysicalTankTableMeta.Physical_fuel_tank_id)
        .BatchSize(100)
        .Inverse()
        .Cascade.AllDeleteOrphan();

      HasMany(FuelPhysicalTank.MapExpressions._FuelAdjustmentLineItems)
        .KeyColumn(fuelPhysicalTankTableMeta.Physical_fuel_tank_id)
        .BatchSize(100)
        .Inverse()
        .Cascade.AllDeleteOrphan();

      HasMany(FuelPhysicalTank.MapExpressions._FuelDeliveryLineItems)
        .KeyColumn(fuelPhysicalTankTableMeta.Physical_fuel_tank_id)
        .BatchSize(100)
        .Inverse()
        .Cascade.AllDeleteOrphan();
    }
  }
}