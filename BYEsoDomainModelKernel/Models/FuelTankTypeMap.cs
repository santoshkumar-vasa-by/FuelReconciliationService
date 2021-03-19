using FluentNHibernate.Mapping;
using WaveDatabaseMetadata = RP.DatabaseMetadata.WaveDatabaseMetadata;


namespace BYEsoDomainModelKernel.Models
{
  public class FuelTankTypeMap : ClassMap<FuelTankType>
  {
    public FuelTankTypeMap()
    {
      var tableMeta = WaveDatabaseMetadata.Fuel_Tank_Type;
      this.Table(tableMeta);
      this.BatchSize(100);

      Id(x => x.ID).Column(tableMeta.Fuel_tank_type_id)
        .GeneratedBy.Custom<TicketGenerator>();

      Map(x => x.Capacity).Column(tableMeta.Maximum_volume);
      Map(x => x.MaximumFillVolume).Column(tableMeta.Fill_volume);
      Map(x => x.ClientId).Column(tableMeta.Client_id);
      Map(x => x.Name).Column(tableMeta.Name);
    }
  }
}