using System;

namespace BYEsoDomainModelKernel.Models
{
  public class FuelMeterReadingsData
  {
    public int UniqueID { get; set; }
    public DateTime BusinessDate { get; set; }
    public int FuelMeterID { get; set; }
    public int FuelTankID { get; set; }
    public decimal PumpedVolume { get; set; }
    public DateTime ReadTimeStamp { get; set; }
    public char LineItemTypeCode { get; set; }
    public decimal ReadValue { get; set; }
    public decimal Delta { get; set; }
  }

  public class LastFuelMeterReadingData
  {
    public int FuelMeterID { get; set; }
    public decimal ReadValue { get; set; }
  }
}
