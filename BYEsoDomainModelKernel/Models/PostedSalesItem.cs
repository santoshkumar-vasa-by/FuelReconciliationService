
namespace BYEsoDomainModelKernel.Models
{
  public class PostedSalesItem : BaseAuditEntity
  {
    public virtual int SalesItemID { get; set; }

    public virtual Item Item { get; set; }

    public virtual string ItemName { get; set; }

    public virtual ItemCategory ItemHierarchy { get; set; }

    public PostedSalesItem()
    {
    }

    public PostedSalesItem(int saleItemID, Item item, string itemName, ItemCategory itemHierarchy)
    {
      SalesItemID = saleItemID;
      Item = item;
      ItemName = itemName;
      ItemHierarchy = itemHierarchy;
    }
  }

}
