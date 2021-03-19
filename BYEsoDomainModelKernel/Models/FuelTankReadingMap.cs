using FluentNHibernate.Mapping;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using WaveDatabaseMetadata = RP.DatabaseMetadata.WaveDatabaseMetadata;


namespace BYEsoDomainModelKernel.Models
{
  public partial class FuelTankReading
  {
    public static class MapExpressions
    {
      public static readonly Expression<Func<FuelTankReading, IEnumerable<FuelTankReadingLineItem>>>
        _LineItems = x => x._LineItems;

      public static readonly Expression<Func<FuelTankReading, object>>
        _ElectronicFlag = x => x._ElectronicFlag;
    }
  }

  public class FuelTankReadingMap : ClassMap<FuelTankReading>
  {
    public FuelTankReadingMap()
    {
      var tableMeta = WaveDatabaseMetadata.Fuel_Tank_Reading;
      this.Table(tableMeta);
      this.BatchSize(100);

      Id(x => x.ID).Column(tableMeta.Fuel_tank_reading_id)
        .GeneratedBy.Custom<TicketGenerator>();

      References(x => x.Site)
        .Column(tableMeta.Business_unit_id)
        .Not.Update();

      Map(x => x.ReadingTimestamp).Column(tableMeta.Read_timestamp);
      Map(x => x.BusinessDate).Column(tableMeta.Business_date);

      Map(x => x.Status).Column(tableMeta.Status_code).CustomType<EnumCharType<TankReadingStatus>>();
      Map(x => x.Type).Column(tableMeta.Reading_type_code).CustomType<EnumCharType<TankReadingType>>();
      Map(x => x.IsImported).Column(tableMeta.Imported_flag).CustomType<LcaseYesNo>();

      Map(FuelTankReading.MapExpressions._ElectronicFlag)
        .Column(tableMeta.Electronic_flag).CustomType<LcaseYesNo>();

      HasMany(FuelTankReading.MapExpressions._LineItems)
      .KeyColumn(tableMeta.Fuel_tank_reading_id)
      .BatchSize(100)
      .Inverse()
      .Cascade.AllDeleteOrphan();
    }
  }
}