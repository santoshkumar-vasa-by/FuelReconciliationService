using ByEsoDomainInfraStructure.UnitOfWork;
using NHibernate;

namespace FuelReconciliationDomainModel
{
  public class BaseRepository
  {
    protected ISession Session { get; set; }

    public BaseRepository(IUnitOfWork unitOfWork)
    {
      Session = unitOfWork.GetSession();
    }
  }
}
