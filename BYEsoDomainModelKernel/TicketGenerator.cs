using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using NHibernate.Engine;
using NHibernate.Id;

namespace BYEsoDomainModelKernel
{
  public interface ITicketGenerator
  {
    object Generate(object session, Type type);

    object Generate(object session, string tableName);

    void ReserveTickets(object session, Type type, int numTickets);

    Task<object> GenerateAsync(ISessionImplementor session, object o, CancellationToken cancellationToken);
  }

  public class TicketGenerator : IIdentifierGenerator
  {
    private ITicketGenerator _Generator;
    
    //public TicketGenerator(ITicketGenerator generator)
    //{
    //  _Generator = generator;
    //}
    public object Generate(ISessionImplementor session, object obj)
    {
      return Generate(session, obj.GetType());
    }

    public object Generate(ISessionImplementor session, Type type)
    {
      _Generator = new ProductionTicketGenerator();
      return _Generator.Generate(session, type);
    }

    public async Task<object> GenerateAsync(ISessionImplementor session, object obj, CancellationToken cancellationToken)
    {
      return await _Generator.GenerateAsync(session, obj, cancellationToken);
    }
  }

  public class TestTicketGenerator : ITicketGenerator
  {
    private readonly Dictionary<string, ReservedTicketInfo> _ReservedTickets = new Dictionary<string, ReservedTicketInfo>();

    public object Generate(object sessionImpl, Type entityType)
    {
      string tableName = EntityTableMap.GetTableName(entityType);

      return Generate(sessionImpl, tableName);
    }

    public object Generate(object sessionImpl, string tableName)
    {
      if (!_ReservedTickets.ContainsKey(tableName) || _ReservedTickets[tableName].RemainingTickets <= 0)
      {
        ReserveTickets(tableName, 1);
      }

      return ConsumeTicket(tableName);
    }

    private int ConsumeTicket(string tableName)
    {
      _ReservedTickets[tableName].RemainingTickets--;
      return _ReservedTickets[tableName].NextTicket++;
    }

    public void ReserveTickets(object s, Type entityType, int numTickets)
    {
      string tableName = EntityTableMap.GetTableName(entityType);

      ReserveTickets(tableName, numTickets);
    }

    public Task<object> GenerateAsync(ISessionImplementor session, object o, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    private void ReserveTickets(string tableName, int numTickets)
    {
      lock (_ReservedTickets)
      {
        if (!_ReservedTickets.ContainsKey(tableName))
        {
          _ReservedTickets[tableName] = new ReservedTicketInfo { NextTicket = 1000000, RemainingTickets = numTickets };
        }
        else
        {
          _ReservedTickets[tableName].RemainingTickets = numTickets;
        }
      }
    }
  }

  public class ProductionTicketGenerator : ITicketGenerator
  {
    private readonly Dictionary<string, ReservedTicketInfo> _ReservedTickets = new Dictionary<string, ReservedTicketInfo>();

    public object Generate(object sessionImpl, Type entityType)
    {
      string tableName = EntityTableMap.GetTableName(entityType);

      return Generate(sessionImpl, tableName);
    }

    public object Generate(object sessionImpl, string tableName)
    {
      if (!_ReservedTickets.ContainsKey(tableName) || _ReservedTickets[tableName].RemainingTickets <= 0)
      {
        ReserveTickets(sessionImpl, tableName, 1);
      }

      return ConsumeTicket(tableName);
    }

    private int ConsumeTicket(string tableName)
    {
      var ticket = _ReservedTickets[tableName].NextTicket;

      _ReservedTickets[tableName].RemainingTickets--;
      _ReservedTickets[tableName].NextTicket++;
      return ticket;
    }

    public void ReserveTickets(object sessionImpl, Type entityType, int numTickets)
    {
      string tableName = EntityTableMap.GetTableName(entityType);
      ReserveTickets(sessionImpl, tableName, numTickets);
    }

    public async Task<object> GenerateAsync(ISessionImplementor session, object o, CancellationToken cancellationToken)
    {
      return this.GenerateAsync(session, o, cancellationToken);
    }

    private void ReserveTickets(object sessionImpl, string tableName, int numTickets)
    {
      var session = (ISessionImplementor)sessionImpl;
      IDbCommand command = session.Connection.CreateCommand();
      command.CommandType = CommandType.StoredProcedure;
      command.CommandText = "plt_get_next_named_ticket";

      command.Parameters.Add(CreateSqlParameter("@table_name", SqlDbType.VarChar, tableName));
      command.Parameters.Add(CreateSqlParameter("@isRed", SqlDbType.NChar, 'n'));
      command.Parameters.Add(CreateSqlParameter("@numTickets", SqlDbType.Int, numTickets));

      var outputParameter = new SqlParameter("@next_ticket", SqlDbType.Int)
      {
        Direction = ParameterDirection.Output
      };
      command.Parameters.Add(outputParameter);

      session.ConnectionManager.CurrentTransaction.Enlist((DbCommand)command);
      command.ExecuteNonQuery();

      var nextTicket = (int)((SqlParameter)command.Parameters["@next_ticket"]).Value + 1 - numTickets;

      /*
       * For now throw away any unused tickets when new tickets are reserved, otherwise we have to keep a list of valid
       * ticket ranges to handle people reserving tickets on other connections.
       */
      _ReservedTickets[tableName] = new ReservedTicketInfo
      {
        NextTicket = nextTicket,
        RemainingTickets = numTickets
      };
    }

    private SqlParameter CreateSqlParameter(string paramName, SqlDbType dbType, object value)
    {
      var param = new SqlParameter(paramName, dbType) { Value = value };
      return param;
    }
  }

  public class ReservedTicketInfo
  {
    public int NextTicket;
    public int RemainingTickets;
  }

  public static class EntityTableMap
  {
    private static readonly Dictionary<Type, string> _Map;

    static EntityTableMap()
    {
      _Map = new Dictionary<Type, string>();
    }

    public static void MapTableToEntity(Type entityType, string tableName)
    {
      _Map[entityType] = tableName;
    }

    public static string GetTableName(Type entityType)
    {
      if (_Map.ContainsKey(entityType))
      {
        return _Map[entityType];
      }
      // TODO: EsoGenericException
      throw new NotSupportedException("No table for entity '" + entityType.Name + "' exists in the map");
    }
  }
}
