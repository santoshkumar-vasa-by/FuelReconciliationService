namespace BYEsoDomainModelKernel.Models
{
  public class OrganizationalHierarchyLevel
  {

    public const float ENTERPRISE_SORT_ORDER = 1.0f;
    public const float SITE_SORT_ORDER = 999.0f;
    //Even though are Domain language calls theses "Sites" and "Clients", The legacy code requires that these
    //still be called BU and Enterprise since these only exist once in the DB and all clients use the same values.
    public const string SITE_NAME = "BU";
    public const string ENTERPRISE_NAME = "Enterprise";

    public virtual int ID { get; set; }
    public virtual string Name { get; set; }
    public virtual decimal SortOrder { get; set; }

    protected internal OrganizationalHierarchyLevel()
    {
    }

    public override bool Equals(object obj)
    {
      var other = obj as OrganizationalHierarchyLevel;

      if (other != null)
      {
        return ID == other.ID;
      }
      return false;
    }

    public override int GetHashCode()
    {
      return ID.GetHashCode();
    }

    public virtual bool IsSiteLevel
    {
      get
      {
        return SortOrder == (decimal)SITE_SORT_ORDER;
      }
    }

  }

  public enum OrgHierarchyLevelIDs
  {
    Corporate = 1,
    Site = 2
  }

 

}
