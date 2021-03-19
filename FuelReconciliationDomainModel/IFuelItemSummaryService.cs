using System.Threading.Tasks;

namespace FuelReconciliationDomainModel
{
  public interface IFuelItemSummaryService
  {
    Task UpdateFuelWac(int? siteID);
  }
}