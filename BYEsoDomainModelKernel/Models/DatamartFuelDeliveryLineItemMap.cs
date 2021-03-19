using FluentNHibernate.Mapping;
using System;
using System.Linq.Expressions;
using WhDatabaseMetadata = RP.DatabaseMetadata.WhDatabaseMetadata;

namespace BYEsoDomainModelKernel.Models
{
  public partial class DatamartFuelDeliveryLineItem
  {
    public static class MapExpressions
    {
      public static readonly Expression<Func<DatamartFuelDeliveryLineItem, object>>
        _GrossVolume = x => x._GrossVolume;

      public static readonly Expression<Func<DatamartFuelDeliveryLineItem, object>>
        _ActualVolume = x => x._ActualVolume;
    }
  }

  public class DatamartFuelDeliveryLineItemMap : ClassMap<DatamartFuelDeliveryLineItem>
  {
    public DatamartFuelDeliveryLineItemMap()
    {
      var tableMeta = WhDatabaseMetadata.F_pcs_fuel_delivery;
      this.Table(tableMeta);
      this.UseWarehouseSchema();

      CompositeId()
        .KeyReference(x => x.FuelDelivery, tableMeta.Fuel_delivery_id)
        .KeyReference(x => x.FuelPhysicalTank, tableMeta.Fuel_tank_id);

      References(x => x.FuelPhysicalTank).Column(tableMeta.Fuel_tank_id)
        .Not.Insert()
        .Not.Update()
        .Cascade.None();

      References(x => x.Site)
        .Column(tableMeta.Bu_id)
        .Not.Update();

      Map(x => x.BusinessDate).Column(tableMeta.Business_date);
      Map(x => x.BillOfLadingNumber).Column(tableMeta.Bill_of_lading_number).CustomType<string>();

      Map(x => x.NetVolume).Column(tableMeta.Fuel_delivery_net_vol);
      Map(DatamartFuelDeliveryLineItem.MapExpressions._GrossVolume).Column(tableMeta.Fuel_delivery_gross_vol);
      Map(DatamartFuelDeliveryLineItem.MapExpressions._ActualVolume).Column(tableMeta.Fuel_delivery_actual_vol);

      References(x => x.FuelItem).Column(tableMeta.Item_id)
        .Cascade.None();
    }
  }
}