using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace ByEsoDomainInfraStructure.Dapper
{
  public class BaseRepository<TEntity> where TEntity : class
  {
    public async Task<T> GetCount<T>(string connectionString, string query, CommandType commandType = CommandType.Text)
    {
      using (var connection = new SqlConnection(connectionString))
      {
        if (connection.State == ConnectionState.Closed)
        {
          connection.Open();
        }

        return await connection.QueryFirstOrDefaultAsync(query, commandType: commandType);
      }
    }

    /// <summary>
    /// Get entity by any of the property inside of the object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="connectionString"></param>
    /// <param name="item"></param>
    /// <param name="query"></param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    public async Task<T> GetEntity<T>(string connectionString, object item, string query, CommandType commandType = CommandType.Text)
    {
      using (var connection = new SqlConnection(connectionString))
      {
        if (connection.State == ConnectionState.Closed)
        {
          connection.Open();
        }

        var entity = await connection.QueryAsync<T>(query, item, commandType: commandType);

        return entity.FirstOrDefault();
      }
    }

    /// <summary>
    /// gets entity by Id
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="connectionString"></param>
    /// <param name="query"></param>
    /// <param name="inputParams"></param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    public async Task<T> GetEntityById<T>(string connectionString, string query, object inputParams, CommandType commandType = CommandType.Text)
    {
      using (var connection = new SqlConnection(connectionString))
      {
        if (connection.State == ConnectionState.Closed)
        {
          connection.Open();
        }

        var result = await connection.QueryAsync<T>(query, inputParams, commandType: commandType);
        return result.FirstOrDefault();
      }
    }

    /// <summary>
    /// 
    /// gets Entity Collection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="connectionString"></param>
    /// <param name="query"></param>
    /// <param name="param"></param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    public async Task<IEnumerable<T>> GetEntityCollection<T>(string connectionString, string query, object param, CommandType commandType = CommandType.Text)
    {
      using (var connection = new SqlConnection(connectionString))
      {
        if (connection.State == ConnectionState.Closed)
        {
          connection.Open();
        }

        return await connection.QueryAsync<T>(query, param: param, commandType: commandType);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="connectionString"></param>
    /// <param name="item"></param>
    /// <param name="query"></param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    public async Task<long> InsertAndGetEntityId<T>(string connectionString, T item, string query, CommandType commandType = CommandType.Text)
    {
      using (var connection = new SqlConnection(connectionString))
      {
        if (connection.State == ConnectionState.Closed)
        {
          connection.Open();
        }

        return await connection.ExecuteScalarAsync<long>(query, item, commandType: commandType);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="connectionString"></param>
    /// <param name="item"></param>
    /// <param name="query"></param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    public async Task<bool> InsertOrUpdate<T>(string connectionString, T item, string query, CommandType commandType = CommandType.Text)
    {
      using (var connection = new SqlConnection(connectionString))
      {
        if (connection.State == ConnectionState.Closed)
        {
          connection.Open();
        }

        return await connection.ExecuteScalarAsync<int>(query, item, commandType: commandType) > 0;
      }
    }
  }
}
