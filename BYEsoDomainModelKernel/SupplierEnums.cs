namespace BYEsoDomainModelKernel
{
  public enum SupplierType
  {
    Expense = 'e',
    Merchandise = 'm',
    Fuel = 'f',
    FuelTerminal = 't',
    FuelCarrier = 'c',
    NewsAndMags = 'n',
    Lottery = 'l'
  }
  public enum SupplierBillingMethod
  {
    BillFromInvoice = '0',
    BillFromDelivery = '1'
  }
  public enum SupplierStatusCode
  {
    Active = 'a',
    Inactive = 'i'
  }
  public enum SupplierDomainErrors
  {
    AttemptToGetCostLevelWithNoDefaultCostLevelConfigured,
    AttemptToAddSupplierOverrideConfigurationWithIncorrectSupplier,
    AttemptToSetInvalidDeliveryNumberFormat,
    SupplierCostNotEditable,
    InvalidSupplierForSite,
    SupplierNotFound,
    AccessDeniedToDeleteSupplier,
    OnlyLoggedInSiteCanCreateOrUpdateContact,
    UnSupportedSupplierTypeForAdd,
    ContactNotAssignedToSupplier,
    OnlyOnePrimaryContactAllowedForASupplier,
    SiteCannotCreatePrimaryContactForEnterpriseSupplier,
    AccessDeniedToSupplier,
    InvalidCountryCode,
    InvalidStateCode,
    StateIsNotBelongToGivenCountry,
    CannotDeleteAsSupplierIsUsedInFuelDelivery,
    InvalidEmailAddressHasBeenProvided,
    InvalidPhoneNumberHasBeenProvided
  }
  public enum DeliveryInvoiceCostType
  {
    PurchaseOrder = 'p',
    Catalog = 'c',
    Invoice = 'i',
    AdvancedShipNotice = 'a'
  }
  public enum DeliveryPOCostType
  {
    PurchaseOrder = 'p',
    Catalog = 'c'
  }
  public enum SupplierOrderHolidayRuleType
  {
    PreviousDay = '1',
    NextDay = '3'
  }
  public enum SupplierDeliveryHolidayRuleType
  {
    PreviousDay = '1',
    PreviousScheduleDay = '2',
    NextDay = '3',
    NextScheduleDay = '4'
  }

  public enum OrderFileTypeCode
  {
    Normal = 'n',
    Detailed = 'e',
    Distributed = 'd',
    Collated = 'c'
  }
}
