namespace BYEsoDomainModelKernel.Models
{
  public class Country : BaseAuditEntity
  {
    public virtual int Id { get; set; }
    public virtual string CountryCode { get; set; }
    public virtual string CountryName { get; set; }
    public virtual string IsoAlpha2Code { get; set; }
    public virtual string IsoAlpha3Code { get; set; }
    public Country()
    {
    }
    public Country(string countryCode, string countryName)
    {
      CountryCode = countryCode;
      CountryName = countryName;
    }
    public Country(string countryCode, string countryName, string isoAlpha2Code, string isoAlpha3Code)
    {
      CountryCode = countryCode;
      CountryName = countryName;
      IsoAlpha2Code = isoAlpha2Code;
      IsoAlpha3Code = isoAlpha3Code;
    }
    public override bool Equals(object obj)
    {
      var ot = obj as Country;
      if (ot == null)
      {
        return false;
      }
      return ot.Id == Id;
    }

    public override int GetHashCode()
    {
      return Id.GetHashCode();
    }
  }
  public enum CountryDomainErrors
  {
    NotFound,
    Alpha2CodeNotFound,
    Alpha3CodeNotFound,
    IsoAlphaCodeLengthShouldNotLessThanTwo,
    IsoAlphaCodeLengthShouldNotGreaterThanThree
  }
}