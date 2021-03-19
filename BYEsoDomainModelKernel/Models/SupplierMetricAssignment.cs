namespace BYEsoDomainModelKernel.Models
{
  public class SupplierMetricAssignment
  {
    public virtual int SupplierID { get; set; }
    public virtual int MetricID { get; set; }

    // public virtual Metric Metric { get; set; }

    protected internal SupplierMetricAssignment()
    {
    }

    //protected internal SupplierMetricAssignment(Supplier supplier, Metric metric)
    //{
    //  SupplierID = supplier.ID;
    //  Metric = metric;
    //  MetricID = metric.ID;
    //}

    public override bool Equals(object other)
    {
      var ot = other as SupplierMetricAssignment;
      if (ot == null)
      {
        return false;
      }
      return ot.SupplierID == SupplierID && ot.MetricID == MetricID;
    }

    public override int GetHashCode()
    {
      return SupplierID.GetHashCode() ^ MetricID.GetHashCode();
    }
  }
}
