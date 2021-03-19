using System;

namespace BYEsoDomainModelKernel.Models
{
  public class SiteClosedDays
  {
    public virtual Site Site { get; set; }
    public virtual DateTime BusinessDate { get; set; }

    public SiteClosedDays()
    {
    }

    public SiteClosedDays(Site site, DateTime businessDate)
    {
      Site = site;
      BusinessDate = businessDate;
    }
  }
}
