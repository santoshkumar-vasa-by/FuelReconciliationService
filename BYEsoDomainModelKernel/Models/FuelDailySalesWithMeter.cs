namespace BYEsoDomainModelKernel.Models
{
  public class FuelDailySalesWithMeter
  {
    public int FuelTankID { get; set; }
    public int FuelMeterID { get; set; }
    public decimal POSVolume { get; set; }
    public decimal POSSales { get; set; }
    public decimal ManualVolume { get; set; }
    public decimal ManualSales { get; set; }
  }
}
