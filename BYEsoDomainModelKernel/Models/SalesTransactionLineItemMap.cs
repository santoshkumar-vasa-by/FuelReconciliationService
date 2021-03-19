using System;
using System.Linq.Expressions;
using FluentNHibernate.Mapping;
using WhDatabaseMetadata = RP.DatabaseMetadata.WhDatabaseMetadata;


namespace BYEsoDomainModelKernel.Models
{
  public partial class SalesTransactionLineItem
  {
    public static class MapExpressions
    {
      public static readonly Expression<Func<SalesTransactionLineItem, object>> _TransactionNumber = x => x._TransactionNumber;
      public static readonly Expression<Func<SalesTransactionLineItem, object>> _SalesItemID = x => x.SalesItemID;
      public static readonly Expression<Func<SalesTransactionLineItem, object>> _RetailPrice = x => x._RetailPrice;
      public static readonly Expression<Func<SalesTransactionLineItem, object>> _StartDate = x => x._StartDate;
      public static readonly Expression<Func<SalesTransactionLineItem, object>> _IsComponent = x => x._IsComponent;
      public static readonly Expression<Func<SalesTransactionLineItem, object>> _EmployeeID = x => x._EmployeeID;
      public static readonly Expression<Func<SalesTransactionLineItem, object>> _ShiftID = x => x._ShiftID;
      public static readonly Expression<Func<SalesTransactionLineItem, object>> _SalesDestinationID = x => x._SalesDestinationID;
      public static readonly Expression<Func<SalesTransactionLineItem, object>> _TransactionLineNumber = x => x._TransactionLineNumber;
    }
  }

  public class SalesTransactionLineItemMap : ClassMap<SalesTransactionLineItem>
  {
    public SalesTransactionLineItemMap()
    {
      var tableMeta = WhDatabaseMetadata.F_gen_sales_item_trans;
      this.Table(tableMeta);
      this.UseWarehouseSchema();
      BatchSize(100);

      CompositeId()
        .KeyReference(x => x.Site, tableMeta.Bu_id)
        .KeyProperty(x => x.BusinessDate, tableMeta.Business_date)
        .KeyProperty(SalesTransactionLineItem.MapExpressions._TransactionNumber, tableMeta.Trans_sequence_number)
        .KeyProperty(SalesTransactionLineItem.MapExpressions._SalesItemID, tableMeta.Sales_item_id)
        .KeyProperty(x => x.SalesType, tableMeta.Sales_type_id)
        .KeyProperty(SalesTransactionLineItem.MapExpressions._RetailPrice, tableMeta.Unit_price_amt)
        .KeyProperty(SalesTransactionLineItem.MapExpressions._StartDate, tableMeta.Start_time)
        .KeyProperty(SalesTransactionLineItem.MapExpressions._IsComponent,
          kp => kp.ColumnName(tableMeta.Component_flag.ColumnName).Type(typeof(LcaseYesNo)))
        .KeyProperty(SalesTransactionLineItem.MapExpressions._EmployeeID, tableMeta.Employee_id)
        .KeyProperty(SalesTransactionLineItem.MapExpressions._ShiftID, tableMeta.Shift_id)
        .KeyProperty(SalesTransactionLineItem.MapExpressions._SalesDestinationID, tableMeta.Sales_dest_id)
        .KeyProperty(SalesTransactionLineItem.MapExpressions._TransactionLineNumber, tableMeta.Trans_line_number);

      References(x => x.Site)
        .Column<Site>(tableMeta.Bu_id)
        .Not.Insert()
        .Not.Update()
        .Cascade.None();

      References(x => x.FuelSalesItem)
        .Column<FuelSalesItem>(tableMeta.Sales_item_id)
        .Not.Insert()
        .Not.Update();

      References(x => x.PostedSalesItem)
        .Column<PostedSalesItem>(tableMeta.Sales_item_id)
        .Not.Insert()
        .Not.Update();

      Map(x => x.TransactionDate).Column(tableMeta.Trans_timestamp);
      Map(x => x.PumpNumber).Column(tableMeta.Pump_number);
      Map(x => x.HoseNumber).Column(tableMeta.Hose);
      Map(x => x.SoldQuantity).Column(tableMeta.Item_gross_sold_qty);
      Map(x => x.SoldAmount).Column(tableMeta.Item_gross_sold_amt);
      Map(x => x.RefundQuantity).Column(tableMeta.Refund_item_qty);
      Map(x => x.RefundAmount).Column(tableMeta.Refund_item_amt);
      Map(x => x.ReductionAmount).Column(tableMeta.Item_reduction_amt);
      Map(x => x.RefundReductionAmount).Column(tableMeta.Item_refund_reduction_amt);
    }
  }
}
