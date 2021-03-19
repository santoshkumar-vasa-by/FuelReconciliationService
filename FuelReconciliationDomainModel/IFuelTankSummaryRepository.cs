using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BYEsoDomainModelKernel.Models;


namespace FuelReconciliationDomainModel
{
  public interface IFuelTankSummaryRepository
  {
    Task Add(FuelTankSummary agg);

    Task Delete(FuelTankSummary agg);

    Task<IEnumerable<FuelTankSummary>> GetLatestTankSummaryForAllTanksBySite(Site site);

    Task<IEnumerable<FuelTankSummary>> GetTankSummaryBySiteAndBusinessDateRange(Site site, DateTime startBusinessDate, 
      DateTime endBusinessDate);

    Task<IEnumerable<FuelTankSummary>> GetTankSummaryBySiteAndBusinessDate(Site site);

    Task<IEnumerable<FuelTankSummary>> GetTankSummaryBySiteAndDate(Site site, DateTime businessDate);

    Task<IEnumerable<FuelTankSummary>> GetEarliestTankSummaryBySite(Site site);

    Task<IEnumerable<FuelTankSummary>> GetTankSummaryBySiteAndTank(int siteId, int fuelTankId);
  }
}