using BYEsoDomainModelKernel.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuelReconciliationDomainModel
{
  public interface IFuelAdjustmentSelectionService
  {
    Task<FuelAdjustmentsDto> FetchWacForAdjustments(Site site, DateTime currentBusinessDate, PhysicalTankReadingDto physicalTankReadings);
  }
  public class FuelAdjustmentSelectionService : IFuelAdjustmentSelectionService
  {
    private readonly IFuelAdjustmentRepository _FuelAdjustmentRepo;
    private readonly IFuelTankRepository _FuelTankRepo;


    public FuelAdjustmentSelectionService(IFuelAdjustmentRepository fuelAdjustmentRepo, IFuelTankRepository fuelTankRepo)
    {
      _FuelAdjustmentRepo = fuelAdjustmentRepo;
      _FuelTankRepo = fuelTankRepo;
    }

    public async Task<FuelAdjustmentsDto> FetchWacForAdjustments(Site site, DateTime currentBusinessDate, PhysicalTankReadingDto physicalTankReadings)
    {
      var fuelAdjustmentResponse = new FuelAdjustmentsDto();
      var fuelAdjustments = await _FuelAdjustmentRepo.GetAdjustmentWithLineItemsBySite(site, currentBusinessDate);

      
      
      var fuelManifoldTanks = await _FuelTankRepo.GetFuelTanksBySite<FuelManifoldTank>(site);
      var manifoldTanks = fuelManifoldTanks as IList<FuelManifoldTank> ?? fuelManifoldTanks.ToList();

      var dailyTankAdjustmentsByTank = (from adjustment in fuelAdjustments
                                        from lineItem in adjustment.LineItems
                                        join fuelPhysicalTankReading in physicalTankReadings.LatestPhysicalTankReadingData on lineItem.FuelPhysicalTank.ID
                                        equals fuelPhysicalTankReading.FuelTankID
                                        into fuelTankAdjustmentsGroups
                                        from fuelTankAdjustmentGroup in fuelTankAdjustmentsGroups
                                        where fuelTankAdjustmentGroup.BusinessDate == currentBusinessDate
                                        select new
                                        {
                                          lineItem,
                                          fuelTankAdjustmentGroup
                                        }).Distinct()
                                 .GroupBy(x => x.fuelTankAdjustmentGroup.FuelTankID)
                                 .Select(g => new
                                 {
                                   FuelTankID = g.Key,
                                   Volume = g.Distinct().Sum(x => x.lineItem.Volume)
                                 }).ToList();

      var tankAdjustmentsManifoldLatest = (from fuelManifoldTank in manifoldTanks
                                           from physicalTank in fuelManifoldTank.PhysicalTanks
                                           join fuelTankAdjustments in dailyTankAdjustmentsByTank
                                           on physicalTank.ID equals fuelTankAdjustments.FuelTankID
                                           into fuelTankReadingsGroups
                                           from fuelTankReadingsGroup in fuelTankReadingsGroups
                                           group fuelTankReadingsGroup by new
                                           {
                                             ManifoldTankID = physicalTank.ManifoldTank.ID
                                           }
                                          into newResult
                                           select new TankDeliveriesAndAdjustments()
                                           {
                                             FuelTankId = newResult.Key.ManifoldTankID,
                                             Volume = newResult.Distinct().Select(x => x?.Volume).Sum()
                                           }).ToList();

      fuelAdjustmentResponse.TankAdjustments.AddRange(tankAdjustmentsManifoldLatest);

      var tankAdjustmentsNonManifoldLatest = (from dailyTankAdjustment in dailyTankAdjustmentsByTank
                                              where !(from fuelManifoldTank in manifoldTanks
                                                      from physicalTank in fuelManifoldTank.PhysicalTanks
                                                      join fuelTankAdjustments in dailyTankAdjustmentsByTank
                                                      on physicalTank.ID equals fuelTankAdjustments.FuelTankID
                                                      select physicalTank.ID
                                              ).Contains(dailyTankAdjustment.FuelTankID)
                                              select new TankDeliveriesAndAdjustments()
                                              {
                                                FuelTankId = dailyTankAdjustment.FuelTankID,
                                                Volume = dailyTankAdjustment.Volume
                                              }).ToList();

      fuelAdjustmentResponse.TankAdjustments.AddRange(tankAdjustmentsNonManifoldLatest);

      var tankAdjustmentsByTankAfterStickReading = (from adjustment in fuelAdjustments
                                                    from lineItem in adjustment.LineItems
                                                    join fuelPhysicalTankReading in physicalTankReadings.LatestPhysicalTankReadingData
                                                    on lineItem.FuelPhysicalTank.ID
                                                    equals fuelPhysicalTankReading.FuelTankID
                                                    into fuelTankAdjustmentsGroups
                                                    from fuelTankAdjustmentGroup in fuelTankAdjustmentsGroups
                                                    where fuelTankAdjustmentGroup.BusinessDate == currentBusinessDate
                                                          && adjustment.AdjustmentDate > fuelTankAdjustmentGroup.ReadTimeStamp
                                                    select new
                                                    {
                                                      lineItem,
                                                      fuelTankAdjustmentGroup
                                                    }).Distinct()
                                                  .GroupBy(x => x.fuelTankAdjustmentGroup.FuelTankID)
                                                  .Select(g => new
                                                  {
                                                    FuelTankID = g.Key,
                                                    Volume = g.Distinct().Sum(x => x.lineItem.Volume)
                                                  }).ToList();

      var tankAdjustmentManifoldAfterStickReading = (from fuelManifoldTank in manifoldTanks
                                                     from physicalTank in fuelManifoldTank.PhysicalTanks
                                                     join fuelTankAdjsutments in tankAdjustmentsByTankAfterStickReading
                                                     on physicalTank.ID equals fuelTankAdjsutments.FuelTankID
                                                     into fuelTankReadingsGroups
                                                     from fuelTankReadingsGroup in fuelTankReadingsGroups
                                                     group fuelTankReadingsGroup by new
                                                     {
                                                       ManifoldTankID = physicalTank.ManifoldTank.ID
                                                     }
                                                     into newResult
                                                     select new TankDeliveriesAndAdjustments()
                                                     {
                                                       FuelTankId = newResult.Key.ManifoldTankID,
                                                       Volume = newResult.Distinct().Select(x => x?.Volume).Sum()
                                                     }).ToList();

      fuelAdjustmentResponse.TankAdjustmentsAfterStickReading.AddRange(tankAdjustmentManifoldAfterStickReading);

      var tankAdjustmentsNonManifoldAfterStickReading = (from dailyTankAdjustment in tankAdjustmentsByTankAfterStickReading
                                                         where !(from fuelManifoldTank in manifoldTanks
                                                                 from physicalTank in fuelManifoldTank.PhysicalTanks
                                                                 join fuelTankAdjustments in tankAdjustmentsByTankAfterStickReading
                                                                 on physicalTank.ID equals fuelTankAdjustments.FuelTankID
                                                                 select physicalTank.ID
                                                         ).Contains(dailyTankAdjustment.FuelTankID)
                                                         select new TankDeliveriesAndAdjustments()
                                                         {
                                                           FuelTankId = dailyTankAdjustment.FuelTankID,
                                                           Volume = dailyTankAdjustment.Volume
                                                         }).ToList();

      fuelAdjustmentResponse.TankAdjustmentsAfterStickReading.AddRange(tankAdjustmentsNonManifoldAfterStickReading);

      var tankAdjustmentWithAuditByTank = (from adjustment in fuelAdjustments
                                           from lineItem in adjustment.LineItems
                                           join fuelPhysicalTankReading in physicalTankReadings.FirstPhysicalTankReadingAuditData
                                                on lineItem.FuelPhysicalTank.ID
                                           equals fuelPhysicalTankReading.FuelTankID
                                           into fuelTankAdjustmentsGroups
                                           from fuelTankAdjustmentGroup in fuelTankAdjustmentsGroups
                                           where fuelTankAdjustmentGroup.BusinessDate == currentBusinessDate
                                                 && adjustment.AdjustmentDate > fuelTankAdjustmentGroup.ReadTimeStamp
                                           select new
                                           {
                                             lineItem,
                                             fuelTankAdjustmentGroup
                                           }).Distinct()
                                    .GroupBy(x => x.fuelTankAdjustmentGroup.FuelTankID)
                                    .Select(g => new
                                    {
                                      FuelTankID = g.Key,
                                      Volume = g.Distinct().Sum(x => x.lineItem.Volume)
                                    }).ToList();

      var tankAdjustmentsAuditForManifoldTanks = (from fuelManifoldTank in manifoldTanks
                                                  from physicalTank in fuelManifoldTank.PhysicalTanks
                                                  join fuelTankAdjustments in tankAdjustmentWithAuditByTank
                                                  on physicalTank.ID equals fuelTankAdjustments.FuelTankID
                                                  into fuelTankReadingsGroups
                                                  from fuelTankReadingsGroup in fuelTankReadingsGroups
                                                  group fuelTankReadingsGroup by new
                                                  {
                                                    ManifoldTankID = physicalTank.ManifoldTank.ID
                                                  }
                                                into newResult
                                                  select new TankDeliveriesAndAdjustments()
                                                  {
                                                    FuelTankId = newResult.Key.ManifoldTankID,
                                                    Volume = newResult.Distinct().Select(x => x?.Volume).Sum()
                                                  }).ToList();

      fuelAdjustmentResponse.TankAdjustmentsAuditData.AddRange(tankAdjustmentsAuditForManifoldTanks);

      var tankAdjustmentsAuditForNonManifoldTanks = (from dailyTankAdjustment in tankAdjustmentWithAuditByTank
                                                     where !(from fuelManifoldTank in manifoldTanks
                                                             from physicalTank in fuelManifoldTank.PhysicalTanks
                                                             join fuelTankAdjustments in tankAdjustmentWithAuditByTank
                                                             on physicalTank.ID equals fuelTankAdjustments.FuelTankID
                                                             select physicalTank.ID
                                                     ).Contains(dailyTankAdjustment.FuelTankID)
                                                     select new TankDeliveriesAndAdjustments()
                                                     {
                                                       FuelTankId = dailyTankAdjustment.FuelTankID,
                                                       Volume = dailyTankAdjustment.Volume
                                                     }).ToList();

      fuelAdjustmentResponse.TankAdjustmentsAuditData.AddRange(tankAdjustmentsAuditForNonManifoldTanks);
      return fuelAdjustmentResponse;
    }
  }

  public class FuelAdjustmentsDto
  {
    public FuelAdjustmentsDto()
    {
      TankAdjustments = new List<TankDeliveriesAndAdjustments>();
      TankAdjustmentsAfterStickReading = new List<TankDeliveriesAndAdjustments>();
      TankAdjustmentsAuditData = new List<TankDeliveriesAndAdjustments>();
    }

    public List<TankDeliveriesAndAdjustments> TankAdjustments;
    public List<TankDeliveriesAndAdjustments> TankAdjustmentsAfterStickReading;
    public List<TankDeliveriesAndAdjustments> TankAdjustmentsAuditData;
  }
}