using FluentNHibernate.Mapping;
using NHibernate.Type;
using RP.DatabaseMetadata;

namespace BYEsoDomainModelKernel.Models
{
  public class TimeEventMap : ClassMap<TimeEvent>
  {
    public TimeEventMap()
    {
      var tableMeta = WaveDatabaseMetadata.Time_Event;
      this.Table(tableMeta);

      Id(x => x.ID).Column(tableMeta.Time_event_id).GeneratedBy.Custom<TicketGenerator>();
      Map(x => x.Start).Column(tableMeta.Start_date);
      Map(x => x.Name).Column(tableMeta.Name);
      Map(x => x.RecurringFlag).Column(tableMeta.Recurring_flag)
        .CustomType<EnumCharType<RecurringFlag>>();
      Map(x => x.TimeRecurrenceCode).Column(tableMeta.Time_recurrence_code)
        .CustomType<EnumCharType<TimeEventRecurrenceThresholdScheduleCode>>(); 
      Map(x => x.RecurrenceCode).Column(tableMeta.Recurrence_code)
                                .CustomType<EnumCharType<TimeEventRecurrenceCode>>();
      Map(x => x.RecurrenceFrequency).Column(tableMeta.Recurrence_freq);
      Map(x => x.DayOfMonth).Column(tableMeta.Recurrence_day_of_month);
      Map(x => x.RecurrenceWeekOfTheMonth).Column(tableMeta.Recurrence_week_of_month)
        .CustomType<EnumCharType<TimeEventRecurrenceWeekOfMonth>>();
      Map(x => x.StartTime).Column(tableMeta.Start_time)
                           .CustomType<StringTimeToTimeSpan>();
      Map(x => x.EventTypeCode).Column(tableMeta.Time_event_type_code)
        .CustomType<EnumCharType<TimeEventTypeCode>>();
      DiscriminateSubClassesOnColumn<string>(tableMeta.Recurrence_code.ColumnName, "o")
        .Formula("case when recurrence_code IN ('w', 'a') then 'w' " +
                 "when (recurrence_code is null and time_event_type_code = 'h') then 'h' " +
                 "when recurrence_code IN ('d', 'f', 'm', 'c') then recurrence_code " +
                 " else 'o' end");
    }
  }
}
