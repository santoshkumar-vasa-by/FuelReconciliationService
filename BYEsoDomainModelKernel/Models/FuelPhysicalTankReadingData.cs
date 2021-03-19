using System;

namespace BYEsoDomainModelKernel.Models
{
  public class FuelPhysicalTankReadingData
  {
    public int PhysicalFuelTankID { get; set; }
    public DateTime? ReadTimeStamp { get; set; }
    public decimal? ReadVolume { get; set; }
    public int FuelTankID { get; set; }
    public string FuelTankNumber { get; set; }
    public int FuelInventoryItemID { get; set; }
    public string FuelInventoryItemName { get; set; }
    public DateTime BusinessDate { get; set; }
    public TankReadingType ReadingTypeCode { get; set; }
  }
}
