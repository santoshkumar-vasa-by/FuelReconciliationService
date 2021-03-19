using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ByEsoDomainInfraStructure.UnitOfWork;
using BYEsoDomainModelKernel.Models;
using NHibernate.Criterion;
using NHibernate.SqlCommand;


namespace FuelReconciliationDomainModel
{
  public class FuelTankSummaryRepository : BaseRepository, IFuelTankSummaryRepository
  {
    public FuelTankSummaryRepository(IUnitOfWork uow) : base(uow)
    {
    }

    public async Task Add(FuelTankSummary agg)
    {
      await Session.SaveAsync(agg);
    }

    public async Task Delete(FuelTankSummary agg)
    {
      await Session.DeleteAsync(agg);
    }

    public async Task<IEnumerable<FuelTankSummary>> GetLatestTankSummaryForAllTanksBySite(Site site)
    {
      var mostRecentPreviousBusinessDate = (DateTime?)(await Session.QueryOver<FuelTankSummary>()
                                           .Where(x => x.Site.ID == site.ID && x.BusinessDate < site.CurrentBusinessDate)
                                           .Select(Projections.Max<FuelTankSummary>(s => s.BusinessDate))
                                           .SingleOrDefaultAsync<object>());

      if (mostRecentPreviousBusinessDate.HasValue)
      {
        return await Session.QueryOver<FuelTankSummary>()
                    .Where(x => x.Site.ID == site.ID && x.BusinessDate == mostRecentPreviousBusinessDate.Value)
                    .ListAsync();
      }
      return new List<FuelTankSummary>();
    }

    public async Task<IEnumerable<FuelTankSummary>> GetTankSummaryBySiteAndBusinessDateRange(Site site, DateTime startBusinessDate,
      DateTime endBusinessDate)
    {
      return await Session.QueryOver<FuelTankSummary>()
                     .Where(x => x.Site.ID == site.ID
                                 && x.BusinessDate >= startBusinessDate
                                 && x.BusinessDate <= endBusinessDate)
                     .ListAsync();
    }

    public async Task<IEnumerable<FuelTankSummary>> GetTankSummaryBySiteAndBusinessDate(Site site)
    {
      FuelTank tankAlias = null;

      return await Session.QueryOver<FuelTankSummary>()
        .JoinAlias(x => x.FuelTank, () => tankAlias, JoinType.InnerJoin)
        .Where(x => x.Site.ID == site.ID 
                    && x.BusinessDate == site.CurrentBusinessDate)
        .ListAsync();
    }

    public async Task<IEnumerable<FuelTankSummary>> GetTankSummaryBySiteAndDate(Site site, DateTime businessDate)
    {
      return await Session.QueryOver<FuelTankSummary>()
                     .Where(x => x.Site.ID == site.ID && x.BusinessDate == businessDate)
                     .ListAsync();
    }

    public async Task<IEnumerable<FuelTankSummary>> GetEarliestTankSummaryBySite(Site site)
    {
      return await Session.QueryOver<FuelTankSummary>()
        .Where(x => x.Site.ID == site.ID && x.BusinessDate < site.CurrentBusinessDate)
        .ListAsync();
    }

    public async Task<IEnumerable<FuelTankSummary>> GetTankSummaryBySiteAndTank(int siteId, int fuelTankId)
    {
      return await Session.QueryOver<FuelTankSummary>()
        .Where(x => x.Site.ID == siteId && x.FuelTank.ID == fuelTankId)
        .ListAsync();
    }
  }
}
