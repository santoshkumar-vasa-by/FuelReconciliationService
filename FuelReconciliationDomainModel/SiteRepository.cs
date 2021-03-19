using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ByEsoDomainInfraStructure.UnitOfWork;
using BYEsoDomainModelKernel.Models;


namespace FuelReconciliationDomainModel
{
  public interface ISiteRepository
  {
    Task<Site> GetByID(int siteID);
    Task<IEnumerable<Site>> GetByIDs(int[] siteIDs);
    Task<Site> Get(int siteID);
  }

  public class SiteRepository : BaseRepository, ISiteRepository
  {
    public SiteRepository(IUnitOfWork uow) : base(uow)
    { }
    public async Task<Site> GetByID(int siteID)
    {
      return await Session.LoadAsync<Site>(siteID).ConfigureAwait(false);
    }

    public async Task<IEnumerable<Site>> GetByIDs(int[] siteIDs)
    {
      return await Session.QueryOver<Site>()
        .WhereRestrictionOn(x => x.ID).IsIn(siteIDs)
        .ListAsync().ConfigureAwait(false);
    }

    //public IEnumerable<Site> GetSitesByEmployee(Employee employee)
    //{
    //  throw new NotImplementedException();
    //}

    public async Task<Site> Get(int siteID)
    {
      return await Session.GetAsync<Site>(siteID);
    }
  }
}
