using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Mapping;
using RP.DatabaseMetadata;
using RP.DomainModelKernel.Common;
using RP.DomainModelKernel.Common.DatabaseConnectionSettings;

namespace BYEsoDomainModelKernel.Models
{
  public static class Extensions
  {
    public static void UseWarehouseSchema<T>(this ClassMap<T> classMap)
    {
      IDatabaseConnectionSettings settings = Registry.GetDatabaseConnectionSettings();
      if (settings.WarehouseSchemaName != null)
      {
        classMap.Schema(settings.WarehouseSchemaName);
      }
    }

    public static void Table<T>(this ClassMap<T> classMap, TableMetadata meta)
    {
      string tableName = meta.GetTableName();
      Type entityType = classMap.GetType();
      EntityTableMap.MapTableToEntity(entityType, tableName);

      classMap.Table(tableName);
      MapAuditFieldsIfPresent(classMap, meta);
      MapOwnerFieldIfPresent(classMap, meta);
    }

    public static void Table<T>(this SubclassMap<T> subClassMap, TableMetadata meta)
    {
      string tableName = meta.GetTableName();
      Type entityType = subClassMap.GetType();
      EntityTableMap.MapTableToEntity(entityType, tableName);

      subClassMap.Table(tableName);
    }

    private static void MapOwnerFieldIfPresent<T>(ClassMap<T> classMap, TableMetadata meta)
    {
      if (typeof(ICDMEntity).IsAssignableFrom(typeof(T)))
      {
        Type t = meta.GetType();

        MemberInfo[] cdmOwnerIDMemberInfo = t.GetMember("Cdm_owner_id");

        if (cdmOwnerIDMemberInfo.Length > 0)
        {
          var reference = classMap
            .References(x => ((ICDMEntity)x).Owner).Column("Cdm_owner_id")
            .Not.Update()
            .Not.Nullable()
            .Cascade.None();

          if (ClassMapHasOwnerInCompositeID(classMap))
          {
            reference.Not.Insert();
          }
        }
      }
    }

    private static bool ClassMapHasOwnerInCompositeID<T>(ClassMap<T> classMap)
    {
      return typeof(ICDMEntityWithOwnerInCompositeID).IsAssignableFrom(typeof(T));
    }

    private static void MapAuditFieldsIfPresent<T>(ClassMap<T> classMap, TableMetadata meta)
    {
      if (typeof(IBaseAuditEntity).IsAssignableFrom(typeof(T)))
      {
        Type t = meta.GetType();

        MemberInfo[] lastModifiedTimestampMemberInfo = t.GetMember("Last_modified_timestamp");

        if (lastModifiedTimestampMemberInfo.Length > 0)
        {
          classMap.Map(x => ((IBaseAuditEntity)x).LastModifiedTimestamp).Column("last_modified_timestamp");
        }

        MemberInfo[] lastModifiedUserIdMemberInfo = t.GetMember("Last_modified_user_id");

        if (lastModifiedUserIdMemberInfo.Length > 0)
        {
          classMap.Map(x => ((IBaseAuditEntity)x).LastModifiedUserID).Column("last_modified_user_id");
        }

        MemberInfo[] clientIdMemberInfo = t.GetMember("Client_id");

        if (clientIdMemberInfo.Length > 0)
        {
          classMap.Map(x => ((IBaseAuditEntity)x).ClientID).Column("client_id");
        }

        classMap.ApplyFilter<ClientIDFilter>();
      }
    }

    public static PropertyPart Column(this PropertyPart propPart, TableColumnMetadata meta)
    {
      return propPart.Column(meta.ColumnName);
    }

    public static IdentityPart Column(this IdentityPart identPart, TableColumnMetadata meta)
    {
      return identPart.Column(meta.ColumnName);
    }

    public static CompositeIdentityPart<T> KeyReference<T>(this CompositeIdentityPart<T> compositeIdentity,
      Expression<Func<T, object>> expression, TableColumnMetadata meta)
    {
      return compositeIdentity.KeyReference(expression, meta.ColumnName);
    }

    public static CompositeIdentityPart<T> KeyProperty<T>(this CompositeIdentityPart<T> compositeIdentity,
      Expression<Func<T, object>> expression, TableColumnMetadata meta)
    {
      return compositeIdentity.KeyProperty(expression, meta.ColumnName);
    }

    public static ManyToOnePart<TOther> Column<TOther>(this ManyToOnePart<TOther> manyToOnePart, TableColumnMetadata meta)
    {
      return manyToOnePart.Column(meta.ColumnName);
    }
    public static KeyPropertyPart Column(this KeyPropertyPart propPart, TableColumnMetadata meta)
    {
      return propPart.ColumnName(meta.ColumnName);
    }

    public static KeyPropertyPart CustomType<TTargetType>(this KeyPropertyPart propPart)
    {
      return propPart.Type(typeof(TTargetType));
    }
    public static OneToManyPart<TChild> KeyColumn<TChild>(this OneToManyPart<TChild> oneToManyPart, TableColumnMetadata meta)
    {
      return oneToManyPart.KeyColumn(meta.ColumnName);
    }

    public static void Join<T>(this ClassMap<T> classMap, TableMetadata meta, Action<JoinPart<T>> action)
    {
      classMap.Join(meta.GetTableName(), action);
    }

    public static void Join<T>(this SubclassMap<T> subClassMap, TableMetadata meta, Action<JoinPart<T>> action)
    {
      subClassMap.Join(meta.GetTableName(), action);
    }

    public static JoinPart<T> KeyColumn<T>(this JoinPart<T> joinPart, TableColumnMetadata meta)
    {
      return joinPart.KeyColumn(meta.ColumnName);
    }
  }
}
