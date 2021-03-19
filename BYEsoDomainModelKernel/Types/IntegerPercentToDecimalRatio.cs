using System;
using System.Data;
using System.Data.Common;
using NHibernate;
using NHibernate.Engine;
using NHibernate.SqlTypes;

namespace BYEsoDomainModelKernel.Types
{
  public class IntegerPercentToDecimalRatio : BaseUserType<decimal>
  {
    //public object NullSafeGet(IDataReader rs, string[] names, object owner)
    //{
    //  var integerPercentage = (int?)NHibernateUtil.Int32.NullSafeSet(rs, names, owner);
    //  if (integerPercentage.HasValue)
    //  {
    //    return Convert.ToDecimal(integerPercentage / 100.0);
    //  }
    //  return null;
    //}

    //public void NullSafeSet(IDbCommand cmd, object value, int index)
    //{
    //  int? integerPercentage = null;
    //  var decimalRatio = (decimal?)value;
    //  if (decimalRatio.HasValue)
    //  {
    //    integerPercentage = Convert.ToInt32(decimalRatio * 100);
    //  }
    //  NHibernateUtil.Int32.NullSafeSet(cmd, integerPercentage, index);
    //}

    public override object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
    {
      var integerPercentage = (int?)NHibernateUtil.Int32.NullSafeGet(rs, names, session, owner);
      if (integerPercentage.HasValue)
      {
        return Convert.ToDecimal(integerPercentage / 100.0);
      }
      return null;
    }

    public override void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
    {
      int? integerPercentage = null;
      var decimalRatio = (decimal?)value;
      if (decimalRatio.HasValue)
      {
        integerPercentage = Convert.ToInt32(decimalRatio * 100);
      }

      NHibernateUtil.Int32.NullSafeSet(cmd, value, index, session);
    }

    public override SqlType[] SqlTypes
    {
      get
      {
        return new[] { new SqlType(DbType.Int32) };
      }
    }
  }
}
