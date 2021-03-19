using System;
using BYEsoDomainModelKernel.Models.Common;
using RPCore;

namespace BYEsoDomainModelKernel.Models
{
  public partial class DatamartFuelDeliveryLineItem : DatamartTicketedEntity
  {
    public virtual Site Site { get; protected internal set; }
    public virtual DateTime BusinessDate { get; protected internal set; }
    public virtual FuelDelivery FuelDelivery { get; protected internal set; }
    public virtual FuelPhysicalTank FuelPhysicalTank { get; protected internal set; }
    public virtual FuelItem FuelItem { get; protected internal set; }
    public virtual int NetVolume { get; protected internal set; }
    private int? _GrossVolume { get; set; }
    private int? _ActualVolume { get; set; }
    public virtual string BillOfLadingNumber { get; set; }

    protected internal DatamartFuelDeliveryLineItem()
    {
    }

    protected internal DatamartFuelDeliveryLineItem(FuelDeliveryLineItem lineItem)
    {
      Site = lineItem.Site;
      BusinessDate = lineItem.FuelDelivery.BusinessDate;
      FuelDelivery = lineItem.FuelDelivery;
      BillOfLadingNumber = lineItem.FuelDelivery.BillOfLadingNumber;
      FuelPhysicalTank = lineItem.FuelPhysicalTank;
      FuelItem = lineItem.FuelPhysicalTank.FuelItem;
      NetVolume = lineItem.Volume;
      _GrossVolume = lineItem.Volume;
      _ActualVolume = lineItem.Volume;
    }

    public override bool Equals(object other)
    {
      DatamartFuelDeliveryLineItem ot = other as DatamartFuelDeliveryLineItem;
      if (ot == null)
      {
        return false;
      }
      return ot.FuelDelivery.ID == FuelDelivery.ID && ot.FuelPhysicalTank.ID == FuelPhysicalTank.ID;
    }

    public override int GetHashCode()
    {
      return FuelDelivery.ID.GetHashCode() ^ FuelPhysicalTank.ID.GetHashCode();
    }
  }
}