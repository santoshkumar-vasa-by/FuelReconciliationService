namespace BYEsoDomainModelKernel.Models
{
  public class FuelSalesItem : BaseAuditEntity
  {
    public virtual int FuelSalesItemID { get; set; }
    public virtual int GradeId { get; set; }
    public virtual int PricingTierId { get; set; }
    public virtual int ServiceModeId { get; set; }
    public virtual string FuelSalesItemName { get; set; }
    public virtual int? BlendItem1Id { get; set; }
    public virtual decimal? BlendItem1Percentage { get; set; }
    public virtual int? BlendItem2Id { get; set; }
    public virtual decimal? BlendItem2Percentage { get; set; }

    protected internal FuelSalesItem()
    {

    }

    protected internal FuelSalesItem(int fuelSalesItemID, int gradeId, int pricingTierId, int serviceModeId,
      string fuelSalesItemName, int blendItem1Id = 0, decimal blendItem1Percentage = 0, int blendItem2Id = 0,
      decimal blendItem2Percentage = 0)
    {
      FuelSalesItemID = fuelSalesItemID;
      GradeId = gradeId;
      PricingTierId = pricingTierId;
      ServiceModeId = serviceModeId;
      FuelSalesItemName = fuelSalesItemName;
      BlendItem1Id = blendItem1Id;
      BlendItem1Percentage = blendItem1Percentage;
      BlendItem2Id = blendItem2Id;
      BlendItem2Percentage = blendItem2Percentage;
    }
  }
}