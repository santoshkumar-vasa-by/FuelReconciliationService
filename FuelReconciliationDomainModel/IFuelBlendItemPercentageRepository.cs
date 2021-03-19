using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ByEsoDomainInfraStructure.UnitOfWork;
using BYEsoDomainModelKernel.Models;

using NHibernate.Criterion;
using NHibernate.Linq;

namespace FuelReconciliationDomainModel
{
  public interface IFuelBlendItemPercentageRepository
  {
    Task<IList<FuelBlendItemPercentage>> GetFuelBlendItemPercentageBySiteAndTanks(Site site);
  }

  public class FuelBlendItemPercentageRepository : BaseRepository, IFuelBlendItemPercentageRepository
  {
    public FuelBlendItemPercentageRepository(IUnitOfWork uow) 
      : base(uow) {}
    public async Task<IList<FuelBlendItemPercentage>> GetFuelBlendItemPercentageBySiteAndTanks(Site site)
    {
      FuelItem blendItem = null;
      FuelBlendItemPercentage dBlendItemPercentage = null;
      var fuelTankIDs = QueryOver.Of<FuelTank>().Where(x => x.Site.ID == site.ID).Select(x => x.FuelItem.ID);

      /*
       * var fuelBlendPercentagesList = _session.QueryOver<FuelBlendItemPercentage>()
         .JoinAlias(x => x.FuelItem, () => blendItem, NHibernate.SqlCommand.JoinType.InnerJoin)
         .WithSubquery.WhereProperty(() => blendItem.ID).In(fuelTankIDs)
         .WithSubquery.WhereProperty(x => x.FuelBlendedItem.ID).NotIn(fuelTankIDs).List();
       */

      var fuelBlendPercentagesList = await Session.QueryOver<FuelBlendItemPercentage>(() => dBlendItemPercentage)
        //.JoinAlias(() => dBlendItemPercentage.FuelItem, () => blendItem)
        .WithSubquery.WhereProperty(x => dBlendItemPercentage.FuelItem.ID).In(fuelTankIDs)
        .WithSubquery.WhereProperty(x => x.FuelBlendedItem.ID).NotIn(fuelTankIDs)
        .ListAsync();
      //var fuelBlendPercentagesList = await Session.Query<FuelBlendItemPercentage>().Join(
      //                                  p => p,
      //                                  q => p.FuelItem,
      //                                  (p, q) => p
      //                                ).ToListAsync<FuelBlendItemPercentage>();

      return fuelBlendPercentagesList;
    }
  }
}