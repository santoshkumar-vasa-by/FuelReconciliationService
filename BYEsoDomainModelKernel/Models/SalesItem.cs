namespace BYEsoDomainModelKernel.Models
{
  public class SalesItem
  {
    public int ID { get; internal set; }
    public virtual Item SoldItem { get; set; }
  }
}