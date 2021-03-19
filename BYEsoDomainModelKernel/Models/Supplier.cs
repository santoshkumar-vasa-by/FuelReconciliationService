using System;
using System.Collections.Generic;
using System.Linq;

namespace BYEsoDomainModelKernel.Models
{
  public partial class Supplier //: EffectiveDateCDMEntity
  {
    public virtual int ID { get; set; }
    public virtual string Name { get; set; }
    public virtual string Description { get; set; }
    public virtual bool UsesDefaultForecastConfiguration { get; set; }
    //public virtual ForecastGroup OrderingForecastGroup { get; set; }
    public virtual decimal? SuggestedOrderQtyRoundingThreshold { get; set; }
    public virtual decimal? DeliveryCostToleranceRatio { get; set; }
    public virtual SupplierType Type { get; set; }
    //public virtual SupplierParameters SupplierParameters { get; set; }
    //public virtual SupplierTerms SupplierTerms { get; set; }
    public virtual SupplierBillingMethod BillingMethod { get; set; }
    public virtual string EDINumber { get; set; }
    public virtual string ExternalId { get; set; }
    public virtual string CustomerNumber { get; set; }
    public virtual string VendorApCode { get; set; }
    public virtual SupplierStatusCode StatusCode { get; set; }
    public virtual bool AllowTotalsAcceptReceiving { get; set; }
    public virtual bool AllowBlindReceiving { get; set; }
    //public virtual IList<SupplierSiteSchedule> SupplierSiteSchedule { get; set; }
    //public virtual IList<SupplierSchedule> SupplierSchedule { get; set; }
    //public virtual IList<SupplierScheduleGroup> SupplierScheduleGroup { get; set; }
    public virtual bool AllowMultipleReceiving { get; set; }
    public virtual bool AllowTotalsOnlyReceiving { get; set; }
    //public virtual SupplierItem TotalsOnlySupplierItem { get; set; }
    public virtual bool UseLowestCost { get; set; }
    public virtual DeliveryInvoiceCostType DeliveryUsingInvoiceCostType { get; set; }
    public virtual DeliveryPOCostType DeliveryUsingPOCostType { get; set; }
    public virtual ExportType? ExportType { get; set; }
    public virtual Address Address { get; set; }
    public virtual string DeliveryNumberFormat { get; set; }
    public virtual bool DeliveryInvoiceDateRequiredFlag { get; set; }
    public virtual bool DeliveryInvoiceRefNumberRequiredFlag { get; set; }
    public virtual string DeliveryInvoiceRefNumberInvalidChars { get; set; }
    public virtual string TermsAndConditions { get; set; }
    public virtual bool ReceiveCheckNumberForCODFlag { get; set; }
    public virtual SupplierOrderHolidayRuleType OrderHolidayRuleType { get; set; }
    public virtual SupplierDeliveryHolidayRuleType DeliveryHolidayRuleType { get; set; }
    public virtual bool ReturnApprovalRequiredFlag { get; set; }
    public virtual bool HideCostsFromSite { get; set; }
    public virtual bool CanSiteEditCost { get; set; }
    public virtual bool IsLocal { get; set; }
    public virtual string ScheduleId { get; set; }
    public virtual string BuControlCostsCode { get; set; }
    public virtual bool AllowFractionalQuantityFlag { get; set; }
    //public virtual CanSupplierAddSupplierItem DealerOperatedBusinessUnitCanCreateCode { get; set; }
    //public virtual CanSupplierAddSupplierItem CompanyOperatedBusinessUnitCanCreateCode { get; set; }
    private IList<SupplierMetricAssignment> _OrderingMetricAssignments { get; set; }
    private IList<SupplierDataAccessorAssignment> _SupplierDataAccessorAssignments { get; set; }
    private IList<SupplierItemGroup> _SupplierItemGroups { get; set; }
    private IList<SupplierHoliday> _SupplierHolidays { get; set; }
    //private IList<SupplierContactList> SupplierContactsList { get; set; }
    private IList<CostLevel> CostLevel { get; set; }
    public virtual string PrimaryContactName { get; set; }
    public virtual string PrimaryContactPhoneNumber { get; set; }
    public virtual int MinimumOrderQuantity { get; set; }
    public virtual decimal MinimumOrderAmount { get; set; }
    public virtual bool IsSupplierLinkedToTransaction { get; set; }
    public virtual bool TrustedSupplierFlag { get; set; }
    public virtual DateTime? DeliveryDate { get; set; }
    public virtual DateTime? NextDeliveryDate { get; set; }
    public virtual IEnumerable<CostLevel> CostLevels
    {
      get
      {
        return CostLevel.AsEnumerable();
      }
    }
    //private IList<SupplierOverrideConfiguration> _SupplierOverrideConfigurations { get; set; }
    //public virtual IList<SupplierOverrideConfiguration> SupplierOverrideConfigurations
    //{
    //  get
    //  {
    //    return _SupplierOverrideConfigurations;
    //  }
    //}
    public Supplier()
    {
      //InitializeCollections();
    }
    //protected internal Supplier(string name, string ediNumber,
    //  OrganizationalHierarchy owner, SupplierType type,
    //  SupplierBillingMethod billingMethod, ForecastGroup aoFcstGroup,
    //  decimal? suggestedOrderQtyRoundingThreshold, decimal? deliveryCostToleranceRatio,
    //  bool usesDefaultForecastConfiguration, SupplierStatusCode statusCode, bool useLowestCost,
    //  DeliveryInvoiceCostType deliveryUsingInvoiceCostType,
    //  DeliveryPOCostType deliveryUsingPOCostType,
    //  SupplierOrderHolidayRuleType orderHolidayRuleType, SupplierDeliveryHolidayRuleType deliveryHolidaRuleType)
    //  : base(owner)
    //{
    //  Name = name;
    //  Type = type;
    //  BillingMethod = billingMethod;
    //  OrderingForecastGroup = aoFcstGroup;
    //  SuggestedOrderQtyRoundingThreshold = suggestedOrderQtyRoundingThreshold;
    //  DeliveryCostToleranceRatio = deliveryCostToleranceRatio;
    //  UsesDefaultForecastConfiguration = usesDefaultForecastConfiguration;
    //  EDINumber = ediNumber;
    //  StatusCode = statusCode;
    //  UseLowestCost = useLowestCost;
    //  DeliveryUsingInvoiceCostType = deliveryUsingInvoiceCostType;
    //  DeliveryUsingPOCostType = deliveryUsingPOCostType;
    //  OrderHolidayRuleType = orderHolidayRuleType;
    //  DeliveryHolidayRuleType = deliveryHolidaRuleType;
    //  InitializeCollections();
    //}
    //protected internal Supplier(string name, string ediNumber,
    //  OrganizationalHierarchy owner, SupplierType type,
    //  SupplierBillingMethod billingMethod, ForecastGroup aoFcstGroup,
    //  decimal? suggestedOrderQtyRoundingThreshold, decimal? deliveryCostToleranceRatio,
    //  bool usesDefaultForecastConfiguration, SupplierStatusCode statusCode, bool useLowestCost, bool returnApprovalRequiredFlag,
    //  DeliveryInvoiceCostType deliveryUsingInvoiceCostType,
    //  DeliveryPOCostType deliveryUsingPOCostType, SupplierOrderHolidayRuleType orderHolidayRuleType,
    //  SupplierDeliveryHolidayRuleType deliveryHolidayRuleType,
    //  Address address, string deliveryNumberFormat, bool deliveryInvoiceDateRequiredFlag,
    //  bool deliveryInvoiceRefNumberRequiredFlag, string deliveryInvoiceRefNumberInvalidChars,
    //  bool receiveCheckNumberForCODFlag, bool hideCostsFromSite)
    //  : this(name, ediNumber, owner, type, billingMethod, aoFcstGroup,
    //    suggestedOrderQtyRoundingThreshold, deliveryCostToleranceRatio,
    //    usesDefaultForecastConfiguration, statusCode, useLowestCost, deliveryUsingInvoiceCostType,
    //    deliveryUsingPOCostType, orderHolidayRuleType, deliveryHolidayRuleType)
    //{
    //  Address = address;
    //  ReceiveCheckNumberForCODFlag = receiveCheckNumberForCODFlag;
    //  ReturnApprovalRequiredFlag = returnApprovalRequiredFlag;
    //  HideCostsFromSite = hideCostsFromSite;
    //  EnforceDeliveryNumberFormatIsValid(deliveryNumberFormat);
    //  DeliveryInvoiceDateRequiredFlag = deliveryInvoiceDateRequiredFlag;
    //  DeliveryInvoiceRefNumberRequiredFlag = deliveryInvoiceRefNumberRequiredFlag;
    //  DeliveryInvoiceRefNumberInvalidChars = deliveryInvoiceRefNumberInvalidChars;
    //}
    //protected internal Supplier(string name, string ediNumber,
    // OrganizationalHierarchy owner, SupplierType type,
    // SupplierBillingMethod billingMethod, ForecastGroup aoFcstGroup,
    // decimal? suggestedOrderQtyRoundingThreshold, decimal? deliveryCostToleranceRatio,
    // bool usesDefaultForecastConfiguration, SupplierStatusCode statusCode, bool useLowestCost, bool returnApprovalRequiredFlag,
    // DeliveryInvoiceCostType deliveryUsingInvoiceCostType,
    // DeliveryPOCostType deliveryUsingPOCostType, SupplierOrderHolidayRuleType orderHolidayRuleType,
    // SupplierDeliveryHolidayRuleType deliveryHolidayRuleType,
    // Address address, string deliveryNumberFormat, bool deliveryInvoiceDateRequiredFlag,
    // bool deliveryInvoiceRefNumberRequiredFlag, string deliveryInvoiceRefNumberInvalidChars,
    // bool receiveCheckNumberForCODFlag, bool hideCostsFromSite,
    // CanSupplierAddSupplierItem dealerOperatedBusinessUnitCanCreateCode,
    // CanSupplierAddSupplierItem companyOperatedBusinessUnitCanCreateCode, bool allowFractionalQuantityFlag,
    // string customerNumber, ExportType? exportType)
    // : this(name, ediNumber, owner, type, billingMethod, aoFcstGroup,
    //   suggestedOrderQtyRoundingThreshold, deliveryCostToleranceRatio,
    //   usesDefaultForecastConfiguration, statusCode, useLowestCost, deliveryUsingInvoiceCostType,
    //   deliveryUsingPOCostType, orderHolidayRuleType, deliveryHolidayRuleType)
    //{
    //  Address = address;
    //  ReceiveCheckNumberForCODFlag = receiveCheckNumberForCODFlag;
    //  ReturnApprovalRequiredFlag = returnApprovalRequiredFlag;
    //  HideCostsFromSite = hideCostsFromSite;
    //  EnforceDeliveryNumberFormatIsValid(deliveryNumberFormat);
    //  DeliveryInvoiceDateRequiredFlag = deliveryInvoiceDateRequiredFlag;
    //  DeliveryInvoiceRefNumberRequiredFlag = deliveryInvoiceRefNumberRequiredFlag;
    //  DeliveryInvoiceRefNumberInvalidChars = deliveryInvoiceRefNumberInvalidChars;
    //  DealerOperatedBusinessUnitCanCreateCode = dealerOperatedBusinessUnitCanCreateCode;
    //  CompanyOperatedBusinessUnitCanCreateCode = companyOperatedBusinessUnitCanCreateCode;
    //  AllowFractionalQuantityFlag = allowFractionalQuantityFlag;
    //  CustomerNumber = customerNumber;
    //  ExportType = exportType;
    //}    
    //public Supplier(string name, string description, SupplierType type, string externalId, string customerNumber, 
    //  Address address, string primaryContactName, string primaryContactPhoneNumber, 
    //  string termsAndConditions, bool isLocal, string buControlCostsCode,OrganizationalHierarchy owner) : base(owner)
    //{
    //  Name = name;
    //  Description = description;
    //  Type = type;
    //  ExternalId = externalId;
    //  CustomerNumber = customerNumber;
    //  StatusCode = SupplierStatusCode.Active;
    //  Address = address;
    //  PrimaryContactName = primaryContactName;
    //  PrimaryContactPhoneNumber = primaryContactPhoneNumber;
    //  TermsAndConditions = termsAndConditions;
    //  IsLocal = isLocal;
    //  BuControlCostsCode = buControlCostsCode;
    //  BillingMethod = SupplierBillingMethod.BillFromDelivery;
    //  DeliveryUsingInvoiceCostType = DeliveryInvoiceCostType.Invoice;
    //  DeliveryUsingPOCostType = DeliveryPOCostType.PurchaseOrder;
    //  OrderHolidayRuleType = SupplierOrderHolidayRuleType.PreviousDay;
    //  DeliveryHolidayRuleType = SupplierDeliveryHolidayRuleType.NextDay;
    //  DealerOperatedBusinessUnitCanCreateCode = CanSupplierAddSupplierItem.LocalSupplierItems;
    //  CompanyOperatedBusinessUnitCanCreateCode = CanSupplierAddSupplierItem.LocalSupplierItems;
    //}

    //private void InitializeCollections()
    //{
    //  _SupplierDataAccessorAssignments = new List<SupplierDataAccessorAssignment>();
    //  _OrderingMetricAssignments = new List<SupplierMetricAssignment>();
    //  _SupplierItemGroups = new List<SupplierItemGroup>();
    //  CostLevel = new List<CostLevel>();
    //  _SupplierHolidays = new List<SupplierHoliday>();
    //  _SupplierOverrideConfigurations = new List<SupplierOverrideConfiguration>();
    //  SupplierScheduleGroup = new List<SupplierScheduleGroup>();
    //  SupplierContactsList =new List<SupplierContactList>();
    //}
    //public virtual IEnumerable<SupplierItemGroup> SupplierItemGroups
    //{
    //  get
    //  {
    //    return _SupplierItemGroups.AsEnumerable();
    //  }
    //}
    //public virtual IEnumerable<SupplierHoliday> SupplierHolidays
    //{
    //  get
    //  {
    //    return _SupplierHolidays.AsEnumerable();
    //  }
    //}
    //public virtual IEnumerable<SupplierContactList> SupplierContacts
    //{
    //  get
    //  {
    //    return SupplierContactsList.AsEnumerable();
    //  }
    //}
    //public virtual IEnumerable<Metric> OrderingMetrics
    //{
    //  get
    //  {
    //    return _OrderingMetricAssignments.Select(x => x.Metric).ToList();
    //  }
    //}
    //public virtual SupplierHoliday AddSupplierHoliday(HolidayTimeEvent holiday, bool appliesToOrders, bool appliesToDeliveries)
    //{
    //  var supplierHoliday = new SupplierHoliday(holiday, this, appliesToOrders, appliesToDeliveries);
    //  _SupplierHolidays.Add(supplierHoliday);
    //  return supplierHoliday;
    //}
    //public virtual IEnumerable<SupplierContactList> AddSupplierContactList(Supplier supplier, OrganizationalHierarchy owner,
    //  IList<SupplierContactList> supplierContactList)
    //{
    //  if (SupplierContactsList == null)
    //  {
    //    SupplierContactsList = new List<SupplierContactList>();
    //  }
    //  foreach (var supplierContact in supplierContactList)
    //  {
    //    var supplierContactLists = new SupplierContactList(supplier, supplierContact, supplierContact.Contact, owner);
    //    SupplierContactsList.Add(supplierContactLists);
    //  }
    //  return SupplierContactsList;
    //}
    //public virtual SupplierItemGroup AddSupplierItemGroup(string name, bool isException, decimal? maxOrderAmount)
    //{
    //  var group = new SupplierItemGroup(this, name, isException, maxOrderAmount);
    //  if (_SupplierItemGroups == null)
    //  {
    //    _SupplierItemGroups = new List<SupplierItemGroup>();
    //  }
    //  _SupplierItemGroups.Add(group);
    //  return group;
    //}
    //public virtual void AddOrderingMetric(Metric metric)
    //{
    //  _OrderingMetricAssignments.Add(new SupplierMetricAssignment(this, metric));
    //}
    //public virtual CostLevel AddCostLevelFromTest(IUnitOfWork uow, string name, int defaultRanking)
    //{
    //  EnforceRunningInTest(uow, "AddCostLevelFromTest");

    //  var costLevel = new CostLevel(this, name, defaultRanking);

    //  CostLevel.Add(costLevel);

    //  return costLevel;
    //}
    //public virtual void AddSupplierOverrideConfiguration(SupplierOverrideConfiguration supplierOverrideConfiguration)
    //{
    //  EnforceSupplierMatchesConfiguration(supplierOverrideConfiguration);
    //  _SupplierOverrideConfigurations.Add(supplierOverrideConfiguration);
    //}
    //public virtual SupplierSiteOverrideConfiguration GetSiteOverrideConfiguration(int siteId)
    //{
    //  if (_SupplierOverrideConfigurations != null)
    //    return _SupplierOverrideConfigurations.SelectMany(x => x.SupplierSiteOverrideConfigurations)
    //      .FirstOrDefault(x => x.Site.ID == siteId);
    //   return null;
    //}
    //public virtual bool GetHideCostsFromSite(Site site)
    //{
    //  return GetHideCostsFromSiteId(site.ID);
    //}
    //public virtual bool GetHideCostsFromSiteId(int siteId)
    //{
    //  var siteOverrideConfig = GetSiteOverrideConfiguration(siteId);

    //  return siteOverrideConfig != null ? siteOverrideConfig.SupplierOverrideConfiguration.HideCostsFromSite : HideCostsFromSite;
    //}
    //public virtual void SetDeliveryUsingInvoiceCostTypeFromTest(IUnitOfWork uow, DeliveryInvoiceCostType costType)
    //{
    //  EnforceRunningInTest(uow, "SetDeliveryUsingInvoiceCostTypeFromTest");
    //  DeliveryUsingInvoiceCostType = costType;
    //}
    //public virtual void SetDeliveryUsingPOCostTypeFromTest(IUnitOfWork uow, DeliveryPOCostType costType)
    //{
    //  EnforceRunningInTest(uow, "SetDeliveryUsingPOCostTypeFromTest");
    //  DeliveryUsingPOCostType = costType;
    //}
    //public virtual void AddDataAccessorAssignment(DataAccessor dataAccessor, DateTime start, DateTime end)
    //{
    //  if (_SupplierDataAccessorAssignments == null)
    //  {
    //    _SupplierDataAccessorAssignments = new List<SupplierDataAccessorAssignment>();
    //  }
    //  _SupplierDataAccessorAssignments.Add(new SupplierDataAccessorAssignment(this, dataAccessor,
    //    start, end));
    //}
    //public override IEnumerable<EffectiveDatedDataAccessorAssignmentEntity> DataAccessorAssignments
    //{
    //  get
    //  {
    //    return _SupplierDataAccessorAssignments.Cast<EffectiveDatedDataAccessorAssignmentEntity>().ToList();
    //  }
    //}
    //public virtual CostLevel GetDefaultCostLevel()
    //{
    //  var defaultCostLevel = CostLevel.FirstOrDefault(x => x.DefaultRanking == RP.BOSDomainModel.BOS.SupplierManagement.CostLevel.MasterRanking);

    //  if (defaultCostLevel == null)
    //  {
    //    ThrowDomainError(SupplierDomainErrors.AttemptToGetCostLevelWithNoDefaultCostLevelConfigured);
    //  }

    //  return defaultCostLevel;
    //}
    //public virtual bool IsOrderDateHoliday(Site site, DateTime scheduledDate, ICalendarLocatorService calendarLocatorService)
    //{
    //  var date = scheduledDate.Date;

    //  return SupplierHolidays
    //    .Where(x => x.AppliesToOrders)
    //    .Any(x => x.HolidayEvent.GetFirstOccurrenceForSiteOnOrAfterDate
    //                (site, date, calendarLocatorService) == date);
    //}
    //public virtual bool IsDeliveryDateHoliday(Site site, DateTime scheduledDate, ICalendarLocatorService calendarLocatorService)
    //{
    //  var date = scheduledDate.Date;
    //  return SupplierHolidays
    //    .Where(x => x.AppliesToDeliveries)
    //    .Any(x => x.HolidayEvent.GetFirstOccurrenceForSiteOnOrAfterDate
    //                (site, date, calendarLocatorService) == date);
    //}
    //public virtual void SetTotalsOnlyReceivingFromTest(IUnitOfWork uow, bool allowTotalsOnlyReceiving, SupplierItem totalsOnlySupplierItem)
    //{
    //  EnforceRunningInTest(uow, "SetTotalsOnlyReceivingFromTest");

    //  AllowTotalsOnlyReceiving = allowTotalsOnlyReceiving;

    //  TotalsOnlySupplierItem = totalsOnlySupplierItem;
    //  if (!allowTotalsOnlyReceiving)
    //  {
    //    TotalsOnlySupplierItem = null;
    //  }
    //}
    //public virtual void SetTotalsAcceptReceivingFromTest(IUnitOfWork uow, bool allowTotalsAcceptReceiving)
    //{
    //  EnforceRunningInTest(uow, "SetTotalsAcceptReceivingFromTest");

    //  AllowTotalsAcceptReceiving = allowTotalsAcceptReceiving;
    //}
    //public virtual void SetBlindReceivingFromTest(IUnitOfWork uow, bool allowBlindReceiving)
    //{
    //  EnforceRunningInTest(uow, "SetBlindReceivingFromTest");

    //  AllowBlindReceiving = allowBlindReceiving;
    //}
    //public virtual void SetOrderHolidayRuleTypeFromTest(IUnitOfWork uow, SupplierOrderHolidayRuleType orderHolidayRuleType)
    //{
    //  EnforceRunningInTest(uow, "SetOrderHolidayRuleTypeFromTest");

    //  OrderHolidayRuleType = orderHolidayRuleType;
    //}
    //public virtual void SetDeliveryHolidayRuleTypeFromTest(IUnitOfWork uow, SupplierDeliveryHolidayRuleType deliveryHolidayRuleType)
    //{
    //  EnforceRunningInTest(uow, "SetDeliveryHolidayRuleTypeFromTest");

    //  DeliveryHolidayRuleType = deliveryHolidayRuleType;
    //}
    //private void EnforceSupplierMatchesConfiguration(SupplierOverrideConfiguration supplierOverrideConfiguration)
    //{
    //  if (ID != supplierOverrideConfiguration.Supplier.ID)
    //  {
    //    ThrowDomainError(SupplierDomainErrors.AttemptToAddSupplierOverrideConfigurationWithIncorrectSupplier);
    //  }
    //}
    //private static void EnforceRunningInTest(IUnitOfWork uow, string funcName)
    //{
    //  uow.EnforceRunningInTest(String.Format("{0} can only be run from test UOW", funcName));
    //}
    //private void EnforceDeliveryNumberFormatIsValid(string deliveryNumberFormat)
    //{
    //  if (String.IsNullOrEmpty(deliveryNumberFormat))
    //  {
    //    return;
    //  }
    //  string validDeliveryNumberFormat = deliveryNumberFormat.Replace(" ", "").ToUpper();
    //  if (String.IsNullOrEmpty(validDeliveryNumberFormat))
    //  {
    //    return;
    //  }
    //  if (!Regex.IsMatch(validDeliveryNumberFormat, @"[^A-Z0-9\?\*]"))
    //  {
    //    var asteriskPos = validDeliveryNumberFormat.IndexOf('*');
    //    if (asteriskPos >= 0)
    //    {
    //      var asteriskMask = validDeliveryNumberFormat.Substring(asteriskPos);
    //      if (!Regex.IsMatch(asteriskMask, @"[A-Z0-9\?]"))
    //      {
    //        DeliveryNumberFormat = validDeliveryNumberFormat;
    //        return;
    //      }
    //    }
    //    else
    //    {
    //      DeliveryNumberFormat = validDeliveryNumberFormat;
    //      return;
    //    }
    //  }
    //  DeliveryNumberFormat = null;
    //  ThrowDomainError(SupplierDomainErrors.AttemptToSetInvalidDeliveryNumberFormat);
    //}
    //public virtual decimal RoundTotalCostComponent(decimal value)
    //{
    //  return Math.Round(value, 2, MidpointRounding.AwayFromZero);
    //}
    //public virtual decimal? RoundTotalCostComponent(decimal? value)
    //{
    //  if (value.HasValue)
    //  {
    //    return RoundTotalCostComponent(value.Value);
    //  }
    //  return null;
    //}
    //private void ThrowDomainError(SupplierDomainErrors error)
    //{
    //  throw new DomainOperationException<SupplierDomainErrors>(error);
    //}
    //public override bool Equals(object other)
    //{
    //  var ot = other as Supplier;
    //  if (ot == null)
    //  {
    //    return false;
    //  }
    //  return ot.ID == ID;
    //}
    //public override int GetHashCode()
    //{
    //  return ID.GetHashCode();
    //}
    //public virtual DateTime? GetDeliveryDateForSite(IUnitOfWork uow, Site site)
    //{
    //  var supplierSiteScheduleService = new SupplierSiteScheduleService(uow);
    //  var siteLocalTime = SiteLocalTimeService.GetSiteLocalTime(uow, site);
    //  return supplierSiteScheduleService.GetNextDeliveryDate(site, this, siteLocalTime); 
    //}
    //public virtual DateTime? GetNextDeliveryDateForSite(IUnitOfWork uow, Site site, DateTime deliveryDate)
    //{
    //  var supplierSiteScheduleService = new SupplierSiteScheduleService(uow);
    //  var siteLocalTime = SiteLocalTimeService.GetSiteLocalTime(uow, site);
    //  return supplierSiteScheduleService.GetNextNextDeliveryDateForAdHocPurchaseOrder(site, this, siteLocalTime, deliveryDate);
    //}
    //public virtual void RemoveSupplierContact(SupplierContactList supplierContactList)
    //{
    //  SupplierContactsList.Remove(supplierContactList);
    //}   
  }
  public enum ExportType
  {
    Empty = ' ',
    None = 'n',
    ArchiveXml = 'a',
    AutoFaxXml = 'f',
    Email = 'm',
    FtpEdiToSupplier = 'e',
    FtpXmlToSupplier = 'x'
  }
}
