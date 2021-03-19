using FluentNHibernate.Mapping;
using NHibernate.Type;
using WaveDatabaseMetadata = RP.DatabaseMetadata.WaveDatabaseMetadata;

namespace BYEsoDomainModelKernel.Models
{
  public class DataAccessorHierarchyAssignmentMap : ClassMap<DataAccessorHierarchyAssignment>
  {
    public DataAccessorHierarchyAssignmentMap()
    {
      var tableMeta = WaveDatabaseMetadata.DA_List_DRO;

      this.Table(tableMeta);
      BatchSize(100);

      CompositeId()
        .KeyReference(x => x.DataAccessor, tableMeta.Current_org_hierarchy_id)
        .KeyReference(x => x.AssignedDataAccessor, tableMeta.Assigned_data_accessor_id);

      References(x => x.DataAccessor)
        .Column(tableMeta.Current_org_hierarchy_id)
        .Not.Insert()
        .Not.Update();

      References(x => x.AssignedDataAccessor)
        .Column(tableMeta.Assigned_data_accessor_id)
        .Not.Insert()
        .Not.Update();

      Map(x => x.AssociationType)
        .Column(tableMeta.Association_type_code)
        .CustomType<EnumCharType<DataAccessorHierarchyAssignmentAssociationType>>();
    }
  }
}
