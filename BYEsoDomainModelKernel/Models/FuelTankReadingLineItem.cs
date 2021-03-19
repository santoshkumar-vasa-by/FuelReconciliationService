using System;

namespace BYEsoDomainModelKernel.Models
{
  public partial class FuelTankReadingLineItem
  {
    public virtual int LineNumber { get; set; }
    public virtual Site Site { get; set; }
    public virtual FuelPhysicalTank FuelPhysicalTank { get; set; }
    public virtual FuelTankReading FuelTankReading { get; set; }
    public virtual decimal? WaterVolume { get; set; }
    public virtual decimal? TotalVolume { get; set; }
    public virtual decimal? Temperature { get; set; }

    private decimal _TotalHeight { get; set; }
    private decimal _WaterHeight { get; set; }
    private DateTime _ReadTimestamp { get; set; }

    public FuelTankReadingLineItem()
    {
    }

    public virtual DateTime GetReadingTimeStamp()
    {
      return _ReadTimestamp;
    }
    public override bool Equals(object other)
    {
      FuelTankReadingLineItem ot = other as FuelTankReadingLineItem;
      if (ot == null)
      {
        return false;
      }
      return (ot.LineNumber == LineNumber) && (ot.FuelTankReading.Equals(FuelTankReading));
    }

    public override int GetHashCode()
    {
      return LineNumber.GetHashCode() ^ FuelTankReading.ID.GetHashCode();
    }
  }
}