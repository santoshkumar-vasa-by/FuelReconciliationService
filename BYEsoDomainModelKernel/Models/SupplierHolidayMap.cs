using FluentNHibernate.Mapping;
using RP.DomainModelKernel.Common.Types;
using WaveDatabaseMetadata = RP.DatabaseMetadata.WaveDatabaseMetadata;

namespace BYEsoDomainModelKernel.Models
{
  public class SupplierHolidayMap : ClassMap<SupplierHoliday>
  {
    public SupplierHolidayMap()
    {
      var tableMeta = WaveDatabaseMetadata.Supplier_closed_list;
      this.Table(tableMeta);
      BatchSize(100);

      CompositeId()
        .KeyReference(x => x.HolidayEvent, tableMeta.Time_event_id)
        .KeyReference(x => x.Supplier, tableMeta.Supplier_id);

      References(x => x.Supplier)
        .Column(tableMeta.Supplier_id)
        .Not.Insert()
        .Not.Update()
        .Cascade.None();

      References(x => x.HolidayEvent)
        .Column(tableMeta.Time_event_id)
        .Not.Insert()
        .Not.Update()
        .Cascade.None();

      Map(x => x.AppliesToOrders).Column(tableMeta.Order_flag).CustomType<LcaseYesNo>();
      Map(x => x.AppliesToDeliveries).Column(tableMeta.Delivery_flag).CustomType<LcaseYesNo>();
    }
  }
}
