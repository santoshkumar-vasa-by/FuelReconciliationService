using System;
using System.Linq;

namespace BYEsoDomainModelKernel.Models
{
  public abstract class EffectiveDateCDMEntity : BaseCDMEntity<EffectiveDatedDataAccessorAssignmentEntity>
  {
    protected EffectiveDateCDMEntity()
    {
    }

    protected EffectiveDateCDMEntity(OrganizationalHierarchy owner)
      : base(owner)
    {
    }

    public override bool IsVisibleToSite(Site site)
    {
      return IsVisibleToSiteOnDate(site, site.OrganizationalHierarchy.CurrentBusinessDate);
    }

    public virtual bool IsVisibleToSiteOnDate(Site site, DateTime date)
    {
      var firstMatch = DataAccessorAssignments.Where(x => x.Start <= date && x.End >= date)
        .FirstOrDefault(x => x.ImpactedDataAccessorsIncludesSite(site));

      return firstMatch != null;
    }
  }
}