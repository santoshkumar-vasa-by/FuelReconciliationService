namespace BYEsoDomainModelKernel.Models
{
  public class FuelDeliveryInvoiceTotal
  {
    public FuelInvoice FuelInvoice { get; set; }
    public virtual Site Site { get; set; }
  }
}
