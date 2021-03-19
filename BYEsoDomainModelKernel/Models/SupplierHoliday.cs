
namespace BYEsoDomainModelKernel.Models
{
  public class SupplierHoliday : BaseAuditEntity
  {
    public virtual HolidayTimeEvent HolidayEvent { get; set; }
    public virtual Supplier Supplier { get; set; }
    public virtual bool AppliesToOrders { get; set; }
    public virtual bool AppliesToDeliveries { get; set; }

    protected internal SupplierHoliday()
    {
    }

    protected internal SupplierHoliday(HolidayTimeEvent holidayEvent, Supplier supplier, bool appliesToOrders, bool appliesToDeliveries)
    {
      HolidayEvent = holidayEvent;
      Supplier = supplier;
      AppliesToOrders = appliesToOrders;
      AppliesToDeliveries = appliesToDeliveries;
    }

    public override bool Equals(object other)
    {
      var ot = other as SupplierHoliday;
      if (ot == null)
      {
        return false;
      }
      return ot.HolidayEvent.Equals(HolidayEvent) && ot.Supplier.Equals(Supplier);
    }

    public override int GetHashCode()
    {
      return HolidayEvent.ID.GetHashCode() ^ Supplier.ID.GetHashCode();
    }
  }
}
