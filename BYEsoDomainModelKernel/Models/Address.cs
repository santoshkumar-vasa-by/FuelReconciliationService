namespace BYEsoDomainModelKernel.Models
{
  public class Address : BaseAuditEntity
  {
    public virtual int Id { get; set; }
    public virtual string AddressLine1 { get; set; }
    public virtual string AddressLine2 { get; set; }
    public virtual string CountryCode { get; set; }
    public virtual string StateCode { get; set; }
    public virtual string City { get; set; }
    public virtual string PostalCode { get; set; }
    public virtual string CellPhone { get; set; }
    public virtual string WorkPhone { get; set; }
    public virtual string HomePhone { get; set; }
    public virtual string Fax { get; set; }
    public virtual string Email { get; set; }
    public virtual Country Country { get; set; }
    public virtual State State { get; set; }
  }
}