using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using ByEsoDomainInfraStructure.UnitOfWork;
using BYEsoDomainModelKernel.Models;



namespace FuelReconciliationDomainModel
{
  public interface ISiteClosedDaysRepository
  {
    Task<IEnumerable<SiteClosedDays>> GetBySiteAndBusinessDate(Site site, DateTime businessDate);
  }

  public class SiteClosedDaysRepository : BaseRepository, ISiteClosedDaysRepository
  {
    public SiteClosedDaysRepository(IUnitOfWork uow)
      : base(uow)
    {
      
    }
    public async Task<IEnumerable<SiteClosedDays>> GetBySiteAndBusinessDate(Site site, DateTime businessDate)
    {
      return await Session.QueryOver<SiteClosedDays>()
        .Where(x => x.Site.ID == site.ID && x.BusinessDate == businessDate)
        .ListAsync();
    }
  }
}