using System;

namespace BYEsoDomainModelKernel.Models
{
  public class FuelItemSummary: BaseAuditEntity
  {
    public virtual Site Site { get; set; }
    public virtual FuelItem FuelItem { get; set; }
    public virtual DateTime BusinessDate { get; set; }
    public virtual decimal? OpeningWac { get; set; }
    public virtual decimal? ClosingWac { get; set; }
    public virtual decimal? Sales { get; set; }
    public virtual decimal? EndOnHandQuantity { get; set; }
    public virtual decimal? ItemVarianceQuantity { get; set; }
    public virtual ItemCategory ItemCategory { get; set; }
    public virtual decimal? PumpTestVolumeReturned { get; set; }
    public virtual decimal? PumpTestVolumeNotReturned { get; set; }
    public virtual int ItemID { get; set; }

    public FuelItemSummary()
    { }

    public FuelItemSummary(Site site, FuelItem fuelItem, DateTime businessDate, ItemCategory itemCategory,
      decimal? openingWac, decimal? closingWac, decimal? sales, decimal? endOnHandQuantity, decimal? itemVarianceQuantity,
      decimal? pumpTestVolumeReturned, decimal? pumpTestVolumeNotReturned)
    {
      Site = site;
      FuelItem = fuelItem;
      BusinessDate = businessDate;
      OpeningWac = openingWac;
      ClosingWac = closingWac;
      Sales = sales;
      EndOnHandQuantity = endOnHandQuantity;
      ItemVarianceQuantity = itemVarianceQuantity;
      ItemCategory = itemCategory;
      PumpTestVolumeReturned = pumpTestVolumeReturned;
      PumpTestVolumeNotReturned = pumpTestVolumeNotReturned;
    }

    public override bool Equals(object other)
    {
      var ot = other as FuelItemSummary;
      if (ot == null)
      {
        return false;
      }
      return ot.Site.ID == Site.ID &&
             ot.FuelItem.ID == FuelItem.ID && ot.BusinessDate == BusinessDate;
    }

    public override int GetHashCode()
    {
      return Site.ID.GetHashCode() ^ FuelItem.ID.GetHashCode() ^ BusinessDate.GetHashCode();
    }
  }
}
