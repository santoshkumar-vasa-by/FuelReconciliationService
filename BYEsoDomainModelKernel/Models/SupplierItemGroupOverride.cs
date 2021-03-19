

namespace BYEsoDomainModelKernel.Models
{
  public class SupplierItemGroupOverride : BaseAuditEntity
  {
    public virtual SupplierItemGroup SupplierItemGroup { get; set; }
    public virtual Supplier Supplier { get; set; }
    public virtual OrganizationalHierarchy OrganizationalHierarchy { get; set; }

    public virtual decimal? MaxOrderAmount { get; set; }

    protected internal SupplierItemGroupOverride()
    {
    }

    protected internal SupplierItemGroupOverride(SupplierItemGroup supplierItemGroup, OrganizationalHierarchy organizationalHierarchy,
      decimal? maxOrderAmount)
    {
      SupplierItemGroup = supplierItemGroup;
      Supplier = supplierItemGroup.Supplier;
      OrganizationalHierarchy = organizationalHierarchy;
      MaxOrderAmount = maxOrderAmount;
    }

    public override bool Equals(object other)
    {
      var ot = other as SupplierItemGroupOverride;
      if (ot == null)
      {
        return false;
      }
      return (GetHashCode() == ot.GetHashCode());
    }

    public override int GetHashCode()
    {
      return SupplierItemGroup.ID.GetHashCode() ^ OrganizationalHierarchy.ID.GetHashCode();
    }
  }
}
