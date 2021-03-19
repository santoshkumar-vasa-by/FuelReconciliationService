using System;

namespace BYEsoDomainModelKernel.Models
{
  public class FuelTankSummary : BaseAuditEntity
  {
    public virtual Site Site { get; set; }
    public virtual FuelTank FuelTank { get; set; }
    public virtual DateTime BusinessDate { get; set; }
    public virtual decimal? CloseVolume { get; set; }
    public virtual decimal? AuditCloseVolume { get; set; }
    public virtual decimal? AuditOpenVolume { get; set; }
    public virtual decimal? BookVolume { get; set; }
    public virtual decimal? AuditBookVolume { get; set; }
    public virtual decimal? OpenVolume { get; set; }
    public virtual decimal? PumpTestVolumeNonReturned { get; set; }
    public virtual decimal? SalesVolume { get; set; }
    public virtual decimal? DeliveryVolume { get; set; }
    public virtual decimal? PumpTestVolumeReturned { get; set; }
    public virtual decimal? AdjustmentVolume { get; set; }
    public virtual decimal? PumpedVolume { get; set; }
    public virtual decimal? Variance
    {
      get
      {
        return CloseVolume.GetValueOrDefault(0)
                 - (OpenVolume.GetValueOrDefault(0)
                   - PumpTestVolumeNonReturned.GetValueOrDefault(0)
                   - SalesVolume.GetValueOrDefault(0)
                   + DeliveryVolume.GetValueOrDefault(0)
                   + AdjustmentVolume.GetValueOrDefault(0)
                   );
      }
    }

    public virtual decimal? TheoreticalOnHand
    {
      get
      {
        return OpenVolume.GetValueOrDefault(0)
                  - PumpTestVolumeNonReturned.GetValueOrDefault(0)
                  - SalesVolume.GetValueOrDefault(0)
                  + DeliveryVolume.GetValueOrDefault(0)
                  + AdjustmentVolume.GetValueOrDefault(0);
      }
    }

    public FuelTankSummary()
    {
    }

    public FuelTankSummary(Site site, FuelTank fuelTank, DateTime businessDate, decimal? closeVolume,
      decimal? openVolume, decimal? pumpTestVolumeNonReturned, decimal? salesVolume, 
      decimal? deliveryVolume, decimal? adjustmentVolume, decimal? pumpTestVolumeReturned, 
      decimal? auditCloseVolume = 0)
      : this()
    {
      Site = site;
      FuelTank = fuelTank;
      BusinessDate = businessDate;
      CloseVolume = closeVolume;
      AuditCloseVolume = auditCloseVolume;
      OpenVolume = openVolume;
      PumpTestVolumeNonReturned = pumpTestVolumeNonReturned;
      SalesVolume = salesVolume;
      DeliveryVolume = deliveryVolume;
      AdjustmentVolume = adjustmentVolume;
      PumpTestVolumeReturned = pumpTestVolumeReturned;
    }

    public override bool Equals(object other)
    {
      FuelTankSummary ot = other as FuelTankSummary;
      if (ot == null)
      {
        return false;
      }
      return ot.Site.ID == Site.ID && ot.FuelTank.ID == FuelTank.ID &&
        ot.BusinessDate == BusinessDate;
    }

    public override int GetHashCode()
    {
      return Site.ID.GetHashCode() ^ FuelTank.ID.GetHashCode() ^ BusinessDate.GetHashCode();
    }
  }
}