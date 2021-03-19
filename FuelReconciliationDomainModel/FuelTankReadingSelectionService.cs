using System.Collections.Generic;
using System.Threading.Tasks;
using ByEsoDomainInfraStructure.UnitOfWork;
using BYEsoDomainModelKernel.Models;


namespace FuelReconciliationDomainModel
{
  public interface IFuelTankReadingSelectionService
  {
    Task<IEnumerable<FuelTankReading>> GetLimitedFuelTankReadingData(Site site);
  }

  public class FuelTankReadingSelectionService : IFuelTankReadingSelectionService
  {
    private readonly IFuelTankReadingRepository _FuelTankReadingRepo;
    public FuelTankReadingSelectionService(IFuelTankReadingRepository fuelTankReadingRepo)
    {
      _FuelTankReadingRepo = fuelTankReadingRepo;
    }

    public async Task<IEnumerable<FuelTankReading>> GetLimitedFuelTankReadingData(Site site)
    { 
      return await _FuelTankReadingRepo.GetLimitedNonDraftFuelTankReadingData(site, site.CurrentBusinessDate);
    }
  }
}