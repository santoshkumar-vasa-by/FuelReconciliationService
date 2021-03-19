using NHibernate;
using RP.DomainModelKernel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ByEsoDomainInfraStructure.Cache;
using ByEsoDomainInfraStructure.Cache.Interfaces;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using RP.DomainModelKernel.Common.DatabaseConnectionSettings;
using NHibernate.Cfg;

namespace ByEsoDomainInfraStructure.UnitOfWork
{
  public class UnitOfWork : IUnitOfWork
  {
    private ISession _Session;
    private ITransaction _Transaction;
    private readonly ISessionFactory _SessionFactory;

    protected ISessionData SessionData;
    protected List<Action> CommitHandlers = new List<Action>();
    protected List<Action> DisposeHandlers = new List<Action>();
    protected List<IQuery> UpdateQueries = new List<IQuery>();

    private ISession _WarehouseSession;
    protected List<Action> AfterCommitHandlers = new List<Action>();
    private readonly ISessionFactory _WarehouseSessionFactory;

    public UnitOfWork(IStateManagementService stateManagementService, ISessionFactory sessionFactory, ISession session, ISessionFactory whSessionFactory)
    {
      SessionData = stateManagementService.GetSessionData("");
      _SessionFactory = _SessionFactory ?? sessionFactory;
      _Session = session;
      _WarehouseSessionFactory = _WarehouseSessionFactory ?? whSessionFactory;
    }

    public async Task Commit()
    {
      FireOnCommit();
      if (_Session != null)
      {
        using (var trans = _Session.BeginTransaction())
        {
          _Transaction = trans;
          RegisterUserInSpIdTable(trans);
          await trans.CommitAsync();
        }
      }

      if (_WarehouseSession != null)
      {
        using (var trans = _WarehouseSession.BeginTransaction())
        {
          await trans.CommitAsync();
        }
      }
      FireAfterCommit();
    }

    public async Task Rollback()
    {
      await _Transaction.RollbackAsync();
    }

    public void CloseTransaction()
    {
      if (_Transaction == null) return;
      _Transaction.Dispose();
      _Transaction = null;
    }

    protected virtual void RegisterUserInSpIdTable(ITransaction trans)
    {
      SpIdHelper.RegisterUserInSpIdTable(trans, _Session, SessionData);
    }

    public ISessionData GetSessionData()
    {
      return SessionData;
    }

    public ISession GetSession()
    {
      return _Session;
    }

    public ISession GetWarehouseSession()
    {
      return _WarehouseSessionFactory.OpenSession();
      //var whConnectionString = @"server=ga1npdvperfdb10\\Retail_2017v;database=qe_manual_bos_wh;uid=waveuser;pwd=xlt3141";
      //var config = PrepareConfiguration(whConnectionString);
      //var sessionFactory = config.BuildSessionFactory();
      //return sessionFactory.OpenSession();
    }

    public void Dispose()
    {
      FireOnDispose();
      if (_Session != null)
      {
        _Session.Dispose();
        _Session = null;
      }
      if (_WarehouseSession != null)
      {
        _WarehouseSession.Dispose();
        _WarehouseSession = null;
      }

      CommitHandlers = null;
      AfterCommitHandlers = null;
      DisposeHandlers = null;
    }

    public void OnCommit(Action callback)
    {
      CommitHandlers.Add(callback);
    }

    public void OnDispose(Action callback)
    {
      DisposeHandlers.Add(callback);
    }

    public void AfterCommit(Action callback)
    {
      AfterCommitHandlers.Add(callback);
    }

    public void AddQueryForTransaction(IQuery updateQuery)
    {
      UpdateQueries.Add(updateQuery);
    }

    public void FireOnDispose()
    {
      foreach (var handler in DisposeHandlers)
      {
        handler();
      }
    }

    public void FireOnCommit()
    {
      try
      {
        foreach (var handler in CommitHandlers)
        {
          handler();
        }
      }
      catch (Exception)
      {
        _Session?.Clear();
        _WarehouseSession?.Clear();
        throw;
      }
    }

    public void FireAfterCommit()
    {
      try
      {
        foreach (var handler in AfterCommitHandlers)
        {
          handler();
        }
      }
      catch (Exception)
      {
        _Session?.Clear();
        _WarehouseSession?.Clear();
        throw;
      }
    }

    private static Configuration PrepareConfiguration(string connectionString)
    {
      var configBase = Fluently.Configure()
        .Database(MsSqlConfiguration.MsSql2005.ConnectionString(c => c.Is(connectionString)))
        .Mappings(m => { m.FluentMappings.AddFromAssembly(typeof(BaseAuditEntity).Assembly).Conventions.Add<DecimalConvention>(); });

      var config = configBase
        .BuildConfiguration();
      config.SetProperty("adonet.batch_size", "100");
      return config;
    }

    internal static ISessionFactory CreateSessionFactory()
    {
      IDatabaseConnectionSettings dbSettings = Registry.GetDatabaseConnectionSettings();
      IPersistenceConfigurer persister = CreatePersistenceConfigurer(dbSettings.OltpConnectionString);
      return CreateSessionFactoryImpl(persister);
    }

    internal static IPersistenceConfigurer CreatePersistenceConfigurer(string connectionString)
    {
      return MsSqlConfiguration.MsSql2005.ConnectionString(c => c.Is(connectionString));
    }

    internal static ISessionFactory CreateSessionFactoryImpl(IPersistenceConfigurer persistenceConfigurer)
    {
      //_Logger.Info("Create session factory");

      var assemblies = AppDomain.CurrentDomain.GetAssemblies()
        .Where(a => !a.IsDynamic && AssemblyContainsClassMap(a));
      //&& string.Compare(a.FullName, "FluentNHibernate", StringComparison.OrdinalIgnoreCase) < 0);

      var configBase = Fluently.Configure()
        .Database(persistenceConfigurer)
        .Mappings(m =>
                  {
                    foreach (var assembly in assemblies)
                    {
                      m.FluentMappings.AddFromAssembly(assembly).Conventions.Add<DecimalConvention>();
                    }
                  });

      var config = configBase
        .BuildConfiguration();

      config.SetProperty("adonet.batch_size", "100");
      NHibernateListenerHelper.CreateAndRegisterListeners(config);

      return config.BuildSessionFactory();
    }
    private static bool AssemblyContainsClassMap(Assembly assembly)
    {
      bool exists = assembly.GetExportedTypes().Any(t => IsClassMap(t));

      return exists;
    }

    private static bool IsClassMap(Type type)
    {
      return type.BaseType != null && type.BaseType.IsGenericType &&
             type.BaseType.GetGenericTypeDefinition() == typeof(ClassMap<>);
    }

  }
}
