using System;
using NHibernate.SqlTypes;
using NHibernate.Type;

namespace BYEsoDomainModelKernel.Models
{
  [Serializable]
  public class LcaseYesNo : CharBooleanType
  {
    // Methods
    public LcaseYesNo()
      : base(new AnsiStringFixedLengthSqlType(1))
    {
    }

    // Properties
    protected override sealed string FalseString
    {
      get
      {
        return "n";
      }
    }

    public override string Name
    {
      get
      {
        return "LcaseYesNo";
      }
    }

    protected override sealed string TrueString
    {
      get
      {
        return "y";
      }
    }
  }
}