using FluentNHibernate.Mapping;
using WaveDatabaseMetadata = RP.DatabaseMetadata.WaveDatabaseMetadata;

namespace BYEsoDomainModelKernel.Models
{
  public class FuelInvoiceItemMap: ClassMap<BYEsoDomainModelKernel.Models.FuelInvoiceItem>
  {
    public FuelInvoiceItemMap()
    {
      var tableMeta = WaveDatabaseMetadata.Fuel_Invoice_Item;
      this.Table(tableMeta);

      CompositeId()
        .KeyReference(x => x.Site, tableMeta.Business_unit_id)
        .KeyReference(x => x.FuelItem, tableMeta.Fuel_inventory_item_id)
        .KeyReference(x => x.FuelInvoice, tableMeta.Fuel_invoice_id);

      References(x => x.Site).Column(tableMeta.Business_unit_id)
        .Not.Update()
        .Not.Insert()
        .Cascade.None();

      References(x => x.FuelItem).Column(tableMeta.Fuel_inventory_item_id)
        .Not.Update()
        .Not.Insert()
        .Cascade.None();

      References(x => x.FuelInvoice).Column(tableMeta.Fuel_invoice_id)
        .Not.Update()
        .Not.Insert()
        .Cascade.None();
      Map(x => x.Quantity).Column(tableMeta.Quantity);
      Map(x => x.Cost).Column(tableMeta.Cost);
    }
  }
}
