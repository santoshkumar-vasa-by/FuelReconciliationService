namespace BYEsoDomainModelKernel.Models
{
  public class FuelTankType
  {
    public virtual int ID { get; protected internal set; }
    public virtual int Capacity { get; protected internal set; }
    public virtual int MaximumFillVolume { get; protected internal set; }
    public virtual int ClientId { get; protected internal set; }
    public virtual string Name { get; protected internal set; }

    protected internal FuelTankType()
    {
    }

    public FuelTankType(int capacity, int maximumFillVolume, int clientId = 0)
      : this()
    {
      Capacity = capacity;
      MaximumFillVolume = maximumFillVolume;
      ClientId = clientId;
    }

    public override bool Equals(object other)
    {
      FuelTankType ot = other as FuelTankType;
      if (ot == null)
      {
        return false;
      }
      return ot.ID == ID;
    }

    public override int GetHashCode()
    {
      return ID.GetHashCode();
    }
  }
}