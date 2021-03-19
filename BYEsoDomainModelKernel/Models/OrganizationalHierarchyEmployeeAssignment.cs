
namespace BYEsoDomainModelKernel.Models
{
  public class OrganizationalHierarchyEmployeeAssignment : BaseAuditEntity
  {
    public virtual int OriganizationalHierarchyID { get; set; }
    public virtual int EmployeeID { get; set; }

    //public virtual Employee Employee { get; set; }
    public virtual OrganizationalHierarchy OrganizationalHierarchy { get; set; }

    public override bool Equals(object obj)
    {
      var other = obj as OrganizationalHierarchyEmployeeAssignment;

      if (other != null)
      {
        return EmployeeID == other.EmployeeID && OriganizationalHierarchyID == other.OriganizationalHierarchyID;
      }
      return false;
    }

    public override int GetHashCode()
    {
      return EmployeeID.GetHashCode() ^ OriganizationalHierarchyID.GetHashCode();
    }
  }
}
