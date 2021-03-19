using RP.DomainModelKernel.Common;

namespace BYEsoDomainModelKernel.Models
{
  public class UnitOfMeasureClass
  {
    public virtual int ID { get; set; }
    public virtual string Name { get; set; }
    public virtual UnitOfMeasure BaseUnitOfMeasure { get; set; }
    public virtual string MeasureTypeCode { get; set; }
    public virtual int ClientID { get; set; }

    public virtual bool IsCount
    {
      get
      {
        return ID == (int)RegisteredUnitOfMeasureClass.Count;
      }
    }

    public virtual bool IsWeight
    {
      get
      {
        bool isImperialWeightClass = ID == (int)RegisteredUnitOfMeasureClass.ImperialWeight;
        bool isMetricWeightClass = ID == (int)RegisteredUnitOfMeasureClass.MetricWeight;

        bool isEitherWeightClass = isImperialWeightClass || isMetricWeightClass;

        return isEitherWeightClass;
      }
    }

    protected internal UnitOfMeasureClass()
    {
    }

    protected internal UnitOfMeasureClass(string name)
    {
      Name = name;
    }

    protected internal UnitOfMeasureClass(int id, string name)
      : this(name)
    {
      ID = id;
    }
    protected internal UnitOfMeasureClass(int id, string name, int clientId)
     : this(id, name)
    {
      ClientID = clientId;
    }
    public virtual void SetBaseUnitOfMeasure(UnitOfMeasure baseUOM)
    {
      if (!baseUOM.UnitOfMeasureClass.Equals(this))
      {
        throw new DomainOperationException<UnitOfMeasureClassErrors>(UnitOfMeasureClassErrors.BaseUOMNotInClass);
      }

      BaseUnitOfMeasure = baseUOM;
    }

    public virtual void SetMeasureTypeCodeFromTest(IUnitOfWork uow, string measureTypeCode)
    {
      uow.EnforceRunningInTest("SetMeasureTypeCodeFromTest can only be called from test");
      MeasureTypeCode = measureTypeCode;
    }
   
    public override bool Equals(object other)
    {
      UnitOfMeasureClass ot = other as UnitOfMeasureClass;
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

  internal enum UnitOfMeasureClassErrors
  {
    BaseUOMNotInClass
  }

  public enum RegisteredUnitOfMeasureClass
  {
    ImperialVolume = 1,
    ImperialWeight = 2,
    Count = 3,
    Time = 6,
    MetricVolume = 14,
    MetricWeight = 15
  }

  public enum MeasureTypeCode
  {
    Metric = 'm',
    Imperial = 'i'
  }
}