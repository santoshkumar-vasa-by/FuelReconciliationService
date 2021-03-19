using System;
using System.Collections.Generic;

namespace BYEsoDomainModelKernel.Models
{
  public partial class FuelDelivery
  {
    public virtual int ID { get; set; }
    public virtual Site Site { get; set; }
    public virtual DateTime BusinessDate { get; set; }
    public virtual DateTime DeliveryTimestamp { get; set; }
    public virtual FuelDeliveryStatus Status { get; set; }
    public virtual FuelDeliveryType DeliveryType { get; set; }
    public virtual string BillOfLadingNumber { get; set; }
    public virtual FuelDeliveryLockedFlagStatus IsLocked { get; set; }
    public virtual Supplier FuelSupplier { get; set; }
    public virtual Supplier TerminalSupplier { get; set; }
    public virtual Supplier CarrierSupplier { get; set; }
    public virtual decimal TotalCostForNonNullableColumn { get; set; }
    public virtual DateTime BillOfLadingDateForNonNullableColumn { get; set; }
    public virtual string InvoiceNumber { get; set; }
    public virtual string TruckNumber { get; set; }

    public virtual decimal? Temparature { get; set; }

    public virtual IList<FuelDeliveryLineItem> LineItems { get; set; }
    public virtual IList<DatamartFuelDeliveryLineItem> DatamartLineItems { get; set; }
    //public virtual IList<FuelDeliveryGlDistribution> GlDistributions { get; set; }

    protected internal FuelDelivery()
    {
      LineItems = new List<FuelDeliveryLineItem>();
      DatamartLineItems = new List<DatamartFuelDeliveryLineItem>();
      //GlDistributions = new List<FuelDeliveryGlDistribution>();
    }
  }

  public enum FuelDeliveryStatus
  {
    Draft = 'd',
    Completed = 'c',
    Posted = 'p'
  }

  public enum FuelDeliveryType
  {
    Imported = 'y',
    Manual = 'n'
  }


  public enum FuelDeliveryLockedFlagStatus
  {
    NotLocked = 'n',
    TemporarilyLocked = 't',
    Locked = 'y'
  }
}
