using System;
using System.Collections.Generic;
using System.Linq;

namespace BYEsoDomainModelKernel.Models
{
  public partial class FuelTankReading
  {
    public virtual int ID { get; set; }
    public virtual Site Site { get; set; }
    public virtual DateTime BusinessDate { get; set; }
    public virtual DateTime ReadingTimestamp { get; set; }
    public virtual TankReadingStatus Status { get; set; }
    public virtual TankReadingType Type { get; set; }
    public virtual bool IsImported { get; set; }
    private bool _ElectronicFlag { get; set; }

    private IList<FuelTankReadingLineItem> _LineItems { get; set; }
    public virtual IEnumerable<FuelTankReadingLineItem> LineItems => _LineItems.AsEnumerable();

    public virtual DateTime TransactionDate => ReadingTimestamp;

    public virtual bool IsElectronicFlag => _ElectronicFlag;

    protected internal FuelTankReading()
    {

      InitializeCollections();
    }

    protected internal FuelTankReading(Site site, DateTime businessDate, DateTime readingTimestamp, TankReadingType type, bool isImported)
    {
      Site = site;
      Status = TankReadingStatus.Draft;
      BusinessDate = businessDate;
      ReadingTimestamp = readingTimestamp;
      Type = type;
      IsImported = isImported;

      _ElectronicFlag = false;
      InitializeCollections();
    }
    
    private void InitializeCollections()
    {
      _LineItems = new List<FuelTankReadingLineItem>();
    }
    public override bool Equals(object other)
    {
      FuelTankReading ot = other as FuelTankReading;
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

  public enum TankReadingType
  {
    Standard = 's',
    OpeningDelivery = 'o',
    ClosingDelivery = 'c',
    EndOfDay = 'e',
    Scheduled = 'v',
    Audit = 'a'
  }

  public enum TankReadingStatus
  {
    Draft = 'd',
    Posted = 'p',
    Completed = 'c'
  }
}
