using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BYEsoDomainModelKernel.Models;
using Site = BYEsoDomainModelKernel.Models.Site;


namespace FuelReconciliationDomainModel
{
  public partial class FuelItemSummaryService : IFuelItemSummaryService
  {
    private readonly IFuelItemSummaryRepository _FuelItemSummaryRepo;
    private readonly IItemRepository _ItemRepo;
    private readonly ISiteRepository _SiteRepository;
    private readonly ISalesTransactionLineItemRepository _SalesTransItemRepo;
    private readonly IFuelTankSummaryRepository _FuelTankSummaryRepo;
    private readonly IDayStatusRepository _DayStatusRepo;
    private readonly ISiteClosedDaysRepository _SiteClosedDaysRepo;
    private readonly IFuelSalesItemRepository _FuelSalesItemRepo;
    private readonly IPostedSaleItemRepository _PostedSaleItemRepo;
    private readonly IFuelInvoiceRepository _FuelInvoiceRepo;
    private readonly IFuelDeliveryInvoiceTotalRepository _FuelDeliveryInvoiceTotalRepo;
    private readonly IFuelTankRepository _FuelTankRepo;

    private readonly IFuelDeliverySelectionService _FuelDeliverySelectionService;
    private readonly IFuelTankLookupService _FuelTankLookupService;
    private readonly IFuelAdjustmentSelectionService _FuelAdjustmentSelectionService;
    private readonly ISalesTransactionLineItemSummaryService _SalesTransactionLineItemSummaryService;
    private readonly IPumpTestSelectionService _PumpTestSelectionService;
    private readonly IFuelMeterReadingSelectionService _FuelMeterReadingSelectionService;

    
    public FuelItemSummaryService(IFuelItemSummaryRepository fuelItemSummaryRepo, IFuelMeterReadingSelectionService fuelMeterReadingSelectionService, IPumpTestSelectionService pumpTestSelectionService, ISalesTransactionLineItemSummaryService salesTransactionLineItemSummaryService, IFuelAdjustmentSelectionService fuelAdjustmentSelectionService, IFuelTankLookupService fuelTankLookupService, IFuelDeliverySelectionService fuelDeliverySelectionService, IFuelDeliveryInvoiceTotalRepository fuelDeliveryInvoiceTotalRepo, IFuelInvoiceRepository fuelInvoiceRepo, IPostedSaleItemRepository postedSaleItemRepo, IFuelSalesItemRepository fuelSalesItemRepo, ISiteClosedDaysRepository siteClosedDaysRepo, IDayStatusRepository dayStatusRepo, IFuelTankSummaryRepository fuelTankSummaryRepo, ISalesTransactionLineItemRepository salesTransItemRepo, ISiteRepository siteRepository, IItemRepository itemRepo, IFuelTankRepository fuelTankRepo)
    {
      _FuelItemSummaryRepo = fuelItemSummaryRepo;
      _FuelMeterReadingSelectionService = fuelMeterReadingSelectionService;
      _PumpTestSelectionService = pumpTestSelectionService;
      _SalesTransactionLineItemSummaryService = salesTransactionLineItemSummaryService;
      _FuelAdjustmentSelectionService = fuelAdjustmentSelectionService;
      _FuelTankLookupService = fuelTankLookupService;
      _FuelDeliverySelectionService = fuelDeliverySelectionService;
      _FuelDeliveryInvoiceTotalRepo = fuelDeliveryInvoiceTotalRepo;
      _FuelInvoiceRepo = fuelInvoiceRepo;
      _PostedSaleItemRepo = postedSaleItemRepo;
      _FuelSalesItemRepo = fuelSalesItemRepo;
      _SiteClosedDaysRepo = siteClosedDaysRepo;
      _DayStatusRepo = dayStatusRepo;
      _FuelTankSummaryRepo = fuelTankSummaryRepo;
      _SalesTransItemRepo = salesTransItemRepo;
      _SiteRepository = siteRepository;
      _ItemRepo = itemRepo;
      _FuelTankRepo = fuelTankRepo;
    }

    public async Task UpdateFuelWac(int? siteID)
    {
      if (siteID.HasValue)
      {
        var site = await _SiteRepository.GetByID(siteID.Value).ConfigureAwait(false);
        var fuelTankSummaryData = await ExecuteFuelTankSummaryRollup(site);

        await DeleteFuelItemSummary(site);
        var nonBlendedItems = await UpdateWacForNonBlendedFuelItems(site, fuelTankSummaryData);
        if (nonBlendedItems != null && nonBlendedItems.Count > 0)
        {
          var blendedItems = await UpdateWacForBlendedFuelItems(site, nonBlendedItems);
          if (blendedItems != null && blendedItems.Count > 0)
          {
            nonBlendedItems.AddRange(blendedItems);
          }
        }
        await AddFuelItemSummary(nonBlendedItems);
      }
      else
      {
        throw new Exception("InvalidSiteIDParameter");
      }
    }

    public async Task<List<FuelItemSummary>> UpdateWacForNonBlendedFuelItems(Site site, List<FuelTankSummary> tankSummaryData)
    {
      var fuelTankSummaryData = await GetFuelTankSummaryData(site, tankSummaryData);
      var itemIds = fuelTankSummaryData.Select(item => item.FuelItem.ID).ToList();

      var fuelItemSummaryData = await GetFuelItemSummaryOfNonBlendedFuelItems(site, fuelTankSummaryData, itemIds);
      return fuelItemSummaryData;
    }

    public async Task<List<FuelItemSummary>> UpdateWacForBlendedFuelItems(Site site, List<FuelItemSummary> nonBlendedItems)
    {
      
      var salesTransactionsBySiteAndDate = await _SalesTransItemRepo.GetBySiteAndBusinessDateWithOutPumpTests(site, site.CurrentBusinessDate);
      var itemSummariesBySiteAndDate = (from item in nonBlendedItems
                                        where item.BusinessDate == site.CurrentBusinessDate && item.Site.ID == site.ID
                                        select item).ToList();

      var fuelItemSummaryBySite = await _FuelItemSummaryRepo.GetBySite(site).ConfigureAwait(false);
      var itemSummariesBySite = fuelItemSummaryBySite as IList<FuelItemSummary> ?? fuelItemSummaryBySite.ToList();

      var salesTransacionsWithItemSummary = (from salesTransaction in salesTransactionsBySiteAndDate
                                             join fuelItem in itemSummariesBySiteAndDate
                                             on salesTransaction.FuelSalesItem.BlendItem1Id equals fuelItem.FuelItem.ID
                                             join fuelItemRef in itemSummariesBySiteAndDate
                                             on salesTransaction.FuelSalesItem.BlendItem2Id equals fuelItemRef.FuelItem.ID
                                             select salesTransaction).ToList();

      var previuosWacBySite = (from itemSummary in itemSummariesBySite
                               where itemSummary.BusinessDate ==
                              ((from fuelSummary2 in itemSummariesBySite
                                where fuelSummary2.BusinessDate < site.CurrentBusinessDate
                                orderby fuelSummary2.BusinessDate descending
                                select fuelSummary2.BusinessDate).Take(1).FirstOrDefault())
                               select itemSummary).Distinct().ToList();

      var itemsummaryList = (from res in salesTransacionsWithItemSummary
                             join previuosWac in previuosWacBySite on
                             res.PostedSalesItem.Item.ID equals previuosWac.FuelItem.ID
                             into itemsWithPreviousWac
                             from resitem in itemsWithPreviousWac.DefaultIfEmpty()
                             join fuelItemSummaryBlend1 in itemSummariesBySiteAndDate
                             on res.FuelSalesItem.BlendItem1Id equals fuelItemSummaryBlend1.FuelItem.ID
                             into fuelItemBlend1
                             from blend1Item in fuelItemBlend1.DefaultIfEmpty()
                             join fuelItemSummaryBlend2 in itemSummariesBySiteAndDate
                             on res.FuelSalesItem.BlendItem1Id equals fuelItemSummaryBlend2.FuelItem.ID
                             into fuelItemBlend2
                             from blend2Item in fuelItemBlend2.DefaultIfEmpty()
                             select new
                             {
                               res.Site,
                               BUDate = res.BusinessDate,
                               ItemID = res.PostedSalesItem.Item.ID,
                               ItemCategory = res.PostedSalesItem.ItemHierarchy,
                               ItemGrossSoldQty = res.SoldQuantity,
                               RefundItemQty = res.RefundQuantity,
                               Blend1ItemPercent = res.FuelSalesItem.BlendItem1Percentage > 0 ?
                               res.FuelSalesItem.BlendItem1Percentage / 100 : 0,
                               Blend2ItemPercent = res.FuelSalesItem.BlendItem2Percentage > 0 ?
                               res.FuelSalesItem.BlendItem2Percentage / 100 : 0,
                               OpenUnitWeightAvgcost = resitem?.ClosingWac ?? 0,
                               Blend1ItemWAC = blend1Item.ClosingWac ?? 0,
                               Blend2ItemWAC = blend2Item.ClosingWac ?? 0
                             }).ToList();

      var blendItemSummaries = (from res in itemsummaryList
                                group res by new
                                {
                                  res.Site.ID,
                                  res.BUDate,
                                  res.ItemID,
                                  res.OpenUnitWeightAvgcost,
                                  res.Blend1ItemWAC,
                                  res.Blend1ItemPercent,
                                  res.Blend2ItemWAC,
                                  res.Blend2ItemPercent,
                                  CategoryID = res.ItemCategory?.ID
                                }
        into newResult
                                select new FuelItemSummary()
                                {
                                  Site = newResult.First().Site,
                                  ItemID = newResult.Key.ItemID,
                                  //FuelItem = await itemRepo.GetByID<FuelItem>(newResult.Key.ItemID),
                                  BusinessDate = newResult.Key.BUDate,
                                  OpeningWac = newResult.Key.OpenUnitWeightAvgcost,
                                  ClosingWac = (newResult.Key.Blend1ItemWAC * newResult.Key.Blend1ItemPercent) +
                                               (newResult.Key.Blend2ItemWAC * newResult.Key.Blend2ItemPercent),
                                  Sales = newResult.Sum(x => x.ItemGrossSoldQty - x.RefundItemQty),
                                  ItemCategory = newResult.First().ItemCategory,
                                  EndOnHandQuantity = 0,
                                  ItemVarianceQuantity = 0,
                                  PumpTestVolumeReturned = 0,
                                  PumpTestVolumeNotReturned = 0
                                }).ToList();
      Parallel.ForEach(blendItemSummaries, async (itemSummary) =>
                                                 {
                                                   itemSummary.FuelItem = 
                                                     await _ItemRepo.GetByID<FuelItem>(itemSummary.ItemID);
                                                 });

      return blendItemSummaries;
    }

    public async Task<List<FuelTankSummary>> ExecuteFuelTankSummaryRollup(Site site)
    {
      var previousBusinessDate = await GetLastPostedDate(site);
      var isFirstDay = await CheckFirstDayOfSite(site);
      var physicalTankReadings = await FetchWacForPhysicalTankReadings(site, isFirstDay);
      var fuelTankReadings = await FetchWacForManifoldAndNonManifoldTanks(site, physicalTankReadings, isFirstDay);
      var fuelDeliveryData = await FetchWacForReceivings(site, physicalTankReadings);
      var fuelAdjustments = await FetchWacForAdjustments(site, physicalTankReadings);
      var fuelWacBlendedData = await FuelWacForFuelBlendedProduct(site);
      var fuelHoseTankLogicalConnections = fuelWacBlendedData.ToList();
      var saleTransactionData = await FetchWacForSales(site, fuelTankReadings, fuelHoseTankLogicalConnections);
      var dailyPumpTestForWacData = await FetchWacForFuelPumpTests(site, fuelHoseTankLogicalConnections);
      var fuelMeterReadings =  await FetchWacForFuelMeterReadings(site, previousBusinessDate, fuelHoseTankLogicalConnections, saleTransactionData.FuelSalesForEachMeter);
      return await AddTankSummaryData(site, previousBusinessDate, 
                                        fuelTankReadings, fuelDeliveryData, fuelAdjustments, 
                                        saleTransactionData, dailyPumpTestForWacData, 
                                        fuelMeterReadings);
    }

    public async Task<bool> CheckFirstDayOfSite(Site site)
    {
      var taskSummary = await _FuelTankSummaryRepo.GetEarliestTankSummaryBySite(site);
      return taskSummary == null || taskSummary.Any();
    }

    public async Task DeleteFuelItemSummary(Site site)
    {
      var fuelItemSummary = await _FuelItemSummaryRepo
        .GetBySiteAndBusinessDate(site, site.CurrentBusinessDate).ConfigureAwait(false);
      foreach (var itemSummary in fuelItemSummary)
      {
        await _FuelItemSummaryRepo.Delete(itemSummary);
      }
    }

    public async Task<DateTime> GetLastPostedDate(Site site)
    {
      var dayStatusList = (await _DayStatusRepo.GetLastPostedDate(site)).ToList();
      var previousBusinessDate = dayStatusList.Count > 0 ? dayStatusList.Max(x => x.BusinessDate) : (DateTime?)null;
      if (previousBusinessDate == null)
      {
        DateTime lastPostedDate = site.CurrentBusinessDate.AddDays(-1);
        var siteAndBusinessDateList = await _SiteClosedDaysRepo.GetBySiteAndBusinessDate(site, lastPostedDate);
        while (siteAndBusinessDateList != null && siteAndBusinessDateList.Any())
        {
          lastPostedDate = lastPostedDate.AddDays(-1);
        }
        previousBusinessDate = lastPostedDate;
      }
      return previousBusinessDate.Value;
    }

    private async Task<PhysicalTankReadingDto> FetchWacForPhysicalTankReadings(Site site, bool isFirstDay)
    {
      return await _FuelTankLookupService.ProcessPhysicalTankReadingData(site, isFirstDay);
    }

    private async Task AddFuelItemSummary(IEnumerable<FuelItemSummary> fuelItemSummaryList)
    {
      foreach (var itemSummary in fuelItemSummaryList)
      {
        await _FuelItemSummaryRepo.Add(itemSummary);
      }
    }
    
    private async Task<List<FuelTankSummary>> AddTankSummaryData(Site site, DateTime previousBusinessDate, 
                                    FuelTankReadingDto fuelTankReadings, FuelDeliveryDto fuelDeliveryData, FuelAdjustmentsDto fuelAdjustmentsData, 
                                    SaleTransactionWacDto fuelSalesData, IEnumerable<DailyPumpTestForWacData> dailyPumpTestForWacData, 
                                    IEnumerable<FuelMeterReadingsData> fuelMeterReadingsData)
    {
      const decimal maxNumericValue = 99999999.9999M;

      var previousDaySummaryList = await _FuelTankSummaryRepo.GetTankSummaryBySiteAndDate(site, previousBusinessDate);
      var currentDateTankSummaryList = await _FuelTankSummaryRepo.GetTankSummaryBySiteAndDate(site, site.CurrentBusinessDate);

      var fuelTankSummaryData = (from tankReadingLatest in fuelTankReadings.TanksReadingsLatest
                                 join previuosDaySummary in previousDaySummaryList
                                   on tankReadingLatest.FuelTankId equals previuosDaySummary.FuelTank.ID
                                   into previousDaySummaryWithTanks
                                 from previousDaySummaryWithTank in previousDaySummaryWithTanks.DefaultIfEmpty()
                                 join tanksReadingFirst in fuelTankReadings.TanksReadingsFirst on tankReadingLatest.FuelTankId equals tanksReadingFirst.FuelTankId
                                   into tankReadingFirstDataList
                                 from tankReadingFirstData in tankReadingFirstDataList.DefaultIfEmpty()
                                 join tankReadingAuditFirst in fuelTankReadings.TanksReadingsAuditFirst on tankReadingLatest.FuelTankId equals tankReadingAuditFirst.FuelTankId
                                   into tankReadingAuditFirstDataList
                                 from tankReadingAuditData in tankReadingFirstDataList.DefaultIfEmpty()
                                 join tankDelivery in fuelDeliveryData.TankDeliveries on tankReadingLatest.FuelTankId equals tankDelivery.FuelTankId
                                   into tankDeliveriesList
                                 from tankDelivery in tankDeliveriesList.DefaultIfEmpty()
                                 join tankAdjustment in fuelAdjustmentsData.TankAdjustments on tankReadingLatest.FuelTankId equals tankAdjustment.FuelTankId
                                   into tankAdjustmentsList
                                 from tankAdjustment in tankAdjustmentsList.DefaultIfEmpty()
                                 join fuelSale in fuelSalesData.FuelSales on tankReadingLatest.FuelTankId equals fuelSale.FuelTankID
                                   into fuelSalesList
                                 from fuelSale in fuelSalesList.DefaultIfEmpty()
                                 join dailyPumpTest in dailyPumpTestForWacData on tankReadingLatest.FuelTankId equals dailyPumpTest.fuelTankId
                                   into dailyPumpTestList
                                 from dailyPumpTest in dailyPumpTestList.DefaultIfEmpty()
                                 join meterWac in fuelMeterReadingsData on tankReadingLatest.FuelTankId equals meterWac.FuelTankID
                                   into meterWacDataList
                                 from meterWacData in meterWacDataList.DefaultIfEmpty()
                                 select new FuelTankSummary
                                 {
                                   FuelTank = _FuelTankRepo.GetByID<FuelTank>(tankReadingLatest.FuelTankId).ConfigureAwait(false).GetAwaiter().GetResult(), // TODO: Optimize
                                   Site = site,
                                   BusinessDate = site.CurrentBusinessDate,
                                   OpenVolume = previousDaySummaryWithTank?.CloseVolume ??
                                                previousDaySummaryWithTank?.BookVolume ?? tankReadingFirstData?.Volume ?? 0,
                                   DeliveryVolume = tankDelivery?.Volume,
                                   AdjustmentVolume = tankAdjustment?.Volume,
                                   SalesVolume = fuelSale?.POSVolume,
                                   AuditOpenVolume = previousDaySummaryWithTank?.AuditCloseVolume ??
                                                     previousDaySummaryWithTank?.AuditBookVolume ?? tankReadingAuditData?.Volume ?? 0,
                                   PumpTestVolumeReturned = dailyPumpTest?.PumpTestVolumeReturned,
                                   PumpTestVolumeNonReturned = dailyPumpTest?.PumpTestVolumeNotReturned,
                                   PumpedVolume = (meterWacData?.PumpedVolume ?? 0) > maxNumericValue ? maxNumericValue : meterWacData?.PumpedVolume
                                 }).ToList();

      var tankSummaryAuditVolumes = (from tanksummaryDetail in fuelTankSummaryData
                                     join tankReadingLatest in fuelTankReadings.TanksReadingsLatest on tanksummaryDetail.FuelTank.ID equals tankReadingLatest.FuelTankId
                                       into tanksReadingsLatestDataList
                                     from tankReadingLatestData in tanksReadingsLatestDataList.DefaultIfEmpty()
                                     join dailySales in fuelSalesData.FuelSalesAfterStickReading on tankReadingLatestData.FuelTankId equals dailySales.FuelTankID
                                       into dailySalesDataList
                                     from dailySale in dailySalesDataList.DefaultIfEmpty()
                                     join delivery in fuelDeliveryData.TankDeliveriesAfterStickReading on tankReadingLatestData.FuelTankId equals delivery.FuelTankId
                                       into dailyDeliveriesDataList
                                     from dailyDeliveryData in dailyDeliveriesDataList.DefaultIfEmpty()
                                     join adjustment in fuelAdjustmentsData.TankAdjustmentsAfterStickReading on tankReadingLatestData.FuelTankId equals adjustment.FuelTankId
                                       into dailyAdjustmentsDataList
                                     from dailyAdjustmentData in dailyAdjustmentsDataList.DefaultIfEmpty()
                                     join tanksReadingsAudit in fuelTankReadings.TanksReadingsAudit on tanksummaryDetail.FuelTank.ID equals tanksReadingsAudit.FuelTankId
                                       into tanksReadingsAuditDataList
                                     from tankReadingAuditData in tanksReadingsAuditDataList.DefaultIfEmpty()
                                     join dailySaleAudit in fuelSalesData.FuelSalesAfterStickAudit on tankReadingLatestData.FuelTankId equals dailySaleAudit.FuelTankID
                                       into dailySalesAuditDataList
                                     from dailySaleAuditData in dailySalesAuditDataList.DefaultIfEmpty()
                                     join deliveryAudit in fuelDeliveryData.TankDeliveriesAuditData on tankReadingLatestData.FuelTankId equals deliveryAudit.FuelTankId
                                       into dailyDeliveriesAuditDataList
                                     from dailyDeliveryAuditData in dailyDeliveriesAuditDataList.DefaultIfEmpty()
                                     join adjustmentAudit in fuelAdjustmentsData.TankAdjustmentsAuditData on tankReadingLatestData.FuelTankId equals adjustmentAudit.FuelTankId
                                       into dailyAdjustmentsAuditDataList
                                     from dailyAdjustmentAuditData in dailyAdjustmentsAuditDataList.DefaultIfEmpty()
                                     select new
                                     {
                                       FuelTankID = tankReadingLatestData.FuelTankId,
                                       SiteID = site.ID,
                                       BusinessDate = site.CurrentBusinessDate,
                                       BookVolume = tankReadingLatestData.Volume == null ?
                                         ((tanksummaryDetail?.OpenVolume ?? 0) +
                                          (tanksummaryDetail?.DeliveryVolume ?? 0) +
                                          (tanksummaryDetail?.AdjustmentVolume ?? 0) -
                                          (tanksummaryDetail?.SalesVolume ?? 0)) :
                                         ((tankReadingLatestData?.Volume ?? 0) +
                                          (dailyDeliveryData?.Volume ?? 0) +
                                          (dailyAdjustmentData?.Volume ?? 0) -
                                          (dailySale?.POSVolume ?? 0)),
                                       CloseVolume = (tankReadingLatestData?.Volume == null) ? tankReadingLatestData?.Volume
                                         : ((tankReadingLatestData?.Volume ?? 0) +
                                            (dailyDeliveryData?.Volume ?? 0) + (dailyAdjustmentData?.Volume ?? 0) - (dailySale?.POSVolume ?? 0)),
                                       AuditBookVolume = tankReadingAuditData.Volume == null ?
                                         ((tanksummaryDetail?.AuditOpenVolume ?? 0) +
                                          (tanksummaryDetail?.DeliveryVolume ?? 0) +
                                          (tanksummaryDetail?.AdjustmentVolume ?? 0) -
                                          (tanksummaryDetail?.SalesVolume ?? 0)) :
                                         ((tankReadingAuditData?.Volume ?? 0) +
                                          (dailyDeliveryAuditData?.Volume ?? 0) +
                                          (dailyAdjustmentAuditData?.Volume ?? 0) -
                                          (dailySaleAuditData?.POSVolume ?? 0)),
                                       AuditCloseVolume = (tankReadingAuditData?.Volume == null) ? tankReadingAuditData?.Volume
                                         : ((tankReadingAuditData?.Volume ?? 0) +
                                            (dailyDeliveryAuditData?.Volume ?? 0) + (dailyAdjustmentAuditData?.Volume ?? 0) -
                                            (dailySaleAuditData?.POSVolume ?? 0))
                                     }).ToList();

      (from tankSummaryDetail in fuelTankSummaryData select tankSummaryDetail).ToList()
        .ForEach(tanksummaryDetail =>
              {
                tanksummaryDetail.BookVolume = tankSummaryAuditVolumes.FirstOrDefault(x => x.FuelTankID == tanksummaryDetail.FuelTank.ID &&
                                                                                           x.SiteID == tanksummaryDetail.Site.ID && x.BusinessDate == tanksummaryDetail.Site.CurrentBusinessDate)?.BookVolume;

                tanksummaryDetail.CloseVolume = tankSummaryAuditVolumes.FirstOrDefault(x => x.FuelTankID == tanksummaryDetail.FuelTank.ID &&
                                                                                            x.SiteID == tanksummaryDetail.Site.ID && x.BusinessDate == tanksummaryDetail.Site.CurrentBusinessDate)?.CloseVolume;

                tanksummaryDetail.AuditBookVolume = tankSummaryAuditVolumes.FirstOrDefault(x => x.FuelTankID == tanksummaryDetail.FuelTank.ID
                                                                                                && x.SiteID == tanksummaryDetail.Site.ID && x.BusinessDate == tanksummaryDetail.Site.CurrentBusinessDate)?.AuditBookVolume;

                tanksummaryDetail.AuditCloseVolume = tankSummaryAuditVolumes.FirstOrDefault(x => x.FuelTankID == tanksummaryDetail.FuelTank.ID
                                                                                                 && x.SiteID == tanksummaryDetail.Site.ID && x.BusinessDate == tanksummaryDetail.Site.CurrentBusinessDate)?.AuditCloseVolume;
              });

      foreach (var currentDateTankSummary in currentDateTankSummaryList)
      {
        await _FuelTankSummaryRepo.Delete(currentDateTankSummary);
      }

      foreach (var tankSummaryDetail in fuelTankSummaryData)
      {
        await _FuelTankSummaryRepo.Add(tankSummaryDetail);
      }

      return fuelTankSummaryData;
    }

    private async Task<IEnumerable<FuelMeterReadingsData>> FetchWacForFuelMeterReadings(Site site, DateTime previousBusinessDate, IEnumerable<FuelHoseTankLogicalConnection> fuelHoseTankLogicalConnection, IEnumerable<FuelDailySalesWithMeter> fuelSalesForEachMeter)
    {
      return await _FuelMeterReadingSelectionService.GetFuelMeterWacData(site, previousBusinessDate,
        fuelHoseTankLogicalConnection ?? new List<FuelHoseTankLogicalConnection>(),
        fuelSalesForEachMeter ?? new List<FuelDailySalesWithMeter>());
    }

    private async Task<FuelAdjustmentsDto> FetchWacForAdjustments(Site site, PhysicalTankReadingDto physicalTankReadings)
    {
      return await _FuelAdjustmentSelectionService.FetchWacForAdjustments(site, site.CurrentBusinessDate, physicalTankReadings);
    }

    private async Task<IEnumerable<DailyPumpTestForWacData>> FetchWacForFuelPumpTests(Site site, IEnumerable<FuelHoseTankLogicalConnection> fuelHoseTankLogicalConnection)
    {
      return await _PumpTestSelectionService.GetFuelPumpTestsForWac(site, fuelHoseTankLogicalConnection);
    }

    private async Task<SaleTransactionWacDto> FetchWacForSales(Site site, FuelTankReadingDto tankReadings, IEnumerable<FuelHoseTankLogicalConnection> fuelHoseTankLogicalConnection)
    {
      return await _SalesTransactionLineItemSummaryService.FetchWacForSalesTransactions(site, site.CurrentBusinessDate,
        tankReadings.TanksReadingsLatest ?? new List<TankReadingsManifold>(),
        tankReadings.TanksReadingsAudit ?? new List<TankReadingsManifold>(),
        fuelHoseTankLogicalConnection);
    }

    private async Task<IEnumerable<FuelHoseTankLogicalConnection>> FuelWacForFuelBlendedProduct(Site site)
    {
      return await _FuelTankLookupService.ProcessFuelBlendedProductData(site);
    }

    private async Task<FuelDeliveryDto> FetchWacForReceivings(Site site, PhysicalTankReadingDto physicalTankReadings)
    { 
      return await _FuelDeliverySelectionService.FetchWacForReceivings(site, site.CurrentBusinessDate,
        physicalTankReadings.LatestPhysicalTankReadingData ?? new List<FuelPhysicalTankReadingData>(),
        physicalTankReadings.LatestPhysicalTankReadingAuditData ?? new List<FuelPhysicalTankReadingData>());
    }

    private async Task<FuelTankReadingDto> FetchWacForManifoldAndNonManifoldTanks(Site site, PhysicalTankReadingDto physicalTankReadings, bool isFirstDay)
    {

      return await _FuelTankLookupService.GetWacForTankReadingsBySite(site, isFirstDay,
        physicalTankReadings.LatestPhysicalTankReadingData ?? new List<FuelPhysicalTankReadingData>(),
        physicalTankReadings.FirstPhysicalTankReadingData ?? new List<FuelPhysicalTankReadingData>(),
        physicalTankReadings.LatestPhysicalTankReadingAuditData ?? new List<FuelPhysicalTankReadingData>(),
        physicalTankReadings.FirstPhysicalTankReadingAuditData ?? new List<FuelPhysicalTankReadingData>());
    }
  }
}
