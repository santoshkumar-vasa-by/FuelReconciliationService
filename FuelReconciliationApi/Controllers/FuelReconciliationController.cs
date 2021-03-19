using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ByEsoDomainInfraStructure.UnitOfWork;
using FuelReconciliationDomainModel;
using Microsoft.Extensions.Logging;
using NHibernate.Exceptions;

namespace FuelReconciliationApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class FuelReconciliationController : ControllerBase
  {
    private readonly IFuelItemSummaryService _FuelItemSummaryService;
    private readonly ILogger<FuelReconciliationController> _Logger;
    private readonly IUnitOfWork _UnitOfWork;
    public FuelReconciliationController(IFuelItemSummaryService fuelItemSummaryService, ILogger<FuelReconciliationController> logger, IUnitOfWork unitOfWork)
    {
      _FuelItemSummaryService = fuelItemSummaryService;
      _Logger = logger;
      _UnitOfWork = unitOfWork;
    }

    [Route("/PostFuelWac")]
    [HttpPost]
    public async Task<ActionResult> PostFuelWac(int siteID)
    {
      try
      {
        var sw = new Stopwatch();
        sw.Start();
        _Logger.LogInformation($"Request Started: {siteID}");
        await _FuelItemSummaryService.UpdateFuelWac(siteID);
        await _UnitOfWork.Commit();
        sw.Stop();
        _Logger.LogInformation($"Request Completed: {siteID}; Time taken in seconds: {sw.Elapsed.TotalSeconds}");
        return new JsonResult(new { Success = true });
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        Response.StatusCode = StatusCodes.Status500InternalServerError;
        var message = e.Message;
        var sqlString = e.GetType() == typeof(GenericADOException) ? ((GenericADOException)e).SqlString : string.Empty;
        _Logger.LogError($"Message: {message}");
        _Logger.LogError($"SQL Query: {sqlString}");

        return new JsonResult(new
        {
          Success = false, 
          message,
          sqlString
        });
      }
    }
  }
}
