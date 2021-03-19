using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ByEsoDomainInfraStructure.UnitOfWork;
using BYEsoDomainModelKernel.Models;
using NHibernate.SqlCommand;


namespace FuelReconciliationDomainModel
{
  public interface IFuelTankMeterReadingLineItemRepository
  {
    Task<IEnumerable<FuelTankMeterReadingLineItem>> GetReadValueWithPrecisionMoreThanThree(Site site);
    Task<IEnumerable<FuelTankMeterReadingLineItem>> GetLastWeekMeterReadings(Site site);
  }

  public class FuelTankMeterReadingLineItemRepository : BaseRepository, IFuelTankMeterReadingLineItemRepository
  {

    public FuelTankMeterReadingLineItemRepository(IUnitOfWork uow)
      : base(uow)
    {
    }
    public async Task<FuelTankMeterReadingLineItem> GetByID(int key)
    {
      return await Session.LoadAsync<FuelTankMeterReadingLineItem>(key);
    }
    public async Task<IEnumerable<FuelTankMeterReadingLineItem>> GetReadValueWithPrecisionMoreThanThree(Site site)
    {
      FuelMeterReading fuelMeterReading = null;
      return await Session.QueryOver<FuelTankMeterReadingLineItem>()
        .JoinAlias(x => x.FuelMeterReading, () => fuelMeterReading, JoinType.InnerJoin)
        .Where(x => x.Site.ID == site.ID && fuelMeterReading.BusinessDate == site.CurrentBusinessDate)
        .Where(x => x.ReadValue != Math.Round(x.ReadValue, 3))
        .ListAsync();
    }
    public async Task<IEnumerable<FuelTankMeterReadingLineItem>> GetLastWeekMeterReadings(Site site)
    {
      FuelMeterReading fuelMeterReading = null;
      return await Session.QueryOver<FuelTankMeterReadingLineItem>()
        .JoinAlias(x => x.FuelMeterReading, () => fuelMeterReading, JoinType.InnerJoin)
        .Where(x => x.Site.ID == site.ID 
                    && ((fuelMeterReading.BusinessDate <= site.CurrentBusinessDate)
                        && (fuelMeterReading.BusinessDate >= site.CurrentBusinessDate.AddDays(-7))))
        .ListAsync();
    }
  }
}