using System;
using FluentNHibernate.Mapping;

namespace BYEsoDomainModelKernel.Models
{
  internal interface IBaseAuditEntity
  {
    DateTime LastModifiedTimestamp { get; }
    int LastModifiedUserID { get; }
    int ClientID { get; }
  }

  public class BaseAuditEntity : IBaseAuditEntity
  {
    public virtual DateTime LastModifiedTimestamp { get; protected set; }
    public virtual int LastModifiedUserID { get; protected set; }
    public virtual int ClientID { get; protected set; }

    public virtual void SetAuditValues(DateTime lastModifiedTimestamp, int lastModifiedUserID,
      int clientID)
    {
      LastModifiedTimestamp = lastModifiedTimestamp;
      LastModifiedUserID = lastModifiedUserID;
      ClientID = clientID;
    }
  }

  internal interface IClientFilter
  {
    string Name { get; }
  }

  public class ClientIDFilter : FilterDefinition, IClientFilter
  {
    private const string _ColumnName = "client_id";

    public ClientIDFilter()
    {
      WithName(GetType().Name)
        .WithCondition(ClientFilterManager.GetFilterConditionTextClientIDColumn(_ColumnName))
        .AddParameter(ClientFilterManager.ClientIDParameterName, ClientFilterManager.ClientIDParameterType);

      ClientFilterManager.RegisterFilter(this);
    }
  }
}
