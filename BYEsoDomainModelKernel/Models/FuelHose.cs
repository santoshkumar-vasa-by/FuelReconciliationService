using System.Collections.Generic;

namespace BYEsoDomainModelKernel.Models
{
  public partial class FuelHose : BaseAuditEntity
  {
    public virtual int ID { get; set; }
    public virtual Site Site { get; set; }
    public virtual FuelPump FuelPump { get; set; }
    public virtual FuelItem FuelItem { get; set; }
    public virtual int HoseNumber { get; set; }
    public virtual bool IsElectronic { get; set; }
    public virtual IList<FuelHoseTankAssignment> FuelHoseTankAssignment { get; set; }
    //public virtual IList<FuelTankMeterReadingLineItem> FuelTankMeterReadingLineItem { get; set; }

    protected internal FuelHose()
    {
      FuelHoseTankAssignment = new List<FuelHoseTankAssignment>();
    }

    public FuelHose(FuelPump fuelPump, int hoseNumber, FuelItem fuelItem, bool isElectronic)
      : this()
    {
      Site = fuelPump.Site;
      FuelPump = fuelPump;
      FuelItem = fuelItem;
      HoseNumber = hoseNumber;
      IsElectronic = isElectronic;
    }

    //public virtual void AddFuelHoseTankAssignment(FuelHoseTankAssignment fuelHoseTankAssignment)
    //{
    //  FuelHoseTankAssignment.Add(fuelHoseTankAssignment);
    //}

    public virtual void SetFuelItem(FuelItem fuelItem)
    {
      FuelItem = fuelItem;
    }

    public override bool Equals(object other)
    {
      FuelHose ot = other as FuelHose;
      if (ot == null)
      {
        return false;
      }
      return (ot.ID == ID);
    }

    public override int GetHashCode()
    {
      return ID.GetHashCode();
    }
  }
}