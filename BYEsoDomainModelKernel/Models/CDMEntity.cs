namespace BYEsoDomainModelKernel.Models
{
  public abstract class CDMEntity : BaseCDMEntity<DataAccessorAssignmentEntity>
  {
    protected CDMEntity()
    {
    }

    protected CDMEntity(OrganizationalHierarchy owner)
      : base(owner)
    {
    }
  }
}
