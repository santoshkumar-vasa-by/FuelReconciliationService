using BYEsoDomainModelKernel.Models;

namespace BYEsoDomainModelKernel.Util
{
  public static class FuelBlendItemPercentageFactory
  {
    public static BYEsoDomainModelKernel.Models.FuelBlendItemPercentage CreateFuelBlendItemPercentage(FuelItem fuelBlendedItem, FuelItem fuelItem, decimal fuelBlendPercentage)
    {
      BYEsoDomainModelKernel.Models.FuelBlendItemPercentage newFueBlendItemPercentage = new BYEsoDomainModelKernel.Models.FuelBlendItemPercentage(fuelBlendedItem, fuelItem, fuelBlendPercentage);

      return newFueBlendItemPercentage;
    }
  }
}
