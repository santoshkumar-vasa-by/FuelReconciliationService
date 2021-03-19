using System;

namespace BYEsoDomainModelKernel.Models
{
  public class SupplierDto
  {
    public virtual BYEsoDomainModelKernel.Models.Supplier Supplier { get; set;}
    public virtual DateTime? DeliveryDate { get; set; }
    public virtual DateTime? NextDeliveryDate { get; set; }
    public virtual string SiteId { get; set; }
  }
}
