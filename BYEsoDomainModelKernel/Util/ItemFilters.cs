namespace BYEsoDomainModelKernel.Util
{
  public static class ItemFilters
  {
    //public static QueryOver<ItemDataAccessorAssignment> GetCDMdItemFilter(int siteID, DateTime date)
    //{
    //  DataAccessor dataAccessor = null;
    //  DataAccessorHierarchyAssignment assignedList = null;

    //  if (date == DateTime.MinValue)
    //  {
    //    return
    //    QueryOver.Of<ItemDataAccessorAssignment>()
    //      .JoinAlias(x => x.DataAccessor, () => dataAccessor, NHibernate.SqlCommand.JoinType.InnerJoin)
    //      .JoinAlias(() => dataAccessor.DataAccessorsThatIImpactThroughAssignment, ()
    //        => assignedList, NHibernate.SqlCommand.JoinType.InnerJoin)
    //      .Where(() => assignedList.DataAccessor.ID == siteID)
    //      .Select(x => x.Item.ID);
    //  }
    //  else
    //  {
    //    return
    //    QueryOver.Of<ItemDataAccessorAssignment>()
    //      .JoinAlias(x => x.DataAccessor, () => dataAccessor, NHibernate.SqlCommand.JoinType.InnerJoin)
    //      .JoinAlias(() => dataAccessor.DataAccessorsThatIImpactThroughAssignment, ()
    //        => assignedList, NHibernate.SqlCommand.JoinType.InnerJoin)
    //      .Where(x => x.Start <= date && x.End >= date)
    //      .And(() => assignedList.DataAccessor.ID == siteID)
    //      .Select(x => x.Item.ID);
    //  }
    //}

    //public static QueryOver<ItemDataAccessorAssignment> GetCdmdItemFilterForItem(int siteID, DateTime date)
    //{
    //  DataAccessor dataAccessor = null;
    //  DataAccessorHierarchyAssignment assignedList = null;
    //  Item itemAlias = null;

    //  if (date == DateTime.MinValue)
    //  {
    //    return
    //      QueryOver.Of<ItemDataAccessorAssignment>()
    //        .JoinAlias(x => x.DataAccessor, () => dataAccessor, NHibernate.SqlCommand.JoinType.InnerJoin)
    //        .JoinAlias(() => dataAccessor.DataAccessorsThatIImpactThroughAssignment, ()
    //          => assignedList, NHibernate.SqlCommand.JoinType.InnerJoin)
    //        .Where(() => assignedList.DataAccessor.ID == siteID)
    //        .And(x=> x.Item.ID == itemAlias.ID)
    //        .Select(x => x.Item.ID);
    //  }
    //  else
    //  {
    //    return
    //      QueryOver.Of<ItemDataAccessorAssignment>()
    //        .JoinAlias(x => x.DataAccessor, () => dataAccessor, NHibernate.SqlCommand.JoinType.InnerJoin)
    //        .JoinAlias(() => dataAccessor.DataAccessorsThatIImpactThroughAssignment, ()
    //         => assignedList, NHibernate.SqlCommand.JoinType.InnerJoin)
    //        .Where(x => x.Start <= date && x.End >= date)
    //        .And(() => assignedList.DataAccessor.ID == siteID)
    //        .And(x => x.Item.ID == itemAlias.ID)
    //        .Select(x => x.Item.ID);
    //  }
    //}   
  }
}