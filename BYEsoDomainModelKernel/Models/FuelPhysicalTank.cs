using System;
using System.Collections.Generic;
using System.Linq;
using BYEsoDomainModelKernel.Util;

namespace BYEsoDomainModelKernel.Models
{
  public partial class FuelPhysicalTank : FuelTank
  {
    public virtual FuelTankType FuelTankType { get; set; }
    private int _JoinedClientID { get; set; }
    private int _JoinedLastModifiedUserID { get; set; }
    private DateTime _JoinedLastModifiedTimestamp { get; set; }
    public override IEnumerable<FuelPhysicalTank> ImpactedPhysicalTanks
    {
      get
      {
        return new FuelPhysicalTank[] { this };
      }
    }

    private IList<FuelManifoldTankPhysicalTankAssignment> _ManifoldTankAssignments { get; set; }
    public virtual FuelManifoldTank ManifoldTank
    {
      get
      {
        return _ManifoldTankAssignments.Select(x => x.ManifoldTank).FirstOrDefault();
      }
    }

    private IList<FuelTankReadingLineItem> FuelTankReadingLineItemList { get; set; }
    public virtual IEnumerable<FuelTankReadingLineItem> FuelTankReadingLineItems
    {
      get
      {
        return FuelTankReadingLineItemList.AsEnumerable();
      }
    }

    private IList<FuelAdjustmentLineItem> FuelAdjustmentLineItemList { get; set; }
    public virtual IEnumerable<FuelAdjustmentLineItem> FuelAdjustmentLineItems
    {
      get
      {
        return FuelAdjustmentLineItemList.AsEnumerable();
      }
    }

    private IList<FuelDeliveryLineItem> FuelDeliveryLineItemList { get; set; }
    public virtual IEnumerable<FuelDeliveryLineItem> FuelDeliveryLineItems
    {
      get
      {
        return FuelDeliveryLineItemList.AsEnumerable();
      }
    }

    public override FuelTank TrackingTank
    {
      get
      {
        if (ManifoldTank != null)
        {
          return ManifoldTank;
        }

        return this;
      }
    }

    public override int TrackingTankMaximumFillVolume
    {
      get
      {
        if (ManifoldTank != null)
        {
          return ManifoldTank.TrackingTankMaximumFillVolume;
        }

        return FuelTankType.Capacity;
      }
    }

    protected internal FuelPhysicalTank()
    {
    }

    protected internal FuelPhysicalTank(Site site, FuelItem fuelItem, short tankNumber, FuelTankType fuelTankType)
      : base(site, fuelItem, tankNumber.ToString())
    {
      FuelTankType = fuelTankType;
      _ManifoldTankAssignments = new List<FuelManifoldTankPhysicalTankAssignment>();
      FuelTankReadingLineItemList = new List<FuelTankReadingLineItem>();
      FuelAdjustmentLineItemList = new List<FuelAdjustmentLineItem>();
      FuelDeliveryLineItemList = new List<FuelDeliveryLineItem>();
    }

    protected internal FuelPhysicalTank(Site site, FuelItem fuelItem, string tankNumber, FuelTankType fuelTankType,
      bool isActive, int? waterThreshold, decimal? lowTankThreshold, decimal? varianceVolumeThreshold,
      decimal? varianceBySalesThresholdPercent, bool isAutomaticTankGauge)
      : base(site, fuelItem, tankNumber, isActive, waterThreshold, lowTankThreshold, varianceVolumeThreshold,
          varianceBySalesThresholdPercent, isAutomaticTankGauge)
    {
      FuelTankType = fuelTankType;
      _ManifoldTankAssignments = new List<FuelManifoldTankPhysicalTankAssignment>();
      _JoinedClientID = 0;
      _JoinedLastModifiedUserID = 0;
      _JoinedLastModifiedTimestamp = DateTime.MinValue;
    }
    public virtual void SetFuelTankType(FuelTankType fuelTankType)
    {
      FuelTankType = fuelTankType;
      _JoinedClientID = 0;
      _JoinedLastModifiedUserID = 0;
      _JoinedLastModifiedTimestamp = DateTime.MinValue;
    }

    protected internal virtual void SetManifoldTank(FuelManifoldTank manifoldTank)
    {
      if (!manifoldTank.PhysicalTanks.Contains(this))
      {
        throw new NotSupportedException("Attempt to link physical tank to manifold it is not in");
      }

      _ManifoldTankAssignments.Add(new FuelManifoldTankPhysicalTankAssignment(manifoldTank, this));
    }

    public override IEnumerable<FuelPhysicalTankVolume> DistributeVolumeToPhysicalTanks(decimal volume)
    {
      return (new FuelPhysicalTankVolume[] { new FuelPhysicalTankVolume(this, volume) }).ToList();
    }

    public override bool Equals(object other)
    {
      FuelPhysicalTank ot = other as FuelPhysicalTank;
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
}