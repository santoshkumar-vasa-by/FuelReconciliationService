using System;
using BYEsoDomainModelKernel.Models;

namespace BYEsoDomainModelKernel.Util
{
  public static class FuelTankSummaryFactory
  {
    public static BYEsoDomainModelKernel.Models.FuelTankSummary CreateFuelTankSummary(Site site, FuelTank fuelTank, DateTime businessDate, decimal? closeVolume,
      decimal? openVolume, decimal? pumpTestVolumeNonReturned, decimal? salesVolume, decimal? deliveryVolume, decimal? adjustmentVolume, 
      decimal? pumpTestVolumeReturned, decimal? auditCloseVolume)
    {
      return new BYEsoDomainModelKernel.Models.FuelTankSummary(site, fuelTank, businessDate, closeVolume, openVolume, pumpTestVolumeNonReturned, salesVolume,
        deliveryVolume, adjustmentVolume, pumpTestVolumeReturned, auditCloseVolume);
    }
  }
}