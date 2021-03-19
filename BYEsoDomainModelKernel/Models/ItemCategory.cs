using System.Collections.Generic;
using System.Linq;


namespace BYEsoDomainModelKernel.Models
{
  public partial class ItemCategory : BaseAuditEntity
  {
    public virtual int ID { get; set; }
    public virtual string Name { get; set; }
    public virtual string Description { get; set; }

    public virtual ItemCategory Parent { get; set; }
    //public virtual ItemCategoryLevel Level { get; set; }

    private IList<ItemCategory> _Children { get; set; }

    public virtual bool IsReclamationItemCategory { get; set; }
    public virtual bool UseInvoiceCostOnDeliveriesItemCategory { get; set; }
    //public virtual ValuationMethod? DefaultValuationMethod { get; set; }
    public virtual bool IsFuelItemCategory { get; set; }
    public virtual bool IsPurged { get; set; }
    public virtual int OwnerID { get; set; }
    public virtual bool RestrictBUEditFlag { get; set; }
    

    //private IList<ItemCategorySiteOverride> _ItemCategorySiteOverrides { get; set; }

    public virtual IEnumerable<ItemCategory> Children
    {
      get
      {
        return _Children.Where(x => !x.IsPurged).AsEnumerable();
      }
    }

    //public virtual ItemCategory ValuationCategory
    //{
    //  get
    //  {
    //    var cat = this;

    //    while (cat != null && !cat.Level.IsValuationLevel)
    //    {
    //      cat = cat.Parent;
    //    }

    //    return cat;
    //  }
    //}

    protected internal ItemCategory()
    {
      InitializeCollections();
    }

    //protected internal ItemCategory (string name, ItemCategory parent, ItemCategoryLevel level,
    //  bool? isReclamationItemCategory, ValuationMethod? defaultValuationMethod, bool? mustUseInvoiceCostOnDeliveriesItemCategory, 
    //  bool? isFuelItemCategory, string description)
    //{
    //  Name = name;
    //  Parent = parent;
    //  Level = level;
    //  IsReclamationItemCategory = isReclamationItemCategory.GetValueOrDefault(false);
    //  UseInvoiceCostOnDeliveriesItemCategory = mustUseInvoiceCostOnDeliveriesItemCategory.GetValueOrDefault(false);
    //  if (level.IsValuationLevel)
    //  {
    //    DefaultValuationMethod = defaultValuationMethod;
    //  }
    //  IsFuelItemCategory = isFuelItemCategory.GetValueOrDefault(false);

    //  Description = description;

    //  InitializeCollections();
    //}

    private void InitializeCollections()
    {
      //_ItemCategorySiteOverrides = new List<ItemCategorySiteOverride>();
    }

    //public virtual IEnumerable<ItemCategory> GetLeafLevelCategories(int maxDepth)
    //{
    //  if (IsPurged)
    //  {
    //    return new ItemCategory[]{};
    //  }

    //  if (Level.Depth == maxDepth)
    //  {
    //    return new ItemCategory[] { this };
    //  }

    //  if (!Children.Any())
    //  {
    //    return new ItemCategory[]{};
    //  }

    //  return Children.SelectMany(x => x.GetLeafLevelCategories(maxDepth));
    //}

    //public virtual ValuationMethod? GetValuationMethod(Site site, ClientSettings clientSettings)
    //{
    //  if (ValuationCategory == null)
    //  {
    //    return null;
    //  }

    //  if (Equals(ValuationCategory, this))
    //  { 
    //    var itemCategorySiteOverride = _ItemCategorySiteOverrides.FirstOrDefault(x => x.Site.ID == site.ID);
    //    if (itemCategorySiteOverride != null && itemCategorySiteOverride.ValuationMethod.HasValue)
    //    {
    //      return itemCategorySiteOverride.ValuationMethod;
    //    }

    //    return DefaultValuationMethod ?? clientSettings.DefaultValuationMethod;
    //  }

    //  return ValuationCategory.GetValuationMethod(site, clientSettings);
    //}

    //public virtual void AddSiteOverride(ItemCategorySiteOverride siteOverride)
    //{
    //  if (Level.IsValuationLevel)
    //  {
    //    _ItemCategorySiteOverrides.Add(siteOverride);
    //  }
    //}

    //public virtual void SetIsPurgedFromTest(IUnitOfWork uow, bool isPurged)
    //{
    //  uow.EnforceRunningInTest("SetIsPurgedFromTest(): Can't use this function except in tests");

    //  IsPurged = isPurged;
    //}

    public override bool Equals(object other)
    {
      var ot = other as ItemCategory;
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