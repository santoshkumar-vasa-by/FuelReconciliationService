using System;
using System.Linq;
using ByEsoDomainInfraStructure;
using BYEsoDomainModelKernel.Models;
using NHibernate;
using NHibernate.SqlCommand;
using NHibernate.Type;

namespace BYEsoDomainModelKernel
{
  public class Interceptor : EmptyInterceptor, IInterceptor
  {
    private readonly ISessionData _SessionData;

    public Interceptor(ISessionData sessionData)
    {
      _SessionData = sessionData;
    }

    public override Boolean OnSave(Object entity, Object id, Object[] state,
      String[] propertyNames, IType[] types)
    {
      var auditEntity = entity as BaseAuditEntity;

      if (auditEntity != null)
      {
        SetAuditPropertiesIfPresent(state, propertyNames, auditEntity, true);
        return true;
      }
      return base.OnSave(entity, id, state, propertyNames, types);
    }

    public override Boolean OnFlushDirty(object entity, object id, object[] state,
      object[] previousState, string[] propertyNames, IType[] types)
    {
      var auditEntity = entity as BaseAuditEntity;

      if (auditEntity != null)
      {
        SetAuditPropertiesIfPresent(state, propertyNames, auditEntity, false);
        return true;
      }
      return base.OnFlushDirty(entity, id, state, previousState, propertyNames, types);
    }

    private void SetAuditPropertiesIfPresent(Object[] state, String[] propertyNames, BaseAuditEntity auditEntity,
      bool useClientIDInSession)
    {
      var now = DateTime.Now;

      var index = Array.IndexOf(propertyNames, "LastModifiedTimestamp");
      if (index >= 0)
      {
        state[index] = now;

        var indices = propertyNames
          .Select((f, i) => new { f, i })
          .Where(x => x.f == "_JoinedLastModifiedTimestamp")
          .Select(x => x.i).ToArray();

        if (indices.Any())
        {
          foreach (var ind in indices)
          {
            state[ind] = now;
          }
        }
      }

      index = Array.IndexOf(propertyNames, "LastModifiedUserID");
      if (index >= 0)
      {
        state[index] = _SessionData.UserId;

        var indices = propertyNames
          .Select((f, i) => new { f, i })
          .Where(x => x.f == "_JoinedLastModifiedUserID")
          .Select(x => x.i).ToArray();

        if (indices.Any())
        {
          foreach (var ind in indices)
          {
            state[ind] = _SessionData.UserId;
          }
        }
      }

      var clientID = _SessionData.ClientId;

      index = Array.IndexOf(propertyNames, "ClientID");
      
      if (index >= 0)
      {
        if (!useClientIDInSession)
        {
          clientID = (int)state[index];
        }

        state[index] = clientID;

        var indices = propertyNames
          .Select((f, i) => new { f, i })
          .Where(x => x.f == "_JoinedClientID")
          .Select(x => x.i).ToArray();

        if (indices.Any())
        {
          foreach (var ind in indices)
          {
            state[ind] = clientID;
          }
        }
      }

     
      auditEntity.SetAuditValues(now, _SessionData.UserId, clientID);
      
    }

    public override SqlString OnPrepareStatement(SqlString sql)
    {
      System.Diagnostics.Debug.WriteLine("NH-santosh: " + sql);
        return base.OnPrepareStatement(sql);
    }
  }
}
