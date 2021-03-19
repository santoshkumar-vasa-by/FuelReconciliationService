using System;

namespace BYEsoDomainModelKernel.Models
{
  public class ItemDataAccessorAssignment : EffectiveDatedDataAccessorAssignmentEntity
  {
    public virtual Item Item { get; set; }

    protected internal ItemDataAccessorAssignment()
    {
    }

    protected internal ItemDataAccessorAssignment(Item item, DataAccessor dataAccessor, DateTime start, DateTime end)
      : base(dataAccessor, start, end)
    {
      Item = item;
    }

    public override bool Equals(object other)
    {
      var ot = other as ItemDataAccessorAssignment;
      if (ot == null)
      {
        return false;
      }
      return (GetHashCode() == ot.GetHashCode());
    }

    public override int GetHashCode()
    {
      return Item.ID.GetHashCode() ^ DataAccessor.ID.GetHashCode();
    }
  }
}
