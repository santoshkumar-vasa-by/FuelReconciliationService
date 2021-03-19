using System;
using System.Collections.Generic;
using System.Text;

namespace ByEsoDomainInfraStructure
{
  public interface ISessionData
  {
    int ClientId { get; set; }
    int DefaultLanguageId { get; set; }
    int UserId { get; set; }
    int CurrentHierarchyId { get; set; }
    int DepartmentFilterId { get; set; }
  }
}
