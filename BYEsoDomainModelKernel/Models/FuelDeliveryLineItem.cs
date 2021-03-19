namespace BYEsoDomainModelKernel.Models
{
  public partial class FuelDeliveryLineItem
  {
    public virtual int LineNumber { get; set; }
    public virtual Site Site { get; set; }
    public virtual FuelDelivery FuelDelivery { get; set; }
    public virtual FuelPhysicalTank FuelPhysicalTank { get; set; }

    public virtual decimal? FuelCostAmount { get; set; }
    public virtual decimal? CurrentTrackingTankOnHandVolume { get; set; }

    public virtual int? GrossVolume { get; set; }
    public virtual int NetVolume { get; set; }
    public virtual decimal? ActualVolume { get; set; }
    public virtual bool NetVolumeNullable { get; set; }

    public virtual int Volume
    {
      get
      {
        return NetVolume;
      }

      protected internal set
      {
        GrossVolume = value;
        NetVolume = value;
        ActualVolume = value;
      }
    }

    protected internal FuelDeliveryLineItem()
    {
    }

    protected internal FuelDeliveryLineItem(FuelDelivery fuelDelivery, FuelPhysicalTank fuelPhysicalTank, bool isNetVolumeNullable = false)
    {
      LineNumber = 0;
      Site = fuelDelivery.Site;
      FuelDelivery = fuelDelivery;
      FuelPhysicalTank = fuelPhysicalTank;
      NetVolume = 0;
      ActualVolume = 0;
      FuelCostAmount = null;
      NetVolumeNullable = isNetVolumeNullable;
    }

    protected internal virtual void SetLineNumber(int lineNumber)
    {
      LineNumber = lineNumber;
    }
    public override bool Equals(object obj)
    {
      FuelDeliveryLineItem ot = obj as FuelDeliveryLineItem;
      if (ot == null)
      {
        return false;
      }
      return (ot.LineNumber == LineNumber) && (ot.FuelDelivery.Equals(FuelDelivery));
    }

    public override int GetHashCode()
    {
      return LineNumber.GetHashCode() & FuelDelivery.GetHashCode();
    }
  }
}