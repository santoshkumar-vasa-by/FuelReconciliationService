using System;
using BYEsoDomainModelKernel.Models;

namespace BYEsoDomainModelKernel.Util
{
  public static class FuelItemSummaryFactory
  {
    public static BYEsoDomainModelKernel.Models.FuelItemSummary CreateFuelItemSummary(Site site, FuelItem fuelItem, DateTime businessDate, ItemCategory itemCategory,
      decimal? openingWac, decimal? closingWac, decimal? sales, decimal? endOnHandQuantity, decimal? itemVarianceQuantity,
      decimal? pumpTestVolumeReturned, decimal? pumpTestVolumeNotReturned)
    {
      BYEsoDomainModelKernel.Models.FuelItemSummary newFuelItemSummary = new BYEsoDomainModelKernel.Models.FuelItemSummary(site, fuelItem, businessDate, itemCategory,openingWac,  closingWac,  
        sales,  endOnHandQuantity,  itemVarianceQuantity, pumpTestVolumeReturned,  pumpTestVolumeNotReturned);

      return newFuelItemSummary;
    }
  }
}
