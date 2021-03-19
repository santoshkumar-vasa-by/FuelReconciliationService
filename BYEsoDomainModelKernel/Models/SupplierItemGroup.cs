using System.Collections.Generic;
using System.Linq;

namespace BYEsoDomainModelKernel.Models
{
  public partial class SupplierItemGroup : BaseAuditEntity
  {
    public virtual int ID { get; set; }
    public virtual Supplier Supplier { get; set; }
    public virtual string Name { get; set; }
    public virtual bool IsException { get; set; }
    public virtual decimal? MaxOrderAmount { get; set; }

    private IList<SupplierItemGroupOverride> _Overrides { get; set; }
    public virtual IEnumerable<SupplierItemGroupOverride> Overrides
    {
      get
      {
        return _Overrides.AsEnumerable();
      }
    }

    protected internal SupplierItemGroup()
    {
      _Overrides = new List<SupplierItemGroupOverride>();
    }

    protected internal SupplierItemGroup(Supplier supplier, string name, bool isException, decimal? maxOrderAmount)
    {
      Supplier = supplier;
      Name = name;
      IsException = isException;
      MaxOrderAmount = maxOrderAmount;

      _Overrides = new List<SupplierItemGroupOverride>();
    }

    public virtual void AddOverride(OrganizationalHierarchy organizationalHierarchy, decimal maxOrderAmount)
    {
      _Overrides.Add(new SupplierItemGroupOverride(this, organizationalHierarchy, maxOrderAmount));
    }

    public virtual decimal? GetMaxOrderAmountBySite(Site site)
    {
      var maxOrderAmount = MaxOrderAmount;

      if (Overrides.Any())
      {
        var orgHierarchy = site.OrganizationalHierarchy;
        var allHierarchies = orgHierarchy.GetHierarchiesAtOrAboveMe();
        var overridesByHierarchies = Overrides.Where(x => allHierarchies.Contains(x.OrganizationalHierarchy))
                                              .ToList();

        if (overridesByHierarchies.Any())
        {
          var overrideAtLowestOrgHierarchy = overridesByHierarchies
            .FirstOrDefault(x => !overridesByHierarchies.Any(y => y.OrganizationalHierarchy.IsLowerInTreeThan(x.OrganizationalHierarchy)));

          if (overrideAtLowestOrgHierarchy != null)
          {
            maxOrderAmount = overrideAtLowestOrgHierarchy.MaxOrderAmount;
          }
        }
      }

      return maxOrderAmount;
    }

    public override bool Equals(object other)
    {
      var ot = other as SupplierItemGroup;
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
