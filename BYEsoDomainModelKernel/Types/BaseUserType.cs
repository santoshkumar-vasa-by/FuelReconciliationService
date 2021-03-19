﻿using System;
using System.Data.Common;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace BYEsoDomainModelKernel.Types
{
  [Serializable]
  public abstract class BaseUserType<T> : IUserType
  {

    public abstract object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner);

    public abstract void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session);
    
    //public abstract object NullSafeGet(IDataReader rs, string[] names, object owner);

    //public abstract void NullSafeSet(IDbCommand cmd, object value, int index);

    public abstract SqlType[] SqlTypes { get; }

    public new bool Equals(object x, object y)
    {
      if (ReferenceEquals(x, y))
      {
        return true;
      }

      if (x == null || y == null)
      {
        return false;
      }

      return x.Equals(y);
    }

    public int GetHashCode(object x)
    {
      return x.GetHashCode();
    }


    public object DeepCopy(object value)
    {
      return value;
    }

    public object Replace(object original, object target, object owner)
    {
      return original;
    }

    public object Assemble(object cached, object owner)
    {
      return DeepCopy(cached);
    }

    public object Disassemble(object value)
    {
      return DeepCopy(value);
    }

    public Type ReturnedType
    {
      get
      {
        return typeof(T);
      }
    }

    public bool IsMutable
    {
      get
      {
        return false;
      }
    }
  }
}
