namespace BYEsoDomainModelKernel.Models
{
  public class FuelHoseTankAssignment
  {
    public virtual Site Site { get; set; }
    public virtual FuelHose FuelHose { get; set; }
    public virtual FuelTank FuelTank { get; set; }

    protected internal FuelHoseTankAssignment()
    {
    }

    public FuelHoseTankAssignment(FuelHose fuelHose,
      FuelTank fuelTank)
    {
      Site = fuelHose.Site;
      FuelHose = fuelHose;
      FuelTank = fuelTank;
    }
  }
}