namespace BYEsoDomainModelKernel.Models.Language
{
  public class Language : BaseAuditEntity
  {
    public virtual int ID { get; set; }
    public virtual string Name { get; set; }
    // public virtual IsoLanguage IsoLanguage { get; set; }

    protected internal Language()
    {
    }

    public override bool Equals(object other)
    {
      var ot = other as Language;
      if (ot == null)
      {
        return false;
      }
      return ot.ID == ID;
    }

    public override int GetHashCode()
    {
      return ID.GetHashCode();
    }
  }
}
