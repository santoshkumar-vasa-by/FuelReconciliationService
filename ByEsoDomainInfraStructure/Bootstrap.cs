using ByEsoDomainInfraStructure.Cache.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using ByEsoDomainInfraStructure.UnitOfWork;
using Moq;
using Newtonsoft.Json;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;

namespace ByEsoDomainInfraStructure
{
  public static class InfrastructureBootstrap
  {
    public static void RegisterDependencies(IServiceCollection services)
    {
      var redisCacheService = new Mock<ICacheService>();
      var sessionData = new SessionData
      {
        ClientId = 1000001,
        CurrentHierarchyId = 1,
        DefaultLanguageId = 123,
        DepartmentFilterId = 7,
        SecurityAccessId = "1",
        SecurityGroups = new int[]{ 123, 456 },
        UserId = 102102
      };
      redisCacheService.Setup(x => x.GetValue(It.IsAny<string>())).Returns(JsonConvert.SerializeObject(sessionData));
      services.AddSingleton<ICacheService>(provider => redisCacheService.Object);
      services.AddScoped<IStateManagementService, StateManagementService>();
      services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
    }
  }

  
}
