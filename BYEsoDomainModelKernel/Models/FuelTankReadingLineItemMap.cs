using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;
using System.Linq.Expressions;
using NHibernate.Type;
using WaveDatabaseMetadata = RP.DatabaseMetadata.WaveDatabaseMetadata;

namespace BYEsoDomainModelKernel.Models
{
  public partial class FuelTankReadingLineItem
  {
    public static class MapExpressions
    {
      public static readonly Expression<Func<FuelTankReadingLineItem, object>>
        _TotalHeight = x => x._TotalHeight;

      public static readonly Expression<Func<FuelTankReadingLineItem, object>>
        _WaterHeight = x => x._WaterHeight;

      public static readonly Expression<Func<FuelTankReadingLineItem, object>>
        _ReadTimestamp = x => x._ReadTimestamp;
    }
  }

  public class FuelTankReadingLineItemMap : ClassMap<FuelTankReadingLineItem>
  {
    public FuelTankReadingLineItemMap()
    {
      var tableMeta = WaveDatabaseMetadata.Fuel_Tank_Reading_Line_Item;
      this.Table(tableMeta);

      CompositeId()
        .KeyProperty(x => x.LineNumber, tableMeta.Fuel_tank_reading_line_item_id)
        .KeyReference(x => x.FuelTankReading, tableMeta.Fuel_tank_reading_id);

      References(x => x.Site).Column(tableMeta.Business_unit_id)
        .Not.Update()
        .Cascade.None();

      References(x => x.FuelPhysicalTank).Column(tableMeta.Physical_fuel_tank_id)
        .Not.Update()
        .Cascade.None();

      References(x => x.FuelTankReading).Column(tableMeta.Fuel_tank_reading_id)
        .Not.Update()
        .Not.Insert()
        .Cascade.None();

      Map(x => x.WaterVolume).Column(tableMeta.Water_volume);
      Map(x => x.TotalVolume).Column(tableMeta.Total_volume);
      Map(x => x.Temperature).Column(tableMeta.Temperature);

      Map(FuelTankReadingLineItem.MapExpressions._TotalHeight)
       .Column(tableMeta.Total_height);
      Map(FuelTankReadingLineItem.MapExpressions._WaterHeight)
       .Column(tableMeta.Water_height);
      Map(FuelTankReadingLineItem.MapExpressions._ReadTimestamp)
       .Column(tableMeta.Read_timestamp);
    }
  }
}