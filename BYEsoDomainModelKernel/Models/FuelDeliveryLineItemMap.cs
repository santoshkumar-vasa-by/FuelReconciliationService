using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;
using System.Linq.Expressions;
using NHibernate.Type;
using WaveDatabaseMetadata = RP.DatabaseMetadata.WaveDatabaseMetadata;

namespace BYEsoDomainModelKernel.Models
{
  public partial class FuelDeliveryLineItem
  {
    public static class MapExpressions
    {
      public static readonly Expression<Func<FuelDeliveryLineItem, object>> _GrossVolume = x => x.GrossVolume;
      public static readonly Expression<Func<FuelDeliveryLineItem, object>> _NetVolume = x => x.NetVolume;
      public static readonly Expression<Func<FuelDeliveryLineItem, object>> _ActualVolume = x => x.ActualVolume;
    }
  }

  public class FuelDeliveryLineItemMap : ClassMap<FuelDeliveryLineItem>
  {
    public FuelDeliveryLineItemMap()
    {
      var tableMeta = WaveDatabaseMetadata.Fuel_Delivery_Line_Item;
      this.Table(tableMeta);

      CompositeId()
        .KeyProperty(x => x.LineNumber, tableMeta.Fuel_delivery_line_item_number)
        .KeyReference(x => x.FuelDelivery, tableMeta.Fuel_delivery_id);

      References(x => x.Site).Column(tableMeta.Business_unit_id)
        .Not.Update()
        .Cascade.None();

      References(x => x.FuelDelivery).Column(tableMeta.Fuel_delivery_id)
        .Not.Update()
        .Not.Insert()
        .Cascade.None();

      References(x => x.FuelPhysicalTank).Column(tableMeta.Physical_fuel_tank_id)
        .Not.Update()
        .Cascade.None();

      Map(x => x.FuelCostAmount).Column(tableMeta.Fuel_cost_amount);
      Map(FuelDeliveryLineItem.MapExpressions._GrossVolume).Column(tableMeta.Gross_volume);
      Map(FuelDeliveryLineItem.MapExpressions._NetVolume).Column(tableMeta.Net_volume);
      Map(FuelDeliveryLineItem.MapExpressions._ActualVolume).Column(tableMeta.Actual_volume);
    }
  }
}