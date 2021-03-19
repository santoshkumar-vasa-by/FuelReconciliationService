using FluentNHibernate.Mapping;
using RP.DatabaseMetadata;
using RP.DomainModelKernel.Common;

namespace BYEsoDomainModelKernel.Models
{
  public class HolidayTimeEventMap : SubclassMap<HolidayTimeEvent>
  {
    public HolidayTimeEventMap()
    {
      DiscriminatorValue(EnumHelper.ToString(TimeEventRecurrenceCode.Holiday));

      var tableMeta = WaveDatabaseMetadata.Time_Event;
      this.Table(tableMeta);
    }
  }
}
