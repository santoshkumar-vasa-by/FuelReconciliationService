using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Mapping;
using NHibernate.Type;
using WaveDatabaseMetadata = RP.DatabaseMetadata.WaveDatabaseMetadata;

namespace BYEsoDomainModelKernel.Models
{
  public partial class FuelInvoice
  {
    public static class MapExpressions
    {
      public static readonly Expression<Func<FuelInvoice, IEnumerable<FuelInvoiceItem>>>
        LineItems = x => x.LineItems;
    }
  }
  public class FuelInvoiceMap : ClassMap<FuelInvoice>
  {
    public FuelInvoiceMap()
    {
      var tableMeta = WaveDatabaseMetadata.Fuel_Invoice;
      this.Table(tableMeta);
      BatchSize(100);

      Id(x => x.ID).Column(tableMeta.Fuel_invoice_id)
        .GeneratedBy.Custom<TicketGenerator>();

      References(x => x.Site)
        .Column<Site>(tableMeta.Business_unit_id)
        .Not.Update()
        .Not.Insert();

      References(x => x.FuelSupplier)
        .Column<Supplier>(tableMeta.Fuel_supplier_id)
        .Not.Update()
        .Not.Insert();

      References(x => x.TerminalSupplier)
        .Column<Supplier>(tableMeta.Terminal_supplier_id)
        .Not.Update()
        .Not.Insert();

      Map(x => x.BillOfLadingNumber).Column(tableMeta.Bill_of_lading_number);
      Map(x => x.IsLocked).Column(tableMeta.Locked_flag)
       .CustomType<EnumCharType<FuelInvoiceLockedFlagStatus>>();
      Map(x => x.WacBusinessDate).Column(tableMeta.Wac_business_date);
      Map(x => x.StatusCode).Column(tableMeta.Status_code)
       .CustomType<EnumCharType<FuelInvoiceStatusCode>>();
      Map(x => x.WacCalculatedFlag).Column(tableMeta.Wac_calculated_flag).
        CustomType<EnumCharType<WacCalculatedFlag>>();
      Map(x => x.FuelInvoiceTypeCode).Column(tableMeta.Invoice_type_code).
        CustomType<EnumCharType<FuelInvoiceTypeCode>>();
      //Map(x => x.FuelInvoiceDate).Column(tableMeta.Fuel_invoice_date).CustomType<CalendarDateFromDateTime>();

      HasMany(FuelInvoice.MapExpressions.LineItems)
        .KeyColumn(tableMeta.Fuel_invoice_id)
        .BatchSize(100)
        .Inverse()
        .Cascade.AllDeleteOrphan();
    }
  }
}
