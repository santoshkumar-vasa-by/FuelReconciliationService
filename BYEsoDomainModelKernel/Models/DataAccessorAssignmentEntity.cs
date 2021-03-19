using System.Collections.Generic;
using System.Linq;

namespace BYEsoDomainModelKernel.Models
{
  public class DataAccessorAssignmentEntity : BaseAuditEntity
  {
    public virtual DataAccessor DataAccessor { get; set; }

    public DataAccessorAssignmentEntity()
    {
    }

    public DataAccessorAssignmentEntity(DataAccessor dataAccessor)
    {
      DataAccessor = dataAccessor;
    }

    private IEnumerable<DataAccessor> _ImpactedDataAccessors
    {
      get
      {
        return DataAccessor.DataAccessorsThatIImpactThroughAssignment.Select(x => x.DataAccessor);
      }
    }

    public virtual bool ImpactedDataAccessorsIncludesSite(Site site)
    {
      return _ImpactedDataAccessors.Contains(site.DataAccessor);
    }
  }
}