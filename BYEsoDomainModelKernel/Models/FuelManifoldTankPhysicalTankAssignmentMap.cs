using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;
using System.Linq.Expressions;
using WaveDatabaseMetadata = RP.DatabaseMetadata.WaveDatabaseMetadata;

namespace BYEsoDomainModelKernel.Models
{
  public class FuelManifoldTankPhysicalTankAssignmentMap : ClassMap<FuelManifoldTankPhysicalTankAssignment>
  {
    public FuelManifoldTankPhysicalTankAssignmentMap()
    {
      var tableMeta = WaveDatabaseMetadata.Fuel_Manifold_Physical_Tank;
      this.Table(tableMeta);
      this.BatchSize(100);

      CompositeId()
        .KeyReference(x => x.ManifoldTank, tableMeta.Manifold_fuel_tank_id)
        .KeyReference(x => x.PhysicalTank, tableMeta.Physical_fuel_tank_id);

      References(x => x.ManifoldTank)
        .Column(tableMeta.Manifold_fuel_tank_id)
        .Not.Insert()
        .Not.Update()
        .Cascade.None();

      References(x => x.PhysicalTank)
        .Column(tableMeta.Physical_fuel_tank_id)
        .Not.Insert()
        .Not.Update()
        .Cascade.None();
    }
  }
}