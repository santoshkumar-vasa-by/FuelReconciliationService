using FluentNHibernate.Mapping;
using WaveDatabaseMetadata = RP.DatabaseMetadata.WaveDatabaseMetadata;

namespace BYEsoDomainModelKernel.Models
{
  public class AddressMap : ClassMap<Address>
  {
    public AddressMap()
    {
      var tableMeta = WaveDatabaseMetadata.Address;
      this.Table(tableMeta);
      BatchSize(100);

      Id(x => x.Id).Column(tableMeta.Address_id)
        .GeneratedBy.Custom<TicketGenerator>();
      Map(x => x.AddressLine1).Column(tableMeta.Address_line_1);
      Map(x => x.AddressLine2).Column(tableMeta.Address_line_2);
      Map(x => x.StateCode).Column(tableMeta.State_code);
      Map(x => x.CountryCode).Column(tableMeta.Country_code);
      Map(x => x.City).Column(tableMeta.City);
      Map(x => x.PostalCode).Column(tableMeta.Postal_code);
      Map(x => x.HomePhone).Column(tableMeta.Home_phone);
      Map(x => x.WorkPhone).Column(tableMeta.Work_phone);
      Map(x => x.CellPhone).Column(tableMeta.Cell_phone);
      Map(x => x.Fax).Column(tableMeta.Fax_number);
      Map(x => x.Email).Column(tableMeta.E_mail);

      References(x => x.Country)
        .Column(tableMeta.Country_id)
        .Cascade.None();

      References(x => x.State)
        .Column(tableMeta.State_id)
        .Cascade.None();
    }
  }
}
