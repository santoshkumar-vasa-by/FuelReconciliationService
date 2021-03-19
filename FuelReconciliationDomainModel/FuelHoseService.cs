using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ByEsoDomainInfraStructure.UnitOfWork;
using BYEsoDomainModelKernel.Models;


namespace FuelReconciliationDomainModel
{
  public interface IFuelHoseService
  {
    Task<List<DatamartFuelHose>> GetPcsFuelMeterTransactions(Site site, IEnumerable<FuelHoseTankLogicalConnection> hoseTankLogicalConnection,
      IEnumerable<FuelDailySalesWithMeter> fuelSalesForEachMeter);
  }

  public class FuelHoseService : IFuelHoseService
  {
    private readonly IFuelHoseRepository _FuelHoseRepo;

    public FuelHoseService(IFuelHoseRepository fuelHoseRepo)
    {
      _FuelHoseRepo = fuelHoseRepo;
    }

    public async Task<List<DatamartFuelHose>> GetPcsFuelMeterTransactions(Site site, IEnumerable<FuelHoseTankLogicalConnection> hoseTankLogicalConnection, IEnumerable<FuelDailySalesWithMeter> fuelSalesForEachMeter)
    {
      var fuelHoseData = await _FuelHoseRepo.GetFuelHoseBySite(site);
      var fuelMeterTransactions = (from hoseTankConnection in hoseTankLogicalConnection
                                   join hoseData in fuelHoseData on hoseTankConnection.FuelMeter.ID equals hoseData.ID
                                   join fuelDailySales in fuelSalesForEachMeter on
                                   hoseTankConnection.FuelMeter.ID equals fuelDailySales.FuelMeterID into tempdata
                                   from tdata in tempdata.DefaultIfEmpty()
                                   select new
                                   {
                                     hoseData.Site,
                                     hoseData.Site.CurrentBusinessDate,
                                     hoseTankConnection.FuelMeter,
                                     hoseTankConnection.FuelPump,
                                     hoseData.FuelItem,
                                     hoseTankConnection.ElectronicFlag,
                                     hoseTankConnection.HoseNumber,
                                     hoseTankConnection.PumpNumber,
                                     ManualVolume = tdata?.ManualVolume ?? 0,
                                     POSVolume = tdata?.POSVolume ?? 0,
                                     ManualSales = tdata?.ManualSales ?? 0,
                                     POSSales = tdata?.POSSales ?? 0
                                   }).ToList();

      var pcsFuelMeterTransactions = (from fmt in fuelMeterTransactions
                                      group fmt by new
                                      {
                                        FuelMeterId = fmt.FuelMeter.ID,
                                        FuelPointId = fmt.FuelPump.ID,
                                        FuelItemId = fmt.FuelItem.ID,
                                        fmt.ElectronicFlag,
                                        fmt.HoseNumber,
                                        fmt.PumpNumber
                                      }
        into results
                                      select new DatamartFuelHose
                                      {
                                        Site = results.First().Site,
                                        BusinessDate = results.First().Site.CurrentBusinessDate,
                                        FuelHose = results.First().FuelMeter,
                                        FuelPump = results.First().FuelPump,
                                        FuelItem = results.First().FuelItem,
                                        IsElectronic = results.Key.ElectronicFlag,
                                        HoseNumber = results.Key.HoseNumber,
                                        PumpNumber = results.Key.PumpNumber,
                                        ManualSalesQuantity = results.Sum(x => x.ManualVolume),
                                        POSSalesQuantity = results.Sum(x => x.POSVolume),
                                        ManualSalesAmount = results.Sum(x => x.ManualSales),
                                        POSSalesAmount = results.Sum(x => x.POSSales)
                                      }).ToList();
      return pcsFuelMeterTransactions;
    }
  }
}