using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Mapping;
using NHibernate.Type;
using WaveDatabaseMetadata = RP.DatabaseMetadata.WaveDatabaseMetadata;
using RP.DomainModelKernel.Common.Types;

namespace BYEsoDomainModelKernel.Models
{
  public partial class Supplier
  {
    public static class MapExpressions
    {
      public static readonly Expression<Func<Supplier, IEnumerable<SupplierItemGroup>>>
        SupplierItemGroups = x => x._SupplierItemGroups;

      public static readonly Expression<Func<Supplier, IEnumerable<SupplierDataAccessorAssignment>>>
        SupplierDataAccessorAssignments = x => x._SupplierDataAccessorAssignments;

      public static readonly Expression<Func<Supplier, IEnumerable<SupplierMetricAssignment>>>
        OrderingMetricAssignments = x => x._OrderingMetricAssignments;

      public static readonly Expression<Func<Supplier, IEnumerable<CostLevel>>>
        CostLevel = x => x.CostLevel;

      public static readonly Expression<Func<Supplier, IEnumerable<SupplierHoliday>>>
        SupplierHolidays = x => x._SupplierHolidays;

      //public static readonly Expression<Func<Supplier, IEnumerable<SupplierContactList>>>
      //  SupplierContacts = x => x.SupplierContactsList;

      //public static readonly Expression<Func<Supplier, IEnumerable<SupplierOverrideConfiguration>>>
      //  SupplierOverrideConfiguration = x => x._SupplierOverrideConfigurations;

      //public static readonly Expression<Func<Supplier, IEnumerable<SupplierSiteSchedule>>>
      //  SupplierSiteSchedule = x => x.SupplierSiteSchedule;

      //public static readonly Expression<Func<Supplier, IEnumerable<SupplierSchedule>>>
      //  SupplierSchedule = x => x.SupplierSchedule;
      
      //public static readonly Expression<Func<Supplier, IEnumerable<SupplierScheduleGroup>>>
      //  SupplierScheduleGroup = x => x.SupplierScheduleGroup;

      //public static readonly Expression<Func<Supplier, SupplierParameters>>
      //  SupplierParameters = x => x.SupplierParameters;

      //public static readonly Expression<Func<Supplier, SupplierTerms>>
      //  SupplierTerms = x => x.SupplierTerms;
      
    }
  }

  public class SupplierMap : ClassMap<Supplier>
  {
    public SupplierMap()
    {
      var tableMeta = WaveDatabaseMetadata.Supplier;
      this.Table(tableMeta);
      BatchSize(100);

      Id(x => x.ID).Column(tableMeta.Supplier_id)
                   .GeneratedBy.Custom<TicketGenerator>();
      Map(x => x.Name).Column(tableMeta.Name);
      Map(x => x.Description).Column(tableMeta.Description); 
      //Map(x => x.SuggestedOrderQtyRoundingThreshold)
      //  .Column(tableMeta.Suggested_order_qty_rounding_threshold)
      //  .CustomType<IntegerPercentToDecimalRatio>();
      Map(x => x.ExternalId).Column(tableMeta.Xref_code);
      Map(x => x.CustomerNumber).Column(tableMeta.Customer_number);
      Map(x => x.VendorApCode).Column(tableMeta.External_vendor_ap_code);
      Map(x => x.UsesDefaultForecastConfiguration)
        .Column(tableMeta.Use_default_forecast_group_flag)
        .CustomType<LcaseYesNo>();
      Map(x => x.Type).Column(tableMeta.Supplier_type_code)
                      .CustomType<EnumCharType<SupplierType>>();
      Map(x => x.BillingMethod).Column(tableMeta.Billing_method_type_code)
                               .CustomType<EnumCharType<SupplierBillingMethod>>();
      Map(x => x.DeliveryCostToleranceRatio)
        .Column(tableMeta.Receive_balance_to_zero_tolerance);
      //References(x => x.OrderingForecastGroup)
      //  .Column(tableMeta.Default_forecast_group_id)
      //  .Cascade.None();
      Map(x => x.EDINumber).Column(tableMeta.Edi_number);
      Map(x => x.TermsAndConditions).Column(tableMeta.Terms_and_conditions);
      Map(x => x.StatusCode).Column(tableMeta.Status_code)
                            .CustomType<EnumCharType<SupplierStatusCode>>();
      Map(x => x.AllowTotalsAcceptReceiving)
        .Column(tableMeta.Allow_totals_accept_receiving_flag)
        .CustomType<LcaseYesNo>();
      Map(x => x.AllowBlindReceiving)
        .Column(tableMeta.Blind_receiving_flag)
        .CustomType<LcaseYesNo>();
      Map(x => x.AllowTotalsOnlyReceiving)
        .Column(tableMeta.Allow_totals_only_receiving_flag)
        .CustomType<LcaseYesNo>();
      Map(x => x.AllowMultipleReceiving)
        .Column(tableMeta.Consolidate_receiving_flag)
        .CustomType<LcaseYesNo>();

      Map(x => x.DeliveryNumberFormat).Column(tableMeta.Delivery_number_mask);
      Map(x => x.DeliveryInvoiceDateRequiredFlag)
        .Column(tableMeta.Inv_receive_invoice_date_required_flag).CustomType<LcaseYesNo>();
      Map(x => x.DeliveryInvoiceRefNumberRequiredFlag)
        .Column(tableMeta.Inv_receive_invoice_num_required_flag).CustomType<LcaseYesNo>();
      Map(x => x.DeliveryInvoiceRefNumberInvalidChars)
        .Column(tableMeta.Inv_receive_invoice_num_invalid_chars);

      Map(x => x.ReceiveCheckNumberForCODFlag)
        .Column(tableMeta.Receive_check_number_for_cod_flag).CustomType<LcaseYesNo>();
      Map(x => x.BuControlCostsCode).Column(tableMeta.Bu_control_costs_code);
      //Map(x => x.DealerOperatedBusinessUnitCanCreateCode).Column(tableMeta.Dealer_operated_business_unit_can_create_code)
      //  .CustomType(typeof(EnumCharType<CanSupplierAddSupplierItem>)); 
      //Map(x => x.CompanyOperatedBusinessUnitCanCreateCode).Column(tableMeta.Company_operated_business_unit_can_create_code)
      //    .CustomType(typeof(EnumCharType<CanSupplierAddSupplierItem>));
      Map(x => x.AllowFractionalQuantityFlag)
        .Column(tableMeta.Allow_fractional_quantity_flag).CustomType<LcaseYesNo>();

      Map(x => x.ExportType).Column(tableMeta.Export_type_code)
        .CustomType(typeof(EnumCharType<ExportType>));
      Map(x => x.MinimumOrderAmount).Column(tableMeta.Minimum_order_amount);
      Map(x => x.MinimumOrderQuantity).Column(tableMeta.Minimum_order_quantity);

      //References(x => x.TotalsOnlySupplierItem)
      //  .Column(tableMeta.Totals_only_supplier_item_id)
      //  .Cascade.None();
      References(x => x.Address)
        .Column(tableMeta.Address_id)
        .Cascade.None().Fetch.Join();

      Map(x => x.UseLowestCost)
        .Column(tableMeta.Use_lowest_cost_flag)
        .CustomType<LcaseYesNo>();
      Map(x => x.DeliveryUsingInvoiceCostType)
        .Column(tableMeta.Rcv_using_inv_default_cost_target_code)
        .CustomType<EnumCharType<DeliveryInvoiceCostType>>();
      Map(x => x.DeliveryUsingPOCostType)
        .Column(tableMeta.Rcv_using_po_default_cost_target_code)
        .CustomType<EnumCharType<DeliveryPOCostType>>();
      Map(x => x.OrderHolidayRuleType)
        .Column(tableMeta.Holiday_order_code)
        .CustomType<EnumCharType<SupplierOrderHolidayRuleType>>();
      Map(x => x.DeliveryHolidayRuleType)
        .Column(tableMeta.Holiday_delivery_code)
        .CustomType<EnumCharType<SupplierDeliveryHolidayRuleType>>();
      Map(x => x.ReturnApprovalRequiredFlag)
        .Column(tableMeta.Return_approval_required_flag)
        .CustomType<LcaseYesNo>();
      Map(x => x.HideCostsFromSite)
        .Column(tableMeta.Hide_costs_for_bu_flag)
        .CustomType<LcaseYesNo>();
      Map(x => x.TrustedSupplierFlag)
        .Column(tableMeta.Rcv_trusted_flag)
        .CustomType<LcaseYesNo>();
      HasMany(Supplier.MapExpressions.SupplierDataAccessorAssignments)
        .KeyColumn(tableMeta.Supplier_id)
        .BatchSize(100)
        .Inverse()
        .Cascade.All();

      //HasMany(Supplier.MapExpressions.SupplierSiteSchedule)
      //  .KeyColumn(tableMeta.Supplier_id)
      //  .BatchSize(100)
      //  .Inverse()
      //  .Cascade.All();

      //var supplierSchedule = WaveDatabaseMetadata.Supplier_Schedule;
      //HasMany(Supplier.MapExpressions.SupplierSchedule)
      //  .KeyColumn(supplierSchedule.Group_supplier_id)
      //  .BatchSize(100)
      //  .Inverse()
      //  .Cascade.All();

      //var supplierScheduleGroup = WaveDatabaseMetadata.Supplier_Schedule_Group;
      //HasMany(Supplier.MapExpressions.SupplierScheduleGroup)
      //  .KeyColumn(supplierScheduleGroup.Group_supplier_id)
      //  .BatchSize(100)
      //  .Inverse()
      //  .Cascade.All();

      HasMany(Supplier.MapExpressions.OrderingMetricAssignments)
        .KeyColumn(tableMeta.Supplier_id)
        .BatchSize(100)
        .Inverse()
        .Cascade.All();

      HasMany(Supplier.MapExpressions.SupplierItemGroups)
        .KeyColumn(tableMeta.Supplier_id)
        .BatchSize(100)
        .Inverse()
        .Cascade.All();

      HasMany(Supplier.MapExpressions.CostLevel)
        .KeyColumn(tableMeta.Supplier_id)
        .BatchSize(100)
        .Inverse()
        .Cascade.All();

      HasMany(Supplier.MapExpressions.SupplierHolidays)
        .KeyColumn(tableMeta.Supplier_id)
        .BatchSize(100)
        .Inverse()
        .Cascade.All();

      //HasMany(Supplier.MapExpressions.SupplierContacts)
      //  .KeyColumn(tableMeta.Supplier_id)
      //  .BatchSize(100)
      //  .Inverse()
      //  .Cascade.All();

      //HasMany(Supplier.MapExpressions.SupplierOverrideConfiguration)
      //  .KeyColumn(tableMeta.Supplier_id)
      //  .BatchSize(100)
      //  .Cascade.All();

      // HasOne(Supplier.MapExpressions.SupplierParameters)
      //  .ForeignKey("Supplier_id")
      //  .Cascade.All();

      // HasOne(Supplier.MapExpressions.SupplierTerms)
      //   .ForeignKey("Supplier_id")
      //   .Cascade.All();

    }
  }
}
