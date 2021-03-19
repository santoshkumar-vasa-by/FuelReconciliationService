using System;
using System.Collections.Generic;
using System.Text;

namespace ByEsoDomainInfraStructure.Cache.Interfaces
{
  public interface IUserData
  {
    int UserId { get; set; }
    string SecurityAccessId { get; set; }
    int[] SecurityGroups { get; set; }
    int DefaultLangId { get; set; }
    int? CurrentHierarchyId { get; set; }
  }
}
