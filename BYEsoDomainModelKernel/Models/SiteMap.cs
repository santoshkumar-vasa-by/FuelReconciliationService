using FluentNHibernate.Mapping;
using WaveDatabaseMetadata = RP.DatabaseMetadata.WaveDatabaseMetadata;

namespace BYEsoDomainModelKernel.Models
{
  public partial class Site
  {
    public static class MapExpressions
    {
      //public static readonly Expression<Func<RP.DomainModelKernel.Hierarchy.Site, IEnumerable<SiteGroupSiteAssignment>>>
      //  SiteGroupAssignments = x => x._SiteGroupAssignments;
    }
  }

  public class SiteMap : ClassMap<Site>
  {
    public SiteMap()
    {
      var tableMeta = WaveDatabaseMetadata.Business_Unit;

      this.Table(tableMeta);
      BatchSize(1000);

      Id(x => x.ID).Column(tableMeta.Business_unit_id)
        .GeneratedBy.Assigned();

      Map(x => x.EDINumber).Column(tableMeta.Edi_number);

      Map(x => x.BusinessOperatorID).Column(tableMeta.Business_operator_id);

      References(x => x.DataAccessor)
        .Column(tableMeta.Business_unit_id)
        .Not.Insert();

      References(x => x.OrganizationalHierarchy)
        .Column(tableMeta.Business_unit_id)
        .Not.Insert()
        .Not.Update();

      References(x => x.TimeZone)
        .Column(tableMeta.Time_zone_id)
        .Not.Update();

      References(x => x.BillingAddress)
        .Column(tableMeta.Billing_address_id)
        .Cascade.SaveUpdate();

      References(x => x.Address)
        .Column(tableMeta.Address_id)
        .Cascade.SaveUpdate();

      //HasMany(RP.DomainModelKernel.Hierarchy.Site.MapExpressions.SiteGroupAssignments)
      //  .KeyColumn(tableMeta.Business_unit_id)
      //  .BatchSize(100)
      //  .Cascade.All();

      //Map(x => x.Status)
      //  .Column(tableMeta.Status_code)
      //  .CustomType<EnumCharType<Status>>();

      Map(x => x.DateCreated)
        .Column(tableMeta.Date_created);

      Map(x => x.DateStatusCodeLastChanged)
        .Column(tableMeta.Date_status_code_last_changed);

    }
  }
}
