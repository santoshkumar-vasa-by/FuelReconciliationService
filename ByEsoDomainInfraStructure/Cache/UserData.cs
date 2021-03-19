using System;
using System.Collections.Generic;
using System.Text;
using ByEsoDomainInfraStructure.Cache.Interfaces;

namespace ByEsoDomainInfraStructure.Cache
{
  public class UserData : IUserData
  {
    public int UserId { get; set; }
    public string SecurityAccessId { get; set; }
    public int[] SecurityGroups { get; set; }
    public int DefaultLangId { get; set; }
    public int? CurrentHierarchyId { get; set; }
  }
}
