using System;
using System.Collections.Generic;

namespace BYEsoDomainModelKernel.Models
{
  public partial class FuelAdjustment
  {
    public virtual int ID { get; set; }
    public virtual Site Site { get; set; }
    public virtual DateTime BusinessDate { get; set; }
    public virtual DateTime AdjustmentDate { get; set; }
    public virtual FuelAdjustmentStatus Status { get; set; }
    public virtual FuelAdjustmentType? FuelAdjustmentType { get; set; }
    public virtual string Remarks { get; set; }

    public virtual FuelAdjustmentLockedFlagStatus IsLocked { get; set; }
    public virtual FuelDelivery FuelDelivery { get; set; }

    public virtual IList<FuelAdjustmentLineItem> LineItems { get; set; }


    protected internal FuelAdjustment()
    {
      LineItems = new List<FuelAdjustmentLineItem>();
    }

    protected internal FuelAdjustment(Site site, DateTime businessDate,
      DateTime adjustmentDate, string remarks = "", FuelAdjustmentStatus status = FuelAdjustmentStatus.Draft,
      FuelAdjustmentType? fuelAdjustmentType = null,
      FuelAdjustmentLockedFlagStatus isLocked = FuelAdjustmentLockedFlagStatus.NotLocked)
      : this()
    {
      Site = site;
      BusinessDate = businessDate;
      AdjustmentDate = adjustmentDate;
      Status = status;
      Remarks = remarks;
      IsLocked = isLocked == 0 ? FuelAdjustmentLockedFlagStatus.NotLocked : isLocked;
      FuelAdjustmentType = fuelAdjustmentType;
    }
    public override bool Equals(object other)
    {
      FuelAdjustment ot = other as FuelAdjustment;
      if (ot == null)
      {
        return false;
      }
      return ot.ID == ID;
    }

    public override int GetHashCode()
    {
      return ID.GetHashCode();
    }
  }
  public enum FuelAdjustmentStatus
  {
    Draft = 'd',
    Completed = 'c',
    Posted = 'p'
  }

  public enum FuelAdjustmentType
  {
    Manual = 'm',
    Standard = 's',
    PumpTest = 'p'
  }

  public enum FuelAdjustmentLockedFlagStatus
  {
    NotLocked = 'n',
    TemporarilyLocked = 't',
    Locked = 'y'
  }

  public enum FuelEventType
  {
    PumpTest = 2,
    PumpTestRefund = 3,
    DriveOff = 4,
    DriveOffRefund = 5
  }
}
