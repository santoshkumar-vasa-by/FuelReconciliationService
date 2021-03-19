namespace BYEsoDomainModelKernel.Models
{
  public class FuelInvoiceItem: BaseAuditEntity
  {
    public virtual Site Site { get; set; }
    public virtual FuelItem FuelItem { get; set; }
    public virtual FuelInvoice FuelInvoice { get; set; }
    public virtual decimal? Quantity { get; set; }
    public virtual decimal? Cost { get; set; }
    protected internal FuelInvoiceItem()
    {
    }
    public FuelInvoiceItem(Site site, FuelItem fuelItem, decimal? quantity, FuelInvoice invoice, decimal? cost)
    {
      Site = site;
      FuelItem = fuelItem;
      Quantity = quantity;
      FuelInvoice = invoice;
      Cost = cost;
    }

    public override bool Equals(object other)
    {
      var ot = other as FuelInvoiceItem;
      if (ot == null)
      {
        return false;
      }
      return ot.Site.ID == Site.ID &&
             ot.FuelItem.ID == FuelItem.ID &&
             ot.FuelInvoice.ID == FuelInvoice.ID;
    }

    public override int GetHashCode()
    {
      return Site.ID.GetHashCode() ^ FuelItem.ID.GetHashCode() ^ FuelInvoice.ID.GetHashCode();
    }
  }
}
