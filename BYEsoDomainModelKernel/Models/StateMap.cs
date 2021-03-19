using FluentNHibernate.Mapping;
using WaveDatabaseMetadata = RP.DatabaseMetadata.WaveDatabaseMetadata;

namespace BYEsoDomainModelKernel.Models
{
  public class StateMap : ClassMap<State>
  {
    public StateMap()
    {
      var tableMeta = WaveDatabaseMetadata.Rad_Sys_State;
      this.Table(tableMeta);
      BatchSize(100);

      Id(x => x.Id).Column(tableMeta.State_id)
        .GeneratedBy.Custom<TicketGenerator>();
      Map(x => x.StateCode).Column(tableMeta.State_code);
      Map(x => x.StateName).Column(tableMeta.Name);
      References(x => x.Country)
        .Column(tableMeta.Country_id)
        .Not.Insert()
        .Not.Update();
    }
  }
}
