using System;
using System.Collections.Generic;
using System.Linq;
using RPCore;

namespace BYEsoDomainModelKernel.Models
{
  public class OrganizationalHierarchy : BaseAuditEntity
  {
    public virtual int ID { get; set; }
    public virtual DataAccessor DataAccessor { get; set; }
    public virtual OrganizationalHierarchyLevel Level { get; set; }
    public virtual OrganizationalHierarchy Parent { get; set; }
    public virtual Language.Language Language { get; set; }

    // ReSharper disable InconsistentNaming
    protected internal virtual IList<OrganizationalHierarchy> _Children { get; set; }
    protected internal virtual IList<OrganizationalHierarchyEmployeeAssignment> _EmployeeAssignments { get; set; }
    protected internal virtual DateTime? _CurrentBusinessDate { get; set; }
    // ReSharper restore InconsistentNaming
    public virtual DateTime CurrentBusinessDate
    {
      get
      {
        if (_CurrentBusinessDate.HasValue)
        {
          return _CurrentBusinessDate.Value;
        }

        var currentDate = Sundial.Now;
        return new DateTime(currentDate.Year, currentDate.Month, currentDate.Day);
      }
      set
      {
        _CurrentBusinessDate = value;
      }
    }

    public virtual IEnumerable<OrganizationalHierarchyEmployeeAssignment> EmployeeAssignments
    {
      get
      {
        return _EmployeeAssignments.AsEnumerable();
      }
    }

    public virtual IEnumerable<OrganizationalHierarchy> Children
    {
      get
      {
        return _Children.AsEnumerable();
      }
    }

    protected internal OrganizationalHierarchy()
    {
      _Children = new List<OrganizationalHierarchy>();
      _EmployeeAssignments = new List<OrganizationalHierarchyEmployeeAssignment>();
    }

    protected internal OrganizationalHierarchy(int id, DataAccessor da,
      OrganizationalHierarchyLevel level, OrganizationalHierarchy parent) : this()
    {
      ID = id;
      DataAccessor = da;
      Level = level;
      if (parent != null)
      {
        parent.AddChild(this);
      }
    }

    protected internal OrganizationalHierarchy(int id, DataAccessor da,
      OrganizationalHierarchyLevel level, Language.Language language, OrganizationalHierarchy parent) : this()
    {
      ID = id;
      DataAccessor = da;
      Level = level;
      Language = language;
      if (parent != null)
      {
        parent.AddChild(this);
      }
    }

    public virtual void AddChild(OrganizationalHierarchy child)
    {
      child.Parent = this;
      _Children.Add(child);
    }

    //public virtual void AddEmployeeAssignment(Employee employee)
    //{
    //  var assignment = new OrganizationalHierarchyEmployeeAssignment
    //  {
    //    OriganizationalHierarchyID = ID,
    //    EmployeeID = employee.ID,
    //    OrganizationalHierarchy = this,
    //    Employee = employee
    //  };

    //  _EmployeeAssignments.Add(assignment);
    //}

    public virtual bool IsLowerInTreeThan(OrganizationalHierarchy orgHierarchy)
    {
      return Level.SortOrder > orgHierarchy.Level.SortOrder;
    }

    public virtual IEnumerable<OrganizationalHierarchy> GetHierarchiesAboveMe()
    {
      var hierarchies = new List<OrganizationalHierarchy>();

      if (Parent != null)
      {
        hierarchies.AddRange(Parent.GetHierarchiesAtOrAboveMe());
      }

      return hierarchies;
    }

    public virtual IEnumerable<OrganizationalHierarchy> GetHierarchiesAtOrAboveMe()
    {
      var hierarchies = new List<OrganizationalHierarchy> { this };

      if (Parent != null)
      {
        hierarchies.AddRange(Parent.GetHierarchiesAtOrAboveMe());
      }

      return hierarchies;
    }

    public virtual void SetBusinessDate(DateTime businessDate)
    {
      CurrentBusinessDate = businessDate;
    }

    public virtual void SetLanguage(Language.Language language)
    {
      Language = language;
    }

    public override bool Equals(object obj)
    {
      var other = obj as OrganizationalHierarchy;

      if (other != null)
      {
        return ID == other.ID;
      }
      return false;
    }

    public override int GetHashCode()
    {
      return ID;
    }
  }

  public enum OrganizationalHierarchyErrorCodes
  {
    DistrictNotFound,
    SitesNotFound
  }
}
