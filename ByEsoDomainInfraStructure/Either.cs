using System;
using System.Collections.Generic;
using System.Text;

namespace ByEsoDomainInfraStructure
{
  public abstract class Either<TLeft, TRight>
  {
    public abstract Either<TNewLeft, TRight> MapLeft<TNewLeft>(Func<TLeft, TNewLeft> mapping);
    public abstract Either<TLeft, TNewRight> MapRight<TNewRight>(Func<TRight, TNewRight> mapping);
    public abstract TLeft Reduce(Func<TRight, TLeft> mapping);
  }

  public class Left<TLeft, TRight> : Either<TLeft, TRight>
  {
    private TLeft _value { get; }
    public Left(TLeft value)
    {
      _value = value;
    }

    public override Either<TNewLeft, TRight> MapLeft<TNewLeft>(Func<TLeft, TNewLeft> mapping) =>
      new Left<TNewLeft, TRight>(mapping(this._value));

    public override Either<TLeft, TNewRight> MapRight<TNewRight>(Func<TRight, TNewRight> mapping) =>
      new Left<TLeft, TNewRight>(this._value);

    public override TLeft Reduce(Func<TRight, TLeft> mapping) =>
      this._value;

  }

  public class Right<TLeft, TRight> : Either<TLeft, TRight>
  {
    private TRight _value { get; }

    public Right(TRight value)
    {
      _value = value;
    }

    public override Either<TNewLeft, TRight> MapLeft<TNewLeft>(Func<TLeft, TNewLeft> mapping) =>
      new Right<TNewLeft, TRight>(this._value);
    public override Either<TLeft, TNewRight> MapRight<TNewRight>(Func<TRight, TNewRight> mapping) =>
      new Right<TLeft, TNewRight>(mapping(this._value));

    public override TLeft Reduce(Func<TRight, TLeft> mapping) =>
      mapping(this._value);

  }
}
