using ByEsoDomainInfraStructure.UnitOfWork;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BYEsoDomainModelKernel.Models;

namespace FuelReconciliationDomainModel
{
  public interface IFuelMeterReadingSelectionService
  {
    Task<IEnumerable<FuelMeterReadingsData>> GetFuelMeterWacData(Site site, DateTime previousDatetime,
      IEnumerable<FuelHoseTankLogicalConnection> hoseTankLogicalConnection, IEnumerable<FuelDailySalesWithMeter> fuelSalesForEachMeter);
  }

  public class FuelMeterReadingSelectionService : IFuelMeterReadingSelectionService
  {
    private readonly IFuelTankMeterReadingLineItemRepository _FuelTankMeterReadingLineItemRepo;
    private readonly IDatamartFuelHoseRepository _DatamartFuelHoseRepo;

    private readonly IFuelHoseService _FuelHoseService;
    



    public FuelMeterReadingSelectionService(IUnitOfWork uow, IFuelTankMeterReadingLineItemRepository fuelTankMeterReadingLineItemRepo, IDatamartFuelHoseRepository datamartFuelHoseRepo, IFuelHoseService fuelHoseService)
    {
      _FuelTankMeterReadingLineItemRepo = fuelTankMeterReadingLineItemRepo;
      _DatamartFuelHoseRepo = datamartFuelHoseRepo;
      _FuelHoseService = fuelHoseService;
    }

    private async Task MassageFuelMeterReadValue(Site site)
    {
      var fuelTankReadingLineItems = await _FuelTankMeterReadingLineItemRepo.GetReadValueWithPrecisionMoreThanThree(site);
      (from tanksummaryDetail in fuelTankReadingLineItems select tanksummaryDetail).ToList()
        .ForEach(x => x.ReadValue = Math.Round(x.ReadValue, 3));
    }

    public async Task<IEnumerable<FuelMeterReadingsData>> GetFuelMeterWacData(Site site, DateTime previousDatetime,
      IEnumerable<FuelHoseTankLogicalConnection> hoseTankLogicalConnection, IEnumerable<FuelDailySalesWithMeter> fuelSalesForEachMeter)
    {
      await MassageFuelMeterReadValue(site);
      var index = 0;
      const double powTen = 10.0000000000;
      var lastWeekFuelTankReadingLineItemList = await _FuelTankMeterReadingLineItemRepo.GetLastWeekMeterReadings(site);

      var fuelMeterReadingData = (from lineItem in lastWeekFuelTankReadingLineItemList
        orderby lineItem.FuelMeter.ID, lineItem.Site.CurrentBusinessDate, lineItem.ReadTimestamp, lineItem.Type
        select new FuelMeterReadingsData
        {
          UniqueID = ++index,
          FuelMeterID = lineItem.FuelMeter.ID,
          BusinessDate = lineItem.FuelMeterReading.BusinessDate,
          ReadTimeStamp = lineItem.ReadTimestamp,
          LineItemTypeCode = (char)lineItem.Type,
          ReadValue = lineItem.ReadValue
        }).ToList();

      var deltaVlauesForMeter = fuelMeterReadingData.Zip(fuelMeterReadingData.Skip(1),
        (current, next) => new { current, next }).ToList().Select(x => new
      {
        x.next.UniqueID,
        DeltaValue = (x.current.FuelMeterID == x.next.FuelMeterID && x.current.UniqueID == x.next.UniqueID - 1 &&
                      x.current.LineItemTypeCode != (char)FuelTankMeterReadingLineItem.MeterReadingLineType.Reset) ?
          ((x.next.ReadValue < x.current.ReadValue) ?
            (decimal)Math.Pow(powTen, Math.Truncate(x.current.ReadValue).ToString().Length) -
            x.current.ReadValue + x.next.ReadValue : x.next.ReadValue - x.current.ReadValue) : 0
      }).ToList();

      fuelMeterReadingData.ForEach(x => x.Delta = deltaVlauesForMeter.Where(p => p.UniqueID == x.UniqueID)
                                     .Select(m => m.DeltaValue).FirstOrDefault());

      await AddDatamartFuelHoseTransactions(site, previousDatetime, hoseTankLogicalConnection, fuelSalesForEachMeter, fuelMeterReadingData);

      var dailyPumpedVolumeData = (from fuelHoseLogicalCon in hoseTankLogicalConnection
        join lastWeekReadingData in (from lastWeekReading in fuelMeterReadingData
            where lastWeekReading.BusinessDate == site.CurrentBusinessDate
            group lastWeekReading by new { lastWeekReading.FuelMeterID }
            into groupdataOld
            select new
            {
              MeterID = groupdataOld.Key.FuelMeterID,
              PumpedVolume = groupdataOld.Sum(x => x.Delta)
            }) on new { x1 = fuelHoseLogicalCon.FuelMeter.ID, x2 = fuelHoseLogicalCon.ElectronicFlag }
          equals new { x1 = lastWeekReadingData.MeterID, x2 = true }
        select new
        {
          fuelHoseLogicalCon.LogicalTankId,
          fuelHoseLogicalCon.PercentageFromLogicalTank,
          lastWeekReadingData.MeterID,
          lastWeekReadingData.PumpedVolume
        }).GroupBy(x => x.LogicalTankId).Select(g => new FuelMeterReadingsData()
      {
        FuelTankID = g.Key,
        PumpedVolume = g.Sum(x => x.PercentageFromLogicalTank * x.PumpedVolume)
      }).ToList();

      return dailyPumpedVolumeData;
    }

    private async Task AddDatamartFuelHoseTransactions(Site site, DateTime previousDatetime,
      IEnumerable<FuelHoseTankLogicalConnection> hoseTankLogicalConnection,
      IEnumerable<FuelDailySalesWithMeter> fuelSalesForEachMeter, IList<FuelMeterReadingsData> fuelMeterReadingData)
    {
      
      var currentDatedatamartFuelHoseList = await _DatamartFuelHoseRepo.GetDatamartFuelHoseBySiteAndDate(site, site.CurrentBusinessDate);
      var pcsFuelMeterTransactions = await _FuelHoseService.GetPcsFuelMeterTransactions(site, hoseTankLogicalConnection, fuelSalesForEachMeter);

      var meterReadingsPreviuosDay = FuelMeterReadingPreviousDay(fuelMeterReadingData, previousDatetime);
      foreach (var reading in meterReadingsPreviuosDay)
      {
        var pcsFuelMeterTransaction = pcsFuelMeterTransactions.FirstOrDefault(p => p.FuelHose.ID == reading.FuelMeterID);
        if (pcsFuelMeterTransaction != null)
        {
          pcsFuelMeterTransaction.OpenMeterReading = reading.ReadValue;
        }
      }

      var meterReadingsCurrentDay = FuelMeterReadingCurrentDay(fuelMeterReadingData, site);
      foreach (var reading in meterReadingsCurrentDay)
      {
        var pcsFuelMeterTransaction = pcsFuelMeterTransactions.FirstOrDefault(p => p.FuelHose.ID == reading.FuelMeterID);
        if (pcsFuelMeterTransaction != null)
        {
          pcsFuelMeterTransaction.CloseMeterReading = reading.ReadValue;
        }
      }

      var fuelMeterReadingLineItemData = (from meterReading in fuelMeterReadingData
                                          where meterReading.BusinessDate == site.CurrentBusinessDate
                                                && meterReading.LineItemTypeCode != (char)FuelTankMeterReadingLineItem.MeterReadingLineType.Reset
                                          group meterReading by new { meterReading.FuelMeterID } into groupData
                                          select new
                                          {
                                            groupData.Key.FuelMeterID,
                                            PumpedVolume = groupData.Sum(x => x.Delta)
                                          }).ToList();
      foreach (var reading in fuelMeterReadingLineItemData)
      {
        var pcsFuelMeterTransaction = pcsFuelMeterTransactions.FirstOrDefault(p => p.FuelHose.ID == reading.FuelMeterID);
        if (pcsFuelMeterTransaction != null)
        {
          pcsFuelMeterTransaction.PumpedVolume = reading.PumpedVolume;
        }
      }

      var fuelMeterReadingCalculatedTypeLineItemData = (from meterReading in fuelMeterReadingData
                                                        where meterReading.BusinessDate == site.CurrentBusinessDate
                                                              && meterReading.LineItemTypeCode == (char)FuelTankMeterReadingLineItem.MeterReadingLineType.Calculated
                                                        group meterReading by new { meterReading.FuelMeterID } into groupData
                                                        select new
                                                        {
                                                          groupData.Key.FuelMeterID,
                                                          AdjustedVolume = groupData.Sum(x => x.Delta)
                                                        }).ToList();
      foreach (var reading in fuelMeterReadingCalculatedTypeLineItemData)
      {
        var pcsFuelMeterTransaction = pcsFuelMeterTransactions.FirstOrDefault(p => p.FuelHose.ID == reading.FuelMeterID);
        if (pcsFuelMeterTransaction != null)
        {
          pcsFuelMeterTransaction.MeterAdjustment = reading.AdjustedVolume;
        }
      }

      foreach (var currentDatedatamartFuelHose in currentDatedatamartFuelHoseList)
      {
        await _DatamartFuelHoseRepo.Delete(currentDatedatamartFuelHose);
      }

      foreach (var pcsFuelMeterTransaction in pcsFuelMeterTransactions)
      {
        await _DatamartFuelHoseRepo.Add(pcsFuelMeterTransaction);
      }
    }

    private List<LastFuelMeterReadingData> FuelMeterReadingCurrentDay(IList<FuelMeterReadingsData> lastWeekFuelMeterReadingData, Site site)
    {
      var meterReadingCurrentDay = (from meterReading in lastWeekFuelMeterReadingData
        join meterReadingPrev in (from meterReadingold in lastWeekFuelMeterReadingData
          where meterReadingold.BusinessDate == site.CurrentBusinessDate
          group meterReadingold by new { meterReadingold.FuelMeterID } into groupdataOld
          select new
          {
            MeterID = groupdataOld.Key.FuelMeterID,
            MaxReadTimeStamp = groupdataOld.Max(x => x.ReadTimeStamp)
          }) on new
          { x1 = meterReading.FuelMeterID, x2 = meterReading.ReadTimeStamp } equals new
          { x1 = meterReadingPrev.MeterID, x2 = meterReadingPrev.MaxReadTimeStamp }
        where meterReading.BusinessDate == site.CurrentBusinessDate
        group meterReading by new { meterReading.FuelMeterID, meterReading.ReadTimeStamp }
        into groupedData
        select new LastFuelMeterReadingData
        {
          FuelMeterID = groupedData.Key.FuelMeterID,
          ReadValue = groupedData.Max(x => x.ReadValue)
        }).ToList();

      return meterReadingCurrentDay;
    }

    private List<LastFuelMeterReadingData> FuelMeterReadingPreviousDay(IList<FuelMeterReadingsData> lastWeekFuelMeterReadingData,
      DateTime previousDatetime)
    {
      var meterReadingsPreviuosDay = (from meterReading in lastWeekFuelMeterReadingData
        join meterReadingPrev in (from meterReadingold in lastWeekFuelMeterReadingData
          where meterReadingold.BusinessDate == previousDatetime
          group meterReadingold by new { meterReadingold.FuelMeterID } into groupdataOld
          select new
          {
            MeterID = groupdataOld.Key.FuelMeterID,
            MaxReadTimeStamp = groupdataOld.Max(x => x.ReadTimeStamp)
          }) on new
          { x1 = meterReading.FuelMeterID, x2 = meterReading.ReadTimeStamp } equals new
          { x1 = meterReadingPrev.MeterID, x2 = meterReadingPrev.MaxReadTimeStamp }
        where meterReading.BusinessDate == previousDatetime
        group meterReading by new { meterReading.FuelMeterID, meterReading.ReadTimeStamp }
        into groupedData
        select new LastFuelMeterReadingData
        {
          FuelMeterID = groupedData.Key.FuelMeterID,
          ReadValue = groupedData.Max(x => x.ReadValue)
        }).ToList();

      return meterReadingsPreviuosDay;
    }
  }
}