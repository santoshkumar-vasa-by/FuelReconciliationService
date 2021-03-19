namespace BYEsoDomainModelKernel.Models
{
  public partial class FuelAdjustmentLineItem
  {
    public virtual int LineNumber { get; set; }
    public virtual Site Site { get; set; }
    public virtual FuelAdjustment FuelAdjustment { get; set; }
    public virtual FuelPhysicalTank FuelPhysicalTank { get; set; }
    public virtual FuelItem FuelItem { get; set; }
    public virtual decimal Volume { get; set; }

    protected internal FuelAdjustmentLineItem()
    {
    }

    protected internal FuelAdjustmentLineItem(FuelAdjustment fuelAdjustment, FuelPhysicalTank fuelPhysicalTank, decimal volume = 0)
    {
      LineNumber = 0;
      Site = fuelAdjustment.Site;
      FuelAdjustment = fuelAdjustment;
      FuelPhysicalTank = fuelPhysicalTank;
      FuelItem = fuelPhysicalTank.FuelItem;
      Volume = volume;
    }
    public override bool Equals(object other)
    {
      FuelAdjustmentLineItem ot = other as FuelAdjustmentLineItem;
      if (ot == null)
      {
        return false;
      }
      return (ot.LineNumber == LineNumber) && (ot.FuelAdjustment.Equals(FuelAdjustment));
    }

    public override int GetHashCode()
    {
      return LineNumber.GetHashCode() & FuelAdjustment.GetHashCode();
    }
  }
}
