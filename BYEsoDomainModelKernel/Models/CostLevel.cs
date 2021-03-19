namespace BYEsoDomainModelKernel.Models
{
  public class CostLevel : BaseAuditEntity
  {
    public virtual int ID { get; set; }
    public virtual Supplier Supplier { get; set; }
    public virtual string Name { get; set; }
    public virtual int DefaultRanking { get; set; }

    public const int MasterRanking = 999;

    protected internal CostLevel()
    {
    }

    protected internal CostLevel(Supplier supplier, string name, int defaultRanking)
    {
      Supplier = supplier;
      Name = name;
      DefaultRanking = defaultRanking;
    }
  }
}
