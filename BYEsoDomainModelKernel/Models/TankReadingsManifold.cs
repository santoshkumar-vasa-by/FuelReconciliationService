using System;

namespace BYEsoDomainModelKernel.Models
{
  public class TankReadingsManifold
  {
    public int FuelTankId { get; set; }
    public DateTime? ReadTimeStamp { get; set; }
    public decimal? Volume { get; set; }
  }
}
