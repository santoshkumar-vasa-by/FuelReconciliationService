using FluentNHibernate.Mapping;
using WaveDatabaseMetadata = RP.DatabaseMetadata.WaveDatabaseMetadata;

namespace BYEsoDomainModelKernel.Models
{
  public class ItemDataAccessorAssignmentMap : ClassMap<ItemDataAccessorAssignment>
  {
    public ItemDataAccessorAssignmentMap()
    {
      var tableMeta = WaveDatabaseMetadata.Item_DA_Effective_Date_List;

      this.Table(tableMeta);

      CompositeId()
        .KeyReference(x => x.Item, tableMeta.Item_id)
        .KeyReference(x => x.DataAccessor, tableMeta.Data_accessor_id);

      References(x => x.Item)
        .Column(tableMeta.Item_id)
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