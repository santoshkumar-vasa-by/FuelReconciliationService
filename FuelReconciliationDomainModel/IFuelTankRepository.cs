using System.Collections.Generic;
using System.Threading.Tasks;
using ByEsoDomainInfraStructure.UnitOfWork;
using BYEsoDomainModelKernel.Models;



namespace FuelReconciliationDomainModel
{
  public interface IFuelTankRepository
  {
    Task<IEnumerable<T>> GetFuelTanksBySite<T>(Site site) where T : FuelTank;

    Task Add(FuelTank agg);

    Task Delete(FuelTank agg);

    Task<T> GetByID<T>(int key) where T : FuelTank;
  }

  public class FuelTankRepository : BaseRepository, IFuelTankRepository
  {
    public FuelTankRepository(IUnitOfWork uow)
      : base(uow)
    {
      
    }

    public async Task Add(FuelTank agg)
    {
      await Session.SaveAsync(agg);
    }

    public async Task Delete(FuelTank agg)
    {
      await Session.DeleteAsync(agg);
    }

    public async Task<T> GetByID<T>(int key)
                  where T : FuelTank
    {
      return await Session.LoadAsync<T>(key);
    }

    public async Task<IEnumerable<T>> GetFuelTanksBySite<T>(Site site) where T : FuelTank
    {
      return await Session.QueryOver<T>()
        .Where(x => x.Site == site)
        .ListAsync();
    }
  }
}