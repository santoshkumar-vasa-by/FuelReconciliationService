using System.Collections.Generic;
using System.Linq;

namespace BYEsoDomainModelKernel.Models
{
  internal interface ICDMEntity
  {
    OrganizationalHierarchy Owner { get; }

    bool IsVisibleToSite(Site site);
  }

  public interface ICDMEntityWithOwnerInCompositeID
  {
  }

  public abstract class BaseCDMEntity<T> : BaseAuditEntity, ICDMEntity where T : DataAccessorAssignmentEntity
  {
    public abstract IEnumerable<T> DataAccessorAssignments { get; }

    public virtual OrganizationalHierarchy Owner { get; set; }

    protected BaseCDMEntity()
    {
    }

    protected BaseCDMEntity(OrganizationalHierarchy owner)
    {
      Owner = owner;
    }

    public virtual bool IsVisibleToSite(Site site)
    {
      var firstMatch = DataAccessorAssignments.FirstOrDefault(x => x.ImpactedDataAccessorsIncludesSite(site));

      return firstMatch != null;
    }
  }
}
