using System.Collections.Generic;
using System.Threading.Tasks;
using ByEsoDomainInfraStructure.UnitOfWork;
using BYEsoDomainModelKernel.Models;
using NHibernate.Criterion;
using NHibernate.SqlCommand;

namespace FuelReconciliationDomainModel
{
  public interface IPostedSaleItemRepository
  {
    Task<IEnumerable<PostedSalesItem>> GetPostedSaleItemsByIds(int[] itemIDs);
  }

  public class PostedSaleItemRepository : BaseRepository, IPostedSaleItemRepository
  {
    public PostedSaleItemRepository(IUnitOfWork uow)
      : base(uow)

    {
    }
    public async Task<IEnumerable<PostedSalesItem>> GetPostedSaleItemsByIds(int[] itemIDs)
    {
      Item itemAlias = null;

      return await Session.QueryOver<PostedSalesItem>()
        .JoinAlias(x => x.Item, () => itemAlias, JoinType.InnerJoin)
        .And(() => itemAlias.ID.IsIn(itemIDs))
        .ListAsync();
    }
  }
}