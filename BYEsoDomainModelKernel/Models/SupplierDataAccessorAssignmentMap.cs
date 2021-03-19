using FluentNHibernate.Mapping;
using WaveDatabaseMetadata = RP.DatabaseMetadata.WaveDatabaseMetadata;

namespace BYEsoDomainModelKernel.Models
{
  public class SupplierDataAccessorAssignmentMap : ClassMap<SupplierDataAccessorAssignment>
  {
    public SupplierDataAccessorAssignmentMap()
    {
      var tableMeta = WaveDatabaseMetadata.Supplier_DA_Effective_Date_List;

      this.Table(tableMeta);

      CompositeId()
        .KeyReference(x => x.Supplier, tableMeta.Supplier_id)
        .KeyReference(x => x.DataAccessor, tableMeta.Data_accessor_id);

      References(x => x.Supplier)
        .Column(tableMeta.Supplier_id)
        .Not.Insert()
        .Not.Update();

      References(x => x.DataAccessor)
        .Column(tableMeta.Data_accessor_id)
        .Not.Insert()
        .Not.Update();

      Map(x => x.Start).Column(tableMeta.Start_date);
      Map(x => x.End).Column(tableMeta.End_date);
    }
  }
}
