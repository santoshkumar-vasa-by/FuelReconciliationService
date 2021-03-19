using BYEsoDomainModelKernel.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuelReconciliationDomainModel
{
  public interface IFuelDeliverySelectionService
  {
    Task<FuelDeliveryDto> FetchWacForReceivings(Site site, DateTime currentDate,
      List<FuelPhysicalTankReadingData> latestPhysicalTankReadingData,
      List<FuelPhysicalTankReadingData> latestPhysicalTankReadingAuditData);
  }

  public class FuelDeliverySelectionService : IFuelDeliverySelectionService
  {
    private readonly IFuelDeliveryRepository _FuelDeliveryRepo;
    private readonly IFuelTankRepository _FuelTankRepo;

    public FuelDeliverySelectionService(IFuelDeliveryRepository fuelDeliveryRepo, IFuelTankRepository fuelTankRepo)
    {
      _FuelDeliveryRepo = fuelDeliveryRepo;
      _FuelTankRepo = fuelTankRepo;
    }

    public async Task<FuelDeliveryDto> FetchWacForReceivings(Site site, DateTime currentDate,
      List<FuelPhysicalTankReadingData> latestPhysicalTankReadingData,
      List<FuelPhysicalTankReadingData> latestPhysicalTankReadingAuditData)
    {
      var fuelDeliverResponse = new FuelDeliveryDto();
      var deliveries = await _FuelDeliveryRepo.GetDeliveryWithLineItemsBySiteAndBusinessDate(site, currentDate);
      //TODO: need? don't need?
      //var deliveries = fuelDeliveries as IList<FuelDelivery> ?? fuelDeliveries.ToList();
      var manifoldTanks = await _FuelTankRepo.GetFuelTanksBySite<FuelManifoldTank>(site);
      //var manifoldTanks = fuelManifoldTanks as IList<FuelManifoldTank> ?? fuelManifoldTanks.ToList();

      var dailyTankDeliveriesByTank = (from delivery in deliveries
          from lineItem in delivery.LineItems
          join fuelPhysicalTankReading in latestPhysicalTankReadingData on lineItem.FuelPhysicalTank.ID
            equals fuelPhysicalTankReading.FuelTankID
            into fuelTankDeliveryGroups
          from fuelTankDeliveryGroup in fuelTankDeliveryGroups
          where fuelTankDeliveryGroup.BusinessDate == currentDate
          select new
          {
            lineItem,
            fuelTankDeliveryGroup
          }).Distinct()
        .GroupBy(x => x.fuelTankDeliveryGroup.FuelTankID)
        .Select(g => new
        {
          FuelTankID = g.Key,
          Volume = g.Distinct().Sum(x => x.lineItem.NetVolume)
        }).ToList();
      var tankDeliveriesManifoldLatest = (from fuelManifoldTank in manifoldTanks
                                          from physicalTank in fuelManifoldTank.PhysicalTanks
                                          join fuelTankDeliviries in dailyTankDeliveriesByTank
                                          on physicalTank.ID equals fuelTankDeliviries.FuelTankID
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

      fuelDeliverResponse.TankDeliveries.AddRange(tankDeliveriesManifoldLatest);

      var tankDeliveriesNonManifoldLatest = (from dailyTankDelivery in dailyTankDeliveriesByTank
                                             where !(from fuelManifoldTank in manifoldTanks
                                                     from physicalTank in fuelManifoldTank.PhysicalTanks
                                                     join fuelTankDeliviries in dailyTankDeliveriesByTank
                                                     on physicalTank.ID equals fuelTankDeliviries.FuelTankID
                                                     select physicalTank.ID
                                             ).Contains(dailyTankDelivery.FuelTankID)
                                             select new TankDeliveriesAndAdjustments()
                                             {
                                               FuelTankId = dailyTankDelivery.FuelTankID,
                                               Volume = dailyTankDelivery.Volume
                                             }).ToList();

      fuelDeliverResponse.TankDeliveries.AddRange(tankDeliveriesNonManifoldLatest);

      var tankDeliveriesByTankAfterStickReading = (from delivery in deliveries
                                                   from lineItem in delivery.LineItems
                                                   join fuelPhysicalTankReading in latestPhysicalTankReadingData on lineItem.FuelPhysicalTank.ID
                                                   equals fuelPhysicalTankReading.FuelTankID
                                                   into fuelTankDeliveryGroups
                                                   from fuelTankDeliveryGroup in fuelTankDeliveryGroups
                                                   where fuelTankDeliveryGroup.BusinessDate == currentDate
                                                         && delivery.DeliveryTimestamp > fuelTankDeliveryGroup.ReadTimeStamp
                                                   select new
                                                   {
                                                     lineItem,
                                                     fuelTankDeliveryGroup
                                                   }).Distinct()
        .GroupBy(x => x.fuelTankDeliveryGroup.FuelTankID)
        .Select(g => new
        {
          FuelTankID = g.Key,
          Volume = g.Distinct().Sum(x => x.lineItem.NetVolume)
        }).ToList();

      var tankDeliveriesManifoldAfterStickReading = (from fuelManifoldTank in manifoldTanks
                                                     from physicalTank in fuelManifoldTank.PhysicalTanks
                                                     join fuelTankDeliviries in tankDeliveriesByTankAfterStickReading
                                                     on physicalTank.ID equals fuelTankDeliviries.FuelTankID
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

      fuelDeliverResponse.TankDeliveriesAfterStickReading.AddRange(tankDeliveriesManifoldAfterStickReading);

      var tankDeliveriesNonManifoldAfterStickReading = (from dailyTankDelivery in tankDeliveriesByTankAfterStickReading
                                                        where !(from fuelManifoldTank in manifoldTanks
                                                                from physicalTank in fuelManifoldTank.PhysicalTanks
                                                                join fuelTankDeliviries in tankDeliveriesByTankAfterStickReading
                                                                on physicalTank.ID equals fuelTankDeliviries.FuelTankID
                                                                select physicalTank.ID
                                                        ).Contains(dailyTankDelivery.FuelTankID)
                                                        select new TankDeliveriesAndAdjustments()
                                                        {
                                                          FuelTankId = dailyTankDelivery.FuelTankID,
                                                          Volume = dailyTankDelivery.Volume
                                                        }).ToList();

      fuelDeliverResponse.TankDeliveriesAfterStickReading.AddRange(tankDeliveriesNonManifoldAfterStickReading);

      var tankDeliverieWithAuditByTank = (from delivery in deliveries
                                          from lineItem in delivery.LineItems
                                          join fuelPhysicalTankReading in latestPhysicalTankReadingAuditData on lineItem.FuelPhysicalTank.ID
                                          equals fuelPhysicalTankReading.FuelTankID
                                          into fuelTankDeliveryGroups
                                          from fuelTankDeliveryGroup in fuelTankDeliveryGroups
                                          where fuelTankDeliveryGroup.BusinessDate == currentDate
                                                && delivery.DeliveryTimestamp > fuelTankDeliveryGroup.ReadTimeStamp
                                          select new
                                          {
                                            lineItem,
                                            fuelTankDeliveryGroup
                                          }).Distinct()
        .GroupBy(x => x.fuelTankDeliveryGroup.FuelTankID)
        .Select(g => new
        {
          FuelTankID = g.Key,
          Volume = g.Distinct().Sum(x => x.lineItem.NetVolume)
        }).ToList();

      var tankDeliverieAuditForManifoldTanks = (from fuelManifoldTank in manifoldTanks
                                                from physicalTank in fuelManifoldTank.PhysicalTanks
                                                join fuelTankDeliveries in tankDeliverieWithAuditByTank
                                                on physicalTank.ID equals fuelTankDeliveries.FuelTankID
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

      fuelDeliverResponse.TankDeliveriesAuditData.AddRange(tankDeliverieAuditForManifoldTanks);

      var tankDeliverieAuditForNonManifoldTanks = (from dailyTankDelivery in tankDeliverieWithAuditByTank
                                                   where !(from fuelManifoldTank in manifoldTanks
                                                           from physicalTank in fuelManifoldTank.PhysicalTanks
                                                           join fuelTankDeliveries in tankDeliverieWithAuditByTank
                                                           on physicalTank.ID equals fuelTankDeliveries.FuelTankID
                                                           select physicalTank.ID
                                                   ).Contains(dailyTankDelivery.FuelTankID)
                                                   select new TankDeliveriesAndAdjustments()
                                                   {
                                                     FuelTankId = dailyTankDelivery.FuelTankID,
                                                     Volume = dailyTankDelivery.Volume
                                                   }).ToList();

      fuelDeliverResponse.TankDeliveriesAuditData.AddRange(tankDeliverieAuditForNonManifoldTanks);
      
      return fuelDeliverResponse;
    }
  }

  public class FuelDeliveryDto
  {
    public List<TankDeliveriesAndAdjustments> TankDeliveries { get; set; }
    public List<TankDeliveriesAndAdjustments> TankDeliveriesAfterStickReading { get; set; }
    public List<TankDeliveriesAndAdjustments> TankDeliveriesAuditData { get; set; }

    public FuelDeliveryDto()
    {
      TankDeliveries = new List<TankDeliveriesAndAdjustments>();
      TankDeliveriesAuditData = new List<TankDeliveriesAndAdjustments>();
      TankDeliveriesAfterStickReading = new List<TankDeliveriesAndAdjustments>();
    }
  }

  public class TankDeliveriesAndAdjustments
  {
    public int FuelTankId { get; set; }
    public decimal? Volume { get; set; }
  }
}