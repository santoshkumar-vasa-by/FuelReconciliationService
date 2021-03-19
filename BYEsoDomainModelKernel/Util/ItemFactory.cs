namespace BYEsoDomainModelKernel.Util
{
  public static class ItemFactory
  {
    //public static Item CreateItem(TicketingService ticketService, string name, String externalID, OrganizationalHierarchy owner,
    //  ItemCategory category, bool isTracked, UnitOfMeasureClass uomc, BYEsoDomainModelKernel.Models.ItemType type, BYEsoDomainModelKernel.Models.ItemSoldAs? soldAs, bool isRecalled,
    //  string description, RetailStrategy retailStrategy, DeviceGroup deviceGroup,DeviceFeature defaultPriceOverrideDeviceFeature,
    //  TaxabilityType taxability, int? shelfLabelQuantity)
    //{
    //  var itemID = (int)ticketService.GenerateTicket(typeof(Item));

    //  var newItem = new Item(itemID, name, externalID, owner, category, isTracked, uomc, type, soldAs, isRecalled, description,
    //    retailStrategy, deviceGroup, defaultPriceOverrideDeviceFeature,taxability,shelfLabelQuantity);

    //  return newItem;
    //}

    //public static Item CreateRecipeItem(TicketingService ticketService, string name, String externalID,
    //  OrganizationalHierarchy owner, ItemCategory category, bool isActive, bool isTracked, decimal? targetWasteRatio,
    //  UnitOfMeasureClass uomc, bool reduceOrderPeriod, decimal? defaultMinVarianceRatio, decimal? defaultMaxVarianceRatio,
    //  int? maxInventoryQty, SiteGroup producingSiteGroup, decimal batchQuantity, UnitOfMeasure batchUOM,
    //  decimal servingsPerBatch, decimal servingQuantity, UnitOfMeasure servingUOM, string description,
    //  RetailStrategy retailStrategy, DeviceGroup deviceGroup,DeviceFeature defaultPriceOverrideDeviceFeature,
    //  TaxabilityType taxability, int? shelfLabelQuantity)
    //{
    //  var itemID = (int)ticketService.GenerateTicket(typeof(Item));

    //  var recipeItem = new RecipeItem(itemID, name, externalID, owner, category, isActive, isTracked, targetWasteRatio,
    //    uomc, reduceOrderPeriod, defaultMinVarianceRatio, defaultMaxVarianceRatio, maxInventoryQty, producingSiteGroup,
    //    batchQuantity, batchUOM, servingsPerBatch,servingQuantity, servingUOM, description, retailStrategy, deviceGroup,
    //    defaultPriceOverrideDeviceFeature,taxability,shelfLabelQuantity);

    //  return recipeItem;
    //}

    //public static Item CreateInventoryItem(TicketingService ticketService, string name, string externalID,
    //  OrganizationalHierarchy owner, ItemCategory category, bool isActive, bool isTracked, decimal? targetWasteRatio,
    //  UnitOfMeasureClass uomc, bool reduceOrderPeriod, bool allowFractionalQuantityForDeliveryAndTransfers,
    //  decimal? defaultMinVarianceRatio, decimal? defaultMaxVarianceRatio, int? maxInventoryQty, BYEsoDomainModelKernel.Models.ItemSoldAs? soldAs,
    //  bool recallFlag, ItemReclamationType? reclamationType, ItemInvoiceCostOnDeliveriesType? invoiceCostOnDeliveriesType,
    //  string description, RetailStrategy retailStrategy, DeviceGroup defaultDeviceGroup, DeviceFeature defaultPriceOverrideDeviceFeature,
    //  TaxabilityType taxability, int? shelfLabelQuantity)
    //{
    //  var itemID = (int)ticketService.GenerateTicket(typeof(Item));

    //  var newItem = new InventoryItem(itemID, name, externalID, owner, category, isActive, isTracked, targetWasteRatio,
    //    uomc, reduceOrderPeriod, allowFractionalQuantityForDeliveryAndTransfers, defaultMinVarianceRatio,
    //    defaultMaxVarianceRatio, maxInventoryQty, soldAs, recallFlag, reclamationType, invoiceCostOnDeliveriesType,
    //    description, retailStrategy, defaultDeviceGroup, defaultPriceOverrideDeviceFeature, taxability, shelfLabelQuantity);

    //  return newItem;
    //}

    //public static Item CreateFuelItem(TicketingService ticketService, string name, String externalID,
    //  OrganizationalHierarchy owner, ItemCategory category, bool isActive, UnitOfMeasureClass uomc,
    //  string description, RetailStrategy retailStrategy, DeviceGroup deviceGroup,DeviceFeature defaultPriceOverrideDeviceFeature,
    //  TaxabilityType taxability, int? shelfLabelQuantity)
    //{
    //  var itemID = (int)ticketService.GenerateTicket(typeof(Item));

    //  var fuelItem = new FuelItem(itemID, name, externalID, owner, category, uomc, description, retailStrategy, deviceGroup, 
    //    defaultPriceOverrideDeviceFeature,taxability,shelfLabelQuantity);

    //  return fuelItem;
    //}

    //public static Item CreateBlendedFuelItem(TicketingService ticketService, string name, String externalID,
    //  OrganizationalHierarchy owner, ItemCategory category, bool isActive, UnitOfMeasureClass uomc,
    //  FuelItem blendItem1, FuelItem blendItem2, decimal blendPercentage, string description,
    //  RetailStrategy retailStrategy, DeviceGroup deviceGroup,DeviceFeature defaultPriceOverrideDeviceFeature,
    //  TaxabilityType taxability, int? shelfLabelQuantity)
    //{
    //  var itemID = (int)ticketService.GenerateTicket(typeof(Item));
    //  var percentageForBlendItem2 = 100 - blendPercentage;

    //  var fuelItem = new FuelItem(itemID, name, externalID, owner, category, uomc, description, retailStrategy, deviceGroup,
    //    defaultPriceOverrideDeviceFeature,taxability,shelfLabelQuantity);

    //  fuelItem.AddBlendedPercentages(new FuelBlendItemPercentage(fuelItem, blendItem1, blendPercentage));
    //  fuelItem.AddBlendedPercentages(new FuelBlendItemPercentage(fuelItem, blendItem2, percentageForBlendItem2));

    //  return fuelItem;
    //}

    //public static Item CreateLotteryItem(TicketingService ticketService, String name, String externalID,
    //  OrganizationalHierarchy owner, ItemCategory category, BYEsoDomainModelKernel.Models.ItemType itemType, BYEsoDomainModelKernel.Models.ItemSoldAs? soldAs,
    //  bool isActive, UnitOfMeasureClass uomc, decimal maxCashWinner, decimal maxMoneyOrderWinner,
    //  RetailStrategy retailStrategy, DeviceGroup deviceGroup,DeviceFeature defaultPriceOverrideDeviceFeature,
    //  TaxabilityType taxability, int? shelfLabelQuantity)
    //{
    //  var itemID = (int)ticketService.GenerateTicket(typeof(Item));

    //  var lotteryItem = new LotteryItem(itemID, name, externalID, owner, category,itemType, soldAs, isActive, uomc,
    //    maxCashWinner, maxMoneyOrderWinner,retailStrategy, deviceGroup, defaultPriceOverrideDeviceFeature,
    //    taxability,shelfLabelQuantity);

    //  return lotteryItem;
    //}

    //public static FuelInventoryItem CreateFuelInventoryItem(Item item)
    //{
    //  var fuelInventoryItem = new FuelInventoryItem(item, null);
    //  return fuelInventoryItem;
    //}
  }
}
