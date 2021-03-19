using System;
using System.Linq;
using System.Reflection;
using ByEsoDomainInfraStructure;
using ByEsoDomainInfraStructure.UnitOfWork;
using BYEsoDomainModelKernel;
using BYEsoDomainModelKernel.Models;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Event;
using NHibernate.Mapping.ByCode;
using BaseAuditEntity = BYEsoDomainModelKernel.Models.BaseAuditEntity;


namespace FuelReconciliationDomainModel
{
  public static class DomainModelBootstrap
  {
    public static void Bootstrap(IServiceCollection services, string connectionString, string whConnectionString)
    {


      //services.AddScoped<ITicketGenerator, ProductionTicketGenerator>();
      //services.AddScoped<TicketGenerator>(t => new TicketGenerator(t.GetRequiredService<ITicketGenerator>()));
      services.AddNHibernate(connectionString, whConnectionString);

      // Repositories
      services.AddScoped<ISiteRepository, SiteRepository>();
      services.AddScoped<IFuelTankSummaryRepository, FuelTankSummaryRepository>();
      services.AddScoped<IDatamartFuelHoseRepository, DatamartFuelHoseRepository>();
      services.AddScoped<IDayStatusRepository, DayStatusRepository>();
      services.AddScoped<IFuelAdjustmentRepository, FuelAdjustmentRepository>();
      services.AddScoped<IFuelBlendItemPercentageRepository, FuelBlendItemPercentageRepository>();
      services.AddScoped<IFuelDeliveryInvoiceTotalRepository, FuelDeliveryInvoiceTotalRepository>();
      services.AddScoped<IFuelDeliveryRepository, FuelDeliveryRepository>();
      services.AddScoped<IFuelEventNonSaleRepository, FuelEventNonSaleRepository>();
      services.AddScoped<IFuelHoseRepository, FuelHoseRepository>();
      services.AddScoped<IFuelInvoiceRepository, FuelInvoiceRepository>();
      services.AddScoped<IFuelSalesItemRepository, FuelSalesItemRepository>();
      services.AddScoped<IFuelTankMeterReadingLineItemRepository, FuelTankMeterReadingLineItemRepository>();
      services.AddScoped<IFuelTankReadingRepository, FuelTankReadingRepository>();
      services.AddScoped<IFuelTankRepository, FuelTankRepository>();
      services.AddScoped<IPostedSaleItemRepository, PostedSaleItemRepository>();
      services.AddScoped<ISiteClosedDaysRepository, SiteClosedDaysRepository>();
      services.AddScoped<IItemRepository, ItemRepository>();
      services.AddScoped<ISalesTransactionLineItemRepository, SalesTransactionLineItemRepository>();
      services.AddScoped<IFuelItemSummaryRepository, FuelItemSummaryRepository>();
      
      // Services
      services.AddScoped<IFuelAdjustmentSelectionService, FuelAdjustmentSelectionService>();
      services.AddScoped<IFuelDeliverySelectionService, FuelDeliverySelectionService>();
      services.AddScoped<IFuelHoseService, FuelHoseService>();
      services.AddScoped<IFuelMeterReadingSelectionService, FuelMeterReadingSelectionService>();
      services.AddScoped<IFuelTankReadingSelectionService, FuelTankReadingSelectionService>();
      services.AddScoped<IPumpTestSelectionService, PumpTestSelectionService>();
      services.AddScoped<ISalesTransactionLineItemSummaryService, SalesTransactionLineItemSummaryService>();
      services.AddScoped<IFuelTankLookupService, FuelTankLookupService>();
      services.AddScoped<IFuelItemSummaryService, FuelItemSummaryService>();
    }
  }

  public static class NHibernateExtensions
  {
    public static IServiceCollection AddNHibernate(this IServiceCollection services, string connectionString, string whConnectionString)
    { 
      var defaultConfiguration = PrepareConfiguration(connectionString);
      //NHibernateListenerHelper.CreateAndRegisterListeners(config);
      var whConfiguration = PrepareConfiguration(whConnectionString);

      var sessionFactory = defaultConfiguration.BuildSessionFactory();
      var whSessionFactory = whConfiguration.BuildSessionFactory();

      services.AddSingleton(sessionFactory);
      var sessionData = new SessionData
      {
        ClientId = 1000001,
        CurrentHierarchyId = 1,
        DefaultLanguageId = 123,
        DepartmentFilterId = 7,
        SecurityAccessId = "1",
        SecurityGroups = new int[] { 123, 456 },
        UserId = 102102
      };
      services.AddTransient(factory => sessionFactory.OpenSession(new Interceptor(sessionData)));
      services.AddScoped<IUnitOfWork>(x => new UnitOfWork(x.GetService<IStateManagementService>(),
                                        x.GetService<ISessionFactory>(),
                                        x.GetService<ISession>(),
                                               whSessionFactory));

      return services;
    }

    private static Configuration PrepareConfiguration(string connectionString)
    {
      var configBase = Fluently.Configure()
        .Database(MsSqlConfiguration.MsSql2005.ConnectionString(c => c.Is(connectionString)))
        .Mappings(m => { m.FluentMappings.AddFromAssembly(typeof(BaseAuditEntity).Assembly).Conventions.Add<RP.DomainModelKernel.Common.DecimalConvention>(); });

      var config = configBase
        .BuildConfiguration();
      config.SetProperty("adonet.batch_size", "100");
      return config;
    }
  }

  public static class NHibernateListenerHelper
  {
    //public static void CreateAndRegisterListeners(Configuration config)
    //{
    //  var deleteManager = new BatchDeleteManager();
    //  var insertManager = new BatchInsertManager();

    //  var deleteListener = new DeleteEventListener(deleteManager);
    //  config.SetListener(ListenerType.Delete, deleteListener);

    //  var saveListener = new SaveEventListener(insertManager);
    //  config.SetListener(ListenerType.Save, saveListener);
    //  var saveOrUpdateListener = new SaveOrUpdateEventListener(insertManager);
    //  config.SetListener(ListenerType.SaveUpdate, saveOrUpdateListener);

    //  var flushListener = new FlushEventListener(deleteManager, insertManager);
    //  config.SetListener(ListenerType.Flush, flushListener);
    //}
  }
}
