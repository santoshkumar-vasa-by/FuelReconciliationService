using System.Collections.Generic;
using System.Threading.Tasks;
using ByEsoDomainInfraStructure.UnitOfWork;
using BYEsoDomainModelKernel.Models;

namespace FuelReconciliationDomainModel
{
  public interface IFuelSalesItemRepository
  {
    Task<IEnumerable<FuelSalesItem>> GetFuelSaleItems();

    Task Add(FuelSalesItem fuelSalesItem);

    Task Delete(FuelSalesItem fuelSalesItem);
  }

  public class FuelSalesItemRepository : BaseRepository, IFuelSalesItemRepository
  {
    public FuelSalesItemRepository(IUnitOfWork uow)
      : base(uow)
    {
    }

    public async Task Add(FuelSalesItem fuelSalesItem)
    {
      await Session.SaveAsync(fuelSalesItem).ConfigureAwait(false);
    }

    public async Task Delete(FuelSalesItem fuelSalesItem)
    {
      await Session.DeleteAsync(fuelSalesItem).ConfigureAwait(false);
    }

    public async Task<IEnumerable<FuelSalesItem>> GetFuelSaleItems()
    {
      return await Session.QueryOver<FuelSalesItem>().ListAsync().ConfigureAwait(false);
    }
  
  }
}