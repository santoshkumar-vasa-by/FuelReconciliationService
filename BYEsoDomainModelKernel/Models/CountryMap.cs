using FluentNHibernate.Mapping;
using WaveDatabaseMetadata = RP.DatabaseMetadata.WaveDatabaseMetadata;

namespace BYEsoDomainModelKernel.Models
{
  public class CountryMap : ClassMap<Country>
  {
    public CountryMap()
    {
      var tableMeta = WaveDatabaseMetadata.Rad_Sys_Country;
      this.Table(tableMeta);
      BatchSize(100);

      Id(x => x.Id).Column(tableMeta.Country_id)
        .GeneratedBy.Custom<TicketGenerator>();
      Map(x => x.CountryCode).Column(tableMeta.Country_code);
      Map(x => x.CountryName).Column(tableMeta.Name);
      Map(x => x.IsoAlpha2Code).Column(tableMeta.Iso_alpha2);
      Map(x => x.IsoAlpha3Code).Column(tableMeta.Iso_alpha3);
    }
  }
}
