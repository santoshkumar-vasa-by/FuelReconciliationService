namespace BYEsoDomainModelKernel.Models.Common
{
  internal interface IDatamartTicketedEntity
  {
    int WaveTicketID { get; }
  }

  public class DatamartTicketedEntity : IDatamartTicketedEntity
  {
    public virtual int WaveTicketID { get; private set; }

    protected internal DatamartTicketedEntity()
    {
    }

    protected internal DatamartTicketedEntity(int waveTicketID)
    {
      WaveTicketID = waveTicketID;
    }
  }
}
