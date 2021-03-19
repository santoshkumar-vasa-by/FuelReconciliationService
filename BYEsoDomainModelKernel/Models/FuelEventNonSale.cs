using System;

namespace BYEsoDomainModelKernel.Models
{
  public class FuelEventNonSale : BaseAuditEntity
  {
    public virtual DateTime BusinessDate { get; set; }
    public virtual decimal Volume { get; set; }
    public virtual decimal Amount { get; set; }
    public virtual string Comments { get; set; }
    public virtual int TransactionNumber { get; set; }
    public virtual FuelAdjustment FuelAdjustment { get; set; }
    public virtual PostedSalesItem PostedSalesItem { get; set; }
    public virtual DateTime Timestamp { get; set; }
    public virtual Site Site { get; set; }
    public virtual int EmployeeID { get; set; }
    public virtual int ShiftID { get; set; }
    public virtual int PumpNumber { get; set; }
    public virtual int TransactionLineNumber { get; set; }
    public virtual FuelEventType FuelEventType { get; set; }
    public virtual int PhysicalTankID { get; set; }
    public virtual SalesItem SalesItem { get; set; }

    public FuelEventNonSale()
    {
    }

    protected internal FuelEventNonSale(DateTime businessDate, decimal volume, decimal amount, string comments, int transactionNumber,
      FuelAdjustment fuelAdjustment, PostedSalesItem postedSalesItem, DateTime timestamp, Site site, int employeeID,
      int shiftID, int pumpNumber, int transactionLineNumber, FuelEventType typeCode)
    {
      BusinessDate = businessDate;
      Volume = volume;
      Amount = amount;
      Comments = comments;
      TransactionNumber = transactionNumber;
      FuelAdjustment = fuelAdjustment;
      PostedSalesItem = postedSalesItem;
      Timestamp = timestamp;
      Site = site;
      EmployeeID = employeeID;
      ShiftID = shiftID;
      PumpNumber = pumpNumber;
      TransactionLineNumber = transactionLineNumber;
      FuelEventType = typeCode;
    }

    //public override bool Equals(object other)
    //{
    //  var ot = other as FuelEventNonSale;
    //  if (ot == null)
    //  {
    //    return false;
    //  }
    //  return (GetHashCode() == ot.GetHashCode());
    //}

    //public override int GetHashCode()
    //{
    //  return BusinessDate.GetHashCode() ^ Site.GetHashCode() ^
    //         EmployeeID.GetHashCode() ^
    //         ShiftID.GetHashCode() ^
    //         TransactionNumber.GetHashCode() ^
    //         TransactionLineNumber.GetHashCode() ^
    //         PostedSalesItem.GetHashCode() ^
    //         PumpNumber.GetHashCode();
    //}
  }
}
