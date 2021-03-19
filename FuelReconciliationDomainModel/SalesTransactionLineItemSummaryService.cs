using ByEsoDomainInfraStructure.UnitOfWork;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BYEsoDomainModelKernel.Models;

namespace FuelReconciliationDomainModel
{
  public interface ISalesTransactionLineItemSummaryService
  {
    Task<SaleTransactionWacDto> FetchWacForSalesTransactions(Site site, DateTime currentDate,
      IEnumerable<TankReadingsManifold> tanksReadings, IEnumerable<TankReadingsManifold> tanksReadingsAudit,
      IEnumerable<FuelHoseTankLogicalConnection> hoseTankLogicalConnection);
  }

  public class SalesTransactionLineItemSummaryService : ISalesTransactionLineItemSummaryService
  {
    private readonly ISalesTransactionLineItemRepository _SalesTransactionLineItemRepo;

    public SalesTransactionLineItemSummaryService(ISalesTransactionLineItemRepository salesTransactionLineItemRepo)
    {
      _SalesTransactionLineItemRepo = salesTransactionLineItemRepo;
    }

    public async Task<SaleTransactionWacDto> FetchWacForSalesTransactions(Site site, DateTime currentDate,
      IEnumerable<TankReadingsManifold> tanksReadings, IEnumerable<TankReadingsManifold> tanksReadingsAudit,
      IEnumerable<FuelHoseTankLogicalConnection> hoseTankLogicalConnection)
    {
      var salesTransactionResponse = new SaleTransactionWacDto();
      var salesTransactionLineItems = (await _SalesTransactionLineItemRepo.GetBySiteWithOutFuelPrepaidAndAdjustments(site,
                                                                     site.CurrentBusinessDate)).ToList();
      var pcsFuelTransactions = (from salesTransactionLineItem in salesTransactionLineItems
                                 group salesTransactionLineItem by new
                                 {
                                   salesTransactionLineItem.Site.ID,
                                   salesTransactionLineItem.Site.CurrentBusinessDate,
                                   salesTransactionLineItem.TransactionDate,
                                   salesTransactionLineItem.PumpNumber,
                                   salesTransactionLineItem.HoseNumber,
                                   salesTransactionLineItem.SalesType
                                 }
                                 into newResults
                                 from result in newResults
                                 select new
                                 {
                                   SiteID = newResults.Key.ID,
                                   newResults.Key.CurrentBusinessDate,
                                   newResults.Key.TransactionDate,
                                   newResults.Key.HoseNumber,
                                   newResults.Key.SalesType,
                                   newResults.Key.PumpNumber,
                                   ItemNetSoldQty = (
                                     newResults.Key.SalesType == (int)SalesTypes.PumpTest ? 0 :
                                     (newResults.Sum(x => x.SoldQuantity - x.RefundQuantity))
                                   ),
                                   ItemNetSoldAmt = (
                                     newResults.Key.SalesType == (int)SalesTypes.PumpTest ? 0 :
                                       (newResults.Sum(x => x.SoldAmount - x.RefundAmount))
                                   ),
                                   ItemNetSoldQtyWithPump = newResults.Sum(x => x.SoldQuantity - x.RefundQuantity),
                                   ItemNetSoldAmtWithPump = newResults.Sum(x => x.SoldAmount - x.RefundAmount)
                                 }).ToList();

      var dailySales = (from salesTrans in pcsFuelTransactions
                        join hoseTankConnection in hoseTankLogicalConnection on
                        new { x1 = salesTrans.PumpNumber ?? 0, x2 = salesTrans.HoseNumber ?? 0 } equals
                        new { x1 = hoseTankConnection.PumpNumber, x2 = hoseTankConnection.HoseNumber }
                        where hoseTankConnection.ElectronicFlag && salesTrans.SiteID == site.ID &&
                        salesTrans.CurrentBusinessDate == currentDate
                        select new
                        {
                          hoseTankConnection.LogicalTankId,
                          Volume = hoseTankConnection.PercentageFromLogicalTank * salesTrans.ItemNetSoldQty
                        }).GroupBy(x => x.LogicalTankId)
                       .Select(x => new FuelDailySalesWithMeter
                       {
                         FuelTankID = x.Key,
                         POSVolume = x.Sum(p => p.Volume)
                       }).ToList();

      salesTransactionResponse.FuelSales.AddRange(dailySales);

      var dailySalesAfterStick = (from salesTrans in pcsFuelTransactions
                                  join hoseTankConnection in hoseTankLogicalConnection on
                                  new { x1 = salesTrans.PumpNumber ?? 0, x2 = salesTrans.HoseNumber ?? 0 } equals
                                  new { x1 = hoseTankConnection.PumpNumber, x2 = hoseTankConnection.HoseNumber }
                                  join tanksReading in tanksReadings on hoseTankConnection.LogicalTankId equals tanksReading.FuelTankId
                                  where hoseTankConnection.ElectronicFlag && salesTrans.TransactionDate > tanksReading.ReadTimeStamp &&
                                  salesTrans.SiteID == site.ID && salesTrans.CurrentBusinessDate == currentDate
                                  select new
                                  {
                                    hoseTankConnection.LogicalTankId,
                                    Volume = hoseTankConnection.PercentageFromLogicalTank * salesTrans.ItemNetSoldQty
                                  }).GroupBy(x => x.LogicalTankId)
                                 .Select(x => new FuelDailySalesWithMeter
                                 {
                                   FuelTankID = x.Key,
                                   POSVolume = x.Sum(p => p.Volume)
                                 }).ToList();

      salesTransactionResponse.FuelSalesAfterStickReading.AddRange(dailySalesAfterStick);

      var dailySalesAfterStickAudit = (from salesTrans in pcsFuelTransactions
                                       join hoseTankConnection in hoseTankLogicalConnection on
                                       new { x1 = salesTrans.PumpNumber ?? 0, x2 = salesTrans.HoseNumber ?? 0 } equals
                                       new { x1 = hoseTankConnection.PumpNumber, x2 = hoseTankConnection.HoseNumber }
                                       join tanksReading in tanksReadingsAudit on hoseTankConnection.LogicalTankId
                                       equals tanksReading.FuelTankId
                                       where hoseTankConnection.ElectronicFlag &&
        salesTrans.TransactionDate > tanksReading.ReadTimeStamp && salesTrans.SiteID == site.ID &&
        salesTrans.CurrentBusinessDate == currentDate
                                       select new
                                       {
                                         hoseTankConnection.LogicalTankId,
                                         Volume = (hoseTankConnection.PercentageFromLogicalTank * salesTrans.ItemNetSoldQty)
                                       }).GroupBy(x => x.LogicalTankId)
                                      .Select(x => new FuelDailySalesWithMeter
                                      {
                                        FuelTankID = x.Key,
                                        POSVolume = x.Sum(p => p.Volume)
                                      }).ToList();

      salesTransactionResponse.FuelSalesAfterStickAudit.AddRange(dailySalesAfterStickAudit);

      var dailySalesForMeter = (from salesTrans in pcsFuelTransactions
                                join hoseTankConnection in hoseTankLogicalConnection on
                                new { x1 = salesTrans.PumpNumber ?? 0, x2 = salesTrans.HoseNumber ?? 0 } equals
                                new { x1 = hoseTankConnection.PumpNumber, x2 = hoseTankConnection.HoseNumber }
                                where salesTrans.SiteID == site.ID && salesTrans.CurrentBusinessDate == currentDate
                                select new
                                {
                                  FuelMeterId = hoseTankConnection.FuelMeter.ID,
                                  Volume = (hoseTankConnection.PercentageFromLogicalTank * salesTrans.ItemNetSoldQtyWithPump),
                                  Sales = (hoseTankConnection.PercentageFromLogicalTank * salesTrans.ItemNetSoldAmtWithPump),
                                  ManualVolume = (salesTrans.SalesType == (int)SalesTypes.ManualFuelSales ?
                                                 (salesTrans.ItemNetSoldQtyWithPump * hoseTankConnection.PercentageFromLogicalTank) : 0),
                                  ManualSales = (salesTrans.SalesType == (int)SalesTypes.ManualFuelSales ?
                                                (salesTrans.ItemNetSoldAmtWithPump * hoseTankConnection.PercentageFromLogicalTank) : 0)
                                }).GroupBy(x => x.FuelMeterId)
                               .Select(x => new FuelDailySalesWithMeter
                               {
                                 FuelMeterID = x.Key,
                                 POSVolume = x.Sum(p => p.Volume),
                                 POSSales = x.Sum(p => p.Sales),
                                 ManualVolume = x.Sum(p => p.ManualVolume),
                                 ManualSales = x.Sum(p => p.ManualSales)
                               }).ToList();

      salesTransactionResponse.FuelSalesForEachMeter.AddRange(dailySalesForMeter);
      return salesTransactionResponse;
    }
  }

  public class SaleTransactionWacDto
  {
    public SaleTransactionWacDto()
    {
      FuelSales = new List<FuelDailySalesWithMeter>();
      FuelSalesAfterStickAudit = new List<FuelDailySalesWithMeter>();
      FuelSalesAfterStickReading = new List<FuelDailySalesWithMeter>();
      FuelSalesForEachMeter = new List<FuelDailySalesWithMeter>();
    }
    public List<FuelDailySalesWithMeter> FuelSales { get; set; }
    public List<FuelDailySalesWithMeter> FuelSalesAfterStickReading { get; set; }
    public List<FuelDailySalesWithMeter> FuelSalesAfterStickAudit { get; set; }
    public List<FuelDailySalesWithMeter> FuelSalesForEachMeter { get; set; }
  }
}