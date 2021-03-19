
using BYEsoDomainModelKernel.Models.Common;

namespace BYEsoDomainModelKernel.Models
{
  public class UnitOfMeasureTranslation : BaseAuditEntity, ITranslation
  {
    public virtual int LanguageId { get; set; }
    public virtual UnitOfMeasure UnitOfMeasure { get; set; }
    public virtual string Name { get; set; }
    public virtual string GetUniqueKey()
    {
      return UnitOfMeasure.GetUniqueKey(LanguageId);
    }

    public override bool Equals(object obj)
    {
      var ot = obj as UnitOfMeasureTranslation;
      if (ot == null)
      {
        return false;
      }
      return UnitOfMeasure.Equals(ot.UnitOfMeasure) && LanguageId.Equals(ot.LanguageId);
    }

    public override int GetHashCode()
    {
      return UnitOfMeasure.ID.GetHashCode() ^ LanguageId.GetHashCode();
    }
  }
}
