using System;

namespace BYEsoDomainModelKernel.Models
{
  [Serializable]
  public class UnitOfMeasureDto
  {
    public virtual int UomId { get; set; }
    public virtual string UomName { get; set; }
    public virtual int BaseUomId { get; set; }
    public virtual string BaseUomName { get; set; }
    public virtual string MeasureType { get; set; }
    public virtual decimal NumberOfBaseUnits { get; set; }
    public virtual string MeasureTypeCode { get; set; }
  }
}
