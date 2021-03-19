using System;
using RP.DomainModelKernel.HumanResources;

namespace BYEsoDomainModelKernel.Models
{
  public partial class SalesTransactionLineItem
  {
    public virtual Site Site { get; set; }
    public virtual DateTime BusinessDate { get; set; }
    public virtual DateTime TransactionDate { get; set; }
    public virtual int? PumpNumber { get; set; }
    public virtual int? HoseNumber { get; set; }
    public virtual decimal SoldQuantity { get; set; }
    public virtual decimal SoldAmount { get; set; }
    public virtual decimal RefundQuantity { get; set; }
    public virtual decimal RefundAmount { get; set; }
    public virtual decimal RefundReductionAmount { get; set; }
    public virtual decimal ReductionAmount { get; set; }
    public virtual int SalesType { get; set; }
    public virtual int SalesItemID { get; set; }
    public virtual FuelSalesItem FuelSalesItem { get; set; }
    public virtual PostedSalesItem PostedSalesItem { get; set; }
    private int _TransactionNumber { get; set; }
    private decimal _RetailPrice { get; set; }
    private DateTime _StartDate { get; set; }
    private bool _IsComponent { get; set; }
    private int _EmployeeID { get; set; }
    private int _ShiftID { get; set; }
    private int _SalesDestinationID { get; set; }
    private int _TransactionLineNumber { get; set; }

    public virtual bool IsFuelSale
    {
      get
      {
        return HoseNumber.HasValue && PumpNumber.HasValue;
      }
    }

    public virtual int GetTransactionNumber
    {
      get
      {
        return _TransactionNumber;
      }
    }

    public virtual int GetShiftID
    {
      get
      {
        return _ShiftID;
      }
    }

    public virtual bool IsFuelPrepay
    {
      get
      {
        return SalesType == (int)SalesTypes.FuelPrepay || SalesType == (int)SalesTypes.FuelPrepayAdjustment;
      }
    }

    protected internal SalesTransactionLineItem()
    {
    }

    public SalesTransactionLineItem(Site site, DateTime businessDate, DateTime transactionDate, FuelPump fuelPump, FuelHose fuelHose,
      decimal soldQuantity, decimal soldAmount, decimal refundQuantity, decimal refundAmount, decimal reductionAmount,
      decimal refundReductionAmount)
      : this()
    {
      Site = site;
      BusinessDate = businessDate;
      TransactionDate = transactionDate;
      PumpNumber = fuelPump?.PumpNumber;
      HoseNumber = fuelHose?.HoseNumber;
      SoldQuantity = soldQuantity;
      SoldAmount = soldAmount;
      RefundQuantity = refundQuantity;
      RefundAmount = refundAmount;
      ReductionAmount = reductionAmount;
      RefundReductionAmount = refundReductionAmount;
    }

    public SalesTransactionLineItem(Site site, DateTime businessDate, DateTime transactionDate, FuelPump fuelPump, FuelHose fuelHose,
      decimal soldQuantity, decimal soldAmount, decimal refundQuantity, decimal refundAmount, int transactionNumber, SalesItem salesItem,
      int salesType, decimal retailPrice, DateTime startDate, bool isComponent, Employee employee, Shift shift, int salesDestinationID,
      int transactionLineNumber, decimal reductionAmount, decimal refundReductionAmount)
      : this(site, businessDate, transactionDate, fuelPump, fuelHose, soldQuantity, soldAmount, refundQuantity, refundAmount,
        reductionAmount, refundReductionAmount)
    {
      _TransactionNumber = transactionNumber;
      SalesItemID = salesItem.ID;
      SalesType = salesType;
      _RetailPrice = retailPrice;
      _StartDate = startDate;
      _IsComponent = isComponent;
      _EmployeeID = employee.ID;
      _ShiftID = shift.ID;
      _SalesDestinationID = salesDestinationID;
      _TransactionLineNumber = transactionLineNumber;
    }

    public SalesTransactionLineItem(Site site, DateTime businessDate, DateTime transactionDate, FuelPump fuelPump, FuelHose fuelHose,
      decimal soldQuantity, decimal soldAmount, decimal refundQuantity, decimal refundAmount, int salesItemID,
      int salesType, decimal reductionAmount, decimal refundReductionAmount, FuelSalesItem fuelSalesItem,
      PostedSalesItem postedSalesItem, int transactionNumber, int shiftID)
      : this(site, businessDate, transactionDate, fuelPump, fuelHose, soldQuantity, soldAmount, refundQuantity, refundAmount,
        reductionAmount, refundReductionAmount)
    {
      SalesItemID = salesItemID;
      SalesType = salesType;
      FuelSalesItem = fuelSalesItem;
      PostedSalesItem = postedSalesItem;
      PumpNumber = fuelPump?.PumpNumber;
      _TransactionNumber = transactionNumber;
      _ShiftID = shiftID;
    }

    public override bool Equals(object other)
    {
      SalesTransactionLineItem ot = other as SalesTransactionLineItem;
      if (ot == null)
      {
        return false;
      }

      return ot.Site.ID == Site.ID && ot.BusinessDate == BusinessDate && ot._TransactionNumber == _TransactionNumber &&
             ot.SalesItemID == SalesItemID && ot.SalesType == SalesType && ot._RetailPrice == _RetailPrice &&
             ot._StartDate == _StartDate && ot._IsComponent == _IsComponent && ot._EmployeeID == _EmployeeID &&
             ot._ShiftID == _ShiftID && ot._SalesDestinationID == _SalesDestinationID &&
             ot._TransactionLineNumber == _TransactionLineNumber;
    }

    public override int GetHashCode()
    {
      return Site.ID.GetHashCode() ^ BusinessDate.GetHashCode() ^ _TransactionNumber.GetHashCode() ^
             SalesItemID.GetHashCode() ^ SalesType.GetHashCode() ^ _RetailPrice.GetHashCode() ^
             _StartDate.GetHashCode() ^ _IsComponent.GetHashCode() ^ _EmployeeID.GetHashCode() ^
             _ShiftID.GetHashCode() ^ _SalesDestinationID.GetHashCode() ^
             _TransactionLineNumber.GetHashCode();
    }
  }

  public enum SalesTypes
  {
    RegularSales = 0,
    FuelPrepay = 1,
    FuelPrepayAdjustment = 2,
    LotteryCashWinner = 3,
    MoneyOrderValue = 4,
    PumpTest = 5,
    DriveOff = 6,
    LotteryTicketWinner = 7,
    StoreValueCard = 8,
    CashBack = 9,
    ContainerRedemption = 10,
    FuelPrepayCompletion = 11,
    StoreValueCardReload = 12,
    GiftCertificate = 13,
    ManualFuelSales = 14,
    ContainerDeposit = 15,
    TrustedSourceAdjustment = 16,
    PhoneCard = 17,
    PhoneCardRecharge = 18,
    StoreValueCardFee = 19
  }
}
