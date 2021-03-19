using FluentNHibernate.Mapping;
using RP.DatabaseMetadata;

namespace BYEsoDomainModelKernel.Models
{
  public class OrganizationalHierarchyLevelMap : ClassMap<OrganizationalHierarchyLevel>
  {
    public OrganizationalHierarchyLevelMap()
    {
      var tableMeta = WaveDatabaseMetadata.Org_Hierarchy_Level;
      this.Table(tableMeta);
      BatchSize(100);

      Id(x => x.ID)
        .Column(tableMeta.Org_hierarchy_level_id)
        .GeneratedBy.Assigned();

      Map(x => x.Name).Column(tableMeta.Name);
      Map(x => x.SortOrder).Column(tableMeta.Sort_order);
    }
  }
}
