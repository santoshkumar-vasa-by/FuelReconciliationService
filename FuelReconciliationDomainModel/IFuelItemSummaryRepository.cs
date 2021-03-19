using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BYEsoDomainModelKernel.Models;


namespace FuelReconciliationDomainModel
{
  public interface IFuelItemSummaryRepository
  {
    Task Add(FuelItemSummary fuelItemSummary);
    Task Delete(FuelItemSummary fuelItemSummary);

    Task<IEnumerable<FuelItemSummary>> GetBySiteAndBusinessDate(Site site, DateTime? businessDate);
    Task<IEnumerable<FuelItemSummary>> GetBySite(Site site);
  }
}
