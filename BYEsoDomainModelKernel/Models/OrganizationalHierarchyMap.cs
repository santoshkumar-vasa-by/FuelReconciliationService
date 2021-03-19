using FluentNHibernate.Mapping;
using RP.DatabaseMetadata;

namespace BYEsoDomainModelKernel.Models
{
  public class OrganizationalHierarchyMap : ClassMap<OrganizationalHierarchy>
  {
    public OrganizationalHierarchyMap()
    {
      var tableMeta = WaveDatabaseMetadata.Org_Hierarchy;
      this.Table(tableMeta);
      BatchSize(100);

      Id(x => x.ID)
        .Column(tableMeta.Org_hierarchy_id)
        .GeneratedBy.Assigned();

      Map(x => x._CurrentBusinessDate).Column(tableMeta.Current_business_date);

      References(x => x.DataAccessor)
        .Column(tableMeta.Org_hierarchy_id)
        .Not.Insert()
        .Not.Update()
        .Cascade.All();

      References(x => x.Language).Column(tableMeta.Primary_language_id)
                                 .Cascade.None();

      References(x => x.Level)
        .Column(tableMeta.Org_hierarchy_level_id)
            .Cascade.None();

      References(x => x.Parent)
        .Column(tableMeta.Parent_org_hierarchy_id)
        .Cascade.None();

      HasMany(x => x._Children)
        .KeyColumn(tableMeta.Parent_org_hierarchy_id)
        .BatchSize(100)
        .Cascade.AllDeleteOrphan();

      HasMany(x => x._EmployeeAssignments)
        .KeyColumn(tableMeta.Org_hierarchy_id)
        .BatchSize(100)
        .Cascade.AllDeleteOrphan();
    }
  }
}
