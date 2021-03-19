namespace BYEsoDomainModelKernel.Util
{
  public static class SupplierFilters
  {
    //public static QueryOver<SupplierDataAccessorAssignment> GetCDMSupplierFilter(int siteID, DateTime date)
    //{
    //  DataAccessor dataAccessor = null;
    //  DataAccessorHierarchyAssignment assignedList = null;

    //  return
    //    QueryOver.Of<SupplierDataAccessorAssignment>()
    //             .JoinAlias(x => x.DataAccessor, () => dataAccessor, JoinType.InnerJoin)
    //             .JoinAlias(() => dataAccessor.DataAccessorsThatIImpactThroughAssignment, () 
    //               => assignedList, JoinType.InnerJoin)
    //             .Where(x => x.Start <= date && x.End >= date)
    //             .And(() => assignedList.DataAccessor.ID == siteID)
    //             .Select(x => x.Supplier.ID);
    //}

    //public static QueryOver<SupplierDataAccessorAssignment> GetCdmSupplierFilterForGetById(int siteID, DateTime date)
    //{
    //  BYEsoDomainModelKernel.Models.Supplier supplierAlias = null;
    //  DataAccessor dataAccessor = null;
    //  DataAccessorHierarchyAssignment assignedList = null;

    //  return
    //    QueryOver.Of<SupplierDataAccessorAssignment>()
    //      .JoinAlias(x => x.DataAccessor, () => dataAccessor, JoinType.InnerJoin)
    //      .JoinAlias(() => dataAccessor.DataAccessorsThatIImpactThroughAssignment, ()
    //                                                                                 => assignedList, JoinType.InnerJoin)
    //      .Where(x => x.Start <= date && x.End >= date)
    //      .And(() => assignedList.DataAccessor.ID == siteID)
    //      .And(x => x.Supplier.ID == supplierAlias.ID)
    //      .Select(x => x.Supplier.ID);
    //}
  }
}
