using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ByEsoDomainInfraStructure.UnitOfWork;
using BYEsoDomainModelKernel.Models;

namespace FuelReconciliationDomainModel
{
  public class FuelItemSummaryRepository : BaseRepository, IFuelItemSummaryRepository
  {

    public FuelItemSummaryRepository(IUnitOfWork uow) :base(uow)
    {
      Session = uow.GetSession();
    }

    public async Task Add(FuelItemSummary fuelItemSummary)
    {
     await Session.SaveAsync(fuelItemSummary).ConfigureAwait(false);     
    }

    public async Task Delete(FuelItemSummary fuelItemSummary)
    {
      await Session.DeleteAsync(fuelItemSummary).ConfigureAwait(false);
    }

    public async Task<IEnumerable<FuelItemSummary>> GetBySiteAndBusinessDate(Site site, DateTime? businessDate)
    {
      return await Session.QueryOver<FuelItemSummary>()
        .Where(x => x.Site.ID == site.ID && x.BusinessDate == businessDate)
        .ListAsync().ConfigureAwait(false);
    }

    public async Task<IEnumerable<FuelItemSummary>> GetBySite(Site site)
    {
      return await Session.QueryOver<FuelItemSummary>()
        .Where(x => x.Site.ID == site.ID)
        .ListAsync().ConfigureAwait(false);
    }
  }
}
