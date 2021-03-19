using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using WaveDatabaseMetadata = RP.DatabaseMetadata.WaveDatabaseMetadata;


namespace BYEsoDomainModelKernel.Models
{
  public partial class FuelManifoldTank
  {
    public static class MapExpressions
    {
      public static readonly Expression<Func<FuelManifoldTank, IEnumerable<FuelManifoldTankPhysicalTankAssignment>>>
         _PhysicalTankAssignments = x => x._PhysicalTankAssignments;
    }
  }

  public class FuelManifoldTankMap : SubclassMap<FuelManifoldTank>
  {
    public FuelManifoldTankMap()
    {
      DiscriminatorValue("M");

      var tableMeta = WaveDatabaseMetadata.Fuel_Tank;
      this.Table(tableMeta);
      this.BatchSize(100);

      var fuelManifoldTankTableMeta = WaveDatabaseMetadata.Fuel_Manifold_Tank;
      this.Join(fuelManifoldTankTableMeta, fmt =>
      {
        fmt.KeyColumn(fuelManifoldTankTableMeta.Manifold_fuel_tank_id);
        fmt.Map(x => x.TankNumber).Column(fuelManifoldTankTableMeta.Name);
      });

      var manifoldPhysicalTankMetaData = WaveDatabaseMetadata.Fuel_Manifold_Physical_Tank;
      HasMany(FuelManifoldTank.MapExpressions._PhysicalTankAssignments)
        .KeyColumn(manifoldPhysicalTankMetaData.Manifold_fuel_tank_id)
        .BatchSize(100)
        .Inverse()
        .Cascade.All();
    }
  }
}