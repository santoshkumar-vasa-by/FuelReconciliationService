namespace BYEsoDomainModelKernel.Models
{
  public static class OrganizationalHierarchyLevelFactory
  {
    public static OrganizationalHierarchyLevel CreateOrganizationalHierarchyLevel(int id, string name, int sortOrder)
    {
      var ohl = new OrganizationalHierarchyLevel
      {
        ID = id,
        Name = name,
        SortOrder = sortOrder
      };

      return ohl;
    }

    public static OrganizationalHierarchyLevel CreateSiteLevel()
    {
      return new OrganizationalHierarchyLevel
      {
        Name = OrganizationalHierarchyLevel.SITE_NAME,
        SortOrder = (decimal)OrganizationalHierarchyLevel.SITE_SORT_ORDER
      };
    }
  }
}
