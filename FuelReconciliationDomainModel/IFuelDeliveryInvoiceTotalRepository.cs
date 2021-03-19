using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ByEsoDomainInfraStructure.UnitOfWork;
using BYEsoDomainModelKernel.Models;
using NHibernate.Criterion;
using NHibernate.SqlCommand;


namespace FuelReconciliationDomainModel
{
  public interface IFuelDeliveryInvoiceTotalRepository
  {
    Task<IEnumerable<FuelDeliveryInvoiceTotal>> GetFuelDeliveryInvoiceList(Site site, IEnumerable<FuelInvoice> fuelInvoiceTemp);
  }

  public class FuelDeliveryInvoiceTotalRepository : BaseRepository, IFuelDeliveryInvoiceTotalRepository
  {
    public FuelDeliveryInvoiceTotalRepository(IUnitOfWork uow) : base(uow)
    {
      
    }
    public async Task<IEnumerable<FuelDeliveryInvoiceTotal>> GetFuelDeliveryInvoiceList(Site site, IEnumerable<FuelInvoice> fuelInvoiceTemp)
    {
      FuelInvoice fuelInvoiceAlias = null;

      return await Session.QueryOver<FuelDeliveryInvoiceTotal>()
        .JoinAlias(x => x.FuelInvoice, () => fuelInvoiceAlias, JoinType.InnerJoin)
        .Where(x => x.Site.ID == site.ID)
        .And(() => (fuelInvoiceAlias.WacCalculatedFlag == WacCalculatedFlag.Temp) ||
                   (fuelInvoiceAlias.ID.IsIn(fuelInvoiceTemp.Select(x => x.ID).ToArray())))
        .ListAsync();
    }
  }
}