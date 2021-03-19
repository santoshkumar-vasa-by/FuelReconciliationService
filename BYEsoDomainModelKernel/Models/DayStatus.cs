using System;

namespace BYEsoDomainModelKernel.Models
{
  public class DayStatus
  {
    public virtual OrganizationalHierarchy OrganizationalHierarchy { get; set; }
    public virtual DateTime BusinessDate { get; set; }

    public virtual DateTime? BuDatePostedTimestamp { get; set; }
    public virtual BusinessDateStatus Status { get; set; }
    //public virtual PublicationDayStatus? PublicationDayStatus { get; set; }
    public virtual bool IsInventoryLocked { get; set; }

    public DayStatus()
    {
    }
  }

  public enum BusinessDateStatus
  {
    InProgress = 'i',
    Posted = 'p',
    Closing = 'g'
  }
}
