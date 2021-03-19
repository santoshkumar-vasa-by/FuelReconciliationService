using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BYEsoDomainModelKernel.Models;

using ByEsoDomainInfraStructure.UnitOfWork;
using NHibernate;
using NHibernate.SqlCommand;

namespace FuelReconciliationDomainModel
{
  public interface ISalesTransactionLineItemRepository
  {
    Task<IEnumerable<SalesTransactionLineItem>> GetBySiteAndBusinessDateWithOutPumpTests(Site site, DateTime siteCurrentBusinessDate);
    Task<IEnumerable<SalesTransactionLineItem>> GetBySiteAndBusinessDate(Site site, DateTime businessDate);
    Task<IEnumerable<SalesTransactionLineItem>> GetBySiteWithOutFuelPrepaidAndAdjustments(Site site, DateTime currentBusinessDate);
    Task<IEnumerable<SalesTransactionLineItem>> GetBySite(Site site);
  }

  public class SalesTransactionLineItemRepository : BaseRepository, ISalesTransactionLineItemRepository
  {
    private readonly ISession _Session;
    public SalesTransactionLineItemRepository(IUnitOfWork uow)
      : base(uow)
    {
      _Session = uow.GetSession();
    }

    public async Task<IEnumerable<SalesTransactionLineItem>> GetBySiteAndBusinessDate(Site site, DateTime businessDate)
    {
      return await _Session.QueryOver<SalesTransactionLineItem>()
        .Where(x => x.Site.ID == site.ID && x.BusinessDate == businessDate)
        .ListAsync();
    }

    public async Task<IEnumerable<SalesTransactionLineItem>> GetBySiteAndBusinessDateWithOutPumpTests(Site site, DateTime businessDate)
    {
      FuelSalesItem fuelSalesItem = null;
      PostedSalesItem postedSalesItem = null;

      return await _Session.QueryOver<SalesTransactionLineItem>()
        .JoinAlias(x => x.FuelSalesItem, () => fuelSalesItem, JoinType.InnerJoin)
        .JoinAlias(x => x.PostedSalesItem, () => postedSalesItem, JoinType.InnerJoin)
        .Where(x => x.Site.ID == site.ID && x.BusinessDate == businessDate && x.SalesType != (int)SalesTypes.PumpTest)
        .ListAsync();
    }

    public async Task<IEnumerable<SalesTransactionLineItem>> GetBySite(Site site)
    {
      return await _Session.QueryOver<SalesTransactionLineItem>()
        .Where(x => x.Site.ID == site.ID)
        .ListAsync();
    }

    public async Task<IEnumerable<SalesTransactionLineItem>> GetBySiteWithOutFuelPrepaidAndAdjustments(Site site, DateTime businessDate)
    {
      return await _Session.QueryOver<SalesTransactionLineItem>()
       .WhereRestrictionOn(x => x.SalesType).Not.IsIn(new[] { (int)SalesTypes.FuelPrepay, (int)SalesTypes.FuelPrepayAdjustment })
       .And(x => x.Site.ID == site.ID && x.BusinessDate == businessDate && x.HoseNumber != null && x.PumpNumber != null)
       .ListAsync();
    }
  }
}
