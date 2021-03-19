using System.Collections.Generic;
using System.Threading.Tasks;
using ByEsoDomainInfraStructure.UnitOfWork;
using BYEsoDomainModelKernel.Models;


namespace FuelReconciliationDomainModel
{
  public interface IDayStatusRepository
  {
    Task<IEnumerable<DayStatus>> GetLastPostedDate(Site site);
  }

  public class DayStatusRepository : BaseRepository, IDayStatusRepository
  {
    public DayStatusRepository(IUnitOfWork uow) : base(uow)
    {
      
    }

    public async Task<IEnumerable<DayStatus>> GetLastPostedDate(Site site)
    {
      return await Session.QueryOver<DayStatus>()
        .Where(x => x.OrganizationalHierarchy.ID == site.ID &&
                    x.BusinessDate < site.CurrentBusinessDate &&
                    x.Status == BusinessDateStatus.Posted)
        .ListAsync().ConfigureAwait(false);
    }
  }
}