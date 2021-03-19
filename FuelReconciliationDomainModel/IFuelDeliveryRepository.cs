using System;

using System.Collections.Generic;
using System.Threading.Tasks;
using ByEsoDomainInfraStructure.UnitOfWork;
using BYEsoDomainModelKernel.Models;
using NHibernate.SqlCommand;

namespace FuelReconciliationDomainModel
{
  public interface IFuelDeliveryRepository
  {
    Task<IEnumerable<FuelDelivery>> GetDeliveryWithLineItemsBySiteAndBusinessDate(Site site, DateTime businessDate);
  }

  public class FuelDeliveryRepository : BaseRepository, IFuelDeliveryRepository
  {
    
    public FuelDeliveryRepository(IUnitOfWork uow)
    :base(uow)
    { }

    public async Task<IEnumerable<FuelDelivery>> GetDeliveryWithLineItemsBySiteAndBusinessDate(Site site, DateTime businessDate)
    {
      FuelDeliveryLineItem fuelDeliveryLineItem = null;

      return await Session.QueryOver<FuelDelivery>()
        .JoinAlias(x => x.LineItems, () => fuelDeliveryLineItem, JoinType.InnerJoin)
        .Where(x => x.Site.ID == site.ID && x.Status != FuelDeliveryStatus.Draft && x.BusinessDate == businessDate)
        .ListAsync();
    }
  }
}