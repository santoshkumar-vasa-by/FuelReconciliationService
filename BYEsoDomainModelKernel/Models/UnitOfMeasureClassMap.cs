using FluentNHibernate.Mapping;
using WaveDatabaseMetadata = RP.DatabaseMetadata.WaveDatabaseMetadata;

namespace BYEsoDomainModelKernel.Models
{
  public class UnitOfMeasureClassMap : ClassMap<UnitOfMeasureClass>
  {
    public UnitOfMeasureClassMap()
    {
      var tableMeta = WaveDatabaseMetadata.Unit_Of_Measure_Class;
      this.Table(tableMeta);
      this.BatchSize(100);

      Id(x => x.ID).Column(tableMeta.Unit_of_measure_class_id)
        .GeneratedBy.Assigned();
      Map(x => x.Name).Column(tableMeta.Name);
      Map(x => x.MeasureTypeCode).Column(tableMeta.Measure_system_type_code);
      Map(x => x.ClientID).Column(tableMeta.Client_id);

      References(x => x.BaseUnitOfMeasure)
        .Column(tableMeta.Unit_of_measure_id);
    }
  }
}