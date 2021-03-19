namespace BYEsoDomainModelKernel.Models
{
  public static class UnitOfMeasureFactory
  {
    public static UnitOfMeasure CreateUnitOfMeasure(int id, string name, decimal factor, UnitOfMeasureClass unitOfMeasureClass,
      bool isActive)
    {
      UnitOfMeasure newUOM = new UnitOfMeasure(id, name, factor, unitOfMeasureClass, isActive);
      return newUOM;
    }
  }
}
