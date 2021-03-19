using System.Collections.Generic;
using System.Linq;

namespace BYEsoDomainModelKernel.Models
{
  public partial class FuelManifoldTank : FuelTank
  {
    private IList<FuelManifoldTankPhysicalTankAssignment> _PhysicalTankAssignments { get; set; }
    public virtual IEnumerable<FuelPhysicalTank> PhysicalTanks
    {
      get
      {
        return _PhysicalTankAssignments.Select(x => x.PhysicalTank);
      }
    }
    public override IEnumerable<FuelPhysicalTank> ImpactedPhysicalTanks
    {
      get
      {
        return PhysicalTanks;
      }
    }
    public override FuelTank TrackingTank
    {
      get
      {
        return this;
      }
    }
    public override int TrackingTankMaximumFillVolume
    {
      get
      {
        return PhysicalTanks.Sum(x => x.FuelTankType.MaximumFillVolume);
      }
    }

    protected internal FuelManifoldTank()
    {
    }
    public override bool Equals(object other)
    {
      FuelManifoldTank ot = other as FuelManifoldTank;
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
