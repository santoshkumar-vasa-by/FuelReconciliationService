namespace BYEsoDomainModelKernel.Models
{
  public class FuelManifoldTankPhysicalTankAssignment
  {
    public virtual FuelManifoldTank ManifoldTank { get; set; }
    public virtual FuelPhysicalTank PhysicalTank { get; set; }

    protected internal FuelManifoldTankPhysicalTankAssignment()
    {
    }

    protected internal FuelManifoldTankPhysicalTankAssignment(FuelManifoldTank manifoldTank, FuelPhysicalTank physicalTank)
      : this()
    {
      ManifoldTank = manifoldTank;
      PhysicalTank = physicalTank;
    }
    public override bool Equals(object other)
    {
      FuelManifoldTankPhysicalTankAssignment ot = other as FuelManifoldTankPhysicalTankAssignment;
      if (ot == null)
      {
        return false;
      }
      return ot.ManifoldTank.ID == ManifoldTank.ID && ot.PhysicalTank.ID == PhysicalTank.ID;
    }

    public override int GetHashCode()
    {
      return ManifoldTank.ID.GetHashCode() ^ PhysicalTank.ID.GetHashCode();
    }
  }
}