using System;
using System.Collections.Generic;
using RP.DomainModelKernel.Common;

namespace BYEsoDomainModelKernel.Models
{
  public class FuelTank: BaseAuditEntity
  {
    public virtual int ID { get; set; }
    public virtual Site Site { get; set; }
    public virtual FuelItem FuelItem { get; set; }
    public virtual bool IsActive { get; set; }
    public virtual string TankNumber { get; set; }
    public virtual int? WaterThreshold { get; set; }
    public virtual decimal? LowTankThreshold { get; set; }
    public virtual decimal? VarianceVolumeThreshold { get; set; }
    public virtual decimal? VarianceBySalesThresholdPercent { get; set; }
    public virtual bool IsAutomaticGauge { get; set; }

    public virtual IEnumerable<FuelPhysicalTank> ImpactedPhysicalTanks
    {
      get
      {
        throw new NotSupportedException("This function should be overriden by subclasses");
      }
    }

    public virtual FuelTank TrackingTank
    {
      get
      {
        throw new NotSupportedException("This function should be overriden by subclasses");
      }
    }

    public virtual int TrackingTankMaximumFillVolume
    {
      get
      {
        throw new NotSupportedException("This function should be overriden by subclasses");
      }
    }

    protected internal FuelTank()
    {
    }

    protected FuelTank(Site site, FuelItem fuelItem, string tankNumber)
      : this()
    {
      Site = site;
      FuelItem = fuelItem;
      IsActive = true;
      TankNumber = tankNumber;
    }

    protected FuelTank(Site site, FuelItem fuelItem, string tankNumber, bool isActive, int? waterThreshold,
      decimal? lowTankThreshold, decimal? varianceVolumeThreshold, decimal? varianceBySalesThresholdPercent,
      bool isAutomaticGauge)
      : this()
    {
      Site = site;
      FuelItem = fuelItem;
      IsActive = isActive;
      TankNumber = tankNumber;
      WaterThreshold = waterThreshold;
      LowTankThreshold = lowTankThreshold;
      VarianceVolumeThreshold = varianceVolumeThreshold;
      VarianceBySalesThresholdPercent = varianceBySalesThresholdPercent;
      IsAutomaticGauge = isAutomaticGauge;
    }

    protected internal virtual void ApplyFuelTankEdits(FuelItem fuelItem, FuelTankDto fuelTankDto, int? waterThreshold,
      decimal? lowTankThreshold, decimal? varianceVolumeThreshold)
    {
      FuelItem = fuelItem;
      IsActive = fuelTankDto.IsActive;
      TankNumber = fuelTankDto.TankNumber;
      WaterThreshold = waterThreshold;
      LowTankThreshold = lowTankThreshold;
      VarianceVolumeThreshold = varianceVolumeThreshold;
      VarianceBySalesThresholdPercent = fuelTankDto.VarianceBySalesThresholdPercent;
      IsAutomaticGauge = fuelTankDto.IsAutomaticTankGauge;
    }

    public virtual IEnumerable<FuelPhysicalTankVolume> DistributeVolumeToPhysicalTanks(decimal volume)
    {
      throw new NotSupportedException("This function should be overriden by subclasses");
    }

    public virtual void SetActiveStatusFromTest(IUnitOfWork uow, bool activeStatus)
    {
      uow.EnforceRunningInTest("SetActiveStatusFromTest can only be run from test UOW");

      IsActive = activeStatus;
    }

    public override bool Equals(object other)
    {
      FuelTank ot = other as FuelTank;
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

  public enum FuelTankDomainErrors
  {
    InvalidFuelTankId,
    FuelTankNotFound,
    FuelItemNotFound,
    FuelTankTypeNotFound,
    AccessDeniedToSite,
    DuplicateTankNumber,
    WaterThresholdCannotExceedCapacity,
    LowTankThresholdCannotExceedCapacity,
    VarianceVolumeThresholdCannotExceedCapacity
  }

  public class FuelPhysicalTankVolume
  {
    public FuelPhysicalTank FuelPhysicalTank;
    public decimal Volume;

    public FuelPhysicalTankVolume(FuelPhysicalTank fuelPhysicalTank, decimal volume)
    {
      FuelPhysicalTank = fuelPhysicalTank;
      Volume = volume;
    }
  }
}