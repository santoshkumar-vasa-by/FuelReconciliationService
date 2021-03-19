using System;
using System.Collections.Generic;
using RPCore.Time;

namespace BYEsoDomainModelKernel.Models
{
  public partial class FuelInvoice : BaseAuditEntity
  {
    public virtual int ID { get;  set; }
    public virtual Site Site { get;  set; }
    public virtual string BillOfLadingNumber { get; set; }
    public virtual FuelInvoiceLockedFlagStatus IsLocked { get;  set; }
    public virtual WacCalculatedFlag WacCalculatedFlag { get;  set; }
    public virtual DateTime? WacBusinessDate { get; set; }
    public virtual FuelInvoiceStatusCode StatusCode { get;  set; }
    public virtual Supplier FuelSupplier { get;  set; }
    public virtual Supplier TerminalSupplier { get;  set; }
    public virtual IList<FuelInvoiceItem> LineItems { get;  set; }
    public virtual FuelInvoiceTypeCode FuelInvoiceTypeCode { get;  set; }
    public virtual CalendarDate FuelInvoiceDate { get;  set; }


    public FuelInvoice()
    {
      LineItems = new List<FuelInvoiceItem>();
    }

    public FuelInvoice(Site site, string billOfLadingNumber,
        Supplier fuelSupplier, Supplier terminalSupplier,
        FuelInvoiceLockedFlagStatus isLocked = FuelInvoiceLockedFlagStatus.NotLocked,
        FuelInvoiceStatusCode statusCode = FuelInvoiceStatusCode.Completed,
        WacCalculatedFlag wacCalculatedFlag = WacCalculatedFlag.No,
        DateTime? wacBusinessDate = null, FuelInvoiceTypeCode fuelInvoiceTypeCode = FuelInvoiceTypeCode.Imported)
      : this()
    {
      Site = site;
      BillOfLadingNumber = billOfLadingNumber;
      FuelSupplier = fuelSupplier;
      TerminalSupplier = terminalSupplier;
      IsLocked = isLocked == 0 ? FuelInvoiceLockedFlagStatus.NotLocked: isLocked;
      StatusCode = statusCode == 0 ? FuelInvoiceStatusCode.Completed : statusCode; 
      WacCalculatedFlag = wacCalculatedFlag == 0 ? WacCalculatedFlag.No : wacCalculatedFlag;
      WacBusinessDate = wacBusinessDate;
      FuelInvoiceTypeCode = fuelInvoiceTypeCode;
    }
    public virtual void AddLineItem(FuelInvoiceItem lineItem)
    {
      LineItems.Add(lineItem);
    }

    public enum FuelLockType
    {
      Temporary = 't',
      Final = 'f',
      No ='n'
    }
  }

  public enum WacCalculatedFlag
  {
    Yes = 'y',
    No = 'n',
    Temp = 't'
  }

  public enum FuelInvoiceLockedFlagStatus
  {
    NotLocked = 'n',
    TemporarilyLocked = 't',
    Locked = 'y'
  }

  public enum FuelInvoiceStatusCode
  {
    Completed = 'c',
    Posted = 'p',
    Draft = 'd'
  }

  public enum FuelInvoiceTypeCode
  {
    Imported = 'i',
    Manual = 'm',
    ManualReversed = 'n',
    AutoReversal = 'r'
  }
}
