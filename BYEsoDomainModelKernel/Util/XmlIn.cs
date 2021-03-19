using NHibernate;
using NHibernate.Criterion;
using NHibernate.Dialect;
using NHibernate.Engine;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Type;
using NHibernate.UserTypes;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Xml;
using SqlString = NHibernate.SqlCommand.SqlString;

namespace BYEsoDomainModelKernel.Util
{
  public class XmlIn : AbstractCriterion
  {
    private readonly AbstractCriterion _Expr;
    private readonly string _PropertyName;
    private readonly object[] _Values;
    private readonly int _MaximumNumberOfParametersToNotUseXml = 100;

    public static AbstractCriterion Create(string property, IEnumerable values)
    {
      return new XmlIn(property, values);
    }

    /// <summary>
    /// Creates the specified property.
    /// </summary>
    /// <param name="property">The property.</param>
    /// <param name="values">The values.</param>
    /// <param name="maximumNumberOfParametersToNotUseXml">The maximum number of paramters allowed 
    /// for the XmlIn to create an xml string.</param>
    /// <returns></returns>
    public static AbstractCriterion Create(string property, IEnumerable values, int maximumNumberOfParametersToNotUseXml)
    {
      return new XmlIn(property, values, maximumNumberOfParametersToNotUseXml);
    }

    public XmlIn(string propertyName, IEnumerable values, int maximumNumberOfParametersToNotUseXml)
      : this(propertyName, values)
    {
      _MaximumNumberOfParametersToNotUseXml = maximumNumberOfParametersToNotUseXml;
    }

    public XmlIn(string propertyName, IEnumerable values)
    {
      _PropertyName = propertyName;
      var arrayList = new ArrayList();
      foreach (var o in values)
      {
        arrayList.Add(o);
      }
      _Values = arrayList.ToArray();
      _Expr = Restrictions.In(propertyName, arrayList);
    }

    public override string ToString()
    {
      return _PropertyName + " big in (" + StringHelper.ToString(_Values) + ')';
    }

    public override SqlString ToSqlString(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      //we only need this for SQL Server, and or large amount of values
      if ((criteriaQuery.Factory.Dialect is MsSql2005Dialect) == false || _Values.Length < _MaximumNumberOfParametersToNotUseXml)
      {
        return _Expr.ToSqlString(criteria, criteriaQuery);
      }

      IType type = criteriaQuery.GetTypeUsingProjection(criteria, _PropertyName);
      if (type.IsCollectionType)
      {
        throw new QueryException("Cannot use collections with InExpression");
      }

      if (_Values.Length == 0)
      {
        // "something in ()" is always false
        return new SqlString("1=0");
      }

      var result = new SqlStringBuilder();
      string[] columnNames = criteriaQuery.GetColumnsUsingProjection(criteria, _PropertyName);

      for (int columnIndex = 0; columnIndex < columnNames.Length; columnIndex++)
      {
        string columnName = columnNames[columnIndex];

        if (columnIndex > 0)
        {
          result.Add(" and ");
        }

        SqlType sqlType = type.SqlTypes(criteriaQuery.Factory)[columnIndex];
        result
          .Add(columnName)
          .Add(" in (")
          .Add("SELECT ParamValues.Val.value('.','")
          .Add(criteriaQuery.Factory.Dialect.GetTypeName(sqlType))
          .Add("') FROM ")
          .AddParameter()
          .Add(".nodes('/items/val') as ParamValues(Val)")
          .Add(")");
      }

      return result.ToSqlString();
    }

    
    public override TypedValue[] GetTypedValues(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      //we only need this for SQL Server, and or large amount of values
      if ((criteriaQuery.Factory.Dialect is MsSql2005Dialect) == false || _Values.Length < _MaximumNumberOfParametersToNotUseXml)
      {
        return _Expr.GetTypedValues(criteria, criteriaQuery);
      }

      IEntityPersister persister = null;
      IType type = criteriaQuery.GetTypeUsingProjection(criteria, _PropertyName);

      if (type.IsEntityType)
      {
        persister = criteriaQuery.Factory.GetEntityPersister(type.ReturnedClass.FullName);
      }
      var sw = new StringWriter();
      XmlWriter writer = XmlWriter.Create(sw);
      writer.WriteStartElement("items");
      foreach (var value in _Values.Where(value => value != null))
      {
        object valToWrite = persister != null ? persister.GetIdentifier(value) : value;
        writer.WriteElementString("val", valToWrite.ToString());
      }
      writer.WriteEndElement();
      writer.WriteEndDocument();
      writer.Flush();
      string xmlString = sw.GetStringBuilder().ToString();

      return new TypedValue[]
      {
      new TypedValue(new CustomType(typeof(XmlType), new Dictionary<string, string>()), xmlString, (bool)(object)EntityMode.Poco)
      };
    }

    private class XmlType : IUserType
    {
      private static readonly SqlType[] _SQLTypes = new SqlType[] { new SqlType(DbType.Xml) };
      private readonly Type _ReturnedType = typeof(string);
      private const bool _IsMutable = false;

      public SqlType[] SqlTypes
      {
        get
        {
          return _SQLTypes;
        }
      }

      public Type ReturnedType
      {
        get
        {
          return _ReturnedType;
        }
      }

      // ReSharper disable MemberHidesStaticFromOuterClass
      public new bool Equals(object x, object y)
      // ReSharper restore MemberHidesStaticFromOuterClass
      {
        return Object.Equals(x, y);
      }

      public int GetHashCode(object x)
      {
        return x.GetHashCode();
      }

      public object NullSafeGet(IDataReader rs, string[] names, object owner)
      {
        return null;
      }

      public void NullSafeSet(IDbCommand cmd, object value, int index)
      {
        var parameter = (IDataParameter)cmd.Parameters[index];
        parameter.Value = value;
      }

      public object DeepCopy(object value)
      {
        return value;
      }

      public bool IsMutable
      {
        get
        {
          return _IsMutable;
        }
      }

      public object Replace(object original, object target, object owner)
      {
        return original;
      }

      public object Assemble(object cached, object owner)
      {
        return cached;
      }

      public object Disassemble(object value)
      {
        return value;
      }

      public object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
      {
        throw new NotImplementedException();
      }

      public void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
      {
        throw new NotImplementedException();
      }
    }

    public override IProjection[] GetProjections()
    {
      return null;
    }
  }
}
