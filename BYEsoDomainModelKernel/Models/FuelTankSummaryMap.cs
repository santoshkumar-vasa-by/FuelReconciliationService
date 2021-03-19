using FluentNHibernate.Mapping;
using WaveDatabaseMetadata = RP.DatabaseMetadata.WaveDatabaseMetadata;

namespace BYEsoDomainModelKernel.Models
{
  public class FuelTankSummaryMap : ClassMap<FuelTankSummary>
  {
    public FuelTankSummaryMap()
    {
      var tableMeta = WaveDatabaseMetadata.Fuel_Tank_Summary;
      this.Table(tableMeta);
      this.BatchSize(100);

      CompositeId()
        .KeyReference(x => x.Site, tableMeta.Business_unit_id)
        .KeyReference(x => x.FuelTank, tableMeta.Fuel_tank_id)
        .KeyProperty(x => x.BusinessDate, tableMeta.Business_date);

      Map(x => x.CloseVolume).Column(tableMeta.Close_volume);
      Map(x => x.AuditCloseVolume).Column(tableMeta.Audit_close_volume).Scale(6);
      Map(x => x.AuditOpenVolume).Column(tableMeta.Audit_open_volume).Scale(6);
      Map(x => x.OpenVolume).Column(tableMeta.Open_volume);
      Map(x => x.PumpTestVolumeNonReturned).Column(tableMeta.Pump_test_volume_non_returned);
      Map(x => x.SalesVolume).Column(tableMeta.Sales_volume);
      Map(x => x.DeliveryVolume).Column(tableMeta.Delivery_volume);
      Map(x => x.PumpTestVolumeReturned).Column(tableMeta.Pump_test_volume_returned);
      Map(x => x.AdjustmentVolume).Column(tableMeta.Adjustment_volume);
      Map(x => x.PumpedVolume).Column(tableMeta.Pumped_volume);
      Map(x => x.BookVolume).Column(tableMeta.Book_volume);
      Map(x => x.AuditBookVolume).Column(tableMeta.Audit_book_volume).Scale(6);
    }
  }
}