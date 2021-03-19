using System;
using BYEsoDomainModelKernel.Models;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ByEsoDomainInfraStructure.UnitOfWork;

using NHibernate.Criterion;

namespace FuelReconciliationDomainModel
{
  public interface IFuelInvoiceRepository
  {
    Task<IList<FuelInvoice>> UpdateFuelInvoiceWacFlags(Site site, FuelInvoice.FuelLockType lockType);
  }

  public class FuelInvoiceRepository : BaseRepository, IFuelInvoiceRepository
  {
    public FuelInvoiceRepository(IUnitOfWork uow) 
      :base(uow)
    {
      
    }

    //TODO: Good item to move the logic away from Repository
    public async Task<IList<FuelInvoice>> UpdateFuelInvoiceWacFlags(Site site, FuelInvoice.FuelLockType lockType)
    {
      IList<FuelInvoice> fuelInvoices;

      if (lockType == FuelInvoice.FuelLockType.Temporary)
      {
        var fuelDeliveries = QueryOver.Of<FuelDelivery>().Where(y => y.Site.ID == site.ID)
          .And(y => y.BusinessDate <= site.CurrentBusinessDate)
          .Select(x => x.BillOfLadingNumber);

        fuelInvoices = await Session.QueryOver<FuelInvoice>().Where(x => x.WacCalculatedFlag == WacCalculatedFlag.No)
          .And(x => x.Site.ID == site.ID).WithSubquery.WhereProperty(x => x.BillOfLadingNumber).In(fuelDeliveries).ListAsync();
      }
      else
      {
        fuelInvoices = await Session.QueryOver<FuelInvoice>().Where(x => x.Site.ID == site.ID).ListAsync();
        fuelInvoices = fuelInvoices.Where(x => x.WacCalculatedFlag == WacCalculatedFlag.Temp).ToList();
      }

      if (fuelInvoices.Any())
      {
        foreach (FuelInvoice fuelInvoice in fuelInvoices)
        {
          fuelInvoice.WacBusinessDate = lockType == FuelInvoice.FuelLockType.Final ? site.CurrentBusinessDate : (DateTime?)null;
          fuelInvoice.WacCalculatedFlag = lockType == FuelInvoice.FuelLockType.Final ? WacCalculatedFlag.Yes
            : (lockType == FuelInvoice.FuelLockType.Temporary ? WacCalculatedFlag.Temp : WacCalculatedFlag.No);

          await Session.UpdateAsync(fuelInvoice);
        }
      }
      return fuelInvoices.ToList();
    }
  }
}