using FluentNHibernate.Mapping;
using WaveDatabaseMetadata = RP.DatabaseMetadata.WaveDatabaseMetadata;

namespace BYEsoDomainModelKernel.Models
{
  public class UnitOfMeasureTranslationMap : ClassMap<UnitOfMeasureTranslation>
  {
    public UnitOfMeasureTranslationMap()
    {
      var tableMeta = WaveDatabaseMetadata.Unit_of_Measure_Lang;

      this.Table(tableMeta);
      BatchSize(100);

      CompositeId()
        .KeyReference(x => x.UnitOfMeasure, tableMeta.Unit_of_measure_id)
        .KeyProperty(x => x.LanguageId, tableMeta.Translated_lang_id);

      References(x => x.UnitOfMeasure)
        .Column(tableMeta.Unit_of_measure_id)
        .Not.Insert()
        .Not.Update();

      Map(x => x.Name).Column(tableMeta.Name);
    }
  }
}
