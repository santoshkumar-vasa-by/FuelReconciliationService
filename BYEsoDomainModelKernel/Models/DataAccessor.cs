using System.Collections.Generic;

namespace BYEsoDomainModelKernel.Models
{
  public class DataAccessor
  {
    public virtual int ID { get; set; }
    public virtual string Code { get; set; }
    public virtual string Name { get; set; }
    public virtual int ClientId { get; set; }
    public virtual IList<DataAccessorHierarchyAssignment> DataAccessorsThatIImpactThroughAssignment { get; set; }
    public virtual IList<DataAccessorHierarchyAssignment> DataAccessorsThatImpactMeThroughAssignment { get; set; }

    protected internal DataAccessor()
    {
      DataAccessorsThatIImpactThroughAssignment = new List<DataAccessorHierarchyAssignment>();
      DataAccessorsThatImpactMeThroughAssignment = new List<DataAccessorHierarchyAssignment>();
    }

    public DataAccessor(int dataAccessorID, string code, string name)
      : this()
    {
      ID = dataAccessorID;
      Code = code;
      Name = name;
    }
  }
}
