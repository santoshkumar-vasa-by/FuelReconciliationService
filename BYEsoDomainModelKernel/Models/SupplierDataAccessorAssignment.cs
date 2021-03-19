using System;

namespace BYEsoDomainModelKernel.Models
{
  public class SupplierDataAccessorAssignment : EffectiveDatedDataAccessorAssignmentEntity
  {
    public virtual Supplier Supplier { get; set; }

    protected internal SupplierDataAccessorAssignment()
    {
    }

    protected internal SupplierDataAccessorAssignment(Supplier supplier, DataAccessor dataAccessor,
      DateTime start, DateTime end)
      : base(dataAccessor, start, end)
    {
      Supplier = supplier;
    }

    public override bool Equals(object other)
    {
      var ot = other as SupplierDataAccessorAssignment;
      if (ot == null)
      {
        return false;
      }
      return (Supplier.Equals(ot.Supplier) && DataAccessor.Equals(ot.DataAccessor));
    }

    public override int GetHashCode()
    {
      return Supplier.ID.GetHashCode() ^ DataAccessor.ID.GetHashCode();
    }
  }
}