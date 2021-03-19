using System.Collections.Generic;
using System.Linq;

namespace BYEsoDomainModelKernel.Models
{
  public partial class FuelItem : Item
  {
    private IList<FuelBlendItemPercentage> _BlendedPercentages { get; set; }
    public virtual IEnumerable<FuelBlendItemPercentage> BlendedPercentages
    {
      get
      {
        return _BlendedPercentages.AsEnumerable();
      }
    }

    public virtual bool IsBlendItem
    {
      get
      {
        if (_BlendedPercentages.Count() > 0)
        {
          return true;
        }
        else
        {
          return false;
        }
      }
    }

    protected internal FuelItem()
    {
    }

    //protected internal FuelItem(int itemID, string name, String externalID,
    //  OrganizationalHierarchy owner, ItemCategory category, UnitOfMeasureClass uomc, string description,
    //  RetailStrategy retailStrategy, DeviceGroup deviceGroup,DeviceFeature defaultPriceOverrideDeviceFeature,
    //  TaxabilityType taxability, int? shelfLabelQuantity)
    //  : base(itemID, name, externalID, owner, category, false, uomc, ItemType.Fuel, ItemSoldAs.Fuel, false, description,
    //      retailStrategy,deviceGroup, defaultPriceOverrideDeviceFeature,taxability,shelfLabelQuantity)
    //{
    //  _BlendedPercentages = new List<FuelBlendItemPercentage>();
    //}

    protected internal virtual void AddBlendedPercentages(FuelBlendItemPercentage fuelBlendedPercentage)
    {
      _BlendedPercentages.Add(fuelBlendedPercentage);
    }

    public virtual decimal GetBlendPercentageForTank(FuelTank tank)
    {
      decimal blendPercentage = 1;

      if (IsBlendItem)
      {
        var blendSplit = BlendedPercentages.FirstOrDefault(x => x.FuelItem.Equals(tank.FuelItem));
        blendPercentage = blendSplit != null ? blendSplit.FuelBlendPercentage / 100m : 0;
      }

      return blendPercentage;
    }

    public override bool Equals(object other)
    {
      FuelItem ot = other as FuelItem;
      if (ot == null)
      {
        return false;
      }
      return ot.ID == ID;
    }

    public override int GetHashCode()
    {
      return ID.GetHashCode();
    }
  }
}