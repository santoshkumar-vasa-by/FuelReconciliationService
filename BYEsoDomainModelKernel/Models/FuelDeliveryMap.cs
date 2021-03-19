using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;
using System.Linq.Expressions;
using NHibernate.Type;
using WaveDatabaseMetadata = RP.DatabaseMetadata.WaveDatabaseMetadata;

namespace BYEsoDomainModelKernel.Models
{
  public partial class FuelDelivery
  {
    public static class MapExpressions
    {
      public static readonly Expression<Func<FuelDelivery, IEnumerable<FuelDeliveryLineItem>>>
        LineItems = x => x.LineItems;

      public static readonly Expression<Func<FuelDelivery, IEnumerable<DatamartFuelDeliveryLineItem>>>
        DatamartLineItems = x => x.DatamartLineItems;

      //public static readonly Expression<Func<FuelDelivery, IEnumerable<FuelDeliveryGlDistribution>>>
      //  GlDistributions = x => x.GlDistributions;

      public static readonly Expression<Func<FuelDelivery, object>>
        _TotalCostForNonNullableColumn = x => x.TotalCostForNonNullableColumn;

      public static readonly Expression<Func<FuelDelivery, object>>
        BillOfLadingDateForNonNullableColumn = x => x.BillOfLadingDateForNonNullableColumn;
    }
  }

  public class FuelDeliveryMap : ClassMap<FuelDelivery>
  {
    public FuelDeliveryMap()
    {
      var tableMeta = WaveDatabaseMetadata.Fuel_Delivery;
      this.Table(tableMeta);
      this.BatchSize(100);

      Id(x => x.ID).Column(tableMeta.Fuel_delivery_id)
        .GeneratedBy.Custom<TicketGenerator>();

      References(x => x.Site)
        .Column(tableMeta.Business_unit_id)
        .Not.Update().Fetch.Join();

      References(x => x.FuelSupplier)
        .Column(tableMeta.Fuel_supplier_id)
        .Update().Fetch.Join();

      References(x => x.TerminalSupplier)
        .Column(tableMeta.Terminal_supplier_id)
        .Update().Fetch.Join();

      References(x => x.CarrierSupplier)
        .Column(tableMeta.Carrier_supplier_id)
        .Update().Fetch.Join();

      Map(x => x.BusinessDate).Column(tableMeta.Business_date);
      Map(x => x.DeliveryTimestamp).Column(tableMeta.Delivery_timestamp);
      Map(x => x.BillOfLadingNumber).Column(tableMeta.Bill_of_lading_number).CustomType<string>();
      Map(x => x.Status).Column(tableMeta.Status_code)
        .CustomType<EnumCharType<FuelDeliveryStatus>>();
      Map(x => x.IsLocked).Column(tableMeta.Locked_flag)
      .CustomType<EnumCharType<FuelDeliveryLockedFlagStatus>>();
      Map(x => x.DeliveryType).Column(tableMeta.Imported_flag)
        .CustomType<EnumCharType<FuelDeliveryType>>();
      Map(FuelDelivery.MapExpressions.BillOfLadingDateForNonNullableColumn)
        .Column(tableMeta.Bill_of_lading_date);
      Map(FuelDelivery.MapExpressions._TotalCostForNonNullableColumn)
        .Column(tableMeta.Amount_total);
      Map(x => x.InvoiceNumber).Column(tableMeta.Invoice_number);
      Map(x => x.Temparature).Column(tableMeta.Temperature);
      Map(x => x.TruckNumber).Column(tableMeta.Truck_number);
      HasMany(FuelDelivery.MapExpressions.LineItems)
       .KeyColumn(tableMeta.Fuel_delivery_id)
       .BatchSize(100)
       .Inverse()
       .Cascade.AllDeleteOrphan();

      HasMany(FuelDelivery.MapExpressions.DatamartLineItems)
       .KeyColumn(tableMeta.Fuel_delivery_id)
       .BatchSize(100)
       .Inverse()
       .Cascade.AllDeleteOrphan();

      //HasMany(FuelDelivery.MapExpressions.GlDistributions)
      //  .KeyColumn(tableMeta.Fuel_delivery_id)
      //  .BatchSize(100)
      //  .Inverse()
      //  .Cascade.AllDeleteOrphan();
    }
  }
}
