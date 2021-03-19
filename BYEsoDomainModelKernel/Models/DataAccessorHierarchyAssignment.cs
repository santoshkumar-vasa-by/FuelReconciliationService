namespace BYEsoDomainModelKernel.Models
{
  public class DataAccessorHierarchyAssignment : BaseAuditEntity
  {
    public virtual DataAccessor DataAccessor { get; set; }
    public virtual DataAccessor AssignedDataAccessor { get; set; }
    public virtual DataAccessorHierarchyAssignmentAssociationType AssociationType { get; set; }

    public DataAccessorHierarchyAssignment()
    {
    }

    public DataAccessorHierarchyAssignment(DataAccessor dataAccessor, DataAccessor assignedDataAccessor,
      DataAccessorHierarchyAssignmentAssociationType associationType)
    {
      DataAccessor = dataAccessor;
      AssignedDataAccessor = assignedDataAccessor;
      AssociationType = associationType;
    }

    public override bool Equals(object other)
    {
      var ot = other as DataAccessorHierarchyAssignment;

      if (ot == null)
      {
        return false;
      }

      return ot.DataAccessor.Equals(DataAccessor) && ot.AssignedDataAccessor.Equals(AssignedDataAccessor);
    }

    public override int GetHashCode()
    {
      return DataAccessor.ID.GetHashCode() ^ AssignedDataAccessor.ID.GetHashCode();
    }
  }

  public enum DataAccessorHierarchyAssignmentAssociationType
  {
    Up = 'u',
    Down = 'd',
    Sideways = 's'
  }
}