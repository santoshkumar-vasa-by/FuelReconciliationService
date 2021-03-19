using System;
using System.Collections.Generic;
using System.Linq;

namespace BYEsoDomainModelKernel.Models
{
  public class FuelMeterReading
  {
    public virtual Site Site { get; set; }
    public virtual int ID { get; set; }
    public virtual DateTime BusinessDate { get; set; }
    private bool _ElectronicFlag { get; set; }
    public virtual TankReadingStatus Status { get; set; }
    public virtual bool IsImported { get; set; }
    public virtual MeterReadingType Type { get; set; }
    private IList<FuelTankMeterReadingLineItem> _LineItems { get; set; }
    public virtual IEnumerable<FuelTankMeterReadingLineItem> LineItems
    {
      get
      {
        return _LineItems.AsEnumerable();
      }
    }
    protected internal FuelMeterReading()
    {
      InitializeCollections();
    }
    public FuelMeterReading(Site site, DateTime businessDate, bool isImported, MeterReadingType
      type = MeterReadingType.Standard, TankReadingStatus status = TankReadingStatus.Draft)
    {
      Site = site;
      Status = status;
      BusinessDate = businessDate;
      Type = type;
      IsImported = isImported;
      _ElectronicFlag = false;
      InitializeCollections();
    }
    private void InitializeCollections()
    {
      _LineItems = new List<FuelTankMeterReadingLineItem>();
    }

    public virtual void AddLineItem(FuelTankMeterReadingLineItem lineItem)
    {
      _LineItems.Add(lineItem);
    }

    public override bool Equals(object other)
    {
      FuelMeterReading ot = other as FuelMeterReading;
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
    public enum MeterReadingType
    {
      Standard = 's',
      Reset = 'r',
      Scheduled = 'v'
    }
  }
}