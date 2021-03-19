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
    private async Task<List<FuelTankSummaryData>> GetFuelTankSummaryData(Site site, List<FuelTankSummary> tankSummaryDetails)
    {
      var fuelInvociesTemp = await UpdateFuelInvoiceWacFlag(site, FuelInvoice.FuelLockType.Temporary);

      var fuelDeliveryInvoiceTotalData = await _FuelDeliveryInvoiceTotalRepo.GetFuelDeliveryInvoiceList(site, fuelInvociesTemp);

      var fuelInvoiceData = fuelDeliveryInvoiceTotalData.SelectMany(x => x.FuelInvoice.LineItems)
        .GroupBy(x => new
        {
          itemId = x.FuelItem.ID
        })
        .Select(x => new
        {
          x.Key,
          TotalQuantity = x.Sum(xi => xi.Quantity),
          TotalCost = x.Sum(xi => xi.Cost)
        }).ToList();

      var tankSummaryData = (from fts in tankSummaryDetails ?? new List<FuelTankSummary>()
                             join inc in fuelInvoiceData on fts.FuelTank?.FuelItem?.ID equals inc.Key.itemId into tsd
                             from ts in tsd.DefaultIfEmpty()
                             select new
                             {
                               fts.Site,
                               fts.BusinessDate,
                               fts.FuelTank.FuelItem,
                               TotalQuantity = ts == null ? 0 : ts.TotalQuantity,
                               TotalCost = ts == null ? 0 : ts.TotalCost,
                               fts.DeliveryVolume,
                               fts.SalesVolume,
                               fts.OpenVolume,
                               fts.CloseVolume,
                               fts.PumpedVolume,
                               fts.AdjustmentVolume,
                               fts.PumpTestVolumeReturned,
                               fts.PumpTestVolumeNonReturned
                             }).ToList();

      var groupedtankSummaryData = (from fr in tankSummaryData
                                    group fr by new
                                    {
                                      fr.Site.ID,
                                      fr.BusinessDate,
                                      itemId = fr.FuelItem.ID,
                                      fr.TotalQuantity,
                                      fr.TotalCost
                                    } into g
                                    select new FuelTankSummaryData()
                                    {
                                      Site = g.First().Site,
                                      BusinessDate = g.First().BusinessDate,
                                      FuelItem = g.First().FuelItem,
                                      InvoiceQuantity = g.Key.TotalQuantity,
                                      InvoiceCost = g.Key.TotalCost,
                                      DeliveryVolume = g.Sum(x => x.DeliveryVolume),
                                      SalesVolume = g.Sum(x => x.SalesVolume),
                                      OpenVolume = g.Sum(x => x.OpenVolume),
                                      CloseVolume = g.Sum(x => x.CloseVolume),
                                      PumpedVolume = g.Sum(x => x.PumpedVolume),
                                      AdjustmentVolume = g.Sum(x => x.AdjustmentVolume),
                                      PumpTestVolumeReturned = g.Sum(x => x.PumpTestVolumeReturned),
                                      PumpTestVolumeNonReturned = g.Sum(x => x.PumpTestVolumeNonReturned)
                                    }).ToList();

      return groupedtankSummaryData;
    }

    public async Task<IList<FuelInvoice>> UpdateFuelInvoiceWacFlag(Site site, FuelInvoice.FuelLockType flagStatus)
    {
      return await _FuelInvoiceRepo.UpdateFuelInvoiceWacFlags(site, flagStatus);
    }

    private async Task<List<FuelItemSummary>> GetFuelItemSummaryOfNonBlendedFuelItems(Site site,
      List<FuelTankSummaryData> fuelTankSummaryData, List<int> itemIds)
    {
      var recentBusinessDateRecordData = await _FuelItemSummaryRepo.GetBySite(site).ConfigureAwait(false);
      var recentBusinessDateRecord = recentBusinessDateRecordData.Where(x => x.BusinessDate < site.CurrentBusinessDate)
        .OrderByDescending(x => x.BusinessDate).Take(1).FirstOrDefault();
      
      var fuelItemSummaryData = await _FuelItemSummaryRepo
        .GetBySiteAndBusinessDate(site, recentBusinessDateRecord?.BusinessDate);

      var itemData = await _ItemRepo.GetItemsByIds<Item>(itemIds.ToArray());
      var postedSalesItemsData = await _PostedSaleItemRepo.GetPostedSaleItemsByIds(itemIds.ToArray());
      var fuelSalesItemsData = await _FuelSalesItemRepo.GetFuelSaleItems();
      
      var salesTransactionLineItemData = (await _SalesTransItemRepo
        .GetBySiteAndBusinessDate(site, site.CurrentBusinessDate))
        .Where(x => x.SalesType != 5).ToList();

      var itemSummaryData = (from tsd in fuelTankSummaryData
                             join fisd in fuelItemSummaryData on tsd.FuelItem.ID equals fisd.FuelItem?.ID into tempfsd
                             from tempfisd in tempfsd.DefaultIfEmpty()
                             join itd in itemData on tsd.FuelItem.ID equals itd.ID into tempitd
                             from tempid in tempitd.DefaultIfEmpty()
                             join psd in postedSalesItemsData on tsd.FuelItem.ID equals psd.Item?.ID into temppsd
                             from tpsd in temppsd.DefaultIfEmpty()
                             join fsd in fuelSalesItemsData on tpsd.SalesItemID equals fsd.FuelSalesItemID into tempfsid
                             from tfsd in tempfsid.DefaultIfEmpty()
                             join std in salesTransactionLineItemData on tpsd.SalesItemID equals std.SalesItemID into tempsd
                             from tstd in tempsd.DefaultIfEmpty()
                             select new
                             {
                               tsd.Site,
                               tsd.BusinessDate,
                               tsd.FuelItem,
                               ClosingWac = tempfisd?.ClosingWac ?? 0,
                               tsd.DeliveryVolume,
                               tsd.SalesVolume,
                               tsd.OpenVolume,
                               tsd.CloseVolume,
                               tsd.InvoiceQuantity,
                               tsd.InvoiceCost,
                               tsd.AdjustmentVolume,
                               tempid?.Category,
                               tsd.PumpTestVolumeReturned,
                               tsd.PumpTestVolumeNonReturned,
                               SoldQuantity = tstd?.SoldQuantity ?? 0,
                               RefundQuanity = tstd?.RefundQuantity ?? 0
                             }).ToList();

      var groupedFuelItemSummaryData = (from fr in itemSummaryData
                                        group fr by new
                                        {
                                          fr.Site.ID,
                                          fr.BusinessDate,
                                          itemId = fr.FuelItem.ID,
                                          fr.ClosingWac,
                                          fr.OpenVolume,
                                          fr.DeliveryVolume,
                                          fr.InvoiceQuantity,
                                          fr.InvoiceCost,
                                          fr.CloseVolume,
                                          fr.SalesVolume,
                                          fr.AdjustmentVolume,
                                          ItemHierarchyId = fr.Category?.ID,
                                          fr.PumpTestVolumeReturned,
                                          fr.PumpTestVolumeNonReturned
                                        } into g
                                        select new FuelItemSummary()
                                        {
                                          Site = g.First().Site,
                                          BusinessDate = g.First().BusinessDate,
                                          FuelItem = g.First().FuelItem,
                                          OpeningWac = g.Key.ClosingWac,
                                          ClosingWac = g.Key.OpenVolume + g.Key.InvoiceQuantity == 0 ? 0 :
                                            (g.Key.OpenVolume * g.Key.ClosingWac + g.Key.InvoiceCost) /
                                            (g.Key.OpenVolume + g.Key.InvoiceQuantity),
                                          Sales = g.Sum(x => x.SoldQuantity - x.RefundQuanity),
                                          EndOnHandQuantity = g.Key.CloseVolume,
                                          ItemVarianceQuantity = g.Key.CloseVolume -
                                          (g.Key.OpenVolume - g.Key.SalesVolume + g.Key.DeliveryVolume
                                                      + g.Key.AdjustmentVolume - g.Key.PumpTestVolumeNonReturned),
                                          ItemCategory = g.First().Category,
                                          PumpTestVolumeReturned = g.Key.PumpTestVolumeReturned,
                                          PumpTestVolumeNotReturned = g.Key.PumpTestVolumeNonReturned
                                        }).ToList();

      return groupedFuelItemSummaryData;
    }
  }

  public class FuelTankSummaryData
  {
    public Site Site { get; set; }
    public FuelItem FuelItem { get; set; }
    public DateTime BusinessDate { get; set; }
    public decimal? CloseVolume { get; set; }
    public decimal? OpenVolume { get; set; }
    public decimal? PumpTestVolumeNonReturned { get; set; }
    public decimal? SalesVolume { get; set; }
    public decimal? DeliveryVolume { get; set; }
    public decimal? PumpTestVolumeReturned { get; set; }
    public decimal? AdjustmentVolume { get; set; }
    public decimal? PumpedVolume { get; set; }
    public decimal? InvoiceQuantity { get; set; }
    public decimal? InvoiceCost { get; set; }
  }
}