using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BYEsoDomainModelKernel.Models;

namespace FuelReconciliationDomainModel
{
  public interface IFuelTankLookupService
  {
    Task<PhysicalTankReadingDto> ProcessPhysicalTankReadingData(Site site, bool firstDate);
    Task<FuelTankReadingDto> GetWacForTankReadingsBySite(Site site, bool isFirstDay, List<FuelPhysicalTankReadingData> lists1, List<FuelPhysicalTankReadingData> lists2, List<FuelPhysicalTankReadingData> lists3, List<FuelPhysicalTankReadingData> lists4);
    Task<IEnumerable<FuelHoseTankLogicalConnection>> ProcessFuelBlendedProductData(Site site);
  }

  public class FuelTankLookupService : IFuelTankLookupService
  {
    private readonly IFuelTankRepository _FuelTankRepo;
    private readonly IItemRepository _ItemRepo;
    private readonly IFuelHoseRepository _FuelHoseRepo;
    private readonly IFuelBlendItemPercentageRepository _FuelBlendItemPercentageRepo;

    private readonly IFuelTankReadingSelectionService _FuelTankReadingSelectionService;
    
    public FuelTankLookupService(IFuelTankRepository fuelTankRepo, IItemRepository itemRepo, IFuelBlendItemPercentageRepository fuelBlendItemPercentageRepo, IFuelHoseRepository fuelHoseRepo, IFuelTankReadingSelectionService fuelTankReadingSelectionService)
    {
      _FuelTankRepo = fuelTankRepo;
      _ItemRepo = itemRepo;
      _FuelBlendItemPercentageRepo = fuelBlendItemPercentageRepo;
      _FuelHoseRepo = fuelHoseRepo;
      _FuelTankReadingSelectionService = fuelTankReadingSelectionService;
    }

    public async Task<PhysicalTankReadingDto> ProcessPhysicalTankReadingData(Site site, bool firstDate)
    {
      var response = new PhysicalTankReadingDto();
      IList<FuelPhysicalTankReadingData> firstPhysicalTankMinReadingPrepData = new List<FuelPhysicalTankReadingData>();
      
      var limitedData = await _FuelTankReadingSelectionService.GetLimitedFuelTankReadingData(site).ConfigureAwait(false);

      IList<FuelPhysicalTankReadingData> limitedFuelTankData = (from fueltankreadingData in limitedData
                                                                from fuelTankReadingLineItem in fueltankreadingData.LineItems
                                                                select new FuelPhysicalTankReadingData
                                                                {
                                                                  ReadingTypeCode = fueltankreadingData.Type,
                                                                  ReadTimeStamp = fuelTankReadingLineItem.GetReadingTimeStamp(),
                                                                  PhysicalFuelTankID = fuelTankReadingLineItem.FuelPhysicalTank.ID,
                                                                  ReadVolume = fuelTankReadingLineItem.TotalVolume - fuelTankReadingLineItem.WaterVolume
                                                                }).ToList();

      IList<FuelPhysicalTankReadingData> physicalTankMaxReadingData = limitedFuelTankData.GroupBy(x => x.PhysicalFuelTankID)
                                                                    .Select(g => new FuelPhysicalTankReadingData
                                                                    {
                                                                      PhysicalFuelTankID = g.Key,
                                                                      ReadTimeStamp = g.Max(x => x.ReadTimeStamp)
                                                                    }).ToList();


      IList<FuelPhysicalTankReadingData> physicalTankLargeVolumeReadingData =
                                                    (from limitData in limitedFuelTankData
                                                     join maxReadingData in physicalTankMaxReadingData
                                                    on new { x1 = limitData.PhysicalFuelTankID, x2 = limitData.ReadTimeStamp }
                                                    equals new { x1 = maxReadingData.PhysicalFuelTankID, x2 = maxReadingData.ReadTimeStamp }
                                                     group limitData by new
                                                     {
                                                       limitData.PhysicalFuelTankID,
                                                       limitData.ReadTimeStamp
                                                     } into groupedData
                                                     select new FuelPhysicalTankReadingData
                                                     {
                                                       PhysicalFuelTankID = groupedData.Key.PhysicalFuelTankID,
                                                       ReadTimeStamp = groupedData.Key.ReadTimeStamp,
                                                       ReadVolume = groupedData.Max(x => x.ReadVolume)
                                                     }).ToList();
      var fuelTanks = await _FuelTankRepo.GetFuelTanksBySite<FuelTank>(site).ConfigureAwait(false);
      var fuelPhysicalTanks = await _FuelTankRepo.GetFuelTanksBySite<FuelPhysicalTank>(site).ConfigureAwait(false);
      var items = await _ItemRepo.GetByIDs<FuelItem>(fuelTanks.Select(x => x.FuelItem.ID).Distinct().ToArray()).ConfigureAwait(false);

      response.LatestPhysicalTankReadingData = (from ft in fuelTanks
        join fpt in fuelPhysicalTanks on ft.ID equals fpt.ID
        join itm in items on fpt.FuelItem.ID equals itm.ID into fuelTankItemData
        from fti in fuelTankItemData
        join flvd in physicalTankLargeVolumeReadingData on fpt.ID
          equals flvd.PhysicalFuelTankID into finalData
        from fd in finalData.DefaultIfEmpty()
        where ft.Site.ID == site.ID
        select new FuelPhysicalTankReadingData
        {
          FuelTankID = ft.ID,
          FuelTankNumber = fpt.TankNumber,
          FuelInventoryItemID = ft.FuelItem.ID,
          FuelInventoryItemName = ft.FuelItem.Name,
          ReadTimeStamp = fd?.ReadTimeStamp,
          BusinessDate = site.CurrentBusinessDate,
          ReadVolume = fd?.ReadVolume
        }).ToList();

      if (firstDate)
      {
        firstPhysicalTankMinReadingPrepData = limitedFuelTankData.GroupBy(x => x.PhysicalFuelTankID)
          .Select(g => new FuelPhysicalTankReadingData
          {
            PhysicalFuelTankID = g.Key,
            ReadTimeStamp = g.Min(x => x.ReadTimeStamp)
          }).ToList();

        response.FirstPhysicalTankReadingData = (from limitData in limitedFuelTankData
          join minPrepData in firstPhysicalTankMinReadingPrepData
            on new { x1 = limitData.PhysicalFuelTankID, x2 = limitData.ReadTimeStamp }
            equals new { x1 = minPrepData.PhysicalFuelTankID, x2 = minPrepData.ReadTimeStamp }
          group limitData by new
          {
            limitData.PhysicalFuelTankID,
            limitData.ReadTimeStamp
          }
          into groupedData
          select new FuelPhysicalTankReadingData
          {
            PhysicalFuelTankID = groupedData.Key.PhysicalFuelTankID,
            ReadTimeStamp = groupedData.Key.ReadTimeStamp,
            ReadVolume = groupedData.Min(x => x.ReadVolume)
          }).ToList();
      }

      physicalTankMaxReadingData.Clear();
      physicalTankMaxReadingData = limitedFuelTankData.Where(x => x.ReadingTypeCode == TankReadingType.Audit)
                                    .GroupBy(x => x.PhysicalFuelTankID)
                                    .Select(g => new FuelPhysicalTankReadingData
                                    {
                                      PhysicalFuelTankID = g.Key,
                                      ReadTimeStamp = g.Max(x => x.ReadTimeStamp)
                                    }).ToList();

      physicalTankLargeVolumeReadingData.Clear();
      physicalTankLargeVolumeReadingData = (from limitData in limitedFuelTankData
                                            join maxReadingData in physicalTankMaxReadingData
                        on new { x1 = limitData.PhysicalFuelTankID, x2 = limitData.ReadTimeStamp, x3 = limitData.ReadingTypeCode }
                        equals new { x1 = maxReadingData.PhysicalFuelTankID, x2 = maxReadingData.ReadTimeStamp, x3 = TankReadingType.Audit }
                                            group limitData by new
                                            {
                                              limitData.PhysicalFuelTankID,
                                              limitData.ReadTimeStamp
                                            } into groupedData
                                            select new FuelPhysicalTankReadingData
                                            {
                                              PhysicalFuelTankID = groupedData.Key.PhysicalFuelTankID,
                                              ReadTimeStamp = groupedData.Key.ReadTimeStamp,
                                              ReadVolume = groupedData.Max(x => x.ReadVolume)
                                            }).ToList();

      response.LatestPhysicalTankReadingAuditData = (from ft in fuelTanks
                                            join fpt in fuelPhysicalTanks on ft.ID equals fpt.ID
                                            join itm in items on fpt.FuelItem.ID equals itm.ID into fuelTankItemData
                                            from fti in fuelTankItemData
                                            join flvd in physicalTankLargeVolumeReadingData on fpt.ID
                                            equals flvd.PhysicalFuelTankID into finalData
                                            from fd in finalData.DefaultIfEmpty()
                                            where ft.Site.ID == site.ID
                                            select new FuelPhysicalTankReadingData
                                            {
                                              FuelTankID = ft.ID,
                                              FuelTankNumber = fpt.TankNumber,
                                              FuelInventoryItemID = ft.FuelItem.ID,
                                              FuelInventoryItemName = ft.FuelItem.Name,
                                              ReadTimeStamp = fd?.ReadTimeStamp,
                                              BusinessDate = site.CurrentBusinessDate,
                                              ReadVolume = fd?.ReadVolume
                                            }).ToList();

      if (firstDate)
      {
        firstPhysicalTankMinReadingPrepData.Clear();
        firstPhysicalTankMinReadingPrepData = limitedFuelTankData.Where(x => x.ReadingTypeCode == TankReadingType.Audit)
                                              .GroupBy(x => x.PhysicalFuelTankID)
                                              .Select(g => new FuelPhysicalTankReadingData
                                              {
                                                PhysicalFuelTankID = g.Key,
                                                ReadTimeStamp = g.Min(x => x.ReadTimeStamp)
                                              }).ToList();

        response.FirstPhysicalTankReadingAuditData = (from limitData in limitedFuelTankData
                                             join minPrepData in firstPhysicalTankMinReadingPrepData
                                             on new { x1 = limitData.PhysicalFuelTankID, x2 = limitData.ReadTimeStamp, x3 = limitData.ReadingTypeCode }
                                             equals new { x1 = minPrepData.PhysicalFuelTankID, x2 = minPrepData.ReadTimeStamp, x3 = TankReadingType.Audit }
                                             group limitData by new
                                             {
                                               limitData.PhysicalFuelTankID,
                                               limitData.ReadTimeStamp
                                             } into groupedData
                                             select new FuelPhysicalTankReadingData
                                             {
                                               PhysicalFuelTankID = groupedData.Key.PhysicalFuelTankID,
                                               ReadTimeStamp = groupedData.Key.ReadTimeStamp,
                                               ReadVolume = groupedData.Min(x => x.ReadVolume)
                                             }).ToList();
      }

      return response;
    }

    
    public async Task<FuelTankReadingDto> GetWacForTankReadingsBySite(Site site, bool isFirstDay,
      List<FuelPhysicalTankReadingData> latestPhysicalTankReadingData,
      List<FuelPhysicalTankReadingData> firstPhysicalTankReadingData,
      List<FuelPhysicalTankReadingData> latestPhysicalTankReadingAuditData,
      List<FuelPhysicalTankReadingData> firstPhysicalTankReadingAuditData)
    {

      var fuelManifoldTanks = await _FuelTankRepo.GetFuelTanksBySite<FuelManifoldTank>(site);
      var manifoldTanks = fuelManifoldTanks as IList<FuelManifoldTank> ?? fuelManifoldTanks.ToList();
      var responseData = new FuelTankReadingDto();

      var tankReadingsManifoldLatest = (from fuelManifoldTank in manifoldTanks
        from physicalTank in fuelManifoldTank.PhysicalTanks
        join fuelPhysicalTankReading in latestPhysicalTankReadingData on physicalTank.ID equals fuelPhysicalTankReading.FuelTankID
          into fuelTankReadingsGroups
        from fuelTankReadingsGroup in fuelTankReadingsGroups
        where fuelTankReadingsGroup.BusinessDate == site.CurrentBusinessDate
        group fuelTankReadingsGroup by new
        {
          ManifoldTankID = physicalTank.ManifoldTank.ID
        }
        into newResult
        select new TankReadingsManifold()
        {
          FuelTankId = newResult.Key.ManifoldTankID,
          ReadTimeStamp = newResult.Distinct().Select(x => x.ReadTimeStamp).Min(),
          Volume = newResult.Distinct().Any(x => x.ReadVolume.HasValue) ?
            newResult.Distinct().Select(x => x.ReadVolume).Sum() : null
        }).ToList();
      responseData.TanksReadingsLatest.AddRange(tankReadingsManifoldLatest);

      var tankReadingsNonManifoldLatest = (from physicalTankReadingLatest in latestPhysicalTankReadingData
        where physicalTankReadingLatest.BusinessDate == site.CurrentBusinessDate &&
              !(from fuelManifoldTank in manifoldTanks
                  from physicalTank in fuelManifoldTank.PhysicalTanks
                  join fuelPhysicalTankReading in latestPhysicalTankReadingData on physicalTank.ID
                    equals fuelPhysicalTankReading.FuelTankID
                  select physicalTank.ID
                ).Contains(physicalTankReadingLatest.FuelTankID)
        select new TankReadingsManifold()
        {
          FuelTankId = physicalTankReadingLatest.FuelTankID,
          ReadTimeStamp = physicalTankReadingLatest.ReadTimeStamp,
          Volume = physicalTankReadingLatest.ReadVolume
        }).ToList();
      responseData.TanksReadingsLatest.AddRange(tankReadingsNonManifoldLatest);

      if (isFirstDay)
      {
        var tankReadingsManifoldFirst = (from fuelManifoldTank in manifoldTanks
                                         from physicalTank in fuelManifoldTank.PhysicalTanks
                                         join fuelPhysicalTankReading in firstPhysicalTankReadingData on physicalTank.ID
                                         equals fuelPhysicalTankReading.PhysicalFuelTankID
                                         into fuelTankReadingsGroups
                                         from fuelTankReadingsGroup in fuelTankReadingsGroups
                                         group fuelTankReadingsGroup by new
                                         {
                                           ManifoldTankID = physicalTank.ManifoldTank.ID
                                         }
          into newResult
                                         select new TankReadingsManifold()
                                         {
                                           FuelTankId = newResult.Key.ManifoldTankID,
                                           ReadTimeStamp = newResult.Distinct().Select(x => x.ReadTimeStamp).Min(),
                                           Volume = newResult.Distinct().Any(x => x.ReadVolume.HasValue) ?
                                     newResult.Distinct().Select(x => x.ReadVolume).Sum() : null
                                         }).ToList();
        responseData.TanksReadingsFirst.AddRange(tankReadingsManifoldFirst);

        var tankReadingsNonManifoldFirst = (from physicalTankReadingFirst in firstPhysicalTankReadingData
                                            where !(from fuelManifoldTank in manifoldTanks
                                                    from physicalTank in fuelManifoldTank.PhysicalTanks
                                                    join fuelPhysicalTankReading in firstPhysicalTankReadingData on physicalTank.ID
                                                    equals fuelPhysicalTankReading.PhysicalFuelTankID
                                                    select physicalTank.ID
                                          ).Contains(physicalTankReadingFirst.PhysicalFuelTankID)
                                            select new TankReadingsManifold()
                                            {
                                              FuelTankId = physicalTankReadingFirst.PhysicalFuelTankID,
                                              ReadTimeStamp = physicalTankReadingFirst.ReadTimeStamp,
                                              Volume = physicalTankReadingFirst.ReadVolume
                                            }).ToList();
        responseData.TanksReadingsFirst.AddRange(tankReadingsNonManifoldFirst);

        var tankReadingsManifoldAuditFirst = (from fuelManifoldTank in manifoldTanks
                                              from physicalTank in fuelManifoldTank.PhysicalTanks
                                              join fuelPhysicalTankReading in firstPhysicalTankReadingAuditData on physicalTank.ID
                                              equals fuelPhysicalTankReading.PhysicalFuelTankID into fuelTankReadingsGroups
                                              from fuelTankReadingsGroup in fuelTankReadingsGroups
                                              group fuelTankReadingsGroup by new
                                              {
                                                ManifoldTankID = physicalTank.ManifoldTank.ID
                                              }
          into newResult
                                              select new TankReadingsManifold()
                                              {
                                                FuelTankId = newResult.Key.ManifoldTankID,
                                                ReadTimeStamp = newResult.Distinct().Select(x => x.ReadTimeStamp).Min(),
                                                Volume = newResult.Distinct().Any(x => x.ReadVolume.HasValue) ?
                                          newResult.Distinct().Select(x => x.ReadVolume).Sum() : null
                                              }).ToList();
        responseData.TanksReadingsAuditFirst.AddRange(tankReadingsManifoldAuditFirst);

        var tankReadingsNonManifoldAuditFirst = (from physicalTankReadingFirst in firstPhysicalTankReadingAuditData
                                                 where !(from fuelManifoldTank in manifoldTanks
                                                         from physicalTank in fuelManifoldTank.PhysicalTanks
                                                         join fuelPhysicalTankReading in firstPhysicalTankReadingAuditData on physicalTank.ID
                                                         equals fuelPhysicalTankReading.PhysicalFuelTankID
                                                         select physicalTank.ID
                                               ).Contains(physicalTankReadingFirst.PhysicalFuelTankID)
                                                 select new TankReadingsManifold()
                                                 {
                                                   FuelTankId = physicalTankReadingFirst.PhysicalFuelTankID,
                                                   ReadTimeStamp = physicalTankReadingFirst.ReadTimeStamp,
                                                   Volume = physicalTankReadingFirst.ReadVolume
                                                 }).ToList();
        responseData.TanksReadingsAuditFirst.AddRange(tankReadingsNonManifoldAuditFirst);
      }

      var tankReadingsManifoldaudit = (from fuelManifoldTank in manifoldTanks
                                       from physicalTank in fuelManifoldTank.PhysicalTanks
                                       join fuelPhysicalTankReading in latestPhysicalTankReadingAuditData on physicalTank.ID
                                       equals fuelPhysicalTankReading.FuelTankID
                                       into fuelTankReadingsGroups
                                       from fuelTankReadingsGroup in fuelTankReadingsGroups
                                       where fuelTankReadingsGroup.BusinessDate == site.CurrentBusinessDate
                                       group fuelTankReadingsGroup by new
                                       {
                                         ManifoldTankID = physicalTank.ManifoldTank.ID
                                       }
        into newResult
                                       select new TankReadingsManifold()
                                       {
                                         FuelTankId = newResult.Key.ManifoldTankID,
                                         ReadTimeStamp = newResult.Distinct().Select(x => x.ReadTimeStamp).Min(),
                                         Volume = newResult.Distinct().Any(x => x.ReadVolume.HasValue) ?
                                   newResult.Distinct().Select(x => (int?)x.ReadVolume).Sum() : null
                                       }).ToList();
      responseData.TanksReadingsAudit.AddRange(tankReadingsManifoldaudit);

      var tankReadingsNonManifoldAudit = (from physicalTankReadingLatest in latestPhysicalTankReadingAuditData
                                          where physicalTankReadingLatest.BusinessDate == site.CurrentBusinessDate && !(from fuelManifoldTank in manifoldTanks
                                                                                                                        from physicalTank in fuelManifoldTank.PhysicalTanks
                                                                                                                        join fuelPhysicalTankReading in latestPhysicalTankReadingAuditData on physicalTank.ID
                                                                                                                        equals fuelPhysicalTankReading.FuelTankID
                                                                                                                        select physicalTank.ID
                                        ).Contains(physicalTankReadingLatest.FuelTankID)
                                          select new TankReadingsManifold()
                                          {
                                            FuelTankId = physicalTankReadingLatest.FuelTankID,
                                            ReadTimeStamp = physicalTankReadingLatest.ReadTimeStamp,
                                            Volume = physicalTankReadingLatest.ReadVolume
                                          }).ToList();
      responseData.TanksReadingsAudit.AddRange(tankReadingsNonManifoldAudit);
      return responseData;
    }

    public async Task<IEnumerable<FuelHoseTankLogicalConnection>> ProcessFuelBlendedProductData(Site site)
    {
      var fuelBlendedItemPercent = await _FuelBlendItemPercentageRepo.GetFuelBlendItemPercentageBySiteAndTanks(site);
      var fuelHose = await _FuelHoseRepo.GetBySite(site);

      IEnumerable<FuelHoseTankLogicalConnection> FuelHoseTankLogicalConnection = (from fm in fuelHose
                                       join mfm in fuelHose on
                                       (new { x1 = fm.ID - 1, x2 = fm.Site.ID })
                                       equals (new { x1 = mfm.ID, x2 = mfm.Site.ID }) into hoseData
                                       from tempmfm in hoseData.DefaultIfEmpty()
                                       join fbp in fuelBlendedItemPercent on
                                             (new { x2 = fm.FuelItem.ID })
                                             equals (new
                                             {
                                               x2 = fbp?.FuelBlendedItem?.ID ?? 0
                                             }) into fuelBlendedItemPercentData
                                       from tempfbp in fuelBlendedItemPercentData.DefaultIfEmpty()

                                       select new FuelHoseTankLogicalConnection
                                       {
                                         RetailItemId = fm.FuelItem.ID,
                                         PumpNumber = fm.FuelPump.PumpNumber,
                                         FuelPump = fm.FuelPump,
                                         HoseNumber = fm.IsElectronic ? fm.HoseNumber : tempmfm.HoseNumber,
                                         FuelMeter = fm,
                                         LogicalTankId = fm.FuelHoseTankAssignment.First()?.FuelTank.ID ?? 0,
                                         PercentageFromLogicalTank = (tempfbp?.FuelBlendPercentage ?? 0) == 0 ?
                                         1 : tempfbp.FuelBlendPercentage / 100,
                                         ElectronicFlag = fm.IsElectronic
                                       }).ToList();
      return FuelHoseTankLogicalConnection;
    }
  }

  public class FuelTankReadingDto
  {
    public List<TankReadingsManifold> TanksReadingsLatest { get; set; }
    public List<TankReadingsManifold> TanksReadingsFirst { get; set; }
    public List<TankReadingsManifold> TanksReadingsAudit { get; set; }
    public List<TankReadingsManifold> TanksReadingsAuditFirst { get; set; }
    //public List<FuelHoseTankLogicalConnection> FuelHoseTankLogicalConnection { get; set; }
    public FuelTankReadingDto()
    {
      TanksReadingsAudit = new List<TankReadingsManifold>();
      TanksReadingsAuditFirst = new List<TankReadingsManifold>();
      TanksReadingsFirst = new List<TankReadingsManifold>();
      TanksReadingsLatest = new List<TankReadingsManifold>();
    }
  }

  public class PhysicalTankReadingDto
  {
    public PhysicalTankReadingDto()
    {
      LatestPhysicalTankReadingData = new List<FuelPhysicalTankReadingData>();
      FirstPhysicalTankReadingData = new List<FuelPhysicalTankReadingData>();
      LatestPhysicalTankReadingAuditData = new List<FuelPhysicalTankReadingData>();
      FirstPhysicalTankReadingAuditData = new List<FuelPhysicalTankReadingData>();
    }
    public List<FuelPhysicalTankReadingData> LatestPhysicalTankReadingData { get; set; }
    public List<FuelPhysicalTankReadingData> FirstPhysicalTankReadingData { get; set; }
    public List<FuelPhysicalTankReadingData> LatestPhysicalTankReadingAuditData { get; set; }
    public List<FuelPhysicalTankReadingData> FirstPhysicalTankReadingAuditData { get; set; }
  }
}