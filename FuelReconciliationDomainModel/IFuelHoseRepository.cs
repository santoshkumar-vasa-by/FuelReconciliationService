using System.Collections.Generic;

using System.Threading.Tasks;
using ByEsoDomainInfraStructure.UnitOfWork;
using BYEsoDomainModelKernel.Models;

using NHibernate.SqlCommand;

namespace FuelReconciliationDomainModel
{
  public interface IFuelHoseRepository
  {
    Task<IEnumerable<FuelHose>> GetBySite(Site site);

    Task<IEnumerable<FuelHose>> GetFuelHoseBySite(Site site);
  }

  public class FuelHoseRepository : BaseRepository, IFuelHoseRepository
  {
    public FuelHoseRepository(IUnitOfWork uow)
      : base(uow)
    {
      
    }

    public async Task<IEnumerable<FuelHose>> GetBySite(Site site)
    {
      FuelPump fuelPump = null;
      FuelHoseTankAssignment fuelHoseTankAssignment = null;
      FuelTank fuelTank = null;
      return await Session.QueryOver<FuelHose>()
        .JoinAlias(x => x.FuelPump, () => fuelPump, JoinType.InnerJoin)
        .JoinAlias(x => x.FuelHoseTankAssignment, () => fuelHoseTankAssignment, JoinType.InnerJoin)
        .JoinAlias(() => fuelHoseTankAssignment.FuelTank, () => fuelTank, JoinType.InnerJoin)
        .Where(x => x.Site.ID == site.ID)
        .ListAsync().ConfigureAwait(false);
    }

    public async Task<IEnumerable<FuelHose>> GetFuelHoseBySite(Site site)
    {
      return await Session.QueryOver<FuelHose>()
        .Where(x => x.Site.ID == site.ID)
        .ListAsync().ConfigureAwait(false);
    }
  }
}