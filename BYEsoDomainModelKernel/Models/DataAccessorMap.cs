using FluentNHibernate.Mapping;
using WaveDatabaseMetadata = RP.DatabaseMetadata.WaveDatabaseMetadata;

namespace BYEsoDomainModelKernel.Models
{
  public class DataAccessorMap : ClassMap<DataAccessor>
  {
    public DataAccessorMap()
    {
      var tableMeta = WaveDatabaseMetadata.Rad_Sys_Data_Accessor;
      this.Table(tableMeta);
      BatchSize(500);

      Id(x => x.ID).Column(tableMeta.Data_accessor_id).GeneratedBy.Assigned();
      Map(x => x.Code).Column(tableMeta.Name);
      Map(x => x.Name).Column(tableMeta.Long_name);
      Map(x => x.ClientId).Column(tableMeta.Client_id);

      HasMany(x => x.DataAccessorsThatIImpactThroughAssignment)
        .KeyColumn(WaveDatabaseMetadata.DA_List_DRO.Assigned_data_accessor_id)
        .BatchSize(100)
        .Inverse()
        .Cascade.All();

      HasMany(x => x.DataAccessorsThatImpactMeThroughAssignment)
        .KeyColumn(WaveDatabaseMetadata.DA_List_DRO.Current_org_hierarchy_id)
        .BatchSize(100)
        .Inverse()
        .Cascade.All();
    }
  }
}
