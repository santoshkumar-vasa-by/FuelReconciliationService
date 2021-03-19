
namespace BYEsoDomainModelKernel.Models.Common
{
  public class TimeZoneDefinition : BaseAuditEntity
  {
    public virtual int Id { get; set; }
    public virtual string Name { get; set; }

    protected internal TimeZoneDefinition()
    {

    }

    public TimeZoneDefinition(string name) : this()
    {
      Name = name;
    }
  }
}