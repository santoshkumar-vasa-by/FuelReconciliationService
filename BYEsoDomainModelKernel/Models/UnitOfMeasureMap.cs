using FluentNHibernate.Mapping;
using WaveDatabaseMetadata = RP.DatabaseMetadata.WaveDatabaseMetadata;
using RP.DomainModelKernel.Common.Types;

namespace BYEsoDomainModelKernel.Models
{
  public class UnitOfMeasureMap : ClassMap<UnitOfMeasure>
  {
    public UnitOfMeasureMap()
    {
      var tableMeta = WaveDatabaseMetadata.Unit_of_Measure;
      this.Table(tableMeta);
      this.BatchSize(100);

      Id(x => x.ID).Column(tableMeta.Unit_of_measure_id)
        .GeneratedBy.Assigned();
      Map(x => x.Name).Column(tableMeta.Name);
      Map(x => x.Factor).Column(tableMeta.Factor);
      Map(x => x.IsActive).Column(tableMeta.Active_flag).CustomType<LcaseYesNo>();
      Map(x => x.IsPurge).Column(tableMeta.Purge_flag).CustomType<LcaseYesNo>();

      References(x => x.UnitOfMeasureClass)
        .Column(tableMeta.Unit_of_measure_class_id)
        .Cascade.None();

      HasMany(x => x.Translations)
        .KeyColumn(tableMeta.Unit_of_measure_id)
        .BatchSize(100)
        .Cascade.All();
    }
  }
}