using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ByEsoDomainInfraStructure.UnitOfWork;
using BYEsoDomainModelKernel.Models;



namespace FuelReconciliationDomainModel
{
  public interface IDatamartFuelHoseRepository
  {
    Task Add(DatamartFuelHose fuelHose);
    Task Delete(DatamartFuelHose fuelHose);
    Task<IEnumerable<DatamartFuelHose>> GetDatamartFuelHoseBySiteAndDate(Site site, DateTime businessDate);
  }

  public class DatamartFuelHoseRepository : BaseRepository, IDatamartFuelHoseRepository
  {
    public DatamartFuelHoseRepository(IUnitOfWork uow) : base(uow)
    {
      
    }

    public async Task Add(DatamartFuelHose fuelHose)
    {
      await Session.SaveAsync(fuelHose);
    }

    public async Task Delete(DatamartFuelHose fuelHose)
    {
      await Session.DeleteAsync(fuelHose);
    }

    public async Task<IEnumerable<DatamartFuelHose>> GetDatamartFuelHoseBySiteAndDate(Site site, DateTime businessDate)
    {
      return await Session.QueryOver<DatamartFuelHose>()
        .Where(x => x.Site.ID == site.ID && x.BusinessDate == businessDate)
        .ListAsync();
    }
  }
}