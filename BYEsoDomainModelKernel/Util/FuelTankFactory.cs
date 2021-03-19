namespace BYEsoDomainModelKernel.Util
{
  public static class FuelTankFactory
  {
    //public static FuelTank CreateFuelPhysicalTank(Site site, FuelItem fuelItem, short tankNumber,
    //  FuelTankType fuelTankType)
    //{
    //  FuelTank newFuelTank = new FuelPhysicalTank(site, fuelItem, tankNumber, fuelTankType);

    //  return newFuelTank;
    //}

    //public static BYEsoDomainModelKernel.Models.FuelTank CreateFuelPhysicalTank(Site site, FuelItem fuelItem, string tankNumber,
    //  FuelTankType fuelTankType, bool isActive, int? waterThreshold, decimal? lowTankThreshold, decimal? varianceVolumeThreshold,
    //  decimal? varianceBySalesThresholdPercent, bool isAutomaticTankGauge)
    //{
    //  BYEsoDomainModelKernel.Models.FuelTank newFuelTank = new FuelPhysicalTank(site, fuelItem, tankNumber, fuelTankType, isActive, waterThreshold,
    //    lowTankThreshold, varianceVolumeThreshold, varianceBySalesThresholdPercent, isAutomaticTankGauge);
    //  return newFuelTank;
    //}

    //public static BYEsoDomainModelKernel.Models.FuelTank CreateFuelManifoldTank(Site site, FuelItem fuelItem, string tankName,
    //  IEnumerable<FuelPhysicalTank> physicalTanks)
    //{
    //  EnforceFuelItemMatchesBetweenManifoldTankAndAllItsPhysicalTanks(fuelItem, physicalTanks);

    //  var newFuelTank = new FuelManifoldTank(site, fuelItem, tankName);
    //  newFuelTank.AddPhysicalTankAssignments(physicalTanks);
    //  return newFuelTank;
    //}

    //private static void EnforceFuelItemMatchesBetweenManifoldTankAndAllItsPhysicalTanks(FuelItem fuelItem,
    //  IEnumerable<FuelPhysicalTank> physicalTanks)
    //{
    //  if (physicalTanks.Any(x => !x.FuelItem.Equals(fuelItem)))
    //  {
    //    throw new NotSupportedException("All physical tanks in a manifold must be for the manifold tank's fuel item");
    //  }
    //}
  }
}