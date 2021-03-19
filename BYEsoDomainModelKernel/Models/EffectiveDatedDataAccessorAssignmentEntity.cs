using System;

namespace BYEsoDomainModelKernel.Models
{
  public class EffectiveDatedDataAccessorAssignmentEntity : DataAccessorAssignmentEntity
  {
    public virtual DateTime Start { get; set; }
    public virtual DateTime End { get; set; }

    public EffectiveDatedDataAccessorAssignmentEntity()
    {
    }

    public EffectiveDatedDataAccessorAssignmentEntity(DataAccessor dataAccessor, DateTime start, DateTime end)
      : base(dataAccessor)
    {
      Start = start;
      End = end;
    }
  }
}
