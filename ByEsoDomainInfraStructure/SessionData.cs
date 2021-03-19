using System;
using System.Collections.Generic;
using System.Text;

namespace ByEsoDomainInfraStructure
{
  public class SessionData : ISessionData
  {
    public int ClientId { get; set; }
    public int DefaultLanguageId { get; set; }
    public int UserId { get; set; }
    public int[] SecurityGroups { get; set; }
    public int CurrentHierarchyId { get; set; }
    public int DepartmentFilterId { get; set; }
    public string SecurityAccessId { get; set; }
  }
}
