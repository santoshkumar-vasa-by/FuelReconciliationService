using System;

namespace BYEsoDomainModelKernel.Models
{
  public class DatamartFuelHose
  {
    public virtual DateTime BusinessDate { get; set; }
    public virtual Site Site { get; set; }
    public virtual FuelHose FuelHose { get; set; }
    public virtual FuelPump FuelPump { get; set; }
    public virtual FuelItem FuelItem { get; set; }
    public virtual int? HoseNumber { get; set; }
    public virtual int? PumpNumber { get; set; }
    public virtual bool IsElectronic { get; set; }
    public virtual decimal? ManualSalesQuantity { get; set; }
    public virtual decimal? POSSalesQuantity { get; set; }
    public virtual decimal? ManualSalesAmount { get; set; }
    public virtual decimal? POSSalesAmount { get; set; }
    public virtual decimal? OpenMeterReading { get; set; }
    public virtual decimal? CloseMeterReading { get; set; }
    public virtual decimal? PumpedVolume { get; set; }
    public virtual decimal? MeterAdjustment { get; set; }

    public DatamartFuelHose()
    {
    }

    public override bool Equals(object other)
    {
      var ot = other as DatamartFuelHose;
      if (ot == null)
      {
        return false;
      }
      return ot.Site.ID == Site.ID &&
             ot.FuelHose.ID == FuelHose.ID && ot.BusinessDate == BusinessDate;
    }

    public override int GetHashCode()
    {
      return Site.ID.GetHashCode() ^ FuelHose.ID.GetHashCode() ^ BusinessDate.GetHashCode();
    }

  }
}
