using System.Collections.Generic;
using System.Linq;

namespace BYEsoDomainModelKernel.Models
{
  public partial class Item : EffectiveDateCDMEntity
  {
    private const int MAXNAMEANDEXTERNALIDTEXTLENGTH = 50;
    private const int MAXITEMUOMCONVERSIONS = 2;
    private const int MAXSHELFLABELQUANTITY = 999999;
    public virtual int ID { get; set; }
    public virtual string Name { get; set; }
    public virtual string Description { get; set; }
    public virtual bool IsTracked { get; set; }
    public virtual bool IsRecalled { get; set; }
    public virtual bool IsPurged { get; set; }
    public virtual string ExternalID { get; set; }
    public virtual int? ShelfLabelQuantity { get; set; }

    public virtual bool IsRecipe { get; protected set; }

    public virtual UnitOfMeasureClass UnitOfMeasureClass { get; set; }
    //public virtual IList<ItemReviewStepItemOrgHierarchy> ItemReviewStepItemOrgHierarchyList { get; set; }

    public virtual ItemCategory Category { get; set; }
    //public virtual ItemCategory ValuationCategory
    //{
    //  get
    //  {
    //    return Category.ValuationCategory;
    //  }
    //}

    //private IList<SalesInfo> _SalesInfo { get; set; }
    //public virtual SalesInfo SalesInfo
    //{
    //  get
    //  {
    //    return _SalesInfo.FirstOrDefault();
    //  }
    //}

    //public virtual IList<ItemDataAccessorAssignment> ItemDataAccessorAssignments { get; set; }

    //public virtual IList<ItemLanguageTranslation> ItemLanguageTranslations { get; set; }

    //public virtual ItemType Type { get; set; }

    //private IList<ItemUnitOfMeasureConversion> _UnitOfMeasureConversions { get; set; }

    //public virtual bool HasUOMConversions { get { return _UnitOfMeasureConversions.Count > 0; } }

    //public virtual IEnumerable<ItemUnitOfMeasureConversion> UnitOfMeasureConversions
    //{
    //  get
    //  {
    //    return _UnitOfMeasureConversions.AsEnumerable();
    //  }
    //}

    //public virtual void SetIsTracked(bool isTracked)
    //{
    //  IsTracked = isTracked;
    //}

    //protected internal Item()
    //{
    //  InitializeCollections();
    //}

    //private Item(int id, String name, String externalID, OrganizationalHierarchy owner, ItemCategory category,
    //  bool isTracked, UnitOfMeasureClass uomc, ItemType type, bool isRecalled, string description, int? shelfLabelQuantity)
    //  : base(owner)
    //{
    //  ID = id;
    //  Name = name?.Trim();
    //  ExternalID = externalID?.Trim();
    //  Category = category;
    //  IsTracked = isTracked;
    //  UnitOfMeasureClass = uomc;
    //  Type = type;
    //  IsRecipe = false;
    //  IsRecalled = isRecalled;
    //  Description = description;
    //  ShelfLabelQuantity = shelfLabelQuantity;
    //  InitializeCollections();
    //}

    //protected internal Item(int id, String name, String externalID, OrganizationalHierarchy owner, ItemCategory category,
    //  bool isTracked, UnitOfMeasureClass uomc, ItemType type, ItemSoldAs? soldAs, bool isRecalled, string description,
    //  RetailStrategy retailStrategy, DeviceGroup deviceGroup, DeviceFeature defaultPriceOverrideDeviceFeature,
    //  TaxabilityType taxability, int? shelfLabelQuantity)
    //  : this(id, name, externalID, owner, category, isTracked, uomc, type, isRecalled, description, shelfLabelQuantity)
    //{
    //  if (soldAs != null)
    //  {
    //    bool isGenericCondiment = soldAs == ItemSoldAs.Extra;
    //    var salesInfo = new SalesInfo(id, name, soldAs.Value, isGenericCondiment, retailStrategy, deviceGroup,
    //      defaultPriceOverrideDeviceFeature, taxability);
    //    _SalesInfo.Add(salesInfo);
    //  }
    //}

    //protected internal Item(int id, String name, String externalID, OrganizationalHierarchy owner, ItemCategory category,
    //  UnitOfMeasureClass uomc, ItemType type, ItemSoldAs? soldAs, decimal maxCashWinner, decimal maxMoneyOrderWinner,
    //  RetailStrategy retailStrategy, DeviceGroup deviceGroup, DeviceFeature defaultPriceOverrideDeviceFeature,
    //  TaxabilityType taxability, int? shelfLabelQuantity)
    //  : this(id, name, externalID, owner, category, false, uomc, type, false, "", shelfLabelQuantity)
    //{
    //  if (soldAs != null)
    //  {
    //    var salesInfo = new SalesInfo(id, name, soldAs.Value, false, maxCashWinner, maxMoneyOrderWinner, retailStrategy,
    //      deviceGroup, defaultPriceOverrideDeviceFeature, taxability);
    //    _SalesInfo.Add(salesInfo);
    //  }
    //}

    //private void InitializeCollections()
    //{
    //  _SalesInfo = new List<SalesInfo>();
    //  _UnitOfMeasureConversions = new List<ItemUnitOfMeasureConversion>();
    //  ItemDataAccessorAssignments = new List<ItemDataAccessorAssignment>();
    //  ItemLanguageTranslations = new List<ItemLanguageTranslation>();
    //  ItemReviewStepItemOrgHierarchyList = new List<ItemReviewStepItemOrgHierarchy>();
    //}

    //public virtual void SetIsPurgedFromTest(IUnitOfWork uow, bool isPurged)
    //{
    //  uow.EnforceRunningInTest("Can't use this function except in tests");

    //  IsPurged = isPurged;
    //}

    //public virtual decimal? ConvertQuantityToAtomic(UnitOfMeasure uom, decimal? quantity)
    //{
    //  decimal? convertedQuantity = quantity;

    //  bool quantityAlreadyAtomic = uom.Equals(UnitOfMeasureClass.BaseUnitOfMeasure);

    //  if (convertedQuantity != null && !quantityAlreadyAtomic)
    //  {
    //    var crossClassConversionFactor = GetUnitOfMeasureConversionFactor(uom.UnitOfMeasureClass);
    //    var uomFactor = uom.Factor;

    //    convertedQuantity = quantity * (crossClassConversionFactor * uomFactor);
    //  }

    //  return convertedQuantity;
    //}

    //public virtual decimal? ConvertCostToAtomic(UnitOfMeasure uom, decimal? cost)
    //{
    //  decimal? convertedAmount = cost;

    //  bool amountAlreadyAtomic = uom.Equals(UnitOfMeasureClass.BaseUnitOfMeasure);
    //  if (convertedAmount != null && !amountAlreadyAtomic)
    //  {
    //    var crossClassConversionFactor = GetUnitOfMeasureConversionFactor(uom.UnitOfMeasureClass);
    //    var uomFactor = uom.Factor;

    //    convertedAmount = cost / (crossClassConversionFactor * uomFactor);
    //  }

    //  return convertedAmount;
    //}

    //public virtual decimal? ConvertCostFromAtomic(UnitOfMeasure uom, decimal? cost)
    //{
    //  decimal? convertedCost = cost;

    //  bool requestedUOMIsAtomic = uom.Equals(UnitOfMeasureClass.BaseUnitOfMeasure);
    //  if (convertedCost != null && !requestedUOMIsAtomic)
    //  {
    //    var crossClassConversionFactor = GetUnitOfMeasureConversionFactor(uom.UnitOfMeasureClass);
    //    var uomFactor = uom.Factor;

    //    convertedCost = cost * (crossClassConversionFactor * uomFactor);
    //  }

    //  return convertedCost;
    //}

    //public virtual decimal? ConvertCostBetweenUOMs(UnitOfMeasure sourceUOM, UnitOfMeasure destinationUOM, decimal? cost)
    //{
    //  decimal? convertedCost = cost;

    //  if (convertedCost != null)
    //  {
    //    decimal? atomicCost = ConvertCostToAtomic(sourceUOM, cost);
    //    convertedCost = ConvertCostFromAtomic(destinationUOM, atomicCost);
    //  }

    //  return convertedCost;
    //}

    //public virtual decimal? ConvertQuantityFromAtomic(UnitOfMeasure uom, decimal? quantity)
    //{
    //  decimal? convertedQuantity = quantity;

    //  bool requestedUOMIsAtomic = uom.Equals(UnitOfMeasureClass.BaseUnitOfMeasure);
    //  if (convertedQuantity != null && !requestedUOMIsAtomic)
    //  {
    //    var crossClassConversionFactor = GetUnitOfMeasureConversionFactor(uom.UnitOfMeasureClass);
    //    var uomFactor = uom.Factor;

    //    convertedQuantity = quantity / (crossClassConversionFactor * uomFactor);
    //  }

    //  return convertedQuantity;
    //}

    //public virtual decimal? ConvertQuantityBetweenUOMs(UnitOfMeasure sourceUOM, UnitOfMeasure destinationUOM, decimal? quantity)
    //{
    //  decimal? convertedQuantity = quantity;

    //  if (convertedQuantity != null)
    //  {
    //    decimal? atomicQuantity = ConvertQuantityToAtomic(sourceUOM, quantity);
    //    convertedQuantity = ConvertQuantityFromAtomic(destinationUOM, atomicQuantity);
    //  }

    //  return convertedQuantity;
    //}

    //public virtual decimal GetUnitOfMeasureConversionFactor(UnitOfMeasureClass uomc)
    //{
    //  decimal conversionFactor = 1;

    //  var conversions = this._UnitOfMeasureConversions.FirstOrDefault(c => c.UnitOfMeasureClass.Equals(uomc));

    //  if (conversions!=null)
    //  {
    //    conversionFactor = conversions.ConversionFactor;
    //  }

    //  return conversionFactor;
    //}

    //public virtual void SetIsPurged(bool isPurged)
    //{
    //  IsPurged = isPurged;
    //}

    //public virtual void SetExternalId(string externalId)
    //{
    //  ExternalID = externalId;
    //}

    //public virtual ItemUnitOfMeasureConversion AddUnitOfMeasureConversion(UnitOfMeasureClass uomClass,
    //  decimal conversionFactor)
    //{
    //  if (uomClass.Equals(UnitOfMeasureClass))
    //  {
    //    throw new DomainOperationException<ItemDomainErrors>(ItemDomainErrors.ConversionToBaseClass);
    //  }

    //  var conversion = new ItemUnitOfMeasureConversion(this, uomClass, conversionFactor);
    //  this._UnitOfMeasureConversions.Add(conversion);
    //  return conversion;
    //}

    //public virtual bool IsParticipatingInPromotionalPriceChange(Site site)
    //{
    //  if (SalesInfo == null)
    //  {
    //    return false;
    //  }
    //  else
    //  {
    //    return SalesInfo.IsParticipatingInPromotionalPriceChange(site);
    //  }
    //}

    //public virtual ItemDepletionItemAssignment AddItemDepletion(InventoryItem depletedItem,
    //  UnitOfMeasure unitOfMeasure)
    //{
    //  if (SalesInfo == null)
    //  {
    //    ThrowDomainError(ItemDomainErrors.AttemptToAddSalesItemDepletionForNonSoldItem);
    //  }

    //  return SalesInfo.AddDepletionItemAssignments(this, depletedItem, unitOfMeasure);
    //}

    //public virtual ItemModifierGroup AddItemModifierGroup(ModifierGroup modifierGroup)
    //{
    //  if (SalesInfo == null)
    //  {
    //    ThrowDomainError(ItemDomainErrors.AttemptToAddSalesItemModifierGroupForNonSoldItem);
    //  }

    //  return SalesInfo.AddItemModifierGroup(modifierGroup);
    //}

    //public virtual void CheckItemOnHandQuantity(IUnitOfWork uow, Site siteID)
    //{
    //  var onHandService = new CurrentSiteItemOnHandService(uow);
    //  var localSiteDateTime = SiteLocalTimeService.GetSiteLocalTime(uow, siteID);
    //  var onHandInfo = onHandService.GetCurrentSiteItemOnHandBySiteDateAndItems(siteID.ID,
    //    localSiteDateTime, new[] { ID });
    //  if (onHandInfo != null && onHandInfo.Count > 0)
    //  {
    //    if (onHandInfo.ElementAt(0).CurrentAtomicQuantity != 0)
    //    {
    //      throw new DomainOperationException<ItemDomainErrors>(ItemDomainErrors.
    //        AttemptToRemoveItemHavingNonZeroOnHandQuantity);
    //    }
    //  }
    //}

    //public virtual IEnumerable<SalesItem> AddSalesItems(RetailStrategy retailStrategy, IEnumerable<BaseRetailModifier> firstLevelModifiers,
    //  IEnumerable<BaseRetailModifier> secondLevelModifiers, IEnumerable<BaseRetailModifier> thirdLevelModifiers,
    //  string externalID, UnitOfMeasure uom, PriceEventLookupService priceEventLookupService)
    //{
    //  if (SalesInfo == null)
    //  {
    //    ThrowDomainError(ItemDomainErrors.AttemptToAddSalesItemsForNonSoldItem);
    //  }
    //  var initialPriceEvent = priceEventLookupService.GetInitialPriceEvent();
    //  return SalesInfo.AddSalesItems(this, retailStrategy, firstLevelModifiers, secondLevelModifiers, thirdLevelModifiers, externalID, uom,
    //      initialPriceEvent);
    //}

    //public virtual decimal? GetAtomicRetailValuationForSite(Site site)
    //{
    //  if (SalesInfo == null)
    //  {
    //    return null;
    //  }

    //  var valuationPoint = SalesInfo.GetSalesItems().FirstOrDefault(x => x.IsRetailValuationPoint);

    //  if (valuationPoint == null)
    //  {
    //    return null;
    //  }

    //  var retailValuationInTermsOfValuationPoint = valuationPoint.GetRetailForSite(site);
    //  return ConvertCostToAtomic(valuationPoint.UnitOfMeasure, retailValuationInTermsOfValuationPoint);
    //}

    //protected internal virtual bool MatchesSalesItemBarcode(string barcodeFragment, StringMatchType matchType)
    //{
    //  if (SalesInfo != null)
    //  {
    //    return SalesInfo.MatchesBarcode(barcodeFragment, matchType);
    //  }

    //  return false;
    //}

    //public virtual ValuationMethod GetValuationMethod(Site site, ClientSettings clientSettings)
    //{
    //  return Category.GetValuationMethod(site, clientSettings).GetValueOrDefault(ValuationMethod.ItemLastActivityCost);
    //}

    //public virtual void ApplyItemEdits(Site site, string name, ItemCategory category, ItemSoldAs? soldAs,
    //  UnitOfMeasureClass uomc, string externalID, int? shelfLabelQuantity, TaxabilityType taxability, 
    //  [Optional]IEnumerable<RecipeTrackingUom> recipeTrackingUoms)
    //{
    //  if (site.OrganizationalHierarchy.ID == Owner.ID)
    //  {
    //    IsTracked = (recipeTrackingUoms != null && recipeTrackingUoms.Any()) ? true : false;
    //    CheckDomainExceptions(name, category, externalID, shelfLabelQuantity);
    //    Name = name?.Trim();
    //    Category = category;
    //    ExternalID = externalID?.Trim();
    //    ShelfLabelQuantity = shelfLabelQuantity;
    //  }
    //  else
    //  {
    //    ThrowDomainError(ItemDomainErrors.AttemptToEditNonBuCreatedItem);
    //  }

    //  SalesInfo?.ApplySalesInfoEdits(site, Owner, taxability);
    //}

    //public virtual void ApplyItemUomConversionEdits(Site site, List<ItemUnitOfMeasureConversion> itemUnitOfMeasureConversions)
    //{
    //  foreach (var uomConversion in itemUnitOfMeasureConversions)
    //  {
    //    var itemUomConversion = _UnitOfMeasureConversions.FirstOrDefault(x => x.Item.ID == uomConversion.Item.ID &&
    //                                           x.UnitOfMeasureClass.ID == uomConversion.UnitOfMeasureClass.ID);
    //    if (itemUomConversion != null)
    //    {
    //      itemUomConversion.ApplyItemUomConversionEdits(uomConversion.ConversionFactor, uomConversion.FromUomId, uomConversion.ToUomId,
    //        uomConversion.FromDisplayQuantity, uomConversion.ToDisplayQuantity);
    //    }
    //    else if(_UnitOfMeasureConversions.Count != MAXITEMUOMCONVERSIONS)
    //    {
    //      _UnitOfMeasureConversions.Add(uomConversion);
    //    }
    //  }
    //}

    //private void CheckDomainExceptions(string name, ItemCategory category, string externalID, int? shelfLabelQuantity)
    //{
    //    ValidateItem(name, externalID, category, shelfLabelQuantity);
    //}

    //private void ValidateItem(string name, string externalID, ItemCategory itemCategory, int? shelfLabelQuantity)
    //{
    //  if (string.IsNullOrEmpty(name) || name.Trim().Length > MAXNAMEANDEXTERNALIDTEXTLENGTH)
    //  {
    //    ThrowDomainError(ItemDomainErrors.AttemptToSetInvalidItemName);
    //  }
    //  if (!string.IsNullOrEmpty(externalID) && externalID.Trim().Length > MAXNAMEANDEXTERNALIDTEXTLENGTH)
    //  {
    //    ThrowDomainError(ItemDomainErrors.AttemptToSetInvalidItemExternalID);
    //  }
    //  if (shelfLabelQuantity != null && !(shelfLabelQuantity >= 0 && shelfLabelQuantity <= MAXSHELFLABELQUANTITY))
    //  {
    //    ThrowDomainError(ItemDomainErrors.AttemptToSetInvalidItemShelfLabelQuantity);
    //  }
    //  if (!DomainModelHelper.ValidateEntityExists(itemCategory))
    //  {
    //    ThrowDomainError(ItemDomainErrors.ItemCategoryNotFound);
    //  }
    //}

    //public override IEnumerable<EffectiveDatedDataAccessorAssignmentEntity> DataAccessorAssignments
    //{
    //  get
    //  {
    //    return ItemDataAccessorAssignments.Cast<EffectiveDatedDataAccessorAssignmentEntity>().ToList();
    //  }
    //}

    //public virtual void AddDataAccessorAssignment(DataAccessor dataAccessor, DateTime start, DateTime end)
    //{
    //  ItemDataAccessorAssignments.Add(new ItemDataAccessorAssignment(this, dataAccessor, start, end));
    //}

    //public virtual void RemoveDataAccessorAssignment(IList<ItemDataAccessorAssignment> itemDataAccessorAssignments)
    //{
    //  foreach (var itemDataAccessorAssignment in itemDataAccessorAssignments.ToList())
    //  {
    //    ItemDataAccessorAssignments.Remove(itemDataAccessorAssignment);
    //  }
    //}

    //public virtual void AddItemReviewStepOrgHierarchy(OrganizationalHierarchy orgHierarchy, ItemReviewStep itemReviewStep)
    //{
    //  var itemReviewStepOrgHierarchy = new ItemReviewStepItemOrgHierarchy(orgHierarchy, this, itemReviewStep);
    //  ItemReviewStepItemOrgHierarchyList.Add(itemReviewStepOrgHierarchy);
    //}

    //public virtual void RemoveItemReviewStepOrgHierarchy(OrganizationalHierarchy orgHierarchy, 
    //  ItemReviewStep itemReviewStep)
    //{
    //  var itemReviewStepOrgHierarchy = new ItemReviewStepItemOrgHierarchy(orgHierarchy, this, itemReviewStep);
    //  ItemReviewStepItemOrgHierarchyList.Remove(itemReviewStepOrgHierarchy);
    //}
    //public virtual void RemoveSalesInfo(SalesInfo saleInfo)
    //{
    //  _SalesInfo.Remove(saleInfo);
    //}

    //public virtual void RemoveSalesItem(SalesItem salesItem)
    //{
    //  if (SalesInfo == null)
    //  {
    //    ThrowDomainError(ItemDomainErrors.AttemptToDeleteSalesItemForNonSoldItem);
    //  }
    //  var salesInfo = _SalesInfo.FirstOrDefault();
    //  salesInfo?.RemoveSalesItem(salesItem);
    //}

    //public static void ThrowDomainError(ItemDomainErrors error)
    //{
    //  throw new DomainOperationException<ItemDomainErrors>(error);
    //}

    //public override bool Equals(object other)
    //{
    //  Item ot = other as Item;
    //  return ot?.ID == ID;
    //}

    //public override int GetHashCode()
    //{
    //  return ID.GetHashCode();
    //}

    //public virtual Item self
    //{
    //  get
    //  {
    //    return this;
    //  }
    //}

    public virtual IList<ItemDataAccessorAssignment> ItemDataAccessorAssignments { get; set; }
    public override IEnumerable<EffectiveDatedDataAccessorAssignmentEntity> DataAccessorAssignments => ItemDataAccessorAssignments.Cast<EffectiveDatedDataAccessorAssignmentEntity>().ToList();
  }

  public enum ItemDomainErrors
  { 
    ItemNotFound,
    RetailItemNotFound,
    RetailModifiedItemNotFound,
    ConversionToBaseClass,
    AttemptToSetDefaultUOMToUOMNotTrackedAtSite,
    AttemptToSetSortOrderOnInvalidTrackingUOM,
    AttemptToRemoveTrackingUOMNotOwned,
    AttemptToAddPurgedIngredient,
    AttemptToAddSalesItemsForNonSoldItem,
    AttemptToAddSalesItemModifierGroupForNonSoldItem,
    AttemptToAddSalesItemDepletionForNonSoldItem,
    AttemptToDeleteSalesItemForNonSoldItem,
    AttemptToEditNonBuCreatedItem,
    AttemptToSetInvalidItemName,
    AttemptToSetInvalidItemExternalID,
    AttemptToSetInvalidItemShelfLabelQuantity,
    AttemptToRemoveAssignedSupplierItem,
    NoSupplierItemsToRemove,
    AttemptToRemoveSupplierItemFromPurgedItem,
    AttemptToSetInvalidItemCategory,
    NoSiteFound,
    ItemNotAssignedToSite,
    AttemptToAddUomNotConfiguredForItem,
    ItemCategoryNotFound,
    AttemptToRemoveItemHavingNonZeroOnHandQuantity,
    InvalidParameterSearchWith,
    InvalidParameterFilterWith,
    InvalidItemId,
    NotAuthorized,
    AttemptToUpdateNonEnterpriseItem,
    ItemIsPurged
  }

  public enum ItemType
  {
    Fuel = 'f',
    InventoryItem = 'i',
    RecipeItem = 'r',
    MoneyOrder = 'o',
    InstantLotteryTicket = 't',
    CarWash = 'w',
    MachineLotteryTicket = 'x'
  }

  public enum ItemSoldAs
  {
    NotSold = 'n',
    GeneralMerchandise = 'g',
    Prepared = 'i',
    Fuel = 'f',
    Pizza = '0',
    CustomPizza = '1',
    SpecialtyPizza = '2',
    UNKNOWN1 = '3',
    GiftCardFee = 'a',
    ComboItem = 'c',
    Condiment = 'd',
    Extra = 'z',
    GiftCertificate = 'e',
    PhoneCard = 'j',
    PhoneCardRecharge = 'k',
    MiscItem = 'm',
    MoneyOrder = 'o',
    GiftCardRefill = 'r',
    GiftCard = 's',
    InstantLotteryTicket = 't',
    CarWash = 'w',
    MachineLotteryTicket = 'x'
  }
}