namespace BYEsoDomainModelKernel.Models
{
  public class FuelHoseTankLogicalConnection
  {
    public int RetailItemId { get; set; }
    public int PumpNumber { get; set; }
    public FuelPump FuelPump { get; set; }
    public int HoseNumber { get; set; }
    public FuelHose FuelMeter { get; set; }
    public int LogicalTankId { get; set; }
    public decimal PercentageFromLogicalTank { get; set; }
    public bool ElectronicFlag { get; set; }
  }
}
