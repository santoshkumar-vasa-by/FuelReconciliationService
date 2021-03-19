using System;
using System.Data;
using System.Data.Common;
using NHibernate.Engine;
using NHibernate.SqlTypes;

namespace BYEsoDomainModelKernel.Models
{
  public class StringTimeToTimeSpan : BaseUserType<TimeSpan>
  {
    public override object NullSafeGet(IDataReader rs, string[] names, object owner)
    {
      throw new NotImplementedException();
      //var time = (string)NHibernateUtil.String.NullSafeGet(rs, names[0]);
      //if (time == null)
      //{
      //  return null;
      //}
      //return TimeSpan.Parse(time);
    }

    public override void NullSafeSet(IDbCommand cmd, object value, int index)
    {
      throw new NotImplementedException();
      //string formattedTimeSpan = null;
      //if (value != null)
      //{
      //  var timeSpan = (TimeSpan)value;
      //  formattedTimeSpan = String.Format("{0}:{1}",
      //    timeSpan.Hours.ToString("00"), timeSpan.Minutes.ToString("00"));
      //}

      //NHibernateUtil.String.NullSafeSet(cmd, formattedTimeSpan, index);
    }

    public override SqlType[] SqlTypes
    {
      get
      {
        return new[]
        {
          new SqlType(DbType.String)
        };
      }
    }

    public override object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
    {
      throw new NotImplementedException();
    }

    public override void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
    {
      throw new NotImplementedException();
    }
  }
}
