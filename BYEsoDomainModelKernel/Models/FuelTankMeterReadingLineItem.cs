using System;
using System.Linq;

namespace BYEsoDomainModelKernel.Models
{
  public class FuelTankMeterReadingLineItem
  {
    public virtual Site Site { get; set; }
    public virtual FuelMeterReading FuelMeterReading { get; set; }
    public virtual int LineNumber { get; set; }
    public virtual FuelHose FuelMeter { get; set; }
    public virtual decimal ReadValue { get; set; }
    public virtual MeterReadingLineType Type { get; set; }
    public virtual DateTime ReadTimestamp { get; set; }
    protected internal FuelTankMeterReadingLineItem()
    {
    }
    public FuelTankMeterReadingLineItem(Site site, FuelMeterReading fuelMeterReading,
      FuelHose fuelMeter, DateTime readTimestamp, MeterReadingLineType type = MeterReadingLineType.Standard, decimal readValue = 0)
      : this()
    {
      Site = site;
      FuelMeterReading = fuelMeterReading;
      FuelMeter = fuelMeter;
      ReadValue = readValue;
      LineNumber = FuelMeterReading.LineItems.Count();
      ReadTimestamp = readTimestamp;
      Type = type;
    }
    public enum MeterReadingLineType
    {
      Standard = 's',
      Reset = 'r',
      Scheduled = 'v',
      Calculated = 'c'
    }
    public override bool Equals(object other)
    {
      FuelTankMeterReadingLineItem ot = other as FuelTankMeterReadingLineItem;
      if (ot == null)
      {
        return false;
      }
      return (ot.LineNumber == LineNumber) && (ot.FuelMeterReading.Equals(FuelMeterReading));
    }
    public override int GetHashCode()
    {
      return LineNumber.GetHashCode() ^ FuelMeterReading.ID.GetHashCode();
    }
  }
}
