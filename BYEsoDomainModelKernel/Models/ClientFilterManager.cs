using System;
using System.Collections.Generic;
using ByEsoDomainInfraStructure.UnitOfWork;
using NHibernate;
using NHibernate.Type;

namespace BYEsoDomainModelKernel.Models
{
  public static class ClientFilterManager
  {
    internal static string ClientIDParameterName = "clientID";
    internal static IType ClientIDParameterType = NHibernateUtil.Int32;

    private static readonly HashSet<string> _ClientFilterNames;

    public const string ClientBeingFilteredKey = "ClientBeingFiltered.Key";

    public static int? ClientBeingFiltered;

    static ClientFilterManager()
    {
      _ClientFilterNames = new HashSet<string>();
    }

    internal static void RegisterFilter(IClientFilter filter)
    {
      var filterName = filter.Name;
      _ClientFilterNames.Add(filterName);
    }

    public static void EnableClientFilters(IUnitOfWork uow, int clientID)
    {
      var session = uow.GetSession();

      ClientBeingFiltered = clientID;
      uow.OnDispose(ClearClientBeingFiltered);

      foreach (var filterName in _ClientFilterNames)
      {
        session.EnableFilter(filterName).SetParameter(ClientIDParameterName, clientID);
      }
    }

    public static void ClearClientBeingFiltered()
    {
      ClientBeingFiltered = null;
    }

    public static void DisableClientFilters(IUnitOfWork uow)
    {
      var session = uow.GetSession();

      ClientBeingFiltered = null;

      foreach (var filterName in _ClientFilterNames)
      {
        session.DisableFilter(filterName);
      }
    }

    internal static string GetFilterConditionTextClientIDColumn(string clientIDColumnName)
    {
      return String.Format(":{0} IN ({1}, 0)", ClientIDParameterName, clientIDColumnName);
    }
  }
}
