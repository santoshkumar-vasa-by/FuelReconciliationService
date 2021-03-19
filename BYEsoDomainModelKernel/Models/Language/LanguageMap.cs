using FluentNHibernate.Mapping;
using WaveDatabaseMetadata = RP.DatabaseMetadata.WaveDatabaseMetadata;

namespace BYEsoDomainModelKernel.Models.Language
{
  public class LanguageMap : ClassMap<Language>
  {
    public LanguageMap()
    {
      var tableMeta = WaveDatabaseMetadata.Language;
      this.Table(tableMeta);

      Id(x => x.ID).Column(tableMeta.Language_id)
        .GeneratedBy.Assigned();

      Map(x => x.Name).Column(tableMeta.Name);

      //References(x => x.IsoLanguage)
      //  .Column(tableMeta.Iso_language_id)
      //  .Cascade.None();
    }
  }
}
