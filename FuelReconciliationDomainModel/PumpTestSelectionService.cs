using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BYEsoDomainModelKernel.Models;

namespace FuelReconciliationDomainModel
{
  public interface IPumpTestSelectionService
  {
    Task<IEnumerable<DailyPumpTestForWacData>> GetFuelPumpTestsForWac(Site site,
      IEnumerable<FuelHoseTankLogicalConnection> fuelHoseTankLogicalConnection);
  }

  public class PumpTestSelectionService : IPumpTestSelectionService
  {
    private readonly IFuelEventNonSaleRepository _FuelEventNonSaleRepo;
    private readonly ISalesTransactionLineItemRepository _SalesTransactionLineItemRepo;

    public PumpTestSelectionService(IFuelEventNonSaleRepository fuelEventNonSaleRepo, ISalesTransactionLineItemRepository salesTransactionLineItemRepo)
    {
      _FuelEventNonSaleRepo = fuelEventNonSaleRepo;
      _SalesTransactionLineItemRepo = salesTransactionLineItemRepo;
    }

    public async Task<IEnumerable<DailyPumpTestForWacData>> GetFuelPumpTestsForWac(Site site,
      IEnumerable<FuelHoseTankLogicalConnection> fuelHoseTankLogicalConnection)
    {
      
      var eventNonSales =
        await _FuelEventNonSaleRepo.GetEventNonSalesWithDateAndEventType(site.ID, site.CurrentBusinessDate, FuelEventType.PumpTest);

      
      var salesLineItems = await _SalesTransactionLineItemRepo.GetBySite(site);

      var pumptestData = (from trans in eventNonSales
                          join sales in salesLineItems
                            on new
                            {
                              x1 = trans.Site.ID,
                              x2 = trans.BusinessDate,
                              x3 = trans.TransactionNumber,
                              x4 = trans.PostedSalesItem.SalesItemID,
                              x5 = trans.PumpNumber,
                              x6 = trans.ShiftID,
                              x7 = 5  // sales type 5= pump tests

                            } equals
                            new
                            {
                              x1 = sales.Site.ID,
                              x2 = sales.BusinessDate,
                              x3 = sales.GetTransactionNumber,
                              x4 = sales.SalesItemID,
                              x5 = sales.PumpNumber ?? 0,
                              x6 = sales.GetShiftID,
                              x7 = sales.SalesType
                            }
                            into salesTransData
                          from std in salesTransData
                          join fht in fuelHoseTankLogicalConnection on new
                          {
                            x1 = trans.PumpNumber,
                            x2 = trans.SalesItem.SoldItem.ID,
                            x3 = true,
                            x4 = std.HoseNumber ?? 0

                          } equals new
                          {
                            x1 = fht.PumpNumber,
                            x2 = fht.RetailItemId,
                            x3 = fht.ElectronicFlag,
                            x4 = fht.HoseNumber
                          }
                          select new
                          {
                            fuelTankId = fht.LogicalTankId,
                            PumpTestVolumeReturned = trans.FuelAdjustment != null ? trans.Volume * fht.PercentageFromLogicalTank : 0,
                            PumpTestVolumeNotReturned = trans.FuelAdjustment == null ? trans.Volume * fht.PercentageFromLogicalTank : 0
                          }).ToList();


      return from pt in pumptestData
             group pt by new { pt.fuelTankId }
              into groupedData
             select new DailyPumpTestForWacData
             {
               fuelTankId = groupedData.Key.fuelTankId,
               PumpTestVolumeReturned = groupedData.Sum(x => x.PumpTestVolumeReturned),
               PumpTestVolumeNotReturned = groupedData.Sum(x => x.PumpTestVolumeNotReturned)
             };
    }
  }

  public class PumpTestReconcileData
  {
    public virtual DateTime BusinessDate { get; set; }
    public virtual decimal Volume { get; set; }
    public virtual decimal Amount { get; set; }
    public virtual string Comments { get; set; }
    public virtual int TransactionNumber { get; set; }
    public virtual FuelAdjustment FuelAdjustment { get; set; }
    public virtual PostedSalesItem PostedSalesItem { get; set; }
    public virtual DateTime Timestamp { get; set; }
    public virtual Site Site { get; set; }
    public virtual int EmployeeID { get; set; }
    public virtual int ShiftID { get; set; }
    public virtual int PumpNumber { get; set; }
    public virtual int TransactionLineNumber { get; set; }
    public virtual string TankNumber { get; set; }
    public virtual string OriginalTanks { get; set; }
    public virtual FuelEventType FuelEventType { get; set; }
    public virtual int? PhysicalTankID { get; set; }
    public virtual int ReturnTankNumber { get; set; }
    public virtual string LookupText { get; set; }

    public virtual char CdmEditable { get; set; }
  }

  public class DailyPumpTestForWacData
  {
    public int fuelTankId { get; set; }
    public decimal PumpTestVolumeReturned { get; set; }
    public decimal PumpTestVolumeNotReturned { get; set; }
  }
}