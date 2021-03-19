using System.Collections.Generic;
using System.Linq;

namespace BYEsoDomainModelKernel.Models
{
  public partial class FuelPump : BaseAuditEntity
  {
    public virtual int ID { get; set; }
    public virtual Site Site { get; set; }
    public virtual int PumpNumber { get; set; }

    public virtual int FuelPumpConfigId { get; set; }

    private IList<FuelHose> _Hoses { get; set; }
    public virtual IEnumerable<FuelHose> Hoses
    {
      get
      {
        return _Hoses.AsEnumerable();
      }
    }

    protected internal FuelPump()
    {
      _Hoses = new List<FuelHose>();
    }

    public FuelPump(Site site, int pumpNumber)
      : this()
    {
      Site = site;
      PumpNumber = pumpNumber;
    }
    public FuelPump(Site site, int pumpNumber, int fuelPumpConfigId)
      : this()
    {
      Site = site;
      PumpNumber = pumpNumber;
      FuelPumpConfigId = fuelPumpConfigId;
    }

    public virtual void AddFuelHose(FuelHose fuelHose)
    {
      _Hoses.Add(fuelHose);
    }

    public override bool Equals(object obj)
    {
      FuelPump ot = obj as FuelPump;
      if (ot == null)
      {
        return false;
      }
      return ot.Hoses == _Hoses;
    }

    public override int GetHashCode()
    {
      return ID.GetHashCode();
    }
  }
}