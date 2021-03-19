using FluentNHibernate.Mapping;
using RP.DatabaseMetadata;

namespace BYEsoDomainModelKernel.Models
{
  public class SupplierMetricAssignmentMap : ClassMap<SupplierMetricAssignment>
  {
    public SupplierMetricAssignmentMap()
    {
      var tableMeta = WaveDatabaseMetadata.Supplier_Metric_List;
      this.Table(tableMeta);

      CompositeId()
        .KeyProperty(x => x.SupplierID, tableMeta.Supplier_id)
        .KeyProperty(x => x.MetricID, tableMeta.Metric_id);

      //References(x => x.Metric).Column(tableMeta.Metric_id)
      //  .Not.Insert()
      //  .Not.Update()
      //  .Cascade.None();
    }
  }
}
