namespace BYEsoDomainModelKernel.Models
{
  public static class UnitOfMeasureClassFactory
  {
    public static UnitOfMeasureClass CreateUnitOfMeasureClass(int id, string name)
    {
      UnitOfMeasureClass newUOMClass = new UnitOfMeasureClass(id, name);

      return newUOMClass;
    }
    public static UnitOfMeasureClass CreateUnitOfMeasureClass(int id, string name, int clientId)
    {
      UnitOfMeasureClass newUOMClass = new UnitOfMeasureClass(id, name, clientId);
      return newUOMClass;
    }
  }
}