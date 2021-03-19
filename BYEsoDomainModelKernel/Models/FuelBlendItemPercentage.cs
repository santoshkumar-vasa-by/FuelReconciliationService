namespace BYEsoDomainModelKernel.Models
{
  public class FuelBlendItemPercentage
  {
    public virtual FuelItem FuelBlendedItem { get; set; }
    public virtual FuelItem FuelItem { get; set; }
    public virtual decimal FuelBlendPercentage { get; set; }

    protected internal FuelBlendItemPercentage()
    {
    }

    protected internal FuelBlendItemPercentage(FuelItem fuelBlendedItem, FuelItem fuelItem, decimal fuelBlendPercentage)
    {
      FuelBlendedItem = fuelBlendedItem;
      FuelItem = fuelItem;
      FuelBlendPercentage = fuelBlendPercentage;
    }

    public override bool Equals(object other)
    {
      FuelBlendItemPercentage ot = other as FuelBlendItemPercentage;
      if (ot == null)
      {
        return false;
      }
      return ot.FuelBlendedItem.ID == FuelBlendedItem.ID && ot.FuelItem.ID == FuelItem.ID;
    }

    public override int GetHashCode()
    {
      return FuelBlendedItem.ID.GetHashCode() ^ FuelItem.ID.GetHashCode();
    }
  }
}