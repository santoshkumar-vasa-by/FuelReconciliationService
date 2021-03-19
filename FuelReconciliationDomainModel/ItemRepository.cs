using System.Collections.Generic;
using System.Threading.Tasks;
using BYEsoDomainModelKernel.Models;
using BYEsoDomainModelKernel.Util;
using IUnitOfWork = ByEsoDomainInfraStructure.UnitOfWork.IUnitOfWork;

namespace FuelReconciliationDomainModel
{
  public interface IItemRepository
  {
    Task<T> GetByID<T>(int itemID) where T : Item;
    Task<IEnumerable<T>> GetItemsByIds<T>(int[] itemIds) where T : Item;
    Task<IEnumerable<T>> GetByIDs<T>(int[] keys) where T : Item;
  }

  public class ItemRepository : BaseRepository, IItemRepository
  {

    public ItemRepository(IUnitOfWork uow)
      : base(uow)
    {
    }
    public async Task<T> GetByID<T>(int itemID) where T : Item
    {
      return await Session.LoadAsync<T>(itemID);
    }

    public async Task<IEnumerable<T>> GetByIDs<T>(int[] keys) where T : Item
    {
      var xmlIn = XmlIn.Create("ID", keys);
      return await Session.QueryOver<T>()
        .Where(xmlIn)
        .And(x => !x.IsPurged)
        .ListAsync();
    }

    public async Task<IEnumerable<T>> GetItemsByIds<T>(int[] itemIds) where T : Item
    {
      return await Session.QueryOver<T>()
        .AndRestrictionOn(x => x.ID).IsIn(itemIds)
        .ListAsync();
    }
  }
}
