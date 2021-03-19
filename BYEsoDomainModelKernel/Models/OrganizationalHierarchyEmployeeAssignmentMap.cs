using FluentNHibernate.Mapping;
using RP.DatabaseMetadata;

namespace BYEsoDomainModelKernel.Models
{
  public class OrganizationalHierarchyEmployeeAssignmentMap : ClassMap<OrganizationalHierarchyEmployeeAssignment>
  {
    public OrganizationalHierarchyEmployeeAssignmentMap()
    {
      var tableMeta = WaveDatabaseMetadata.Rad_Sys_User_Hier_List;
      this.Table(tableMeta);
      BatchSize(100);

      CompositeId()
        .KeyProperty(x => x.OriganizationalHierarchyID, tableMeta.Org_hierarchy_id)
        .KeyProperty(x => x.EmployeeID, tableMeta.User_id);

      //References(x => x.Employee)
      //  .Column(tableMeta.User_id)
      //  .Not.Insert()
      //  .Not.Update()
      //  .Cascade.None();

      References(x => x.OrganizationalHierarchy)
        .Column(tableMeta.Org_hierarchy_id)
        .Not.Insert()
        .Not.Update()
        .Cascade.None();
    }
  }
}
