using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ByEsoDomainInfraStructure.UnitOfWork;
using BYEsoDomainModelKernel.Models;

using NHibernate.SqlCommand;

namespace FuelReconciliationDomainModel
{
  public interface IFuelEventNonSaleRepository
  {
    Task<IEnumerable<FuelEventNonSale>> GetEventNonSalesWithDateAndEventType(int siteID, DateTime businessDate,
      FuelEventType eventType);
  }

  public class FuelEventNonSaleRepository : BaseRepository, IFuelEventNonSaleRepository
  {

    public FuelEventNonSaleRepository(IUnitOfWork uow) : base(uow)
    {
    }
    public async Task<IEnumerable<FuelEventNonSale>> GetEventNonSalesWithDateAndEventType(int siteID, DateTime businessDate, FuelEventType eventType)
    {
      SalesItem salesItem = null;

      var result = await Session.QueryOver<FuelEventNonSale>()
        .JoinAlias(x => x.SalesItem, () => salesItem, JoinType.InnerJoin)
        .Where(x => x.Site.ID == siteID && x.BusinessDate == businessDate && x.FuelEventType == eventType
        ).ListAsync().ConfigureAwait(false);

      return result;
    }
  }
}