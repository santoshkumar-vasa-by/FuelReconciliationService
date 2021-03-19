using System;

namespace BYEsoDomainModelKernel.Models
{
  public partial class Site : BaseAuditEntity
  {
    public virtual int ID { get; set; }
    public virtual DataAccessor DataAccessor { get; set; }
    public virtual OrganizationalHierarchy OrganizationalHierarchy { get; set; }
    public virtual Common.TimeZone TimeZone { get; set; }
    public virtual string EDINumber { get; set; }

    public virtual int? BusinessOperatorID { get; set; }

    public virtual Address BillingAddress { get; set; }
    public virtual Address Address { get; set; }
    public virtual Status Status { get;  set; }

    public virtual DateTime DateCreated { get;  set; }

    public virtual DateTime DateStatusCodeLastChanged { get;  set; }


    public virtual DateTime CurrentBusinessDate => OrganizationalHierarchy.CurrentBusinessDate;

    //IList<SiteGroupSiteAssignment> _SiteGroupAssignments { get; set; }
    //public virtual IEnumerable<SiteGroup> SiteGroups
    //{
    //  get
    //  {
    //    return _SiteGroupAssignments.Select(x => x.SiteGroup).ToList();
    //  }
    //}

    public Site()
    {
    }

    // Site(OrganizationalHierarchy oh, TimeZone timeZone, string ediNumber,
    //  Address address, Address billingAddress)
    //{
    //  ID = oh.ID;
    //  DataAccessor = oh.DataAccessor;
    //  OrganizationalHierarchy = oh;
    //  TimeZone = timeZone;
    //  Address = address;
    //  BillingAddress = billingAddress;
    //  EDINumber = ediNumber;

    //  _SiteGroupAssignments = new List<SiteGroupSiteAssignment>();
    //}

    // Site(OrganizationalHierarchy oh, TimeZone timeZone, string ediNumber,
    //  Address address, Address billingAddress, Status siteStatus,
    //  DateTime dateCreated, DateTime dateStatusCodeLastChanged)
    //  : this(oh, timeZone, ediNumber, address, billingAddress)
    //{
    //  Status = siteStatus;
    //  DateCreated = dateCreated;
    //  DateStatusCodeLastChanged = dateStatusCodeLastChanged;

    //}
  }

  public enum Status
  {
    PreOpen = 'p',
    Open = 'o',
    Closed = 'c',
    TemporarilyClosed = 'r',
  }

  public enum SiteDomainErrors
  {
    InvalidSite,
    AccessDeniedToSite
  }
}
