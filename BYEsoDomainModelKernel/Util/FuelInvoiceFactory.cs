using System;
using BYEsoDomainModelKernel.Models;
using Supplier = BYEsoDomainModelKernel.Models.Supplier;

namespace BYEsoDomainModelKernel.Util
{
  public static class FuelInvoiceFactory
  {
    public static FuelInvoice CreateFuelInvoice(Site site, string billOfLadingNumber, Supplier fuelSupplier, Supplier terminalSupplier,
        BYEsoDomainModelKernel.Models.FuelInvoiceLockedFlagStatus isLocked, BYEsoDomainModelKernel.Models.FuelInvoiceStatusCode statusCode, BYEsoDomainModelKernel.Models.WacCalculatedFlag wacCalculatedFlag, 
        DateTime? wacBusinessDate, BYEsoDomainModelKernel.Models.FuelInvoiceTypeCode fuelInvoiceTypeCode = BYEsoDomainModelKernel.Models.FuelInvoiceTypeCode.Imported)
    {
      return new FuelInvoice(site, billOfLadingNumber, fuelSupplier, terminalSupplier, isLocked, statusCode, wacCalculatedFlag, 
        wacBusinessDate, fuelInvoiceTypeCode);
    }
  }
}
