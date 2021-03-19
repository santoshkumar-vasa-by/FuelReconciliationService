using NHibernate;
using System;
using System.Threading.Tasks;

namespace ByEsoDomainInfraStructure.UnitOfWork
{
  public interface IUnitOfWork
  {
    Task Commit();
    Task Rollback();
    void CloseTransaction();
    ISessionData GetSessionData();
    ISession GetSession();
    ISession GetWarehouseSession();
    void Dispose();
    void OnCommit(Action callback);
    void OnDispose(Action callback);
    void AfterCommit(Action callback);
   //void AddQueryForTransaction(IQuery updateQuery);
   
  }
}
