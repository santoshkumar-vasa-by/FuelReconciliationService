using System;
using System.Collections.Generic;
using RP.DomainModelKernel.Common;

namespace BYEsoDomainModelKernel.Models
{
  public class UnitOfMeasure : BaseAuditEntity, Common.ITranslatable<UnitOfMeasureTranslation>
  {
    public virtual int ID { get; set; }
    public virtual string Name { get; set; }
    public virtual decimal Factor { get; set; }
    public virtual UnitOfMeasureClass UnitOfMeasureClass { get; set; }
    public virtual bool IsActive { get; set; }
    public virtual bool IsPurge { get; set; }
    public virtual IEnumerable<UnitOfMeasureTranslation> Translations { get; set; }
    private IList<UnitOfMeasureTranslation> TranslationsList => Translations as IList<UnitOfMeasureTranslation>;

    public virtual string GetUniqueKey(int languageID)
    {
      return ID + "|" + languageID;
    }
    protected internal UnitOfMeasure()
    {
      Translations = new List<UnitOfMeasureTranslation>();
    }

    protected internal UnitOfMeasure(string name, decimal factor, UnitOfMeasureClass uomClass)
    {
      Name = name;
      Factor = factor;
      UnitOfMeasureClass = uomClass;
      Translations = new List<UnitOfMeasureTranslation>();
    }

    protected internal UnitOfMeasure(int id, string name, decimal factor, UnitOfMeasureClass uomClass, bool isActive)
      : this(name, factor, uomClass)
    {
      ID = id;
      IsActive = isActive;
      Translations = new List<UnitOfMeasureTranslation>();
    }

    public virtual void SetIsActiveFromTest(IUnitOfWork uow, bool active)
    {
      uow.EnforceRunningInTest("SetIsActiveFromTest can only be called from test");

      IsActive = active;
    }

    public virtual void SetIsPurgeFromTest(IUnitOfWork uow, bool purge)
    {
      uow.EnforceRunningInTest("SetIsPurgeFromTest can only be called from test");

      IsPurge = purge;
    }

    public override bool Equals(object other)
    {
      UnitOfMeasure ot = other as UnitOfMeasure;
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

    public virtual decimal ConvertAmountFromUnitOfMeasure(decimal amount, UnitOfMeasure sourceUOM)
    {
      if (!UnitOfMeasureClass.Equals(sourceUOM.UnitOfMeasureClass))
      {
        throw new NotSupportedException("Can't convert between UOM classes");
      }

      return amount / sourceUOM.Factor * Factor;
    }

    public virtual decimal ConvertQuantityFromUnitOfMeasure(decimal qty, UnitOfMeasure sourceUOM)
    {
      if (!UnitOfMeasureClass.Equals(sourceUOM.UnitOfMeasureClass))
      {
        throw new NotSupportedException("Can't convert between UOM classes");
      }

      return qty * sourceUOM.Factor / Factor;
    }

    public virtual void AddTranslation(Language.Language language, string translatedName)
    {
      TranslationsList.Add(new UnitOfMeasureTranslation
      {
        UnitOfMeasure = this,
        LanguageId = language.ID,
        Name = translatedName
      });
    }
  }

  public enum UnitOfMeasureDomainErrors
  {
    ItemNotFound,
    NoUomFound,
    NoUnitOfMeasureFound,
    UomClassNotFound,
    InvalidTrackingUomId
  }

  public enum RegisteredUnitOfMeasure
  {
    Each = 3,
    Case = 8,
    Pound = 2,
    FlOunce = 5,
    WtOunce = 4
  }
}