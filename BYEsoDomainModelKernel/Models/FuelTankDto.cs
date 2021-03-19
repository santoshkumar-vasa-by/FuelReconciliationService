namespace BYEsoDomainModelKernel.Models
{
  public class FuelTankDto
  {
    public bool IsActive { get; internal set; }
    public string TankNumber { get; internal set; }
    public decimal? VarianceBySalesThresholdPercent { get; internal set; }
    public bool IsAutomaticTankGauge { get; internal set; }
  }
}