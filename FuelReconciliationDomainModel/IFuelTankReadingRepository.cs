using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ByEsoDomainInfraStructure.UnitOfWork;
using BYEsoDomainModelKernel.Models;



namespace FuelReconciliationDomainModel
{
  public interface IFuelTankReadingRepository
  {
    Task<IEnumerable<FuelTankReading>> GetLimitedNonDraftFuelTankReadingData(Site site, DateTime businessDate);
  }

  public class FuelTankReadingRepository : BaseRepository, IFuelTankReadingRepository
  {
    public FuelTankReadingRepository(IUnitOfWork uow)
      : base(uow)
    {
    }

    public async Task<IEnumerable<FuelTankReading>> GetLimitedNonDraftFuelTankReadingData(Site site, DateTime businessDate)
    {
      return await Session.QueryOver<FuelTankReading>()
        .Where(x => x.Site.ID == site.ID && x.BusinessDate == site.CurrentBusinessDate && x.Status != TankReadingStatus.Draft)
        .ListAsync();
    }
  }
}