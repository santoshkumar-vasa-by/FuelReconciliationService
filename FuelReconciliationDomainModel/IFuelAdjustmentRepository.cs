using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ByEsoDomainInfraStructure.UnitOfWork;
using BYEsoDomainModelKernel.Models;

using NHibernate.SqlCommand;


namespace FuelReconciliationDomainModel
{
  public interface IFuelAdjustmentRepository
  {
    Task<IList<FuelAdjustment>> GetAdjustmentWithLineItemsBySite(Site site, DateTime businessDate);
  }

  public class FuelAdjustmentRepository : BaseRepository, IFuelAdjustmentRepository
  {
    public FuelAdjustmentRepository(IUnitOfWork uow) : base(uow)
    {
      
    }
    public async Task<IList<FuelAdjustment>> GetAdjustmentWithLineItemsBySite(Site site, DateTime businessDate)
    {
      FuelAdjustmentLineItem fuelAdjustmentLineItem = null;

      return await Session.QueryOver<FuelAdjustment>()
        .JoinAlias(x => x.LineItems, () => fuelAdjustmentLineItem, JoinType.InnerJoin)
        .Where(x => x.Site.ID == site.ID && x.BusinessDate == businessDate
                                         && x.Status != FuelAdjustmentStatus.Draft && x.FuelAdjustmentType != FuelAdjustmentType.PumpTest)
        .ListAsync().ConfigureAwait(false);
    }
  }
}